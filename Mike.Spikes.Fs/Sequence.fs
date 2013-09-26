module Mike.Spikes.Fs.Sequence

open System.IO

let nums = seq { 0 .. 9 }

let ``iterate nums`` () =
    for n in nums do printfn "%i" n

let ``transform`` () =
    nums |> Seq.map (fun x -> (x, x*x))

let ``print directory tree`` () =
    let rec directoryTree directory =
        seq {
            for file in Directory.GetFiles(directory) do yield file
            for childDirectory in Directory.GetDirectories(directory) do yield! directoryTree childDirectory
        }
    directoryTree "D:\Temp" |> Seq.map Path.GetFileName |> Seq.iter (printfn "%s")



