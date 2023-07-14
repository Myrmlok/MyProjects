using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
namespace MVMM
{
    internal class ViewModal:INotifyPropertyChanged
    {
        public ObservableCollection<WaetherModel> listWather
        {
            get;
            set;
        }
        public ViewModal(List<string> Hours,List<int> Temps,List<BitmapImage> Icons)
        {
            listWather = new ObservableCollection<WaetherModel>();
            for(int i = 0; i < Icons.Count; i++)
            {
                WaetherModel waetherModel = new WaetherModel()
                {
                    DaylyWatherIcon = Icons[i],
                    TempNight = Temps[i],
                    Hour = Hours[i]
                };
                listWather.Add(waetherModel);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnpropatyChanged([CallerMemberName] string propety = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propety));
        }
       
    }
}
