namespace FSharpWebAPIExample

module Validation =

    open System
    module E = ErrorHandling

    let validate propName prop validators obj =
        let result =
            validators
            |> List.fold (
                fun acc f ->
                    match acc with
                    | E.Failure x -> acc
                    | E.Success x -> f x ) (E.ok prop)
        match result with
        | E.Failure x -> E.fail <| sprintf "%s: %s" propName x
        | E.Success _ -> E.ok obj

    let validateRecord msg f o =
        match Null.asOption o with
        | None -> E.fail msg
        | _ ->
        f o
        |> E.collectErrors (String.concat "\n") id o


    let required x =
        if (obj.Equals(x, null))
        then E.fail "This item is required."
        else E.ok x

    let notEmptyString (x : string) =
        if x.Length = 0
        then E.fail "Cannot be an empty string."
        else E.ok x

    let mustBeBefore (date : DateTime)  (x : DateTime) =
        if date < x
        then E.fail <| sprintf "Date must be before %A." date
        else E.ok x

    let requiredString = [
            required
            notEmptyString
        ]
