using System.Windows;

namespace RpTimerDemo;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is IDisposable disposable)
            disposable.Dispose();
    }
}
