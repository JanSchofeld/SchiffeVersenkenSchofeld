using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchiffeVersenken
{
    public partial class GameBoardPanel : ObservableObject, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        private PanelType panelType;
        public PanelType PanelType
        {
            get { return panelType; }
            set
            {
                panelType = value;
                ShipType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PanelType)));
            }
        }

        private PanelType shipType;
        public PanelType ShipType { get; set; }

        public bool isHidden = false;

        public Coordinates Coordinates { get; set; }




        public GameBoardPanel(int row, int column)
        {
            Coordinates = new Coordinates(row, column);
            PanelType = PanelType.Wasser;
        }

        public bool isUnsunkShip()
        {
            if(PanelType == PanelType.Wasser || PanelType == PanelType.Hit || PanelType == PanelType.Miss)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Hide()
        {
            isHidden = true;
        }

    }
}
