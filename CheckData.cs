using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DNS3010_PPN3
{
   public class CheckData : INotifyPropertyChanged
    {
        private bool[] chekk { get; set; } = new bool[16];
        public bool[] Chekk 
        {
            get { return chekk; }
            set
            {
                chekk = value;
                OnPropertyChanged("Chekk");
            }
        } 


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
