using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace SchiffeVersenken
{
    public class GameBoard
    {
        public ObservableCollection<GameBoardPanel> GameBoardPanels { get; set; }

        public GameBoard()
        {
            GameBoardPanels = new ObservableCollection<GameBoardPanel>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    GameBoardPanels.Add(new GameBoardPanel(i, j));
                }
            }
        }

        public void HideAllPanels()
        {
            foreach(GameBoardPanel panel in GameBoardPanels)
            {
                panel.Hide();
            }
        }
    }
}
