# Basic Configuration

The minimum you need for a working mock endpoint is a `.route.json` file containing the following:

```json
{
  "request": {
    "httpMethod": "GET",
    "path": "/api/hello"
  },
  
  "response": {
    "body": "Hello, World!"
  }
}
```

Let's look at each parameter and talk about some more advanced mocking abilities.

<br />

## Request

```json
"request": {
  "httpMethod": "GET",
  "path": "/api/hello"
},
```

The `httpMethod` specifies which HTTP verb the mock will respond to (`GET`, `PUT`, `POST`, etc.).

The `path` is the location of this endpoint from the root. 

For example, the above route will respond to a `GET` to `http://localhost:5050/api/hello` (assuming you are using the default port of `:5050`).

<br />

## Response

This is a simple example of a Response configuration:

```json
"response": {
  "statusCode": 200,
  "body": {    
    "hello": "world"
  }
}
```

This API will respond to the request with a `HTTP 200 (OK)` response code.

The response will have a `Content-Type` header set to `application/json`, and the body will be the following JSON object:

```json
{    
  "hello": "world"
}
  ```

<br />

The `body` property of the Response can either be a string:

```json
"response": {  
  "body": "Hello, World!"
}
```

.. or JSON:

```json
"response": {  
  "body": {
    "hello": "world"
  }
}
```

<br />

---

➡️ Next: [Advanced Path Matching](path-matching.md)
