# Hot Reload

<br />

By default, Spoof will watch the file system and re-load all routes when a `*.route.json` is modified, added, or deleted.

This makes iterating on new mocks much faster.

```bash
$ spoof
# Hot reload enabled. Watching for changes to *.route.json files.
# Starting mock API server for 1 route on port 5050. CTRL+C to stop

When a file is changed:

# File change detected. Reloading routes.
# Loading routes from *.route.json files
# Loading route from file hello-world.route.json
# Validating 1 routes
# Routes reloaded.
```

## Turning hot-reload off

Sometimes you don't want hot-reload watching your every move.

To switch off hot-reload, pass the `-nr` or `--no-reload` option:

```bash
$ spoof --no-reload
```

<br />

---

➡️ Next: [Command Line Options](command-line-options.md)
