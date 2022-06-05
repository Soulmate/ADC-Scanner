#pragma managed
// ����� �� "D:\2020 07 lcard puddle\E14-140\Examples\Borland C++ 5.02\ReadData\ReadData.cpp"
//******************************************************************************
// ������ E14-140.
// ���������� ��������� � ������������ ���������� ����� ������ � ���
// � ������������� ������� ���������� ������ �� ���� � �������� �������� �������.
// ���� �������������� � ������ ������ ������� ��� �� ������� 100 ���.
//******************************************************************************
#include <stdio.h>
#include <conio.h>
#include "Lusbapi.h"

//#define CHANNELS_QUANTITY			(0x4)
int CHANNELS_QUANTITY = 0x4; //2021

// ��������� ����� �� ���������
void AbortProgram(char *ErrorString, bool AbortionFlag = true);
// ������� ������ ����� ������ � ���
DWORD WINAPI ServiceReadThread(PVOID /*Context*/);
// �������� ���������� ������������ ������� �� ���� ������
BOOL WaitingForRequestCompleted(OVERLAPPED *ReadOv);
// ������� ������ ��������� � ��������
void ShowThreadErrorMessage(void);

// ����������� ��������� ���-�� ������������ 
// ����������� ������ (��� ������ ������)
const WORD MaxVirtualSoltsQuantity = 127;

// ������������� �����
HANDLE hFile;

// ������������� ������ ����� ������
HANDLE hReadThread;
DWORD ReadTid;

// ������ ����������
DWORD DllVersion;
// ��������� �� ��������� ������
ILE140 *pModule;
// ���������� ����������
HANDLE ModuleHandle;
// �������� ������
char ModuleName[7];
// �������� ������ ���� USB
BYTE UsbSpeed;
// ��������� � ������ ����������� � ������
MODULE_DESCRIPTION_E140 ModuleDescription;
// ��������� ���������� ������ ��� ������
ADC_PARS_E140 ap;

// ���-�� ���������� �������� (������� 32) ��� �. ReadData()
//DWORD DataStep = 64*1024;
DWORD DataStep = 128;
// ����� �������� NDataBlock ������ �� DataStep �������� � ������
const WORD NDataBlock = 80;
// ������� ������ ��� � ���     //// �� 0,122 �� 100
double AdcRate = 10.0;

//������������ ������� ����������
int ADC_INPUT_RANGE;

//������ ��������� ���������� ���
SHORT DacSample0;

// ����� ������
SHORT *ReadBuffer;


//�������� ������
const int ReadBuffer_Size = 10000; //� DataStep'��
int ReadBuffer_lastWritten = -1;
int ReadBuffer_lastRead = -1;

// ������ ���������� ������ ������ ����� ������
bool IsReadThreadComplete;
// ����� ������ ��� ���������� ����� ������
WORD ReadThreadErrorNumber;

// �������� �������-���������
DWORD Counter = 0x0, OldCounter = 0xFFFFFFFF;



// ��������� �������� �����
WORD TtlOut;



