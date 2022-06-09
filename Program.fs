open FSharp.SystemCommandLine
open System.IO

let readText filePath = File.ReadAllText(filePath)

let writeText filePath text = File.WriteAllText(filePath, text)

let replaceThreat1 (text:string) = text.Replace("{{1}}", String.replicate 1 "🟥")
let replaceThreat2 (text:string) = text.Replace("{{2}}", String.replicate 2 "🟧")
let replaceThreat3 (text:string) = text.Replace("{{3}}", String.replicate 3 "🟨")
let replaceThreat4 (text:string) = text.Replace("{{4}}", String.replicate 4 "🟩")
let replaceThreat5 (text:string) = text.Replace("{{5}}", String.replicate 5 "🟩")

let replaceThreats text =
    text
    |> replaceThreat1
    |> replaceThreat2
    |> replaceThreat3
    |> replaceThreat4
    |> replaceThreat5


let decode (inputFile: FileInfo, outputFileMaybe: FileInfo option) =
    let outputFile = outputFileMaybe |> Option.defaultValue inputFile

    if inputFile.Exists
    then
        printfn $"Decoding {inputFile.Name} to {outputFile.Name}"
        let text = readText inputFile.FullName
        text |> replaceThreats |> writeText outputFile.FullName
        printfn $"Decoded {inputFile.Name} to {outputFile.Name}"
    else printfn $"File does not exist: {inputFile.FullName}"

[<EntryPoint>]
let main argv =

    let inputFile = Input.Argument<FileInfo>("The file to decode")
    let outputFileMaybe = Input.OptionMaybe<FileInfo>(["--output"; "-o"], "The output file")

    rootCommand argv {
        description "Decodes an .md file"
        inputs (inputFile, outputFileMaybe) // must be set before setHandler
        setHandler decode
    }