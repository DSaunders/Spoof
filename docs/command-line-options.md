# Command Line Options

<br />

### Help

Lists the commands shown on this page:

```bash
$ spoof -h
```

### Change the port used to host Spoof

Spoof runs on port `:5050` by default, to change this:

```bash
$ spoof -p 5000
$ spoof --port 5000
```

### Quiet Mode

Reduces logging (for example, in CI environments where it is less useful)

```bash
$ spoof -q
$ spoof --quiet
```

### Disable hot-reload

Spoof watches the file system for changes to mocks, and reloads then automatically.

To disable this:

```bash
$ spoof -nr
$ spoof --no-reload
```

### Developer debug mode

Spoof supresses logs shown by the .NET web host (Kestral). To enable these for debugging:

```bash
$ spoof -i
$ spoof --debug-internals
```

### Show the Spoof version

To display the currently installed version of spoof:

```bash
$ spoof --version
```