void InitializeModule(void)
{
	WORD i;
//	WORD DacSample;

	// ������� ������ ���������� ������ ����� ������
	IsReadThreadComplete = false;
	// ���� ������ �� �������� ��� ����� ������
	ReadBuffer = NULL;
	// ���� �� ������ ����� ����� ������
	hReadThread = NULL;
	// ���� �������� ����� ��� :(
	hFile = INVALID_HANDLE_VALUE;
	// ������� ���� ������ ������ ����� ������
	ReadThreadErrorNumber = 0x0;

	// �������� ����� ��������	
	//system("cls");

	printf(" *******************************\n");
	printf(" Module E14-140                 \n");
	printf(" Console example for ADC Stream \n");
	printf(" *******************************\n\n");

	// �������� ������ ������������ ���������� Lusbapi.dll
	if((DllVersion = GetDllVersion()) != CURRENT_VERSION_LUSBAPI)
	{
		char String[128];
		printf(String, " Lusbapi.dll Version Error!!!\n   Current: %1u.%1u. Required: %1u.%1u",
											DllVersion >> 0x10, DllVersion & 0xFFFF,
											CURRENT_VERSION_LUSBAPI >> 0x10, CURRENT_VERSION_LUSBAPI & 0xFFFF);

		AbortProgram(String);
	}
	else printf(" Lusbapi.dll Version --> OK\n");

	// ��������� �������� ��������� �� ���������
	pModule = static_cast<ILE140 *>(CreateLInstance("e140"));
	if(!pModule) AbortProgram(" Module Interface --> Bad\n");
	else printf(" Module Interface --> OK\n");

	// ��������� ���������� ������ E14-140 � ������ 256 ����������� ������
	for(i = 0x0; i < MaxVirtualSoltsQuantity; i++) if(pModule->OpenLDevice(i)) break;
	// ���-������ ����������?
	if(i == MaxVirtualSoltsQuantity) AbortProgram(" Can't find any module E14-140 in first 127 virtual slots!\n");
	else printf(" OpenLDevice(%u) --> OK\n", i);

	// ��������� ��������� ���������� ����������
	ModuleHandle = pModule->GetModuleHandle();
	if(ModuleHandle == INVALID_HANDLE_VALUE) AbortProgram(" GetModuleHandle() --> Bad\n");
	else printf(" GetModuleHandle() --> OK\n");

	// ��������� �������� ������ � ������������ ����������� �����
	if(!pModule->GetModuleName(ModuleName)) AbortProgram(" GetModuleName() --> Bad\n");
	else printf(" GetModuleName() --> OK\n");
	// ��������, ��� ��� 'E14-140'
	if(strcmp(ModuleName, "E140")) AbortProgram(" The module is not 'E14-140'\n");
	else printf(" The module is 'E14-140'\n");

	// ��������� �������� �������� ������ ���� USB
	if(!pModule->GetUsbSpeed(&UsbSpeed)) AbortProgram(" GetUsbSpeed() --> Bad\n");
	else printf(" GetUsbSpeed() --> OK\n");
	// ������ ��������� �������� ������ ���� USB
	printf("   USB is in %s\n", UsbSpeed ? "High-Speed Mode (480 Mbit/s)" : "Full-Speed Mode (12 Mbit/s)");

	// ������� ���������� �� ���� ������
	if(!pModule->GET_MODULE_DESCRIPTION(&ModuleDescription)) AbortProgram(" GET_MODULE_DESCRIPTION() --> Bad\n");
	else printf(" GET_MODULE_DESCRIPTION() --> OK\n");

	// ������� ������� ��������� ������ ���
	if(!pModule->GET_ADC_PARS(&ap)) AbortProgram(" GET_ADC_PARS() --> Bad\n");
	else printf(" GET_ADC_PARS() --> OK\n");
	// ��������� �������� ��������� ������ ���
	ap.ClkSource = INT_ADC_CLOCK_E140;							// ���������� ������ ���
	ap.EnableClkOutput = ADC_CLOCK_TRANS_DISABLED_E140; 	// ��� ���������� �������� �������� ���
	ap.InputMode = NO_SYNC_E140;									// ��� ������������� ����� ������
	ap.ChannelsQuantity = CHANNELS_QUANTITY; 					// ���-�� �������� �������



	// ��������� ����������� ������� 
	for(i = 0x0; i < ap.ChannelsQuantity; i++) ap.ControlTable[i] = (WORD)(i | (ADC_INPUT_RANGE << 0x6));
	ap.AdcRate = AdcRate;								// ������� ������ ��� � ���
	ap.InterKadrDelay = 0.0;							// ����������� �������� � ��
	// ��������� ��������� ��������� ������ ��� � ������
	if(!pModule->SET_ADC_PARS(&ap)) AbortProgram(" SET_ADC_PARS() --> Bad\n");
	else printf(" SET_ADC_PARS() --> OK\n");

		
	// ��������� ��������� ����� ������ ������ �� ������ ��������
	printf(" \n");
	printf(" Module E14-140 (S/N %s) is ready ... \n", ModuleDescription.Module.SerialNumber);
	printf("   Module Info:\n");
	printf("     Module  Revision   is '%c'\n", ModuleDescription.Module.Revision);
	printf("     MCU Driver Version is %s (%s)\n", ModuleDescription.Mcu.Version.Version, ModuleDescription.Mcu.Version.Date);
	printf("   Adc parameters:\n");
	printf("     ChannelsQuantity = %2d\n", ap.ChannelsQuantity);
	printf("     AdcRate = %8.3f kHz\n", ap.AdcRate);
	printf("     InterKadrDelay = %2.4f ms\n", ap.InterKadrDelay);
	printf("     KadrRate = %8.3f kHz\n", ap.KadrRate);

	// ����������������� ��������� �������� �������� �����
	TtlOut = 0x0;
	pModule->ENABLE_TTL_OUT(true);
}

void RunReading()
{
	// ������� ������ ��� �����
	ReadBuffer = new SHORT[ReadBuffer_Size*DataStep];
	if(!ReadBuffer) AbortProgram(" Can not allocate memory\n");

	// ������ � ��������� ����� ����� ������
	printf(" \n");
	hReadThread = CreateThread(0, 0x2000, ServiceReadThread, 0, 0, &ReadTid);
	if(!hReadThread) AbortProgram(" ServiceReadThread() --> Bad\n");
	else printf(" ServiceReadThread() --> OK\n");	
}

