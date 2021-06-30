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
using System.Windows.Shapes;

namespace DNS3010_PPN3
{
    /// <summary>
    /// Логика взаимодействия для ErrorHistoty.xaml
    /// </summary>
    public partial class ErrorHistory : Window
    {
        Logger.Logger logger = new Logger.Logger();
        private class Errors
        {
            public string Time { get; set; }
        
            public string Alarm { get; set; }
        }
        public ErrorHistory()
        {
            InitializeComponent();
            Date_start.SelectedDate = DateTime.Now.Date;
            Date_end.SelectedDate = DateTime.Now.Date.AddHours(24);
            Show_Data();
        }
        private void Show_Data()
        {
            try
            {
                string sqlExpression;
                string Dir = Properties.Settings.Default.DBadress;
                string connectionString = $@"Data Source={Dir};Initial Catalog=DNS3010_PPN3;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string date_start = Convert.ToDateTime(Date_start.SelectedDate).ToString("yyyy-MM-ddTHH:mm:ss");
                    string date_end = Convert.ToDateTime(Date_end.SelectedDate).ToString("yyyy-MM-ddTHH:mm:ss");
                    List<Errors> errors = new List<Errors>();
                    connection.Open();
                    if (OnlyCurrentAlarm.IsChecked==true)
                    {
                        sqlExpression = $"SELECT * FROM Errors WHERE Activ=1 ORDER BY DateTime DESC";
                        Date_start.IsEnabled = false;
                        Date_end.IsEnabled = false;
                    }
                    else
                    {
                        sqlExpression = $"SELECT * FROM Errors WHERE DateTime>='{date_start}' AND DateTime<='{date_end}' ORDER BY DateTime DESC";
                        Date_start.IsEnabled = true;
                        Date_end.IsEnabled = true;
                    }
                  
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                     
                            errors.Add(new Errors()
                            {
                                Time = Convert.ToDateTime(reader.GetValue(1)).ToString("yyyy-MM-dd HH:mm:ss"),
                                Alarm = (string)(reader.GetValue(3)),
                              

                            });

                        }
                    }

                    History.ItemsSource = errors;

                    connection.Close();
                }
            }
            catch (Exception Ex)
            {
                logger.AddEvent(true, Ex.Message);
            }

        }
        private void Date_start_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Show_Data();
        }

        private void Date_end_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Show_Data();
        }

        private void OnlyCurrentAlarm_Click(object sender, RoutedEventArgs e)
        {
            Show_Data();
        }
    }
}
