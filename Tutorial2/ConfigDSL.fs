﻿module ConfigDSL

open System
open System.IO
open System.Collections.Generic
open Microsoft.FSharp.Compiler.Interactive.Shell

let runConfig fileName  =
    let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()

    let commonOptions = [| "fsi.exe"; "--noninteractive" |]

    let sbOut = new Text.StringBuilder()  
    let sbErr = new Text.StringBuilder()  // TODO: evtl. irgendwo ausgeben
    let outStream = new StringWriter(sbOut)
    let errStream = new StringWriter(sbErr)

    let stdin = new StreamReader(Stream.Null)   
    
    try
        let session = FsiEvaluationSession.Create(fsiConfig, commonOptions, stdin, outStream, errStream)
        session.EvalExpression("let source x = 22") |> ignore

        try 
            session.EvalScript fileName            
        with    
        | _ -> printfn "Error: %s" <| sbErr.ToString()
            
    with    
    | exn ->
        printfn "FsiEvaluationSession could not be created."
        
        raise exn    