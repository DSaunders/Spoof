
# Scaffolding

To make creating new APIs easier, Spoof ships with pre-build templates for building new APIs.

To show these templates:

```bash
$ spoof scaffold list

# The following scaffolding templates are available:
# 
#   Template Name           | Description
#   ------------------------|------------------------------------------------------------------------------
#   hello-world             | Responds to all GET /api/hello requests with a greeting
#   example-route-matching  | An example of more complex route matching
#   example-header-matching | An example of requests that only match when the correct headers are provided
#   example-echo            | An example of a response that contains values from the request
# 
# For example:
#   'spoof scaffold hello-world'
```

<br />

> [!TIP]
> More templates are added frequently. Check `spoof scaffold list` in each new release.

<br />

## Using a template

To create a new API from the template, pass the template to the `spoof scaffold` command:

```bash
$ spoof scaffold hello-world
# Created new mock file: hello-world.route.json
```

<br />

## Command line options

#### Automatically open the new route for editing

Including the `-o` parameter will automatically open the new `.route.json` file in the associated text editor after creation.

```bash
$ spoof scaffold hello-world -o
# Created new mock file: hello-world.route.json
# Opening hello-world.route.json in 2 seconds. CTRL+C to cancel
```

#### Provide a custom route file name

By default, the new `.route.json` file will be created using the name of the template.

You can provide an alternative name as a second parameter:

```bash
$ spoof scaffold hello-world my-api
# Created new mock file: my-api.route.json
```
<br />

---

➡️ Next: [Hot Reload](hot-reload.md)
