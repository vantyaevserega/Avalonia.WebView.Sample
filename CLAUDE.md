# CLAUDE.md

Sample Avalonia desktop app that hosts a native WebView (`WebView.Avalonia`) and demonstrates two-way messaging between C# and JavaScript.

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
- There are no tests; verify changes by running the desktop app (the button switches from "Loading" to "Click me" once the JS -> host message round-trip works).

## Layout

- `Source/WebViewSample` — shared UI library (`net10.0`): `App`, `Views/MainView` (WebView + button), ReactiveUI view models.
- `Source/WebViewSample.Desktop` — desktop entry point; `Program.cs` calls `.UseDesktopWebView()` (WebView2 on Windows).
- `Source/WebViewSample.Android`, `Source/WebViewSample.iOS` — legacy mobile heads, **not in the solution** and not maintained.
- `Directory.Packages.props` — central package management (all versions live here).
- `Directory.Build.props`, `Build/Directory.build.common.*` — shared MSBuild settings (nullable warnings are errors).

## Version constraints

- `WebView.Avalonia` 11.0.0.1 is the last published version and uses Avalonia private APIs. Keep Avalonia on the **11.x** line; do not upgrade to 12.x without replacing the WebView library.
- `Avalonia.ReactiveUI` is versioned separately (`AvaloniaReactiveUIVersion`) because its 11.x line ends at 11.3.9.
- After any Avalonia bump, run the app: private-API breakage shows up only at runtime, not at compile time.
