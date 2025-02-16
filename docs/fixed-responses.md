# Fixed Responses
<br />

> 🖥️ **Try it yourself**
> ```bash
> $ spoof scaffold -o hello-world
> ```

<br />

## Text

The simplest response is a single string, like this:

```json
"response": {
  "statusCode": 200,
  "body": "Hello, World!"
}
```
<sup>(other parts of the JSON body have been hidden for clarity. See the scaffolded example for a complete file)</sup>

<br />

## JSON

You can also provide JSON in the `body` property, instead of a string:

```json
"response": {
  "statusCode": 200,
  "body": {
     "message": "This is a JSON response!"
  }
}
```

<br />

## Response Headers

Set the headers on the response like this:

```json
"response": {
  "statusCode": 200,

  "headers": {
    "Content-Type": "text/plain",
    "x-custom-header": "custom-value"
  },

  "body": "Hello, World!"
}
```

> Note: Response headers do not affect the format of the response.
>
> For example, it is entirely possible to have Spoof return a plain text body and a 'Content-Type' header of 'application/json'.



<br />

---

➡️ Next: [Dynamic Responses](dynamic-responses.md)
