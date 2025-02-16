
# Request Header Matching

<br />

> 🖥️ **Try it yourself**
> ```bash
> $ spoof scaffold -o example-header-matching
> ```

<br />

## Basic Matching

You can provide a list of headers, and their values, in the `Request` configuration, like this:

```json
"request": {
  "httpMethod": "GET",
  "path": "/api/example-header-matching",

  "headers": {
    "Accept": "application/json"
  }
},
```
<sup>(other parts of the JSON body have been hidden for clarity. See the scaffolded example for a complete file)</sup>

In the example above, the API will only return a response if the request contains an `Accept` header with the value `application/json`.
Any request without this header will return a HTTP 404 (Not Found)

<br />

You can provide mulitple headers (including custom headers), in which case _all_ must match:

```json
"request": {
  "httpMethod": "GET",
  "path": "/api/example-header-matching",

  "headers": {
    "Accept": "application/json",
    "x-api-key": "1234",
  }
},
```


<br />

---

➡️ Next: [Fixed Responses](fixed-responses.md)
