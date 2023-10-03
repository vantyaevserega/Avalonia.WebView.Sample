using Avalonia.Controls;

namespace WebViewSample.Views;
public partial class MainView : UserControl
{
    private int cnt = 1;

    public MainView()
    {
        InitializeComponent();
        PART_Button.Click += PART_Button_Click;
        PART_WebView.NavigationCompleted += PART_WebView_NavigationCompleted;
        PART_WebView.WebMessageReceived += PART_WebView_WebMessageReceived;
    }

    private void PART_WebView_NavigationCompleted(object? sender, WebViewCore.Events.WebViewUrlLoadedEventArg e)
    {
        try
        {
            PART_WebView.PlatformWebView?.ExecuteScriptAsync("window.chrome.webview.addEventListener('message', function(e) { alert('Clicks: ' + e.data); });");
            PART_WebView.PlatformWebView?.ExecuteScriptAsync("window.chrome.webview.postMessage(\"Click me\");");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void PART_WebView_WebMessageReceived(object? sender, WebViewCore.Events.WebViewMessageReceivedEventArgs e)
    {
        PART_Button.Content = e.Message;
        PART_Button.IsEnabled = true;
    }

    private void PART_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => PART_WebView.PostWebMessageAsString((++cnt).ToString(), PART_WebView.Url);
}