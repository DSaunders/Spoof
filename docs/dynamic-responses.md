# Dynamic Responses
<br />

> 🖥️ **Try it yourself**
> ```bash
> $ spoof scaffold -o example-echo
> ```

<br />

> [!IMPORTANT]
> More token replacements are coming, so this document is a work in progress.  
> Keep an eye on this page for more enhancements.

<br />

## Token Replacement

You can include sections of the request in your response, by using `{{tokens}}` that are replaced in each response.

Here are some examples:

<br />

### `{{request.body}}`

The following response will contain the entire body of the request.

```json
"response": {
  "statusCode": 200,
  "body": {
    "originalRequest": "{{request.body}}"
  }
}
```

If the request body is a plain text string, it will be returned in quotes, like this:

```bash
$ curl -s http://localhost:5050/api/echo --request POST --data "Hi!"

# Response
{
  "originalRequest": "Hi!"
}
```

If the original request body JSON, the JSON will be inserted in the correct place and re-formatted:

```bash
$ curl -s http://localhost:5050/api/echo --request POST --data "{ 'hello': 'world' }"

# Response
{
  "originalRequest": {
    "hello": "world"
  }
}
```

<br />

The response can also be a string, in which case the request body will be inserted in the correct place.

If the request body is JSON, it will be 'stringified'.


```json
"response": {
  "statusCode": 200,
  "body": "You sent us: {{request.body}}"
}
```

```bash
$ curl -s http://localhost:5050/api/echo --request POST --data "{ 'hello': 'world' }"
"You sent us: { 'hello': 'world' }"

$ curl -s http://localhost:5050/api/echo --request POST --data "{ 'hello': 'world' }"
"You sent us: Hi!"
```

<br />

---

➡️ Next: [Scaffolding](scaffolding.md)
