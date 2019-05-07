using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Damn_Simple_Shutdown_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public Mode Mode { get; set; }
        public Action Action { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // init Action dropdown
            this.ActionComboBox.ItemsSource = Enum.GetValues(typeof(Damn_Simple_Shutdown_Timer.Action));
            this.ActionComboBox.SelectedValue = Action.Shutdown;

            // init Mode dropdown
            this.ModeComboBox.ItemsSource = Enum.GetValues(typeof(Damn_Simple_Shutdown_Timer.Mode));
            this.ModeComboBox.SelectedValue = Mode.In;

            // init hours dropdown
            var hours = new List<int>(24);
            for (int i = 0; i < 24; i++)
            {
                hours.Add(i);
            }
            this.HourComboBox.ItemsSource = hours;
            this.HourComboBox.SelectedValue = 1;
            
            // init minutes and seconds
            var minutesSeconds = new List<int>(60);
            for (int i = 0; i < 60; i++)
            {
                minutesSeconds.Add(i);
            }
            this.MinutesComboBox.ItemsSource = minutesSeconds;
            this.MinutesComboBox.SelectedValue = 0;
            this.SecondsComboBox.ItemsSource = minutesSeconds;
            this.SecondsComboBox.SelectedValue = 0;
            
        }

        private void HourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Hours = int.Parse(HourComboBox.SelectedValue.ToString());
        }

        private void MinutesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Minutes = int.Parse(MinutesComboBox.SelectedValue.ToString());
        }

        private void SecondsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Seconds = int.Parse(SecondsComboBox.SelectedValue.ToString());
        }

        private void InAtComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Mode = (Mode)Enum.Parse(typeof(Mode), ModeComboBox.SelectedValue.ToString());
        }

        private void StartTimerButton_Click(object sender, RoutedEventArgs e)
        {
            var secondsToShutdown = 0;
            if (Mode == Mode.In)
            {
                var shutdown = DateTime.Now.AddHours(this.Hours).AddMinutes(this.Minutes).AddSeconds(this.Seconds);
                secondsToShutdown = Convert.ToInt32((shutdown - DateTime.Now).TotalSeconds);
            }
            else     // Mode = "At"
            {
                // TODO consider if the timer is the day after
                secondsToShutdown = 36000;  // just so I dont shutdown this rig again by mistake
            }

            this.RunProcess("shutdown", "/s /t " + secondsToShutdown);
        }

        private void CancelTimerButton_Click(object sender, RoutedEventArgs e)
        {
            this.RunProcess("shutdown", "/a");
        }

        private void RunProcess(string process, string args)
        {
            var processInfo = new ProcessStartInfo(process, args);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            Process.Start(processInfo);
        }

        private void ActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Action = (Action)Enum.Parse(typeof(Action), ActionComboBox.SelectedValue.ToString());
        }
    }
}
