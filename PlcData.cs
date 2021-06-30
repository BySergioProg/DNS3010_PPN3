using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DNS3010_PPN3
{
   public class PlcData : INotifyPropertyChanged
    {
        private string stage { get; set; }//Текущий этап работы контроллера
        public string Stage
        {
            get { return stage; }
            set
            {
                stage = value;
                OnPropertyChanged("Stage");
            }
        }

        #region Данные аналоговых входов
        private float oilCount { get; set; }//Расход продукта
        public float OilCount
        {
            get { return oilCount; }
            set
            {
                oilCount = value;
                OnPropertyChanged("OilCount");
            }
        }
        private float pressControl { get; set; }//Давление контроля герметичности
        public float PressControl
        {
            get { return pressControl; }
            set
            {
                pressControl = value;
                OnPropertyChanged("PressControl");
            }
        }
        private float tempInOil { get; set; }//Температура нефти на входе
        public float TempInOil
        {
            get { return tempInOil; }
            set
            {
                tempInOil = value;
                OnPropertyChanged("TempInOil");
            }
        }
        private float tempOutOil { get; set; }//Температура нефти на выходе
        public float TempOutOil
        {
            get { return tempOutOil; }
            set
            {
                tempOutOil = value;
                OnPropertyChanged("TempOutOil");
            }
        }
        private float pressOutOil { get; set; }//Давление нефти на выходе
        public float PressOutOil
        {
            get { return pressOutOil; }
            set
            {
                pressOutOil = value;
                OnPropertyChanged("PressOutOil");
            }
        }
        private float tempSmokePoint1 { get; set; }//Температура дымовых газов в точке 1
        public float TempSmokePoint1
        {
            get { return tempSmokePoint1; }
            set
            {
                tempSmokePoint1 = value;
                OnPropertyChanged("TempSmokePoint1");
            }
        }
        private float tempSmokePoint2 { get; set; }//Температура дымовых газов в точке 2
        public float TempSmokePoint2
        {
            get { return tempSmokePoint2; }
            set
            {
                tempSmokePoint2 = value;
                OnPropertyChanged("TempSmokePoint2");
            }
        }
        private float airPressure { get; set; }//Давление воздуха
        public float AirPressure
        {
            get { return airPressure; }
            set
            {
                airPressure = value;
                OnPropertyChanged("AirPressure");
            }
        }
        private float pressInGas { get; set; }//Давление газа на входе
        public float PressInGas
        {
            get { return pressInGas; }
            set
            {
                pressInGas = value;
                OnPropertyChanged("PressInGas");
            }
        }
        private float pressInOil { get; set; }//Давление нефти на входе
        public float PressInOil
        {
            get { return pressInOil; }
            set
            {
                pressInOil = value;
                OnPropertyChanged("PressInOil");
            }
        }
        private float gasValvePos { get; set; }//Положение заслонки газа
        public float GasValvePos
        {
            get { return gasValvePos; }
            set
            {
                gasValvePos = value;
                OnPropertyChanged("GasValvePos");
            }
        }
        private float currVent { get; set; }//Частота приводного вентилятора
        public float CurrVent
        {
            get { return currVent; }
            set
            {
                currVent = value;
                OnPropertyChanged("CurrVent");
            }
        }
        private float gasInTemp { get; set; }//Температура газа на входе
        public float GasInTemp
        {
            get { return gasInTemp; }
            set
            {
                gasInTemp = value;
                OnPropertyChanged("GasInTemp");
            }
        }
        private float sampleSelTemp { get; set; }//Температура отбора проб
        public float SampleSelTemp
        {
            get { return sampleSelTemp; }
            set
            {
                sampleSelTemp = value;
                OnPropertyChanged("SampleSelTemp");
            }
        }
        private float airTemp { get; set; }//Температура воздуха
        public float AirTemp
        {
            get { return airTemp; }
            set
            {
                airTemp = value;
                OnPropertyChanged("AirTemp");
            }
        }
        private float gasArea { get; set; }//Загазованность на площадке
        public float GasArea
        {
            get { return gasArea; }
            set
            {
                gasArea = value;
                OnPropertyChanged("GasArea");
            }
        }
        #endregion


        #region Статус аналоговых входов
        private string oilCountSt { get; set; }//Расход продукта
        public string OilCountSt
        {
            get { return oilCountSt; }
            set
            {
                oilCountSt = value;
                OnPropertyChanged("OilCountSt");
            }
        }
        private string pressControlSt { get; set; }//Давление контроля герметичности
        public string PressControlSt
        {
            get { return pressControlSt; }
            set
            {
                pressControlSt = value;
                OnPropertyChanged("PressControlSt");
            }
        }
        private string tempInOilSt { get; set; }//Температура нефти на входе
        public string TempInOilSt
        {
            get { return tempInOilSt; }
            set
            {
                tempInOilSt = value;
                OnPropertyChanged("TempInOilSt");
            }
        }
        private string tempOutOilSt { get; set; }//Температура нефти на выходе
        public string TempOutOilSt
        {
            get { return tempOutOilSt; }
            set
            {
                tempOutOilSt = value;
                OnPropertyChanged("TempOutOilSt");
            }
        }
        private string pressOutOilSt { get; set; }//Давление нефти на выходе
        public string PressOutOilSt
        {
            get { return pressOutOilSt; }
            set
            {
                pressOutOilSt = value;
                OnPropertyChanged("PressOutOilSt");
            }
        }
        private string tempSmokePoint1St { get; set; }//Температура дымовых газов в точке 1
        public string TempSmokePoint1St
        {
            get { return tempSmokePoint1St; }
            set
            {
                tempSmokePoint1St = value;
                OnPropertyChanged("TempSmokePoint1St");
            }
        }
        private string tempSmokePoint2St { get; set; }//Температура дымовых газов в точке 2
        public string TempSmokePoint2St
        {
            get { return tempSmokePoint2St; }
            set
            {
                tempSmokePoint2St = value;
                OnPropertyChanged("TempSmokePoint2St");
            }
        }
        private string airPressureSt { get; set; }//Давление воздуха
        public string AirPressureSt
        {
            get { return airPressureSt; }
            set
            {
                airPressureSt = value;
                OnPropertyChanged("AirPressureSt");
            }
        }
        private string pressInGasSt { get; set; }//Давление газа на входе
        public string PressInGasSt
        {
            get { return pressInGasSt; }
            set
            {
                pressInGasSt = value;
                OnPropertyChanged("PressInGasSt");
            }
        }
        private string pressInOilSt { get; set; }//Давление нефти на входе
        public string PressInOilSt
        {
            get { return pressInOilSt; }
            set
            {
                pressInOilSt = value;
                OnPropertyChanged("PressInOilSt");
            }
        }
        private string gasValvePosSt { get; set; }//Положение заслонки газа
        public string GasValvePosSt
        {
            get { return gasValvePosSt; }
            set
            {
                gasValvePosSt = value;
                OnPropertyChanged("GasValvePosSt");
            }
        }
        private string currVentSt { get; set; }//Частота приводного вентилятора
        public string CurrVentSt
        {
            get { return currVentSt; }
            set
            {
                currVentSt = value;
                OnPropertyChanged("CurrVentSt");
            }
        }
        private string gasInTempSt { get; set; }//Температура газа на входе
        public string GasInTempSt
        {
            get { return gasInTempSt; }
            set
            {
                gasInTempSt = value;
                OnPropertyChanged("GasInTempSt");
            }
        }
        private string sampleSelTempSt { get; set; }//Температура отбора проб
        public string SampleSelTempSt
        {
            get { return sampleSelTempSt; }
            set
            {
                sampleSelTempSt = value;
                OnPropertyChanged("SampleSelTempSt");
            }
        }
        private string airTempSt { get; set; }//Температура воздуха
        public string AirTempSt
        {
            get { return airTempSt; }
            set
            {
                airTempSt = value;
                OnPropertyChanged("AirTempSt");
            }
        }
        private string gasAreaSt { get; set; }//Загазованность на площадке
        public string GasAreaSt
        {
            get { return gasAreaSt; }
            set
            {
                gasAreaSt = value;
                OnPropertyChanged("GasAreaSt");
            }
        }
        #endregion


        #region Фон входов
        private string oilCountCo { get; set; }//Расход продукта
        public string OilCountCo
        {
            get { return oilCountCo; }
            set
            {
                oilCountCo = value;
                OnPropertyChanged("OilCountCo");
            }
        }
        private string pressControlCo { get; set; }//Давление контроля герметичности
        public string PressControlCo
        {
            get { return pressControlCo; }
            set
            {
                pressControlCo = value;
                OnPropertyChanged("PressControlCo");
            }
        }
        private string tempInOilCo { get; set; }//Температура нефти на входе
        public string TempInOilCo
        {
            get { return tempInOilCo; }
            set
            {
                tempInOilCo = value;
                OnPropertyChanged("TempInOilCo");
            }
        }
        private string tempOutOilCo { get; set; }//Температура нефти на выходе
        public string TempOutOilCo
        {
            get { return tempOutOilCo; }
            set
            {
                tempOutOilCo = value;
                OnPropertyChanged("TempOutOilCo");
            }
        }
        private string pressOutOilCo { get; set; }//Давление нефти на выходе
        public string PressOutOilCo
        {
            get { return pressOutOilCo; }
            set
            {
                pressOutOilCo = value;
                OnPropertyChanged("PressOutOilCo");
            }
        }
        private string tempSmokePoint1Co { get; set; }//Температура дымовых газов в точке 1
        public string TempSmokePoint1Co
        {
            get { return tempSmokePoint1Co; }
            set
            {
                tempSmokePoint1Co = value;
                OnPropertyChanged("TempSmokePoint1Co");
            }
        }
        private string tempSmokePoint2Co { get; set; }//Температура дымовых газов в точке 2
        public string TempSmokePoint2Co
        {
            get { return tempSmokePoint2Co; }
            set
            {
                tempSmokePoint2Co = value;
                OnPropertyChanged("TempSmokePoint2Co");
            }
        }
        private string airPressureCo { get; set; }//Давление воздуха
        public string AirPressureCo
        {
            get { return airPressureCo; }
            set
            {
                airPressureCo = value;
                OnPropertyChanged("AirPressureCo");
            }
        }
        private string pressInGasCo { get; set; }//Давление газа на входе
        public string PressInGasCo
        {
            get { return pressInGasCo; }
            set
            {
                pressInGasCo = value;
                OnPropertyChanged("PressInGasCo");
            }
        }
        private string pressInOilCo { get; set; }//Давление нефти на входе
        public string PressInOilCo
        {
            get { return pressInOilCo; }
            set
            {
                pressInOilCo = value;
                OnPropertyChanged("PressInOilCo");
            }
        }
        private string gasValvePosCo { get; set; }//Положение заслонки газа
        public string GasValvePosCo
        {
            get { return gasValvePosCo; }
            set
            {
                gasValvePosCo = value;
                OnPropertyChanged("GasValvePosCo");
            }
        }
        private string currVentCo { get; set; }//Частота приводного вентилятора
        public string CurrVentCo
        {
            get { return currVentCo; }
            set
            {
                currVentCo = value;
                OnPropertyChanged("CurrVentCo");
            }
        }
        private string gasInTempCo { get; set; }//Температура газа на входе
        public string GasInTempCo
        {
            get { return gasInTempCo; }
            set
            {
                gasInTempCo = value;
                OnPropertyChanged("GasInTempCo");
            }
        }
        private string sampleSelTempCo { get; set; }//Температура отбора проб
        public string SampleSelTempCo
        {
            get { return sampleSelTempCo; }
            set
            {
                sampleSelTempCo = value;
                OnPropertyChanged("SampleSelTempCo");
            }
        }
        private string airTempCo { get; set; }//Температура воздуха
        public string AirTempCo
        {
            get { return airTempCo; }
            set
            {
                airTempCo = value;
                OnPropertyChanged("AirTempCo");
            }
        }
        private string gasAreaCo { get; set; }//Загазованность на площадке
        public string GasAreaCo
        {
            get { return gasAreaCo; }
            set
            {
                gasAreaCo = value;
                OnPropertyChanged("GasAreaCo");
            }
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
