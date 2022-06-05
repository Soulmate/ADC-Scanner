clear all;

pos_list = [0 10 20 1000];



%% сканирующее
addpath('D:\Кандауров\Программы\2020 12 Сканирующее ардуино\matlab');
!exe\Scanner\Arduino_scanner_control.exe &
%!!! надо предварительно запустить exe сканера и там подключиться к сканеру
sc = Stepper_controller_TCP_v1('127.0.0.1', 5573);
if ~sc.is_connected
    return;
end
sc.Home();
%% АЦП
adc_ch = 16; %Число каналов
adc_freq_Hz = 1000; %Частота опроса, Гц
adc_duration_s = int32(5 * adc_freq_Hz); % Число отсчетов
adc_saveFolder  = [pwd '\test1']; % !!! абсолютный путь, т.к. он читается другой программой
mkdir(adc_saveFolder);
% путь к исполняемому файлу
exe_path = 'exe\ADC\WindowsFormsApplication_ADC_DAC.exe';
%%




for i = 1:numel(pos_list)
    pos = pos_list(i);
    adc_savePath = [adc_saveFolder '\rec' num2str(i) '.dat']; % !!! абсолютный путь, т.к. он читается другой программой
    %% Передвижение к точке
    sc.Move(pos);
    pause(1); % чтобы успокоилось всё
    disp(['Положение pos: ' num2str(pos)]);
    %% Запуск программы АЦП-ЦАП с параметрами
    disp(['Запись в ' adc_savePath]);
    system( ...
    sprintf( '"%s" %d %d %d "%s"', ... channels, freq_Hz, numberOfSamples, savePath
        exe_path,...
        adc_ch,...
        adc_freq_Hz,...
        adc_duration_s,...
        adc_savePath)...
        );
    
   
    f = load(adc_savePath);
    clf
    subplot(2,1,1);
    plot(f(:,1),f(:,2))
    subplot(2,1,2);
    plot(f(:,1),f(:,end))
    drawnow();
end
