Splice
======

A programming language based on C that adds node-oriented programming features.

Features:
---------

[Implemented]:

[Unimplemented]:
* C-derived syntax for increased ease-of-use and a familiar feel for most developes
* Module system, which allows for better version management for dependencies and the ability to define equivalent dependencies
* Freeze and melt operators, which allow for expressions and statements to be "reserved" until melted
* Node-orientation, an experimental programming paradigm
* The "soup" type, an optionally-indexed, multi-type, dynamically-allocated heap
* A standard library, including math utilities, I/O, and data containers

Using Splice:
-------------

### Your First Program:

Splice follows a very simple specification when building your code. While you can manually define how it will build a program,
the easiest way is to let Splice automatically do this. Splice tries to do as much as possible without using meta-files to define
how the program will compile. To get around this, it uses a standardized file structure to understand how to run your code.

When you invoke the `$ splice build` command, the Splice compiler will attempt to automatically detect the file system structure
and figure out how to build the program. If it doesn't know how, Splice will ask a few questions (although it would be much more
efficient to create a Makefile or shell script to build it for you if you are not using the Splice standards). The standard
directory structure is as follows:

```
? - Working directory, where you invoke `$ splice build` from.
|
|- ?/sp-src - The source code directory. Place all source code in this folder (or its children).
|	|
|	|- ?/sp-src/module/names/split/by/dots
|		|
|		|- ?/sp-src/module/names/split/by/dots/main.spl - The main file. Always titled main.spl.
|
|- ?/sp-dep - The dependencies directory. Place all precompiled libraries here or dep.txt to import external dependencies.
|
|- ?/sp-bin - The output directory. All compiled binaries will be placed here.
	|
	|- ?/sp-bin/release - The stable, release-ready executables. (Use -r flag to build a release).
	|
	|- ?/sp-bin/debug - The debugging executables.
```

An example of this directory structure can be found in the code in the `test` directory.
