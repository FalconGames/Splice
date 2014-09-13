The Splice Programming Language Specification:
==============================================

Note:
-----
This document will not be a formal specification for the time being. If the language ever develops past infancy and becomes
something other than an experimental toy language, this document will be edited to reflect that. Until that point, this will
likely remain in the same format it is now in.
	
The Basic Syntax:
-----------------
The basic syntax of Splice is similar to what is found in other C-derived languages. Like Java and C#, Splice uses a familiar
syntax. Without further ado, here is the specification for variables:
	
### Variables:
All variables in Splice follow the format used in C. This means that they are declared in the following notation:

`<type> <name> = <value>;`

An example of this would be the following:

`int i = 0;`

### Functions:
While not widely used in Splice, functions still exist, so as to provide some familiarity for new users. As a user
progresses through the stages of learning Splice, they will begin to use functions less and less. Nevertheless,
functions are included in the language, and are used as such:

Definition:

`<return type> <name>(<arguments>[, <arguments]*) { [...] }`

For example:
```
int squareNumber(int num) {
	return num * num;
}
```

As can be seen in the previous declaration, return values and arithmetic are identical to those in C and other
related languages.

Calling a function:

`<name>(<arguments>[, <arguments>]*);`

For example:

`squareNumber(5);`

As you can see, this behaves in the same way as you are likely used to it behaving. For those unfamiliar with C
or other related languages, this can be used in other statements. Here are some examples:
	
`int i = squareNumber(10);`

`squareNumber(squareNumber(2));`
		
### Basic Flow-Control Logic Statements:
Splice features the same basic flow-control logic statements as C, C++, C#, and Java. An example of each will be
shown in the following, as well as an explanation:

* The `if()` statement:
The `if()` statement is the most simple statement of the following. It is used to compare values. The basic
syntax is this:

`if (<firstVariable> <operator> <seccondVariable> [[&&/||] <firstVariable> <operator> <secondVariable>]*) { ... }`

While other types of comparisons exist, this is the simplest format. Functions can also be tested, and certain
statements can be implied (such as anything which resolves to a boolean bit). Here is an example:

```
if (i == 25) {
	print("i is equal to 25");
}
```

In addition to this basic format, if statements can conditionally branch the program by using the else keyword.
This keyword is used for any other comparison that should occur if, and only if, the first did not occur. The
else statement can both be used with its own block, and with a conjoined if statmement. Here is an example:

```
if (i == 25) {
	print("i is equal to 25");
} else if (i == 100) {
	print("i is equal to 100");
} else {
	print("i is not equal to 25 or 100");
}
```

New Syntax:
-----------

### Nodes:
Nodes are the largest feature added in Splice. While they pose no real advantage over functions and methods in other languages,
they do have some interesting properties that help in certain cases, such as multi-threading, which is much easier using the
freeze and melt syntax in combination with node forking and joining. Another feature is the resolution to an object once a node
has completed, which allows for the preservation of all results of a function very easily. Nodes are partially inspired by the
pipe operator in UNIX shells.

### Modules:
Modules are a way of grouping your code together to maximize compatibility on different systems. Modules are selected from a
master record of all available modules. Modules with the same name are resolved to the one with the highest presedence, unless
the `#prec <module>/<identifier> <number>` preprocessor directive or command line arguments are used to overwrite the default
precedence. Modules also allow for multiple versions to be installed on the system, with the newest selected by default and
others available through version selection while including the module.

All code can optionally be declared in modules. While not required in any way, modules help organize code. While modules can
only encapsulate code, other files can be included to modules as well. The following is an example of the basic syntax for
declaring new modules:

```
module <name> {
	// Code
}
```

This is only the most basic version of modules. Modules can be declared with versions and can be declared to replace, or extend,
other modules. Replacing makes it an alternative library, meaning that if it is either the only available option or if it is
rated higher in the precedence options it will replace the other library (NOTE: this requires the module to have the same
identifiers for functions, variables, nodes, and objects for them to be used in place of another compatible library). Extending
merely adds new functions onto a basic library. Below is an example of this:

```
module math.ext [1.0.0] extend lang.math [stable] replace splicex.math [1.5.1] {
	// Code
}
```

To import a module to the project, you must first make sure it is either in the working directory as code, in the lib sub-
directory compiled to a native library format, or in the /etc/.splice/lib/ (or OS equivalent). It is highly recommended that
you distribute libraries by source, as this not only simplifies integration, but supports open-source software, and allows for
the benefits of free software, such as the ability of the user to fix bugs and add new features into the library and greatly
simplifies the debugging process. The following is the format for importing modules:

`using module <moduleName>;`

Some important things to note are: the `using module` statement includes the the module into the entire file if it is at the
root scope, but stays within any module or other code unit (and shares to all other parts of that code unit); it only includes
the highest precendence with its compatible extensions of the highest precedence (if the `!` operator is used before the module
name, it forces the use of the official module (unless none is found) and if it is used after, it forces the use of the module
without extensions (unless explicitly included)); and the version can be forced using the brackets afterwards (`[1.3.2]`).

### Freezing/Melting:
Of all of the features added, these are the simplest. Essentially, freezing (`expr ice = {{doLongProcess()}};`) allows
the use of expression variables that store an expression that hasn't been run yet. Melting (`[[ice]];`), or running the frozen
expression, allows for you to release the previous expression and run it at the correct time.


### Soup:
The final major feature added in Splice is the soup type. The soup type is an optionally-indexed, variable-length, dynamically-
typed heap, that is used to store large collections of arbitrary data. The soup type can also be extended by "ignores", which
can be used to limit what types it may or may not hold, and other features that allow for it to take the shape of any data
container needed with relative ease. The basic format for a soup variable is the following:

`soup <name> = ~[]~;`

The brackets can be used for options and can have data entered to them. Below is an example of an int dictionary with a size
limit of 20:

`soup intDict20 = ~[ignore {* ? !pair(string, int)}, index {string index, int value}, limit {20}]~;`

Data can be added to a soup like so:

`<someData> ~> <someSoup>;`

For example, when adding to the previously created soup, one would use:

`{'x', 5} ~> intDict20;`

If the soup rejects the data, it can be forwarded like so:

`{'x', 5} ~> intDict20 !~> (soup waste = ~[]~);`

Soups can also be used to buffer. When a space opens, they can get passed on further. Here is an example:

```
soup topPosts = ~[ignore {* ? !Post}, limit {5}]~; // Create a soup of the top 5 posts
soup posts = ~[ignore {* ? !Post}]~; // Create a buffer of posts

when({{requests => isPost}}, {{$_.post ~> posts >> topPosts}}); // Use when to listen for new posts and add them. >> is buffer.
```

One final use of soup is as an anonymous object. Here is an example:

```
soup anon = ~[index {obj}]~;
{'x', 0} ~> anon;
{'y', 0} ~> anon;
{'z', 0} ~> anon;
```
