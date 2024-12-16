# Args kata

Implementation of the Args kata

## Introduction

The solution consists of two console app projects and a test project.
In order to use the args you must modify the `args.generator/mappings.json` file with the proper parameters and switches.

The Filepath property in the mappings is the base output directory for the generated files.

## Schema for the mappings file

The schema is very simple:

* a Switch is a character prefixed with `-` e.g. `-p` or `-f`.
* a Parameter is a word prefixed with `-` and a following value, e.g. `-path /some/path/`.
* The Filepath is the output directory for the generated files.

## Arguments classes generation

When the `mappings` file is prepared and saved you can run the generator project:

```bash
dotnet build
./args.generator/bin/Debug/net8.0/args.generator
```

This will generate two files in the args.console project:

* Arguments.cs
* ArgumentMappings.cs

After generation of these classes you can build the console app:

`dotnet build`

and now you should have a working console app that can accept the arguments you specified in the mappings file (i.e. switches and parameters).

The idea is to use the `Parser` in a console app - see the `args.console/Program.cs for an example`.

### Arguments.cs

This generated class has:

* a set of `Boolean` properties for the specified switches.
* a set of `string` properties for the specified parameters.
* a property HasErrors that returns whether or not there were errors in the parsing of arguments. This can be used to throw an exception or notify the user of any errors.
* two properties for failing switches or parameters - UnknownSwitches and UnknownParameters, respectively. Will by empty if all arguments were good.

### ArgumentMappings.cs

This generated class has two dictionaries for mapping command line arguments names with internal property names.

## Build, generate, build and run

In order to build and run the solution in the debugger, the `.vscode/launch.json` and `.vscode/tasks.json` have been prepared so when hitting F5 or click run in vscode the following tasks are run in sequence:

* build the `args.generator` project
* run the `args.generator` using the `mappings.json` configuration file for generating the classes in the `args.console` project.
* build the `args.console` project with the generated classes.
* run the `args.console` app with a set of predefined arguments (see `.vscode/launch.json`)
