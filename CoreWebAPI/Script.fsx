//#r "./bin/"
#I @"..\packages\FSharp.Data.2.3.2\lib\net40"
#r "FSharp.Data.dll"
#I @"..\packages\FSharp.Data.SqlClient.1.8.2\lib\net40"
#r "FSharp.Data.SqlClient.dll"
#load "./Configuration.fs"

module Config = FSharpWebAPIExample.Configuration

open FSharp.Data

let addPerson () =
    use cmd = new SqlCommandProvider<"
        INSERT INTO Person
            (FirstName, LastName, BirthDate)
        Values ('George', 'IsMe', '2015-01-01')
        ", Config.TestDbConnection>(Config.TestDbConnectionString Config.currentEnvironment)
    cmd.Execute()

let getPerson () =
    use cmd = new SqlCommandProvider<"
        SELECT *
        FROM Person p
        WHERE p.PersonID = 1
        ", Config.TestDbConnection>(Config.TestDbConnectionString Config.currentEnvironment)
    cmd.Execute ()

printfn "%A" (addPerson ())
printfn "%A" (getPerson ())