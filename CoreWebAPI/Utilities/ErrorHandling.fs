namespace FSharpWebAPIExample

module ErrorHandling =

    open System.Web.Http
    open System.Web.Http.Results
    open System.Net
    open System.Net.Http

    type ServerError = {
        Message : string
        StackTrace : string
    }

    type ErrorTypes =
        | BadRequest of string
        | Unauthorized of string
        | InternalServerError of ServerError
        | NotFound of string

    let (|Success|Failure|) =
        function
        | Choice1Of2 s -> Success s
        | Choice2Of2 f -> Failure f

    let fail x = Choice2Of2 x
    let ok x = Choice1Of2 x

    let getFailureMessage =
        function
        | Success _ -> None
        | Failure x -> Some x

    let collectErrors combineErrors errorType o fs =
        let errors =
            fs
            |> List.choose (fun f -> getFailureMessage (f o))

        match errors with
        | [] -> ok o
        | _ -> fail (errors |> combineErrors |> errorType)

    let httpFailedResult (controller : ApiController) logError failed =
        let logError = logError controller

        match failed with
        | BadRequest msg ->
            logError "BadRequest" msg
            controller.Request.CreateErrorResponse(HttpStatusCode.BadRequest, msg)
        | Unauthorized msg ->
            logError "Unauthorized" msg
            controller.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, msg)
        | InternalServerError msg ->
            let msg = (sprintf "%s\n%s" msg.Message msg.StackTrace)
            logError "InternalServerError" msg
            controller.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, msg)
        | NotFound msg ->
            logError "NotFound" msg
            controller.Request.CreateErrorResponse(HttpStatusCode.NotFound, msg)

    let toHttpResult (controller : ApiController) logError f result =
        match result with
        | Success s ->
            let response = new HttpResponseMessage()
            f controller response s
        | Failure failed ->
            httpFailedResult controller logError failed

    let badRequest choice =
        match choice with
        | Success x -> ok x
        | Failure x -> fail <| BadRequest x

    let bind switchFunction twoTrackInput =
        match twoTrackInput with
        | Success s -> switchFunction s
        | Failure f -> fail f
    
    let tryCatch f x =
        try
            ok <| f x
        with
        | ex -> fail <| InternalServerError { Message = ex.Message; StackTrace = ex.StackTrace }
