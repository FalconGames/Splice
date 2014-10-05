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
```C#
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

```C#
if (i == 25) {
	print("i is equal to 25");
}
```

In addition to this basic format, if statements can conditionally branch the program by using the else keyword.
This keyword is used for any other comparison that should occur if, and only if, the first did not occur. The
else statement can both be used with its own block, and with a conjoined if statmement. Here is an example:

```C#
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

### Captures:
Splice takes the idea of C++'s lambda captures and further expands it. A quick refresher of the C++ lambda syntax
in case you don't know what I'm talking about:

```C++
int x = 5;
auto squareLambda = [&x]() {
	return x * x;
}
```

The brackets in the expression signify captures. Since lambdas don't inherit the scope of their parent block,
you must use captures to 'grab' things from the parent scope. In Splice, however, this mechanism can be employed
outside of just lambdas. The capture can be added to any block declaration and can be excluded. When you add it in
you are limiting the scope and only inheriting the items listed in the brackets. While this may seem counterintuitive, it keeps you from having to think about variable scope in lots of places. Consider the following:

```C++
class ScopeExample {
public:
	int x, y, z;	
	
	ScopeExample(int x, int y, int z) {
		this->x = x;
		this->y = y;
		this->z = z;
	}
};
```

As you can see, the scope becomes ambiguous, so you must use the `this` pointer, which is often inconvenient to use and can be forgetten quite easily. Obviously there are ways around this: many programmers use the `m_` prefix to show that a variable is a member of an object. But then this creates
an issue: rather than favoring the names that are accessed by things outside of the object, you are favoring the
internals of it. Instead, this could be fixed with captures:

```C++
class ScopeExample {
	int x, y, z;
	
	ScopeExample(int x, int y, int z) [x as _x, y as _y, z as _z] {
		_x = x;
		_y = y;
		_z = z;
	}
};
```

As you can see, everything viewable externally is favored. But that can still be done by using `_` as a prefix for the constructor's parameters, right? Well, yes to some extent, but this method sort of falls apart when using named arguments.

