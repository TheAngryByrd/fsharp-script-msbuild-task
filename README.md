# F# Script MSBuild Task

This project demonstrates how to create and use MSBuild tasks written in F# script (`.fsx`) files. It provides a simple but powerful approach to extend MSBuild's capabilities using F# scripts that can be compiled on-the-fly during the build process.

## Overview

The project shows how to:

1. Define an MSBuild task in an F# script file
2. Compile the F# script to a usable assembly during the build process
3. Reference and use the compiled task within the same MSBuild project

This approach combines the flexibility of F# scripting with the power of MSBuild's extensibility model.

## How It Works

### The F# Script Task (SomeMsbuildTask.fsx)

The project includes an example MSBuild task written in F#:

```fsharp
namespace SomeMsbuildTask

type FSharpTask() =
    inherit Microsoft.Build.Utilities.Task()

    [<Required>]
    member val Property1 = "" with get, set
    member val Property2 = 0 with get, set
    [<Output>]
    member val Result = "" with get, set

    override this.Execute() =
        // Task implementation logic
        // ...
```

The task:
- Inherits from `Microsoft.Build.Utilities.Task`
- Defines input properties (`Property1` and `Property2`)
- Defines an output property (`Result`)
- Implements the `Execute()` method required by MSBuild

### Build Process Integration

The project uses the following approach to integrate the F# script task:

1. **Compilation**: The F# script is compiled to a DLL during the build process using `dotnet fspack`:
   ```xml
   <Target Name="CompileTaskScript" BeforeTargets="BeforeBuild" Inputs="$(ScriptTaskFsx)" Outputs="$(ScriptTaskAssemblyPath)">
     <Exec Command="dotnet fspack $(ScriptTaskFsx) -o $(TaskOutputDir) -f $(TargetFramework)" />
   </Target>
   ```

   This uses incremental compilation to ensure that the task is only recompiled if the script file changes.

2. **Registration**: The compiled task is registered with MSBuild:
   ```xml
   <UsingTask TaskName="SomeMsbuildTask.FSharpTask" AssemblyFile="$(ScriptTaskAssemblyPath)" />
   ```

3. **Execution**: The task is executed as part of the build process:
   ```xml
   <Target Name="RunFSharpScriptTask" AfterTargets="Build">
     <FSharpTask Property1="Value1" Property2="2">
       <Output TaskParameter="Result" PropertyName="FSharpTaskResult" />
     </FSharpTask>
     <Message Text="F# Task Result: $(FSharpTaskResult)" Importance="high" />
   </Target>
   ```

## Requirements

- .NET SDK 9.0 or higher
- `fspack` tool for compiling F# scripts to assemblies

## Getting Started

1. Clone this repository
2. Restore tools:
   ```
   dotnet tool restore
   ```
3. Build the project:
   ```
   dotnet build
   ```

This will compile the F# script to a DLL and execute the MSBuild task as part of the build process.

## Creating Your Own F# Script MSBuild Tasks

To create your own MSBuild tasks:

1. Create an F# script file with your task implementation
2. Ensure your task class inherits from `Microsoft.Build.Utilities.Task`
3. Implement the required `Execute()` method
4. Add appropriate properties with getters/setters for inputs and outputs
5. Use the `[<Required>]` attribute for mandatory properties
6. Use the `[<Output>]` attribute for output properties

Then modify the project file to compile and use your task during the build process.

## Benefits

- **Flexibility**: Easily create MSBuild tasks without needing to build and reference separate assemblies
- **Simplicity**: Write tasks in F# script files, which are easier to read and maintain compared to [C# inline fragments](https://learn.microsoft.com/en-us/visualstudio/msbuild/walkthrough-creating-an-inline-task?view=vs-2022#to-add-a-basic-hello-task)
- **F# Power**: Use F#'s expressive syntax and powerful features in your build tasks
- **On-the-fly Compilation**: Tasks are compiled during the build process, ensuring they're always up-to-date

## Limitations and Notes

- The project includes a workaround for an MSBuild issue related to file cleanup (see: https://github.com/dotnet/msbuild/pull/12096)
- Currently targets .NET 9.0, but can be adapted to other frameworks

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
