using Gma.System.MouseKeyHook;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace xyekot
{

    public partial class cat : Window
    {
        private Saver _save;
        private string skinPath = "skin.png";
        public cat()
        {
            InitializeComponent();
            _save = new();
            UpdateInterface();
            Subscribe();

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

        private void UpdateInterface()
        {
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

        public void Subscribe()
        {
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            m_GlobalHook.KeyUp += GlobalHookKeyPress;
        }

        private void GlobalHookKeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            _save.ClickKeyboard += 1;
            UpdateInterface();
        }

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            _save.ClickMouse += 1;
            UpdateInterface();
        }

        private void DrugCat_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) //pin
        {
            this.Topmost = !this.Topmost;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //close
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _save.ForceSave();
            Environment.Exit(0);
        }
    }
}
