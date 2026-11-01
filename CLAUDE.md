# CLAUDE.md

Sample Avalonia desktop app that hosts a native WebView (`NativeWebView` from the official `Avalonia.Controls.WebView` package) and demonstrates two-way messaging between C# and JavaScript.

## Workflow rules

- Never commit to `main` directly. All changes go through a feature branch and a GitHub pull request (`gh pr create`).
- Never add Claude as a co-author: no `Co-Authored-By` trailers in commits and no "Generated with Claude Code" line in PR descriptions.

## Build & run

```sh
dotnet build Avalonia.WebView.Sample.sln
dotnet run --project Source/WebViewSample.Desktop
```

- SDK is pinned in `global.json` (.NET 10, `rollForward: latestFeature`).
- Build output goes to `Binary/<tfm>/`, intermediates to `.vs/<solution>/Intermediate/` (see `Directory.Build.props`), not to `bin/obj`.
- `nuget.config` clears machine-wide sources and uses only nuget.org.
- There are no tests; verify changes by running the desktop app: the button switches from "Loading" to "Click me" (JS -> host works), and clicking it shows an alert "Clicks: N" (host -> JS works). UI Automation `Invoke` may not trigger the click reliably; use a real mouse click.

## Layout

- `Source/WebViewSample` — shared UI library (`net10.0`): `App`, `Views/MainView` (WebView + button), ReactiveUI view models.
- `Source/WebViewSample.Desktop` — desktop entry point (`Program.cs`); no WebView-specific setup is needed, the platform adapter (WebView2 on Windows) is picked automatically.
- `Directory.Packages.props` — central package management (all versions live here).
- `Directory.Build.props`, `Build/Directory.build.common.*` — shared MSBuild settings (nullable warnings are errors).

## WebView bridge

- JS -> host: the page calls the global `invokeCSharpAction(data)` (injected by `Avalonia.Controls.WebView`), which raises `NativeWebView.WebMessageReceived` (`e.Body`).
- Host -> JS: there is no `PostWebMessage` API; call a page function via `NativeWebView.InvokeScript(...)`. Wrap calls that may open modal dialogs (`alert`) in `setTimeout`, otherwise `InvokeScript` does not complete until the dialog is closed.

## Version constraints

- Avalonia 12.x. `Avalonia.Controls.WebView` is released separately from Avalonia, so its version differs (`12.1.1` vs Avalonia `12.1.4`).
- ReactiveUI integration is `ReactiveUI.Avalonia` (the successor of `Avalonia.ReactiveUI`); `UseReactiveUI` requires a builder callback. Its `14.x` version is an older line for Avalonia 11, not a newer one.
- `Avalonia.Diagnostics` is not published for Avalonia 12 (DevTools became a separate product).
