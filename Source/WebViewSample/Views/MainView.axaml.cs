using Avalonia.Controls;
using Avalonia.Interactivity;

namespace WebViewSample.Views;

public partial class MainView : UserControl
{
    // Installs a page-side handler for host -> JS messages and notifies the host that the page is ready.
    // invokeCSharpAction is injected into every page by Avalonia.Controls.WebView and raises WebMessageReceived.
    private const string BridgeScript = """
        window.onHostMessage = function (data) { alert('Clicks: ' + data); };
        invokeCSharpAction('Click me');
        """;

    private int cnt = 1;

    public MainView()
    {
        InitializeComponent();
        PART_Button.Click += PART_Button_Click;
        PART_WebView.NavigationCompleted += PART_WebView_NavigationCompleted;
        PART_WebView.WebMessageReceived += PART_WebView_WebMessageReceived;
    }

    private async void PART_WebView_NavigationCompleted(object? sender, WebViewNavigationCompletedEventArgs e)
    {
        if (!e.IsSuccess)
            return;

        try
        {
            await PART_WebView.InvokeScript(BridgeScript);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void PART_WebView_WebMessageReceived(object? sender, WebMessageReceivedEventArgs e)
    {
        PART_Button.Content = e.Body;
        PART_Button.IsEnabled = true;
    }

    private async void PART_Button_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            // setTimeout lets InvokeScript return immediately instead of waiting for the modal alert to be closed.
            await PART_WebView.InvokeScript($"setTimeout(function () {{ window.onHostMessage && window.onHostMessage({++cnt}); }});");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}
