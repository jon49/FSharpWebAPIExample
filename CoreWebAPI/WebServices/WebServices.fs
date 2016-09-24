namespace FSharpWebAPIExample.WebServices

module Call =

    open FSharp.Data
    module Config = FSharpWebAPIExample.Configuration
    open FSharpWebAPIExample.Models

    let addPerson (person : DTO.Person) =
        use cmd = new SqlCommandProvider<"
            INSERT INTO Person
                (FirstName, LastName, BirthDate)
            Values (@firstName, @lastName, @birthDate)
            ", Config.TestDbConnection>(Config.TestDbConnectionString Config.currentEnvironment)
        cmd.AsyncExecute(person.Name.First, person.Name.Last, person.BirthDate)

    let getPerson id =
        use cmd = new SqlCommandProvider<"
            SELECT *
            FROM Person p
            WHERE p.PersonID = @id
            ", Config.TestDbConnection>(Config.TestDbConnectionString Config.currentEnvironment)
        cmd.AsyncExecute id
