using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;


namespace TaskManager_HW1
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private ObservableCollection<string> allProcess;
        public ObservableCollection<string> AllProcess
        {
            get { return allProcess; }
            set { allProcess = value; OnPropertyChanged(); }
        }


        private ObservableCollection<string> blackBox;
        public ObservableCollection<string> BlackBox
        {
            get { return blackBox; }
            set { blackBox = value; OnPropertyChanged(); }
        }


        DispatcherTimer timer;
        DispatcherTimer timer2;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(1000);
            timer.Tick += Timer_Tick;
            timer.Start();

            timer2 = new DispatcherTimer();
            timer2.Interval = new TimeSpan(1000);
            timer2.Tick += Timer2_Tick;
            timer2.Start();

            var allProcess = Process.GetProcesses();

            AllProcess = new ObservableCollection<string>();
            BlackBox = new ObservableCollection<string>();

            foreach (var process in allProcess)
            {
                AllProcess.Add(process.ProcessName);
            }

            allProcesslistbx.ItemsSource = AllProcess;

        }

        private void Timer2_Tick(object? sender, EventArgs e)
        {
            var allProcess = Process.GetProcesses();
            for (int i = 0; i < allProcess.Count(); i++)
            {
                for (int k = 0; k < BlackBox.Count; k++)
                {
                    if (allProcess[i].ProcessName == BlackBox[k])
                    {
                        allProcess[i].Kill();
                    }
                }
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            var allProcess = Process.GetProcesses();
            AllProcess = new ObservableCollection<string>();

            foreach (var process in allProcess)
            {
                AllProcess.Add(process.ProcessName);
            }
            allProcesslistbx.ItemsSource = AllProcess;
        }

        private void RunTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (taskTxtBx.Text != null)
            {
                Process.Start(taskTxtBx.Text);
            }
            else
            {
                MessageBox.Show("Write task name for open it for ex(notepad) !");
            }
        }

        private void endTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedProcess = allProcesslistbx.SelectedItem as string;
            if (selectedProcess != null)
            {
                var allProcess = Process.GetProcesses();
                foreach (var process in allProcess)
                {
                    if (process.ProcessName == selectedProcess)
                    {
                        process.Kill();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select task to end this task");
            }
        }

    }
}