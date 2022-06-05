classdef TCP_reader_v1 < handle
    %UNTITLED Summary of this class goes here
    %   Detailed explanation goes here
    
    properties
        out_raw = ''; %неразобранный выход
        out_package_arr = {}; %разобранный выход (по <пакетам>)
        
        
        tcp_client = 0;
        connected = false;
        ip_adress;
        port_name;        
        server_is_ready = false;
    end
    %%
    methods
        %% конструктор и подключение/отключение
        function obj = TCP_reader_v1(ip_adress, port_name)
            obj.port_name = port_name;
            obj.ip_adress = ip_adress;            
        end        
        
        % подключиться
        function Connect(obj)
            try
                obj.tcp_client = tcpclient('127.0.0.1', 5573);
                obj.connected = true;
            catch
                disp('Server if not found');
                obj.connected = false;                
                return;
            end
%             disp(['TCP connected: ' obj.ip_adress ':' num2str(obj.port_name)]);            
        end
        
        % отключиться
        function Disconnect(obj)
            if (obj.connected)   
                obj.tcp_client.write('<exit>'); % по этой штуке сервер должен закрыть соединение, иначе так и будет висеть, собака
                delete(obj.tcp_client);                
                obj.connected = false;
%                 disp(['TCP port closed: '  obj.ip_adress ':' num2str(obj.port_name)]);
            end
        end
        %% Публичные функции        
        function Send_package(obj,str) % послать пакет
            str = ['<' str '>'];
            obj.tcp_client.writeline(str);
        end
        function Update(obj) % выкачать все что пришло в буфер                
            while (obj.tcp_client.NumBytesAvailable > 0)
                obj.out_raw = [obj.out_raw obj.tcp_client.read()];
                if numel(obj.out_raw) > 1e5
                    obj.out_raw = '';
                    disp('out_raw is cleared');
                end
            end
        end
        %% Получение пакетов
           
        % считать один пакет из буфера (и удалить оттуда)
        function [str, status] = Extract_package(obj)
            mark_1 = '<';
            mark_2 = '>';
            ci_1 = find(obj.out_raw == mark_1, 1, 'first');
            if isempty(ci_1)
                str = '';
                status = -1;
                return
            end
            obj.out_raw(1:ci_1) = [];
            ci_2 = find(obj.out_raw == mark_2, 1, 'first');
            if isempty(ci_2)
                str = '';
                status = -2;
                return
            end
            if ci_2 > 1 %если не сразу наткнулись на закрывающий символ
                str = obj.out_raw(1:ci_2-1);
            else
                str = 'del'; %реально получили пустую строку
            end
            obj.out_raw(1:ci_2) = [];
            status = 0;
        end        
        % считать все пакеты из буфера в массив пакетов
        function num_of_new_packages = Extract_all_packages(obj)
            num_of_new_packages = 0;
            while true
                [str, status] = Extract_package(obj);
                if status == 0
                    obj.out_package_arr = [
                        obj.out_package_arr;
                        str];
                    if numel(obj.out_package_arr) > 1e5
                        obj.out_package_arr = {};
                        disp('out_package_arr is cleared');
                    end
                    num_of_new_packages = num_of_new_packages + 1;
                else
                    break;
                end
            end
        end
        %% Работа с пакетами
        % найти номера подходящих пакетов
        function packages_i = Find_packages(obj, strats_with)
            % strats_with может быть строкой или cell массивом строк
            packages_i = [];
            for i = 1:numel(obj.out_package_arr)
                str = obj.out_package_arr{i};
                if ~iscell(strats_with)
                    if strncmp( str, strats_with, numel(strats_with) )
                        packages_i = [packages_i, i];
                    end
                else
                    for strats_with_i = 1:numel(strats_with)
                        if strncmp( str, strats_with{strats_with_i}, numel(strats_with{strats_with_i}) )
                            packages_i = [packages_i, i];
                        end
                    end
                end
            end
        end
        % удалить пакеты
        function Delete_packages(obj, packages_i)
            obj.out_package_arr(packages_i) = [];
        end        
        % вытащить данные из пакетов, начинающихся с strats_with, распрарсить с '%f,', пакеты удалить
        function float_table = Get_packages__float_table(obj, strats_with, table_width)
            if ~exist('table_width','var')
                table_width = -1; %потом определеится автоматически
            end
            float_table = [];
            packages_i = Find_packages(obj, strats_with);
            for i = packages_i
                str = obj.out_package_arr{i};
                out = sscanf(str(numel(strats_with)+1:end),'%f,');
                if (table_width == -1)
                    table_width = numel(out);
                end
                if numel(out) == table_width
                    float_table = [float_table; out'];
                end
            end
            Delete_packages(obj, packages_i);
        end        
        % вытащить данные из пакетов, начинающихся с strats_with, оставить строками, пакеты удалить
        function string_cell = Get_packages__string_cell(obj, strats_with)
            string_cell = {};
            packages_i = Find_packages(obj, strats_with);
            for i = packages_i
                str = obj.out_package_arr{i};
                out = str(numel(strats_with)+1:end);
                string_cell = [string_cell; out];
            end
            Delete_packages(obj, packages_i);
        end        
        %% высокоуровневые:
        % дождаться пакета, начинающегося с strats_with, c таймаутом (без учета уже имеющихся пакетов
        function sucsess = Wait_for_package(obj, strats_with, timeout_sec)
            gp_before = Find_packages(obj, strats_with); %какие уже есть пакеты            
            sucsess = false;
            tic
            while(toc < timeout_sec) %ждем n секунд  
                obj.Update();
                if (obj.Extract_all_packages() > 0)     
                    gp = Find_packages(obj, strats_with);                     
                    if any(~ismember(gp,gp_before))       % если есть новые пакеты         
                        sucsess = true;
                        break;
                    end
                end
            end
            
        end
    end
    
end