//------------------------------------------------------------------------
// �����, � ������� �������������� ���� ������
//------------------------------------------------------------------------
DWORD WINAPI ServiceReadThread(PVOID /*Context*/)
{
	WORD i;
	WORD RequestNumber;
//	DWORD FileBytesWritten;
	// ������ OVERLAPPED �������� �� ���� ���������
	OVERLAPPED ReadOv[2];
	// ������ �������� � ����������� ������� �� ����/����� ������
	IO_REQUEST_LUSBAPI IoReq[2];

	// ��������� ������ ��� � ������������ ������� USB-����� ������ ������
	if(!pModule->STOP_ADC()) { ReadThreadErrorNumber = 0x1; IsReadThreadComplete = true; return 0x0; }

	// ��������� ����������� ��� ����� ������ ���������
	for(i = 0x0; i < 0x2; i++)
	{
		// ������������� ��������� ���� OVERLAPPED
		ZeroMemory(&ReadOv[i], sizeof(OVERLAPPED));
		// ������ ������� ��� ������������ �������
		ReadOv[i].hEvent = CreateEvent(NULL, FALSE , FALSE, NULL);
		// ��������� ��������� IoReq		
		IoReq[i].Buffer = ReadBuffer + i*DataStep;
		IoReq[i].NumberOfWordsToPass = DataStep;
		IoReq[i].NumberOfWordsPassed = 0x0;
		IoReq[i].Overlapped = &ReadOv[i];
		IoReq[i].TimeOut = (DWORD)(DataStep/ap.AdcRate + 1000);
	}

	// ������ ��������������� ������ �� ���� ������
	RequestNumber = 0x0;
	if(!pModule->ReadData(&IoReq[RequestNumber])) { CloseHandle(ReadOv[0].hEvent); CloseHandle(ReadOv[1].hEvent); ReadThreadErrorNumber = 0x2; IsReadThreadComplete = true; return 0x0; }


	// �������� ���
	if(pModule->START_ADC())
	{
		// ���� ����� ������		
		while(true)
		{
			// ������� ������ �� ��������� ������ ������
			RequestNumber ^= 0x1;
			int positionToWrite = (ReadBuffer_lastWritten + 2)%ReadBuffer_Size;
			
			
			IoReq[RequestNumber].Buffer = ReadBuffer + positionToWrite * DataStep;		

			if(!pModule->ReadData(&IoReq[RequestNumber])) { ReadThreadErrorNumber = 0x2; break; }
			if(ReadThreadErrorNumber) break;

			// ��� ���������� �������� ����� ���������� ������ ������
			if(!WaitingForRequestCompleted(IoReq[RequestNumber^0x1].Overlapped)) break;
			if(ReadThreadErrorNumber) break;


			if (*IoReq[RequestNumber^0x1].Buffer == -23131)
				printf("aasdf\n");
			ReadBuffer_lastWritten = (IoReq[RequestNumber^0x1].Buffer - ReadBuffer)/DataStep;

			if(ReadThreadErrorNumber) break;
			else Sleep(20);
			Counter++;
		}
	}
	else { ReadThreadErrorNumber = 0x6; }

	// ��������� ������ ���
	if(!pModule->STOP_ADC()) ReadThreadErrorNumber = 0x1;
	// ������ �������� ������������� ����������� ������ �� ���� ������
	if(!CancelIo(ModuleHandle)) { ReadThreadErrorNumber = 0x7; }
	// ��������� ��� �������������� �������
	for(i = 0x0; i < 0x2; i++) CloseHandle(ReadOv[i].hEvent);
	// ��������� ��������
	Sleep(100);
	// ��������� ������ ���������� ������ ������ ����� ������
	IsReadThreadComplete = true;
	// ������ ����� �������� �������� �� ������
	return 0x0;
}

//---------------------------------------------------------------------------
//
//---------------------------------------------------------------------------
BOOL WaitingForRequestCompleted(OVERLAPPED *ReadOv)
{
	DWORD ReadBytesTransferred;

	while(TRUE)
	{
		if(GetOverlappedResult(ModuleHandle, ReadOv, &ReadBytesTransferred, FALSE)) break;
		else if(GetLastError() !=  ERROR_IO_INCOMPLETE) { ReadThreadErrorNumber = 0x3; return FALSE; }
		//else if(kbhit()) { ReadThreadErrorNumber = 0x5; return FALSE; }
		else Sleep(20);
	}
	return TRUE;
}

