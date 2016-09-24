namespace FSharpWebAPIExample

module Configuration =

    open FSharp.Data

    type CurrentEnvironment = JsonProvider<"""{"env" : "Dev"}""">

    type Environment = Dev | Stage | Prod

    let currentEnvironment =
        match CurrentEnvironment.GetSample().Env with
        | "Dev" -> Dev
        | "Stage" -> Stage
        | "Prod" -> Prod
        | _ -> Dev

    [<Literal>]
    let TestDbConnection = "Server=JON;Database=Test;Trusted_Connection=True;"

    let TestDbConnectionString env =
        match env with
        | Dev -> "Server=JON;Database=Test;Trusted_Connection=True;"
        | Stage -> ""
        | Prod -> ""
