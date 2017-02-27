using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Logmaku
{
    /// <summary>
    ///     TextWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TextWindow
    {
        public static readonly DependencyProperty FontBrushProperty = DependencyProperty.Register(
            "FontBrush", typeof(Brush), typeof(TextWindow), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty OutlineBrushProperty = DependencyProperty.Register(
            "OutlineBrush", typeof(Brush), typeof(TextWindow), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
            "Duration", typeof(Duration), typeof(TextWindow), new PropertyMetadata(default(Duration)));

        public TextWindow()
        {
            InitializeComponent();
            ContentRendered += (sender, args) =>
            {
                var fuck = new DoubleAnimation(Left, -ActualWidth, Duration);
                fuck.Completed += (s, e) => Dispatcher.Invoke(Close);
                BeginAnimation(LeftProperty, fuck);
            };
        }

        public Brush FontBrush
        {
            get { return (Brush) GetValue(FontBrushProperty); }
            set { SetValue(FontBrushProperty, value); }
        }

        public Brush OutlineBrush
        {
            get { return (Brush) GetValue(OutlineBrushProperty); }
            set { SetValue(OutlineBrushProperty, value); }
        }

        public Duration Duration
        {
            get { return (Duration) GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
    }
}