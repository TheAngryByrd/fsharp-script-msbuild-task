namespace SomeMsbuildTask

#r "nuget: Microsoft.Build.Utilities.Core"
#r "nuget: IcedTasks"


#nowarn "0988" // warning FS0988: Main module of program is empty: nothing will happen when it is run. This is expected for MSBuild tasks.

open Microsoft.Build.Framework
open IcedTasks
open System

type FSharpTask() =
    inherit Microsoft.Build.Utilities.Task()

    member private x.CancellationTokenSource = new System.Threading.CancellationTokenSource()

    [<Required>]
    member val Property1 = "" with get, set
    member val Property2 = 0 with get, set
    [<Output>]
    member val Result = "" with get, set

    member inline private x.ExecuteCore = cancellableTask {

        // Example logic for the task
        if String.IsNullOrWhiteSpace x.Property1 then
            x.Log.LogError("Property1 must be not null or whitespace.")
            return false
        else
            if x.Property1.Contains "Error" then
                failwith "Simulated failure for demonstration purposes."

            // Using warning log to demonstrate logging
            x.Log.LogWarning($"Property1: {x.Property1}, Property2: {x.Property2}")
            x.Result <- $"Task executed successfully. Property1: {x.Property1}, Property2: {x.Property2}"
            return true
    }

    override x.Execute() = x.ExecuteCore(x.CancellationTokenSource.Token).GetAwaiter().GetResult()

    interface ICancelableTask with
        member x.Cancel() = x.CancellationTokenSource.Cancel()
