namespace SomeMsbuildTask

#r "nuget: Microsoft.Build.Utilities.Core"

open Microsoft.Build.Framework


type FSharpTask() =
    inherit Microsoft.Build.Utilities.Task()

    [<Required>]
    member val Property1 = "" with get, set
    
    member val Property2 = 0 with get, set
    [<Output>]
    member val Result = "" with get, set

    override this.Execute() =
        try
            // Example logic for the task
            if this.Property1 = "" then
                this.Log.LogError("Property1 must be set.")
                false
            else
                this.Log.LogMessage($"Property1: {this.Property1}, Property2: {this.Property2}")
                this.Result <- "Task executed successfully."
                true
        with
        | ex ->
            this.Log.LogErrorFromException(ex)
            false