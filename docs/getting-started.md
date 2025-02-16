
# Getting Started

Spoof is an API mocking tool where all mocks are stored in `.route.json` files. 
These can be checked in to source control to be used by other developers and in CI pipelines.

<br />

### 1. Download Spoof

Spoof is a single executable. Download the latest version from the 'Releases' page.

(consider adding the executable to your `$PATH` so you can run it from anywhere)

<br />

### 2. Creating a new endpoint

On start-up, Spoof will recursively search the working folder and sub-folders for `*.route.json` files.

Each file describes a mock API endpoint that will be available when Spoof is running.

You can create these files manually, or start with a template.

This command shows all available templates:

```bash
$ spoof scaffold list
```

Let's use one of them to create our first mock:

```bash
$ spoof scaffold hello-world -o
```

A new route will be created in the current working directory, with the name `hello-world.route.json`.

The `-o` option tells Spoof to automatically open the new `.json` file in your text editor so you can edit it.

We'll discuss the contents of this file later, so let's close it try it out.

<br />

### 3. Start the Mock API Server

```bash
$ ./spoof

## You should see the following:
> Loading routes from *.route.json files
> Loading route from file hello-world.route.json
> Validating 1 routes
> Starting mock API server for 1 route on port 5050. CTRL+C to stop.
```

Now open a browser and navigate to http://localhost:5050/api/hello.

Spoof will reply with 'Hello, World!'.

<br />

---

➡️ Next: [Basic Configuration](basic-configuration.md)️
