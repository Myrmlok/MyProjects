using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Runtime.CompilerServices;

namespace MVMM
{
    public class WaetherModel : INotifyPropertyChanged
    {
        string time;
        public string Hour
        {
            get => time;
            set
            {
                time = value;
                OnpropatyChanged();
            }
        }
        
        int tempNight;
        public int TempNight
        {
            get => tempNight;
            set
            {
                tempNight = value;
                OnpropatyChanged();
            }
        }

        BitmapImage daylyWatherIcon;
        public BitmapImage DaylyWatherIcon
        {
            get => daylyWatherIcon;
            set
            {
                daylyWatherIcon = value;
                OnpropatyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropatyChanged([CallerMemberName] string propety = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propety));
        }
    }
}
