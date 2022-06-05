sc = Stepper_controller_TCP_v1('127.0.0.1', 5573);
if ~sc.is_connected
    return;
end
sc.Home();


%%
for pos = [0:100:1000]
sc.Move(pos);
pause(5)
end
sc.Move(0);
return
% %%
% 
% r = TCP_reader_v1('127.0.0.1', 5573);
% r.Connect();
% r.Disconnect();

%%

%%
%%
clc
r.Send_package('m:100');
r.Wait_for_package('s is_ready',20);
for i = 1:10
    
    r.Send_package('m:0');
    r.Wait_for_package('s is_ready',20);
    tic
    
    r.Send_package('m:1017,9');
    r.Wait_for_package('s is_ready',20);
    
    time_to_travel = toc;
    % disp(time_to_travel)
    disp(['Speed is ' num2str(100 / time_to_travel) ' cm/s']);
end
%%

%
% TODO
% сделать чтобы можно было переподключаться. в C# похоже виснет сессия
% и исправить, чтобы читал и точки и запятые
% чтобы не висло если в ту же точку встать
% чтобы подключался к ардуине сразу
% чтобы проверялось подключен ли к ардуине

% считывание текущей позиции, обработка ошибок при неправильном формате