//------------------------------------------------------------------------
// ��������� ��������� � �������
//------------------------------------------------------------------------
void ShowThreadErrorMessage(void)
{
	switch(ReadThreadErrorNumber)
	{
		case 0x1:
			printf(" ADC Thread: STOP_ADC() --> Bad\n");
			break;

		case 0x2:
			printf(" ADC Thread: ReadData() --> Bad\n");
			break;

		case 0x3:
			printf(" ADC Thread: Timeout is occured!\n");
			break;

		case 0x4:
			printf(" ADC Thread: Writing data file error!\n");
			break;

		case 0x5:
			// ���� ��������� ���� ������ ��������, ��������� ���� ��������
			printf(" ADC Thread: The program was terminated!\n");
			break;

		case 0x6:
			printf(" ADC Thread: START_ADC() --> Bad\n");
			break;

		case 0x7:
			printf(" ADC Thread: Can't cancel ending input and output (I/O) operations!\n");
			break;

		default:
			printf(" Unknown error!\n");
			break;
	}

	return;
}

//------------------------------------------------------------------------
// ��������� ���������� ���������
//------------------------------------------------------------------------
void AbortProgram(char *ErrorString, bool AbortionFlag)
{
	//���������� ������� ���������� �� ���
	SHORT DacSample0 = (SHORT)(0x0);
	if(!pModule->DAC_SAMPLE(&DacSample0, WORD(0x0))) printf(" DAC_SAMPLE(0) --> Bad\n");

	// ��������� ��������� ������
	if(pModule)
	{
		// ��������� ��������� ������
		if(!pModule->ReleaseLInstance()) printf(" ReleaseLInstance() --> Bad\n");
		else printf(" ReleaseLInstance() --> OK\n");
		// ������� ��������� �� ��������� ������
		pModule = NULL;
	}

	// ��������� ������ ������
	if(ReadBuffer) { delete[] ReadBuffer; ReadBuffer = NULL; }
	// ��������� ������������� ������ ����� ������
	if(hReadThread) { CloseHandle(hReadThread); hReadThread = NULL; }
	// ��������� ������������� ����� ������
	if(hFile != INVALID_HANDLE_VALUE) { CloseHandle(hFile); hFile = INVALID_HANDLE_VALUE; }

	// ������� ����� ���������
	if(ErrorString) printf(ErrorString);

	// ��������� ������� ����������
	//if(kbhit()) { while(kbhit()) getch(); }

	// ���� ����� - �������� ��������� ���������
	if(AbortionFlag) 
		if(ErrorString) throw ErrorString;
		//exit(0x1);
	// ��� �������� ������� �� �������   
	else return;
}


//���������� ���������� �� ������ ��� (� ������)
bool SetVoltage(double U)
{	
	DacSample0 = (SHORT)((2047 * U/DAC_OUTPUT_RANGE_E140 + ModuleDescription.Dac.OffsetCalibration[0])*ModuleDescription.Dac.ScaleCalibration[0]);

	if(!pModule->DAC_SAMPLE(&DacSample0, WORD(0x0)))
	{
		printf(" DAC_SAMPLE(0) --> Bad\n");
		return false;
	}
	return true;
}

//���������� �������� ������ ���� ��� ������� (�� "D:\2020 07 lcard puddle\E14-140\Examples\Borland C++ 5.02\DigitalIo\DigitalIo.cpp")
bool SetTTL(bool s1, bool s2) 
{
	WORD TtlOut_1 = (WORD)s1;
	WORD TtlOut_2 = (WORD)s2 << 1;
	TtlOut = TtlOut_1 | TtlOut_2;
	

	if (pModule->TTL_OUT(TtlOut))
	{
		printf(" Digital Lines States:     0x%04X\r", TtlOut);
		return true;
	}
	else
	{
		printf("\n\n  TTL_OUT() --> Bad\n");	
		return false;
	}
}


