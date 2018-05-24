## Responsive Tables

This extension changes the markup produced for tables to be compatible with [this](https://www.jeremytcd.com/articles/css-only-responsive-tables)
method for creating responsive tables. 

In particular, the contents of `<td>` elements are wrapped and `<td>` elements are assigned `data-label` attributes. For example using a pipe table:

```````````````````````````````` example
 a | b | c 
 - | - | -
 0 | 1 | 2
 3 | 4 | 5
.
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
<th>c</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label="a"><span>0</span></td>
<td data-label="b"><span>1</span></td>
<td data-label="c"><span>2</span></td>
</tr>
<tr>
<td data-label="a"><span>3</span></td>
<td data-label="b"><span>4</span></td>
<td data-label="c"><span>5</span></td>
</tr>
</tbody>
</table>
````````````````````````````````

Similarly, using a grid table:

```````````````````````````````` example
+---+---+---+
| a | b | c |
+===+===+===+
| 0 | 1 | 2 |
+---+---+---+
| 3 | 4 | 5 |
.
<table>
<col style="width:33.33%">
<col style="width:33.33%">
<col style="width:33.33%">
<thead>
<tr>
<th>a</th>
<th>b</th>
<th>c</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label="a"><span>0</span></td>
<td data-label="b"><span>1</span></td>
<td data-label="c"><span>2</span></td>
</tr>
<tr>
<td data-label="a"><span>3</span></td>
<td data-label="b"><span>4</span></td>
<td data-label="c"><span>5</span></td>
</tr>
</tbody>
</table>
````````````````````````````````

The contents of `<th>` elements are HTML escaped when used as values of `data-label` attributes:

```````````````````````````````` example
 "a" | &b&
 - | - 
 0 | 1 
 2 | 3 
.
<table>
<thead>
<tr>
<th>&quot;a&quot;</th>
<th>&amp;b&amp;</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label="&quot;a&quot;"><span>0</span></td>
<td data-label="&amp;b&amp;"><span>1</span></td>
</tr>
<tr>
<td data-label="&quot;a&quot;"><span>2</span></td>
<td data-label="&amp;b&amp;"><span>3</span></td>
</tr>
</tbody>
</table>
````````````````````````````````

HTML tags are removed from the contents of `<th>` elements when such contents are used as values of `data-label` attributes:

```````````````````````````````` example
+---+---+---+
| a | b | c |
|   |   |   |
| a |   |   |
+===+===+===+
| 0 | 1 | 2 |
+---+---+---+
| 3 | 4 | 5 |
.
<table>
<col style="width:33.33%">
<col style="width:33.33%">
<col style="width:33.33%">
<thead>
<tr>
<th><p>a</p>
<p>a</p>
</th>
<th>b</th>
<th>c</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label="aa"><span>0</span></td>
<td data-label="b"><span>1</span></td>
<td data-label="c"><span>2</span></td>
</tr>
<tr>
<td data-label="aa"><span>3</span></td>
<td data-label="b"><span>4</span></td>
<td data-label="c"><span>5</span></td>
</tr>
</tbody>
</table>
````````````````````````````````

The label attribute's name can be customized using `ResponsiveTablesExtensionOptions.DefaultResponsiveTableOptions.LabelAttributeName`:

```````````````````````````````` options
{
    "responsivetables": {
        "defaultResponsiveTableOptions": {
            "labelAttributeName": "custom-name"
        }
    }
}
```````````````````````````````` example
 a | b
 - | - 
 0 | 1 
 2 | 3 
.
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td custom-name="a"><span>0</span></td>
<td custom-name="b"><span>1</span></td>
</tr>
<tr>
<td custom-name="a"><span>2</span></td>
<td custom-name="b"><span>3</span></td>
</tr>
</tbody>
</table>
````````````````````````````````

To avoid rendering the label attribute, set `ResponsiveTablesExtensionOptions.DefaultResponsiveTableOptions.LabelAttributeName` to an empty string:

```````````````````````````````` options
{
    "responsivetables": {
        "defaultResponsiveTableOptions": {
            "labelAttributeName": ""
        }
    }
}
```````````````````````````````` example
 a | b
 - | - 
 0 | 1 
 2 | 3 
.
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td><span>0</span></td>
<td><span>1</span></td>
</tr>
<tr>
<td><span>2</span></td>
<td><span>3</span></td>
</tr>
</tbody>
</table>
````````````````````````````````

The `<td>` content wrapper element can be customized using `ResponsiveTablesExtensionOptions.DefaultResponsiveTableOptions.WrapperElementName`:

```````````````````````````````` options
{
    "responsivetables": {
        "defaultResponsiveTableOptions": {
            "wrapperElementName": "div"
        }
    }
}
```````````````````````````````` example
 a | b
 - | - 
 0 | 1 
 2 | 3 
.
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label="a"><div>0</div></td>
<td data-label="b"><div>1</div></td>
</tr>
<tr>
<td data-label="a"><div>2</div></td>
<td data-label="b"><div>3</div></td>
</tr>
</tbody>
</table>
````````````````````````````````

To avoid rendering wrapper elements, set `ResponsiveTablesExtensionOptions.DefaultResponsiveTableOptions.WrapperElementName` to an empty string:

```````````````````````````````` options
{
    "responsivetables": {
        "defaultResponsiveTableOptions": {
            "wrapperElementName": ""
        }
    }
}
```````````````````````````````` example
 a | b
 - | - 
 0 | 1 
 2 | 3 
.
<table>
<thead>
<tr>
<th>a</th>
<th>b</th>
</tr>
</thead>
<tbody>
<tr>
<td data-label="a">0</td>
<td data-label="b">1</td>
</tr>
<tr>
<td data-label="a">2</td>
<td data-label="b">3</td>
</tr>
</tbody>
</table>
````````````````````````````````