
# Advanced Route Matching

<br />

> 🖥️ **Try it yourself**
> ```bash
> $ spoof scaffold -o example-route-matching
> ```

<br />

## Fixed routes

Mock API URLs can be fixed, like this:

```json
"request": {   
  "path": "/api/hello"
}
```
<sup>(other parts of the JSON body have been hidden for clarity. See the scaffolded example for a complete file)</sup>

<br />

## Parameterised routes

You can also include parameters in routes, like this:

```json
"request": {   
  "path": "/api/{category:int}/products/{productName}"
}
```

The parts of the route surrounded by `{ }` will match any value, as long as the type is correct.

You can expicitly match `int` or `uuid`. If you do not specify a type, _any_ value will be matched.

<br />

Here are some examples:

| Route  | Will match | Will _not_ match |
| ------------- | ------------- |----|
| /api/{<span>$\textsf{\color{#0550ae}{name}}$</span>}/products  | /api/<span>$\textsf{\color{#4ac260}{foo}}$</span>/products <br /> /api/<span>$\textsf{\color{#4ac260}{12345}}$</span>/products,  | |
| /api/{<span>$\textsf{\color{#0550ae}{category:int}}$</span>}/products | /api/<span>$\textsf{\color{#4ac260}{123}}$</span>/products | /api/<span>$\textsf{\color{#d15a5a}{foo}}$</span>/products  |
| /api/{<span>$\textsf{\color{#0550ae}{category:uuid}}$</span>}/products | /api/<span>$\textsf{\color{#4ac260}{832015d8-faaf-4bdc-8e4c-f2a410ba0028}}$</span>/products | /api/<span>$\textsf{\color{#d15a5a}{foo}}$</span>/products <br /> /api/<span>$\textsf{\color{#d15a5a}{66}}$</span>/products  |



<br />

---

➡️ Next: [Request Header Matching](header-matching.md)
