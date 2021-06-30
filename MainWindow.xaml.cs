using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DNS3010_PPN3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Logger.Logger logger = new Logger.Logger();
        private DispatcherTimer ordersWorker = new DispatcherTimer();
        private DispatcherTimer ordersWorker2 = new DispatcherTimer();
        PlcData plcData = new PlcData();
        ModbusRW.Modbus modbus = new ModbusRW.Modbus(
               Properties.Settings.Default.PortName,
               Properties.Settings.Default.BaudRate,
               Properties.Settings.Default.DataBits,
               Properties.Settings.Default.Parity,
               Properties.Settings.Default.StopBits);
        int ModbusOffset;//Смещение регистра modbus
        int LinkCount;//Качество связи
        List<AlarmData> alarmDatas = new List<AlarmData>();
        public MainWindow()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
            logger.AddEvent(false, "Приложение запущено");
            InitializeComponent();
            LinkCount = 5;
            InitAlarmData();
            this.DataContext = plcData;
           
            ModbusOffset = Properties.Settings.Default.ModbusOffset;
            ordersWorker.Interval = new TimeSpan(0, 0, 0, 2, 0);
            ordersWorker.Tick += ReadModbus;
            ordersWorker.Start();
            ordersWorker2.Interval = new TimeSpan(0, 0, 0, 60, 0);
            ordersWorker2.Tick += SaveTrends;
            ordersWorker2.Start();
        }
        private void SaveTrends(object sender, EventArgs e)
        {
            try
            {
                string Dir = Properties.Settings.Default.DBadress;
                string connectionString = $@"Data Source={Dir};Initial Catalog=DNS3010_PPN3;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DateTime date = DateTime.Now;
                    string date_str = date.ToString("yyyy-MM-ddTHH:mm:ss");
                    string sqlExpression = $"INSERT INTO History (DateTime, OilCount, " +
                        $"PressControl, TempInOil, TempOutOil, PressOutOil, TempSmokePoint1, " +
                        $"TempSmokePoint2, AirPressure, PressInGas, PressInOil, GasValvePos, CurrVent, " +
                        $"GasInTemp, SampleSelTemp, AirTemp, GasArea) VALUES " +
                        $"( '{date_str}', {plcData.OilCount}, {plcData.PressControl}, {plcData.TempInOil}, {plcData.TempOutOil}, " +
                        $"{plcData.PressOutOil}, {plcData.TempSmokePoint1}, {plcData.TempSmokePoint2}, {plcData.AirPressure}," +
                        $"{plcData.PressInGas}, {plcData.PressInOil}, {plcData.GasValvePos}, {plcData.CurrVent}, {plcData.GasInTemp}, " +
                        $"{plcData.SampleSelTemp}, {plcData.AirTemp}, {plcData.GasArea})";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int number = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception Ex)
            {
                logger.AddEvent(true, Ex.Message);
            }
        }
        private void LinkQuality(bool State)
        {
        if (State)
            {
                if (LinkCount < 10) LinkCount++;
            }
        else
            {
                if (LinkCount > 0) LinkCount--;
            }
        
        }
        private void ReadModbus(object sender, EventArgs e)
        {
            try
            {
                bool State;
                ushort Addr = (ushort)(640 + ModbusOffset);
                var reg = modbus.ReadFloat(1, Addr, 16, out State);
                LinkQuality(State);
                if (State)
                {
                    plcData.OilCount = reg[0];
                    plcData.PressControl = reg[1];
                    plcData.TempInOil = reg[2];
                    plcData.TempOutOil = reg[3];
                    plcData.PressOutOil = reg[4];
                    plcData.TempSmokePoint1 = reg[5];
                    plcData.TempSmokePoint2 = reg[6];
                    plcData.AirPressure = reg[7];
                    plcData.PressInGas = reg[8];
                    plcData.PressInOil = reg[9];
                    plcData.GasValvePos = reg[10];
                    plcData.CurrVent = reg[11];
                    plcData.GasInTemp = reg[12];
                    plcData.SampleSelTemp = reg[13];
                    plcData.AirTemp = reg[14];
                    plcData.GasArea = reg[15];

                }
                Addr = (ushort)(864 + ModbusOffset);
                var reg1 = modbus.ReadHr(1, Addr, 16, out State);
                LinkQuality(State);
                if (State)
                {
                    plcData.OilCountSt = GetStatus(reg1[0]);
                    plcData.PressControlSt = GetStatus(reg1[1]);
                    plcData.TempInOilSt = GetStatus(reg1[2]);
                    plcData.TempOutOilSt = GetStatus(reg1[3]);
                    plcData.PressOutOilSt = GetStatus(reg1[4]);
                    plcData.TempSmokePoint1St = GetStatus(reg1[5]);
                    plcData.TempSmokePoint2St = GetStatus(reg1[6]);
                    plcData.AirPressureSt = GetStatus(reg1[7]);
                    plcData.PressInGasSt = GetStatus(reg1[8]);
                    plcData.PressInOilSt = GetStatus(reg1[9]);
                    plcData.GasValvePosSt = GetStatus(reg1[10]);
                    plcData.CurrVentSt = GetStatus(reg1[11]);
                    plcData.GasInTempSt = GetStatus(reg1[12]);
                    plcData.SampleSelTempSt = GetStatus(reg1[13]);
                    plcData.AirTempSt = GetStatus(reg1[14]);
                    plcData.GasAreaSt = GetStatus(reg1[15]);

                    plcData.OilCountCo = GetColor(reg1[0]);
                    plcData.PressControlCo = GetColor(reg1[1]);
                    plcData.TempInOilCo = GetColor(reg1[2]);
                    plcData.TempOutOilCo = GetColor(reg1[3]);
                    plcData.PressOutOilCo = GetColor(reg1[4]);
                    plcData.TempSmokePoint1Co = GetColor(reg1[5]);
                    plcData.TempSmokePoint2Co = GetColor(reg1[6]);
                    plcData.AirPressureCo = GetColor(reg1[7]);
                    plcData.PressInGasCo = GetColor(reg1[8]);
                    plcData.PressInOilCo = GetColor(reg1[9]);
                    plcData.GasValvePosCo = GetColor(reg1[10]);
                    plcData.CurrVentCo = GetColor(reg1[11]);
                    plcData.GasInTempCo = GetColor(reg1[12]);
                    plcData.SampleSelTempCo = GetColor(reg1[13]);
                    plcData.AirTempCo = GetColor(reg1[14]);
                    plcData.GasAreaCo = GetColor(reg1[15]);

                }
                Addr = (ushort)(3840 + ModbusOffset);
                var reg2 = modbus.ReadCoil(1, Addr, 71, out State);
                LinkQuality(State);
                if (State)
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("Resources/Signal_lamp_red_off.png", UriKind.Relative);
                    bi3.EndInit();

                    for (int i = 0; i < reg2.Length; i++)
                    {
                        if (reg2[i])
                        {
                            bi3.BeginInit();
                            bi3.UriSource = new Uri("Resources/Signal_lamp_red.png", UriKind.Relative);
                            bi3.EndInit();

                        }

                        if (alarmDatas[i].Satus != reg2[i])
                        {
                            SaveErrors(reg2[i], alarmDatas[i].AlarmText);
                            alarmDatas[i].Satus = reg2[i];
                            // Запись тревоги в базу данных
                        }
                    }
                    Lamp.Source = bi3;
                }
                Addr = (ushort)(1026 + ModbusOffset);
                var reg3 = modbus.ReadHr(1, Addr, 1, out State);
                LinkQuality(State);
                if (State)
                {
                    plcData.Stage = GetStage(reg3[0]);
                }
                if (LinkCount == 0)
                {
                    SaveErrors(true, "Нет связи с контроллером");
                    AlarmConnect.Visibility = Visibility.Visible;
                }
                else
                {
                    SaveErrors(false, "Нет связи с контроллером");
                    AlarmConnect.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception Ex)
            {
                logger.AddEvent(true, Ex.Message);
            }
        }
        private string GetColor (ushort Prop)
        {
            string Data = "";
            switch (Prop)
            {
                case 0:
                    Data = "Green";
                    break;
                case 1:
                    Data = "Yellow";
                    break;
                case 2:
                    Data = "Red";
                    break;
                case 3:
                    Data = "Yellow";
                    break;
                case 4:
                    Data = "Red";
                    break;
                case 5:
                    Data = "Gray";
                    break;
                default:
                    Data = "Gray";
                    break;

            }


            return Data;


        }
        private string GetStatus(ushort Prop)
        {
            string Data = "";
            switch(Prop)
            {
                case 0:
                    Data = "Канал в норме";
                    break;
                case 1:
                    Data = "Нижний предупредительный уровень";
                    break;
                case 2:
                    Data = "Нижний аварийный уровень";
                    break;
                case 3:
                    Data = "Верхний предупредительный уровень";
                    break;
                case 4:
                    Data = "Верхний аварийный уровень";
                    break;
                case 5:
                    Data = "Неисправность канала";
                    break;
                default:
                    Data = "Не корректные данные";
                    break;

            }


            return Data;
        }
        private string GetStage (ushort Prop)
        {
            string Data = "";
            switch (Prop)
            {
                case 0:
                    Data = "Останов";
                    break;
                case 1:
                    Data = "Предпроверка";
                    break;
                case 2:
                    Data = "Установка затвора для проверки герметичности";
                    break;
                case 3:
                    Data = "Опрессовка отсечного клапана";
                    break;
                case 4:
                    Data = "Заполнение коллектора газом";
                    break;
                case 5:
                    Data = "Опрессовка коллектора";
                    break;
                case 6:
                    Data = "Сброс газа";
                    break;
                case 7:
                    Data = "Вентиляция";
                    break;
                case 8:
                    Data = "Подготовка к розжигу";
                    break;
                case 9:
                    Data = "Розжиг запальной горелки";
                    break;
                case 10:
                    Data = "Стабилизация запальной горелки";
                    break;
                case 11:
                    Data = "Продувка на свечу";
                    break;
                case 12:
                    Data = "Розжиг основной горелки";
                    break;
                case 13:
                    Data = "Стабилизация основной горелки";
                    break;
                case 14:
                    Data = "Прогрев";
                    break;
                case 15:
                    Data = "Работа";
                    break;
                case 16:
                    Data = "Плавный останов";
                    break;
                case 17:
                    Data = "Финишная продувка";
                    break;
                case 18:
                    Data = "Аварийная продувка";
                    break;
                case 19:
                    Data = "Авария";
                    break;
                case 20:
                    Data = "Ручной режим";
                    break;
                default:
                    Data = "Не корректные данные";
                    break;
            }
            return Data;
        }
        private void InitAlarmData()
        {
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Расход продукта нижний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление контроля герметичности нижний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление контроля герметичности верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура нефти на входе верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура нефти на выходе верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление нефти на выходе нижний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление нефти на выходе верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура дымовых газов в точке 1 верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура дымовых газов в точке 2 верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление воздуха нижний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление воздуха верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление газа на входе нижний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление газа на входе верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление нефти на входе нижний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление нефти на входе верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура газа на воде нижнийаварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура газа на входе верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура отбора проб верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура воздуха верхний аварийны уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Загазованность площадки верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Разница давления вход-выход" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Не герметичен отсечной клапан" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Не герметичен коллектор" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Низкое давление в коллекторе при опрессовке" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Пожар (температура пожара)" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Пожар (Т дыма 1 верхний аварийный уровень и Т дыма 2 верхний аварийный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Расход продукта" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Давление контроля герметичности" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Температура нефти на входе" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Температура нефти на выходе" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Давление нефти на выходе" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Температура дымовых газов в точке 1" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Температура дымовых газов в точке 2" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Давление воздуха" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Давление газа на входе" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Давление нефти на входе" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Положение заслонки газа" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Частота привода вентилятора" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика:Температура газа на входе" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Температура отбора проб" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Температура воздуха" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Неисправность датчика: Загазованность на площадке" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Отказ ПЧ" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Пламя до розжига" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Нет пламени" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Загазованность площадки порог 2" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Отказ ГСМ05" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Загазованность отбора проб порог 2" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Аварийный останов" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Расход продукта нижний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление контроля герметичности нижний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление контроля герметичности верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура нефти на входе верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура нефти на выходе верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление нефти на выходе нижний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление нефти на выходе верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура дымовых газов в точке 1 верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура дымовых газов в точке 2 верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление воздуха нижний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление воздуха верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление газа на входе нижний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление газа на входе верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление нефти на входе нижний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Давление нефти на входе верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура газа на входе нижний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура газа на входе верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура отбора проб верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Температура воздуха верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Загазованность площадки верхний предельный уровень" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Загазованность площадки порог 1" });
            alarmDatas.Add(new AlarmData { Satus = false, AlarmText = "Загазованность отбора порог 1" });
           
        }
        private void SaveErrors(bool Err, string ErrText)
        {
            try
            {
                string Dir = Properties.Settings.Default.DBadress;
                string connectionString = $@"Data Source={Dir};Initial Catalog=DNS3010_PPN3;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    if (Err)
                    {

                        string sqlExpression = $"SELECT * FROM Errors WHERE Message='{ErrText}' and Activ=1";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            using (SqlConnection connection2 = new SqlConnection(connectionString))
                            {
                                connection2.Open();
                                DateTime date = DateTime.Now;
                                string date_str = date.ToString("yyyy-MM-ddTHH:mm:ss");
                                sqlExpression = $"INSERT INTO Errors (DateTime, Activ, Message) VALUES ('{date_str}', 1, '{ErrText}' )";
                                SqlCommand command2 = new SqlCommand(sqlExpression, connection2);
                                int number = command2.ExecuteNonQuery();
                                connection2.Close();
                            }
                        }
                    }
                    else
                    {
                        using (SqlConnection connection2 = new SqlConnection(connectionString))
                        {
                            connection2.Open();
                            DateTime date = DateTime.Now;
                            string date_str = date.ToString("yyyy-MM-ddTHH:mm:ss");
                            string sqlExpression = $"UPDATE Errors SET Activ=0 WHERE Activ=1 AND  Message='{ErrText}'";
                            SqlCommand command2 = new SqlCommand(sqlExpression, connection2);
                            int number = command2.ExecuteNonQuery();
                            connection2.Close();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception Ex)
            {
                logger.AddEvent(true, Ex.Message);
            }

        }

        private void ShowAlarms(object sender, MouseButtonEventArgs e)
        {
            ErrorHistory errorHistory = new ErrorHistory();
            errorHistory.Owner = this;
            errorHistory.Show();
        }

        private void ShowHistory(object sender, RoutedEventArgs e)
        {
            Journal journal = new Journal();
            journal.Owner = this;
            journal.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            logger.AddEvent(false, "Приложение закрыто");
        }
    }
}
