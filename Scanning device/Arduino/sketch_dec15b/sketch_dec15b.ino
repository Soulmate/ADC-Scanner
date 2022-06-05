#define FIRMWARE_ID "Arduino scaner v0.1"
#define MM_IN_STEP 0.01

//stepper
#include <AccelStepper.h>

#define dirPin 2
#define stepPin 3
#define motorInterfaceType 1

#define home_switch_button_pun 4


#define STEPPER_SPEED  4000 //больше 4000 контроллер точно не будет справляться
#define STEPPER_ACCELERATION  50000

#define HOME_SHIFT 200
#define HOME_SPEED_SLOW 500
#define MAX_STEPS 200000 //число шагов, заведомо большее чем длина всего сканирующего (для home)

AccelStepper stepper = AccelStepper(motorInterfaceType, stepPin, dirPin); // Define stepper motor connections and motor interface type. Motor interface type must be set to 1 when using a driver:

bool home_switch_is_pressed = false;

enum state {
  invalid,        // 0 не инициализирован, позиция неизвестра
  is_ready,       // 1 стоит, позиция известра, готов к дввижению
  is_moving,      // 2 движется
  is_stopping,    // 3 останавливается, прерывая движение
  is_going_home,  // 4 поиск концевика
  error           // 5 ошибка, перезапустить сканер
};
state current_state = invalid;

//serial:
const byte numChars = 32;
char receivedChars[numChars];
boolean newData = false;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void setup() {
  stepper.setMaxSpeed(STEPPER_SPEED);  // Set the maximum speed and acceleration:
  stepper.setAcceleration(STEPPER_ACCELERATION);
  //pinMode(LED_BUILTIN, OUTPUT);
  pinMode(home_switch_button_pun, INPUT_PULLUP);
  Serial.begin(115200);  while (!Serial) {;}     // wait for serial port to connect. Needed for native USB port only      
  print_info();
  print_state();
  Serial.println("<help: send h to move and find home (zero) position>");
}
void loop () {
  recvWithStartEndMarkers();
  parseNewData();
  check_limit_switch();
  switch (current_state) {    
    case is_moving:  //движется вверх
      if  (!stepper.isRunning()){
        current_state = is_ready;        
        print_state();
        Serial.println("<help: ready>");
      }
      break;    
    case is_going_home: // поиск концевика - быстро к нему
      if (home_switch_is_pressed) {
        //Serial.println("<debug: home switch pressed>");
        // остановиться, отъехать назад, медленно вперед и снова назад с блокировкой:
        run_to_stop();
        stepper.runToNewPosition ( stepper.currentPosition() + HOME_SHIFT );
        // ехать до концевика без блокировки медленно:
        stepper.setMaxSpeed(HOME_SPEED_SLOW);
        stepper.moveTo( -MAX_STEPS * 2 ); //TODO проверить сторону
        check_limit_switch();
        if (home_switch_is_pressed) {
          lock();
        }
        while (!home_switch_is_pressed)        {
          check_limit_switch();
          stepper.run();
          Serial.println(home_switch_is_pressed);
        }
        // остановиться, отъехать назад, медленно вперед и снова назад с блокировкой:
        run_to_stop();
        stepper.setMaxSpeed(STEPPER_SPEED); //вернем обратно скорость
        stepper.runToNewPosition ( stepper.currentPosition() + HOME_SHIFT );
        stepper.setCurrentPosition (0); // считаем нулем
        current_state = is_ready;
        print_state();
        Serial.println("<help: ready>");
      }
      break;
  }
  stepper.run();

  //print_state();
}

void home()  //найти домашний концевик
{
  current_state = is_going_home;
  stepper.moveTo( -MAX_STEPS * 2 ); // ехать до концевика без блокировки: //TODO проверить сторону
}

void run_to_stop() //остановиться до конца с блокировкой
{
  current_state = emerg_stopping; //начало остановки
  print_state();
  stepper.stop();
  while ( stepper.isRunning() )
  {
    stepper.run();
  }
  current_state = is_ready;
  print_state();
}

void lock() //выкинуть ошибку и заблокироваться до перезагрузки
{
  current_state = error;
  print_state();
  run_to_stop();
  // while ( true )  //TODO включить блокировку
  print_state();
}

void check_limit_switch()
{
  home_switch_is_pressed = !digitalRead(home_switch_button_pun);
}
/*
     digitalWrite(LED_BUILTIN, LOW);
  // Set the target position:
  stepper.moveTo(20000);
  // Run to target position with set speed and acceleration/deceleration:
  stepper.runToPosition();
  delay(1000);
  // Move back to zero:
  digitalWrite(LED_BUILTIN, HIGH);
  stepper.moveTo(0);
  stepper.runToPosition();
  delay(1000);
*/
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void print_info() {
  Serial.print("<info:");
  Serial.print("FIRMWARE_ID ");Serial.print(FIRMWARE_ID); Serial.print(", ");
  Serial.print("MAX_STEPS ");  Serial.print(MAX_STEPS);  Serial.print(", ");
  Serial.print("MM_IN_STEP ");  Serial.print(MM_IN_STEP);  //Serial.print(", ");
  Serial.println(">");
}
void print_state() {
  Serial.print("<state:");
  Serial.print(current_state);
  Serial.println(">");
}
void print_error(int error_flag) {
  Serial.print("<err:");
  Serial.print(millis()); Serial.print(",");
  Serial.print(error_flag);
  Serial.println(">");
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//считать из сериал порта данные, заключенные в < >
void recvWithStartEndMarkers() {
  static boolean recvInProgress = false;
  static byte ndx = 0;
  char startMarker = '<';
  char endMarker = '>';
  char rc;
  while (Serial.available() > 0 && newData == false) {
    rc = Serial.read();
    if (recvInProgress == true) {
      if (rc != endMarker) {
        receivedChars[ndx] = rc;
        ndx++;
        if (ndx >= numChars) {
          Serial.println("err:too_long");
          ndx = numChars - 1;
        }
      }
      else {
        receivedChars[ndx] = '\0'; // terminate the string
        recvInProgress = false;
        ndx = 0;
        newData = true;
      }
    }
    else if (rc == startMarker) {
      recvInProgress = true;
    }
  }
}

void parseNewData() {
  int val;
  if (newData == true) {    //receivedChars
    switch (receivedChars[0]) {

      case 'm'://move
        if (sscanf(receivedChars, "%*s%d", &val) == 1)
        {
          if (val >= 0 && val <= MAX_STEPS)
            if (current_state == is_ready) {
              current_state = is_moving;
              stepper.moveTo(val);
            }
            else
              print_error(17);
          else
            print_error(16);
        }
        else
          print_error(1); // p_scanf
        break;

      case 'g'://get
        if (receivedChars[1] == 'p') {
          Serial.print("<pos:");
          Serial.print(stepper.currentPosition());
          Serial.println(">");
        }
        if (receivedChars[1] == 's')
          print_state();
        if (receivedChars[1] == 'i')
          print_info();
        break;

      case 's'://stop
        Serial.println("Emergency stop");
        run_to_stop();
        break;

      case 'h': //home
        newData = false;
        home();
        //Serial.println("HOME");
        break;

      case 'e'://emulate limit switc
        if (sscanf(receivedChars, "%*s%d", &val) == 1)
        {
          home_switch_is_pressed = val;
          Serial.print("emulate limit switch ");  Serial.println(val);
        }
        else
          print_error(1); // p_scanf
        break;

      default:
        print_error(2); //wrong_first_char
        break;
    }
    newData = false;
  }
}

// TODO
// EEPROM!
// режим медленного сканирования
