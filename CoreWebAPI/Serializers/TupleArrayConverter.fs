namespace Newtonsoft.Json.FSharp

open System
open Microsoft.FSharp.Reflection
open Newtonsoft.Json

/// Converts F# tuples to/from JSON arrays. (a,b) = [a,b]
/// https://github.com/eulerfx/JsonNet.FSharp/blob/master/src/JsonNet.FSharp/TupleArrayConverter.fs
type TupleArrayConverter() =
    inherit JsonConverter()

    override x.CanConvert(t:Type) = 
        FSharpType.IsTuple(t)

    override x.WriteJson(writer, value, serializer) =
        let values = FSharpValue.GetTupleFields(value)
        serializer.Serialize(writer, values)

    override x.ReadJson(reader, t, _, serializer) =
        let advance = reader.Read >> ignore
        let deserialize t = serializer.Deserialize(reader, t)
        let itemTypes = FSharpType.GetTupleElements(t)

        let readElements() =
            let rec read index acc =
                match reader.TokenType with
                | JsonToken.EndArray -> acc
                | _ ->
                    let value = deserialize(itemTypes.[index])
                    advance()
                    read (index + 1) (acc @ [value])
            advance()
            read 0 []

        match reader.TokenType with
        | JsonToken.StartArray ->
            let values = readElements()
            FSharpValue.MakeTuple(values |> List.toArray, t)
        | _ -> failwith "invalid token"

