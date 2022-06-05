classdef Stepper_controller_TCP_v1 < handle
    %UNTITLED Summary of this class goes here
    %   Detailed explanation goes here
    
    properties
        tcp_reader; 
        is_connected % смог ли успешно подключиться в конструкторе
        is_valid % инициализировано ли сканирующее (если нет, надо сделать HOME)
        current_pos = nan;
    end
    
    methods
        function obj = Stepper_controller_TCP_v1(ip_adress, port_name)
            obj.tcp_reader = TCP_reader_v1(ip_adress, port_name);
            
            try 
                obj.tcp_reader.Connect();            
            catch                
                obj.is_connected = false;
                return;
            end
            
            if (~obj.tcp_reader.connected)                
                obj.is_connected = false;
                return;
            end
            
            
            disp('Initial connection...')   
            if obj.tcp_reader.Wait_for_package('Server is ready',5)
                disp('Server is ready')
                obj.is_connected  = true;                
            else
                disp('Server is not answering')
                obj.is_connected  = false;
            end
            
            obj.tcp_reader.Disconnect();
            obj.is_valid = false;
        end
        
        % ждет ответа сервера
        function Home(obj)
            if (~obj.is_connected)
                disp('Error: no TCP connection')
                return; 
            end
            disp('Home')
            obj.tcp_reader.Connect();
            obj.tcp_reader.Send_package('h');
            obj.tcp_reader.Wait_for_package('status:is_ready',100);
            obj.tcp_reader.Disconnect();
            obj.is_valid = true;
            obj.current_pos = 0;
            disp('Ready')  
            return;
        end
        
         function Move(obj, pos_mm)
            if (~obj.is_connected)
                disp('Error: no TCP connection')
                return; 
            end
            if (~obj.is_valid)
                disp('Error: make HOME to initialize scanner')
                return; 
            end
            disp('Moving...')
            obj.tcp_reader.Connect();
            obj.tcp_reader.Send_package(['m:' num2str(pos_mm)]);            
            obj.tcp_reader.Wait_for_package({'status:is_ready' 'error:out_of_bounds'},100);
            c_status = obj.tcp_reader.Get_packages__string_cell('status:');
            c_err = obj.tcp_reader.Get_packages__string_cell('status:');
%             a_pos = obj.tcp_reader.
            
            obj.tcp_reader.Disconnect();            
            disp('Ready')                
         end
        
         function Disconnect(obj)
             if (obj.is_connected)
                 disp('Disconnecting');
                 obj.tcp_reader.Disconnect();
            end
         end
    end
end