#pragma managed
namespace LusbApi_Wrapper
{
	public ref class Module : System::IDisposable
	{
	public: 
		bool emulationMode;
		double emulationVoltage;
		double emulationT;

		double encoderEmulationOutputVoltage = 0;

		bool s1_current = false;
		bool s2_current = false;

		//�������� �� ���������� ������ ������ ��������
		array<double>^ ReadOutVoltageArray () 
		{ 	
			if (!emulationMode)
			{
				int resultSize = ReadBuffer_lastWritten - ReadBuffer_lastRead;
				if (resultSize < 0)
					resultSize += ReadBuffer_Size;

				array<double>^ result = gcnew array<double>(resultSize*DataStep);
				for (int i =0; i < resultSize; i++)	
				{
					ReadBuffer_lastRead = (ReadBuffer_lastRead + 1)%ReadBuffer_Size;
					for(int t = 0; t<DataStep; t++)
					{
						//result[i*DataStep+t] = (double)((10.0*(double)ReadBuffer[ReadBuffer_lastRead*DataStep+t]/8000.0 + ModuleDescription.Adc.OffsetCalibration[0])*ModuleDescription.Adc.ScaleCalibration[0]);						
						result[i*DataStep+t] = ADC_INPUT_RANGES_E140[ADC_INPUT_RANGE]*(double)ReadBuffer[ReadBuffer_lastRead*DataStep+t]/8000.0;
						//result[i*DataStep+t] = (double)ReadBuffer[ReadBuffer_lastRead*DataStep+t];
						//result[i*DataStep+t] = 5.0;
					}

					/*
					//TEMP - �������� ��������
					//��������� ��������
					//�� ������ ����� ���� �����
					double freq = encoderEmulationOutputVoltage  * 3 + 0.5;
					for (int i = 0; i < resultSize * DataStep - 3; i += 4)
					{						
						result[i + 1] = 5 * (1 + System::Math::Sin(freq * (emulationT += 0.001)));
					}*/
				}				
				return result;
			}
			else
			{
				int resultSize = 7;
				array<double>^ result = gcnew array<double>(resultSize*DataStep);
				for (int i = 0; i < resultSize * DataStep - 3; i += 4)
				{
					result[i + 0] = System::Math::Sin(emulationVoltage * (emulationT += 0.001));
					result[i + 1] = 0;
					result[i + 2] = s1_current;
					result[i + 3] = s2_current;
				}
				return result;
			}
		}			

		//�������� ���������� �� ���
		property double outputVoltage
		{
			double get () 
			{
				if (!emulationMode)
					return (DacSample0/ModuleDescription.Dac.ScaleCalibration[0] - ModuleDescription.Dac.OffsetCalibration[0])*DAC_OUTPUT_RANGE_E140/2047.0;
				else
					return emulationVoltage;
			}			
			void set(double value)
			{
				encoderEmulationOutputVoltage = value;

				if (!emulationMode)
					SetVoltage(value);
				else
					emulationVoltage = value;
			}
		};

		//����� ����� ���������
		property double deltaT
		{
			double get () 
			{
				if (!emulationMode)
					return (1/ap.KadrRate)/1000.0;
				else
					return (1/AdcRate * 2)/1000.0;
			}						
		};

		//���������� �������� ������� � ������� ���
		bool SetTTL_1and2(bool s1, bool s2) //����� �������� ��� ���������, ���� ������ �� ����
		{
			if (s1 != s1_current || s2 != s2_current)
			{
				bool sucesess;
				if (!emulationMode)				
					sucesess = SetTTL(s1, s2);				

				if (sucesess || emulationMode)
				{
					s1_current = s1;
					s2_current = s2;
					return true;
				}
				else
					return false;
			}
			else
				return false; //���  � ��� �� ���� ���������
		}


	//����������� � ����������
		Module(bool emulation, int channelsQuantity, double adcRate, double inputRange, int dataStep)
		{	
			emulationMode = emulation;

			if (emulationMode)
			{				
				emulationVoltage = 0;
				emulationT = 0;
			}
			else
			{
				CHANNELS_QUANTITY = channelsQuantity;
				AdcRate = adcRate;
				DataStep = dataStep;  // ���-�� ���������� �������� (������� 32) ��� �. ReadData()

				if (inputRange < 156)
				{
					ADC_INPUT_RANGE = ADC_INPUT_RANGE_156mV_E140;
					inputRange = 156;
				}
				else if (inputRange < 625)
				{
					ADC_INPUT_RANGE = ADC_INPUT_RANGE_625mV_E140;
					inputRange = 625;
				}
				else if (inputRange < 2500)
				{
					ADC_INPUT_RANGE = ADC_INPUT_RANGE_2500mV_E140;
					inputRange = 2500;
				}
				else
				{
					ADC_INPUT_RANGE = ADC_INPUT_RANGE_10000mV_E140;
					inputRange = 10000;
				}

				InitializeModule();
				RunReading();	

				SetTTL(false, false);
			}

			
		}

		~Module()
		{
			//TODO ����� ����� �� ���������� ������ ����������������
			if (!emulationMode)
			{
				//�������� ����� ������, �������������� ������� ���������� ������ �����.
				ReadThreadErrorNumber = 0x5;
				//�������� ���� ����� ����� ����������
 				Sleep(200);
			}
		}			
	};
}