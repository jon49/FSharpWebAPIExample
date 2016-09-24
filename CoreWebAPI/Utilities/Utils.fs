namespace FSharpWebAPIExample

module Null =

    let isNull x =
        obj.ReferenceEquals(x, null)

    let asOption x =
        if isNull x
        then None
        else Some x

module Log =

    open System.Web

    let logError (controller : Http.ApiController) logType msg =
        ()
    
module Utils =

    open System.Web.Http
    open System.Net.Http

    let jsonResponse (controller : ApiController) _ data =
        controller.Request.CreateResponse(data)


