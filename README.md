# Spoof

A config-driven mock API server in your terminal.

Create and host a new mock API in 2 commands:

```bash
$ ./spoof scaffold hello-world
#  Created new mock file: hello-world.route.json

$ ./spoof
#  Starting mock API server for 1 route on port 5050
```

```bash
$ curl -s http://localhost:5050/api/hello
> Hello, World!
```

<br />

All mocks in Spoof are defined in `*.route.json` files, so you can check them in to source control to share with other developers or use in CI pipelines.


(check out [this](src/Runner/_routes/hello.route.json) bare-bones example)

<br />

## Features

- [Scaffolding](docs/scaffolding.md) - Create new Mock APIs from templates
- [Hot Reload](docs/hot-reload.md) - Mocks APIs are updated 'live' as you edit them
- [Parametrised route matching](docs/path-matching.md)
- [Header matching](docs/header-matching.md)
- [Fixed responses](docs/fixed-responses.md)
- [Dynamic responses](docs/dynamic-responses.md) - Use token replacement to modify the response

<br />

## Getting Started

Follow [this guide](docs/getting-started.md) to create your first mock, then explore the features above!
