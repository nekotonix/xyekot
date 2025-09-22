using Gma.System.MouseKeyHook;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace xyekot
{

    public partial class cat : Window
    {
        private CatSave _save;
        private string skinPath = "skin.png";
        private string savePath = "save.txt";
        private int saveDelayMs = 1000*60*2; //2 mins


        public cat()
        {
            _save = new(savePath);
            this.Left = _save.LastPosX;
            this.Top = _save.LastPosY;
            InitializeComponent();

            UpdateInterface();
            Subscribe();

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(saveDelayMs);
                    if (!isDaily)
                        _save.WriteSave();
                }
            });


            if (File.Exists(skinPath))
            {
                string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, skinPath);
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));

                this.DrugCat.Source = bitmapImage;
            }

            deform = maxCatHeight - _save.Deform;
        }

        bool Drug = false;
        int maxCatHeight = 123;
        int deform = 116;
        bool isDaily = false;

        private void UpdateInterface()
        {
            this.Clicks.Content = _save.ClickMouse.ToString();
            this.Clicks.Content = _save.ClickMouse.ToString();
            this.Keys.Content = _save.ClickKeyboard.ToString();
            this.Total.Content = (_save.ClickMouse + _save.ClickKeyboard).ToString();
            Drug = !Drug;
            if (Drug)
            {
                this.DrugCat.Height = maxCatHeight;
            }
            else
            {
                this.DrugCat.Height = deform;
            }
        }

        private IKeyboardMouseEvents m_GlobalHook;
        bool subscribed = false;
        public void Subscribe()
        {
            m_GlobalHook = Hook.GlobalEvents();

            //m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            m_GlobalHook.KeyUp += GlobalHookKeyPress;
            m_GlobalHook.MouseDown += GlobalHookMouseDown;
            subscribed = true;
        }
        //laggy without it on close
        public void UnSubscribe()
        {
            if (subscribed)
            {
                m_GlobalHook = Hook.GlobalEvents();

                m_GlobalHook.KeyUp -= GlobalHookKeyPress;
                m_GlobalHook.MouseDown -= GlobalHookMouseDown;
            }
            subscribed = false;
        }

        private void GlobalHookMouseDown(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            _save.ClickMouse += 1;
            UpdateInterface();
        }

        private void GlobalHookKeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            _save.ClickKeyboard += 1;
            UpdateInterface();
        }

        private void GlobalHookMouseDown(object sender, MouseEventExtArgs e)
        {
            _save.ClickMouse += 1;
            UpdateInterface();
        }

        private void DrugCat_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
                _save.LastPosX = this.Left;
                _save.LastPosY = this.Top;
            }
        }

        private void Pin_Click(object sender, RoutedEventArgs e) //pin
        {
            this.Topmost = !this.Topmost;
            if (Topmost)
                this.pinBtn.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF,0xFF,0xDE,0x00));
            else
                this.pinBtn.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF,0x73,0x64,0x00));

        }

        private void Close_Click(object sender, RoutedEventArgs e) //close
        {
            this.Close();
        }
        private void Daily_Click(object sender, RoutedEventArgs e) //close
        {
            isDaily = !isDaily;
            if (isDaily)
            {
                this.toggleDailyBtn.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x00, 0x1B, 0x4E));
                _save.ClickMouse = 0;
                _save.ClickKeyboard = 0;
            }
            else
            {
                _save.ReadSave();
                this.toggleDailyBtn.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x00, 0x3E, 0xD5));
            }
            UpdateInterface();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UnSubscribe();
            if(!isDaily)
                _save.WriteSave();
            Environment.Exit(0);
        }
    }
}
