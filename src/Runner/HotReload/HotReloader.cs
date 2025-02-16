using System.Diagnostics.CodeAnalysis;
using Timer = System.Timers.Timer;

namespace Runner.HotReload;

[ExcludeFromCodeCoverage]
public class HotReloader
{
    private readonly FileSystemWatcher _watcher;
    private const int DebounceInterval = 400;
    private readonly Timer _debounceTimer;

    public HotReloader(string path, Action filesChanged)
    {
        _watcher = new FileSystemWatcher();
        _watcher.Path = path;
        _watcher.IncludeSubdirectories = true;
        _watcher.NotifyFilter = NotifyFilters.LastAccess |
                                NotifyFilters.LastWrite |
                                NotifyFilters.FileName |
                                NotifyFilters.DirectoryName;

        _watcher.Changed += OnChanged;
        _watcher.Created += OnChanged;
        _watcher.Deleted += OnChanged;
        _watcher.Renamed += OnChanged;

        _watcher.Filter = "*.route.json";
        _watcher.EnableRaisingEvents = true;

        _debounceTimer = new Timer(DebounceInterval);
        _debounceTimer.Elapsed += (_, _) => filesChanged();
        
        _debounceTimer.AutoReset = false;
    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
        _debounceTimer.Stop();
        _debounceTimer.Start();
    }
}