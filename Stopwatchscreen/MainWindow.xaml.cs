using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20Min
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SolidColorBrush _red = new SolidColorBrush(Colors.DarkRed);
        private readonly SolidColorBrush _black = new SolidColorBrush(Colors.Black);
        private System.Drawing.Point _lm;
        private bool _clicado = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var ttla = new TimeToLookAway();
            var ctrLeft = this.Left;
            var ctrTop = this.Top;
            ttla.LookAway += (o, args) =>
            {
                this.Dispatcher.Invoke(
                    async () =>
                    {
                        this.Activate();
                        this.Topmost = true;

                        this.Left = SystemParameters.PrimaryScreenWidth / 2 - Width / 2;
                        this.Top =  SystemParameters.PrimaryScreenHeight / 2 - Height / 2;

                        TextBlock.Foreground = _red;
                        TextBlock.FontSize += TextBlock.FontSize;

                        await Task.Delay(20000).ContinueWith(t =>
                        {
                            this.Dispatcher.Invoke(
                                () =>
                                {
                                    this.Topmost = false;
                                    TextBlock.Foreground = _black;
                                    TextBlock.FontSize = TextBlock.FontSize/2;
                                });
                        });
                    }
                    );
            };

            this.DataContext = ttla;
        }

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(
            IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y,
            int cx, int cy,
            uint uFlags);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        static void SendWpfWindowBack(Window window)
        {
            var hWnd = new WindowInteropHelper(window).Handle;
            SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_clicado)
            {
                this.Left += (System.Windows.Forms.Control.MousePosition.X - this._lm.X);
                this.Top += (System.Windows.Forms.Control.MousePosition.Y - this._lm.Y);
                this._lm = System.Windows.Forms.Control.MousePosition;
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            _clicado = false;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            CaptureMouse();
            _lm = System.Windows.Forms.Control.MousePosition;
            _clicado = true;
        }
    }
}