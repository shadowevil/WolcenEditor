using System;
using System.Collections.Generic;
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
using WolcenData;

namespace WolcenEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CharacterData cd = new CharacterData(@"C:\Users\ShadowEvil\Saved Games\wolcen\savegames\characters\shadowevil.json");
            cd.Stats.Gold = "99999";

            cd.Telemetry.GoldDropped.Total = cd.Stats.Gold;
            cd.Telemetry.GoldPicked.Total = cd.Stats.Gold;
            cd.Telemetry.GoldGainedQuests.Total = cd.Stats.Gold;
            cd.Telemetry.GoldGainedMerchant.Total = cd.Stats.Gold;

            cd.Telemetry.GoldDropped.PerLevel = cd.Stats.Gold;
            cd.Telemetry.GoldPicked.PerLevel = cd.Stats.Gold;
            cd.Telemetry.GoldGainedQuests.PerLevel = cd.Stats.Gold;
            cd.Telemetry.GoldGainedMerchant.PerLevel = cd.Stats.Gold;
            cd.Save();
        }
    }
}
