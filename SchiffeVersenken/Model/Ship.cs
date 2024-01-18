using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    public partial class Ship : ObservableObject
    {
        public string name { get; set; }
        public int width { get; set; }
        public int visualWidth { get { return width * 50; } }
        public PanelType panelType { get; set; }
        public int hits {  get; set; }
        public bool isSunk { get { return hits >= width; } }
        public bool isHorizontal = true;

        [ObservableProperty]
        private bool isSet = false;

    }

    public class UBoot : Ship
    {
        public UBoot()
        {
            name = "UBoot";
            width = 1;
            panelType = PanelType.UBoot;
        }
    }

    public class Zerstörer : Ship
    {
        public Zerstörer()
        {
            name = "Zerstörer";
            width = 2;
            panelType = PanelType.Zerstörer;
        }
    }

    public class Kreuzer : Ship
    {
        public Kreuzer()
        {
            name = "Kreuzer";
            width = 3;
            panelType = PanelType.Kreuzer;
        }
    }

    public class Schlachtschiff : Ship
    {
        public Schlachtschiff()
        {
            name = "Schlachtschiff";
            width = 4;
            panelType = PanelType.Schlachtschiff;
        }
    }

    public class Flugzeugträger : Ship
    {
        public Flugzeugträger()
        {
            name = "Flugzeugträger";
            width = 5;
            panelType = PanelType.Flugzeugträger;
        }
    }
}