(This idea is based off of Jonathan Blow's *Ideas For A New Programming Language*. You can find part one [here](https://www.youtube.com/watch?v=TH9VCN6UkyQ) and part two [here](https://www.youtube.com/watch?v=5Nc68IdNKdg)).

### Nodes:
Nodes are the largest feature added in Splice. While they pose no real advantage over functions and methods in other languages,
they do have some interesting properties that help in certain cases, such as multi-threading, which is much easier using the
freeze and melt syntax in combination with node forking and joining. Another feature is the resolution to an object once a node
has completed, which allows for the preservation of all results of a function very easily. Nodes are partially inspired by the
pipe operator in UNIX shells.

Nodes are defined like so:

```C#
node <name> {
	=>[<in>];
	<=[<out>];
	
	// Main code
	
	return [<out>];
}
```

For example: here is a comparison of a standard C-style fucntion definition and a Splice node definition:

C function                  | Splice node
----------------------------|----------------------
`int squareNumber(int n) {` | `node squareNumber {`
	`return n*n;`			| `	=>[int n];`
`}`							| `	<=[int n];`
							| `	return n*n;`
							| `}`
							
Now for a comparison of calling each type:

C function					| Splice node
----------------------------|-----------------------------
`int n = squareNumber(5);`	| `int n = 5 => squareNumber;`

#### But *WHY* use nodes?

It's easy to see that nodes are not suitable for everything. Nodes take more code to define and use, and
they are a feature that new Splice programmers won't immediately get, so why introduce them?

* Nodes are an alternative syntax to functions that are easier to visualize (an example of this will be shown later).
* Nodes make more sense when planning a program's control flow.
* Nodes keep code grouped better.
* Nodes are a better way of managing concurrency.

Hold on... let's look at that last point again:

>Nodes are a better way of managing concurrecny.

For this to make sense, I'll have to introduce some new concepts related to nodes and concurrency.

* `<` - the fork operator, used to fork the current thread into another thread. It can be used like this:

`Thread *nodeBThread = this.thread < => b; // Fork into b, then continue this code path`

* `>` - the join/merge operator (more about this will be explained later)

`nodeBThread > => this.thread;`

* `handle` - Handles 'messages' from other threads (similar to events.) The join operator calls the `MERGE` handler.

* `inform` - Send a message to another thread.

* `try inform` - Only inform the thread if it is not already busy handling the event specified.

Here is a complete example:

```C++
void main() {
	game; // Run the game node
}

node game {
	=>[];
	<=[];

	// Exclamation shows pointer ownership
	Sound *!sfx = loadSound("../res/sounds/sfx_clip.ogg");
	int volume = 100;
	Thread *soundPlayer = this.thread < => playsound;

	bool running = true;

	void run() {
		while (running) {
			try inform soundPlayer PLAY; // Inform soundPlayer to PLAY only if it isn't already playing (PLAY is running)

			// ~50% chance (without algorithm weight & floating point inaccuracies, at least)
			if (rand() => 0.5f) {
				inform soundPlayer PAUSE;
				wait(5); // Wait 5ms
				inform soundPlayer RESUME;
			}

			volume = rand() * 100;
			inform soundPlayer SET_VOLUME;
		}
		inform soundPlayer PAUSE;
	}

	handle PAUSE {
		running = false;
		inform soundPlayer PAUSE;
	}

	handle RESUME {
		running = true;
		run();
	}

	handle CLEANUP {
		playSound > => this.thread;
		delete sfx;
	}
}

node playsound [&sfx, &volume] {
	=>[Thread *parent];
	<=[];

	sfx.setVolume(volume);

	handle PLAY {
		sfx.play();
	}

	handle PAUSE {
		sfx.pause();
	}

	// Captures as inputs for handlers
	handle SET_VOLUME [&volume] {
		sfx.setVolume(volume);
	}
		
	handle MERGE {
		sfx.pause();
		inform parent CLEANUP;
		return;
	}
		
}
```

As you can see, this provides an easy way for threads to be forked and merged, as well as a good way for communicating between threads and sharing data between threads (through use of captures).

**NOTE**: `this.thread` also provides a way to use functions as threads. This can be done like so:

```C++
void main() {
	// The fork method branches off into a new function declared as a lambda

	Thread *forkedThread = this.thread.fork(void () {
		// Do code here
	}); // Starts automatically because no bool given
		
	Thread *yetAnotherForkedThread = this.thread.fork(void () {
		// Do more code here
	}, false); // Bool means start when initialized

	inform yetAnotherForkedThread START; // It still functions as a node, though a public inner function could be used (Thread::start())
	wait(5); // Wait 5 ms
	yetAnotherForkedThread > => this.thread;
	forkedThread > => this.thread;
}
```

### Modules:
Modules are a way of grouping your code together to maximize compatibility on different systems. Modules are selected from a
master record of all available modules. All code can optionally be declared in modules. While not required in any way, modules
help organize code. While modules can only encapsulate code, other files can be included to modules as well. The following is
an example of the basic syntax for declaring new modules:

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

Including additional files into a module does not follow a very specific set of rules, but certain practices are encouraged.
It is recommended that all other files included be place in a directory hierarchy mimicking the module name (with dots used as
a seperator between directories, much like the "/" on Unix systems and the "\" on DOS systems). For example, an image (`img.png`)
that is part of the module `gui.icons` should be placed in the path `./gui/icons/img.png` (this can also be done with code, so
it is often useful to distinguish between code and other files by using a separate base path (i.e. `./src/gui/icons/icon.spl`
and `./res/img/gui/icons/icon.spl`). To include the actual item, declare the following within the module:

```C#
module gui.icons {
	[includeFile(":res/img/gui/icons/img.png")];
	/* The colon signifies that it should start at the working directory, or the parent of ./src */
	[includeDir(":res/img/gui/icons/")];
}
```

### Freezing/Melting:
Of all of the features added, these are the simplest. Essentially, freezing (`expr ice = {{doLongProcess();}};`) allows
the use of expression variables that store an expression that hasn't been run yet. Melting (`[[ice]];`), or running the frozen
expression, allows for you to release the previous expression and run it at the correct time.
