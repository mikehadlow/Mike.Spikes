module Mike.Spikes.Fs.TypeProviderPlay

open FSharp.Data
open System

let data = FreebaseData.GetDataContext()

let elements = data.``Science and Technology``.Chemistry.``Chemical Elements``

let ``print out elements`` () =
    elements |> Seq.map (fun x -> (x.Name, x.``Atomic number``)) |> Seq.iter (printfn "%A")

let celebrities = data.Society.Celebrities.Celebrities

let ``count celebrities`` () =
    celebrities |> Seq.length

let ``list celebrities`` () =
    celebrities |> Seq.map (fun c -> c.Name) |> Seq.iter (printfn "%A")

let bands = query { 
        for band in data.``Arts and Entertainment``.Music.``Musical Groups`` do
        where (band.``Active as Musical Artist (start)`` = "1965")
        select band.Name
    }

let ``list late 60s bands`` () =
    bands |> Seq.iter (printfn "%s")

