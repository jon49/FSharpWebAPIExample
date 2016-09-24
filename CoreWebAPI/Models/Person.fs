namespace FSharpWebAPIExample.Models

module DTO =

    open System
    module V = FSharpWebAPIExample.Validation

    [<CLIMutable>]
    type Name = {
        First : string
        Last : string
    }

    [<CLIMutable>]
    type Person = {
        Name : Name
        BirthDate : DateTime
    } with
    static member Validate =
        V.validateRecord "The person is not Existential" <|
        fun p -> [
                    V.validate "First Name" p.Name.First V.requiredString
                    V.validate "Last Name" p.Name.Last V.requiredString
                    V.validate "Birthdate" p.BirthDate [V.required; (V.mustBeBefore <| new DateTime(2000, 1, 1))]
                 ]


