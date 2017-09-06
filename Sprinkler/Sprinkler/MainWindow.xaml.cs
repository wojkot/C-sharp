using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Timers;
using System.Diagnostics;
using System.ComponentModel;

namespace Sprinkler
{
    /// <summary>
    /// Project based on National Instruments example of Certivied LabVIEW Developer exam "Sprinkler Controller"
    /// http://www.ni.com/gate/gb/GB_EKITCLDEXMPRP/US
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        Timer t = new Timer();
        int starttime;
        Stopwatch watch;
        States s = new States();
        int stateiterator;
        bool istimerdefined=false;
        bool continousmode = false;
        bool stop;
        bool rain=false;

        public struct States
        {
            public int[] times;
            public string[] states;
        }

        public delegate void RaiseChecking();
        public delegate void UpdateInterface();

        void statusDispatcher(object sender, ElapsedEventArgs e)
        {
            water_press_slider.Dispatcher.Invoke(new RaiseChecking(checkStatus));
           
        }

        public string [,] fileloader(string filepath)
        {
            // Read data from configuration file and returns it as 2D array
            string config=File.ReadAllText(filepath);
            string [] configData=Regex.Split(config, "\r\n");
            string [,] configdata=new string [4,2];

            for(int i=0;i<4; i++)
            {
                string [] configpart = Regex.Split(configData[i], ",");
                for(int j = 0; j < 2; j++)
                {
                    configdata[i, j] = configpart[j];
                }
                
            }
            return configdata;
        }

         void checkStatus()
        {
            // checks if specified conditions occured
            if (stop == true)
            {
                t.Stop();
                switchoff();
                return;
            }
            if (water_press_slider.Value < 50)
            {
                tbox_contstatus.Text = "Low water pressure";

                if(istimerdefined==true) watch.Stop();
            }
            else if (rain == true)
            {
                tbox_contstatus.Text = "Raining";

                if (istimerdefined == true)
                {
                    //stop timer
                    watch.Stop();
                    istimerdefined = false;
                    stateiterator = 0;
                }
            }
            else
            {
               // no special conditions occured
                tbox_contstatus.Text = "Running";
                if (istimerdefined == false)
                {
                    starttime = DateTime.Now.Second;
                    istimerdefined = true;
                    watch= Stopwatch.StartNew();
   
                }
                watch.Start();

                if (s.times[stateiterator] > (watch.ElapsedMilliseconds/1000))
                {
                    // make diodes colored
                    if (s.states[stateiterator] == "North")
                    {
                        diode_north.Fill = Brushes.YellowGreen;
                        diode_west.Fill = Brushes.DarkGreen;
                        diode_south.Fill = Brushes.DarkGreen;
                        diode_east.Fill = Brushes.DarkGreen;
                    }
                    else if (s.states[stateiterator] == "West")
                    {
                        diode_south.Fill = Brushes.DarkGreen;
                        diode_east.Fill = Brushes.DarkGreen;
                        diode_north.Fill = Brushes.DarkGreen;
                        diode_west.Fill = Brushes.YellowGreen;
                    }
                    else if (s.states[stateiterator] == "South")
                    {
                        diode_east.Fill = Brushes.DarkGreen;
                        diode_north.Fill = Brushes.DarkGreen;
                        diode_west.Fill = Brushes.DarkGreen;
                        diode_south.Fill = Brushes.YellowGreen;
                    }
                    else if (s.states[stateiterator] == "East")
                    {
                        diode_north.Fill = Brushes.DarkGreen;
                        diode_west.Fill = Brushes.DarkGreen;
                        diode_south.Fill = Brushes.DarkGreen;
                        diode_east.Fill = Brushes.YellowGreen;
                    }

                    tbox_timeleft.Text = (s.times[stateiterator] - watch.ElapsedMilliseconds / 1000).ToString();
                }
                else if (s.times[stateiterator] < (watch.ElapsedMilliseconds / 1000))
                {
                    if (stateiterator < 3)
                    {
                        // reset status before next stage
                        tbox_timeleft.Text = "0";
                        stateiterator++;
                        starttime = DateTime.Now.Second;
                        watch.Reset();
                        watch.Start();
                    }
                    else
                    {
                        // end turn
                        switchoff();
                        if (continousmode == true)
                        {
                            stateiterator = 0;
                            watch.Reset();
                            watch.Start();
                        }
                    }
                    

                }

            }

        }
 
        public void switchoff()
        {
            //switches off all diodes
            diode_north.Fill = Brushes.DarkGreen;
            diode_west.Fill = Brushes.DarkGreen;
            diode_south.Fill = Brushes.DarkGreen;
            diode_east.Fill = Brushes.DarkGreen;
            tbox_timeleft.Text = "0";
            if (continousmode == false || stop==true)
            {
                t.Stop();
                t.Elapsed -= statusDispatcher;
                t.Enabled = false;
            }

        }

        public void controlsEnabler(bool status)
        {
            //chenges status of each checkboxes and time boxes
            cbox_state1.IsEnabled = status;
            cbox_state2.IsEnabled = status;
            cbox_state3.IsEnabled = status;
            cbox_state4.IsEnabled = status;

            tbox_time_s1.IsEnabled = status;
            tbox_time_s2.IsEnabled = status;
            tbox_time_s3.IsEnabled = status;
            tbox_time_s4.IsEnabled = status;
        }
        void cboxchanger(string [,] configdata)
        {
            // set data read from config file to boxes
            cbox_state1.SelectedValue = configdata[0,0];
            tbox_time_s1.Text = configdata[0, 1];

            cbox_state2.SelectedValue = configdata[1, 0];
            tbox_time_s2.Text = configdata[1, 1];

            cbox_state3.SelectedValue = configdata[2, 0];
            tbox_time_s3.Text = configdata[2, 1];

            cbox_state4.SelectedValue = configdata[3, 0];
            tbox_time_s4.Text = configdata[3, 1];
        }
        public MainWindow()
        {
            InitializeComponent();
            string [,] configdata=fileloader(); // path of config file
            cboxchanger(configdata);
            tbox_contstatus.Text = "Controller Initialized";
            tbox_timeleft.Text = "0";

            controlsEnabler(false);

        }

        private void setup_btn_Click(object sender, RoutedEventArgs e)
        {
            controlsEnabler(true);

            tbox_contstatus.Text = "Setup mode";
        }

        private void start_btn_Click(object sender, RoutedEventArgs e)
        {

            s.times = new int[4];
            s.states = new string[4];

            s.times[0] = Convert.ToInt32(tbox_time_s1.Text);
            s.times[1] = Convert.ToInt32(tbox_time_s2.Text);
            s.times[2] = Convert.ToInt32(tbox_time_s3.Text);
            s.times[3] = Convert.ToInt32(tbox_time_s4.Text);

            s.states[0] = cbox_state1.Text;
            s.states[1] = cbox_state2.Text;
            s.states[2] = cbox_state3.Text;
            s.states[3] = cbox_state4.Text;

            stateiterator = 0;
            stop = false;
            istimerdefined = false;

            controlsEnabler(false);
            t.Interval = 100;
            t.Elapsed += statusDispatcher;
            t.Enabled = true;
            

        }

        private void run_selector_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (run_selector_slider.Value == 1)
            {
                continousmode = true;
            }
            else
            {
                continousmode = false;
            }
        }

        private void stop_btn_Click(object sender, RoutedEventArgs e)
        {
            stop = true;
            tbox_contstatus.Text = "Stopped";
            Application.Current.Shutdown();

        }

        private void rain_btn_Click(object sender, RoutedEventArgs e)
        {
            rain = !rain;

        }
    }
}
