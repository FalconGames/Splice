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
		
Basic Flow-Control Logic Statements:
------------------------------------
Splice features the same basic flow-control logic statements as C, C++, C#, and Java. An example of each will be
shown in the following, as well as an explanation:
-The `if()` statement:
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
