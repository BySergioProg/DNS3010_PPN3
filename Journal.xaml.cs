using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
using System.Windows.Shapes;

namespace DNS3010_PPN3
{
    /// <summary>
    /// Логика взаимодействия для Journal.xaml
    /// </summary>
  
    public partial class Journal : Window
    {
        Logger.Logger logger = new Logger.Logger();
        private class Data
        {
            public string DateTime { get; set; }
            public float OilCount { get; set; }
            public float PressControl { get; set; }
            public float TempInOil { get; set; }
            public float TempOutOil { get; set; }
            public float PressOutOil { get; set; }
            public float TempSmokePoint1 { get; set; }
            public float TempSmokePoint2 { get; set; }
            public float AirPressure { get; set; }
            public float PressInGas { get; set; }
            public float PressInOil { get; set; }
            public float GasValvePos { get; set; }
            public float CurrVent { get; set; }
            public float GasInTemp { get; set; }
            public float SampleSelTemp { get; set; }
            public float AirTemp { get; set; }
            public float GasArea { get; set; }
        }

        public CheckData check = new CheckData();
        public Journal()
        {
            check.Chekk[0] = true;
            check.Chekk[2] = true;
            check.Chekk[3] = true;
            InitializeComponent();
            Date_start.SelectedDate = DateTime.Now.Date;
            DateTime Date1 = new DateTime();
            Date1 = Convert.ToDateTime(Date_start.SelectedDate).AddHours(24);
            this.DataContext = check;
     

        }
        private void Show_Data()
        {
            try
            {
         
                string Dir = Properties.Settings.Default.DBadress;
                string connectionString = $@"Data Source={Dir};Initial Catalog=DNS3010_PPN3;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string date_start = Convert.ToDateTime(Date_start.SelectedDate).ToString("yyyy-MM-ddTHH:mm:ss");
                    DateTime Date1 = new DateTime();
                    Date1 = Convert.ToDateTime(Date_start.SelectedDate).AddHours(24);
                    string date_end = Convert.ToDateTime(Date1).ToString("yyyy-MM-ddTHH:mm:ss");
                    List<Data> data = new List<Data>();
                    connection.Open();
                    string sqlExpression = $"SELECT * FROM History WHERE DateTime>='{date_start}' AND DateTime<='{date_end}' ORDER BY DateTime";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            data.Add(new Data()
                            {
                                DateTime = Convert.ToDateTime(reader.GetValue(1)).ToString("yyyy-MM-dd HH:mm:ss"),
                                OilCount = Convert.ToSingle( reader.GetValue(2)),
                                PressControl = Convert.ToSingle(reader.GetValue(3)),
                                TempInOil = Convert.ToSingle(reader.GetValue(4)),
                                TempOutOil = Convert.ToSingle(reader.GetValue(5)),
                                PressOutOil = Convert.ToSingle(reader.GetValue(6)),
                                TempSmokePoint1 = Convert.ToSingle(reader.GetValue(7)),
                                TempSmokePoint2 = Convert.ToSingle(reader.GetValue(8)),
                                AirPressure = Convert.ToSingle(reader.GetValue(9)),
                                PressInGas = Convert.ToSingle(reader.GetValue(10)),
                                PressInOil = Convert.ToSingle(reader.GetValue(11)),
                                GasValvePos = Convert.ToSingle(reader.GetValue(12)),
                                CurrVent = Convert.ToSingle(reader.GetValue(13)),
                                GasInTemp = Convert.ToSingle(reader.GetValue(14)),
                                SampleSelTemp = Convert.ToSingle(reader.GetValue(15)),
                                AirTemp = Convert.ToSingle(reader.GetValue(16)),
                                GasArea = Convert.ToSingle(reader.GetValue(17))

                            });

                        }
                    }

                    DataExit.ItemsSource = data;
                    for (int i = 0; i < check.Chekk.Length; i++)
                    {
                        if (check.Chekk[i])
                        {
                            DataExit.Columns[i+1].Visibility = Visibility.Visible;
                        }
                        else
                        {
                            DataExit.Columns[i+1].Visibility = Visibility.Collapsed;
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
        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Show_Data();
        }

        private void ChCheck(object sender, RoutedEventArgs e)
        {
            Show_Data();
        }
    }
}
