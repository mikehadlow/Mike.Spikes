module Mike.Spikes.Fs.ImageProcessor

open System
open System.IO

let numberOfImages = 200
let size = 512
let numberOfPixels = size * size

let makeImageFiles () =
    printfn "Making %d %dx%d images..." numberOfImages size size
    let pixels = Array.init numberOfPixels (fun i -> byte i)
    for i = 1 to numberOfImages do
        File.WriteAllBytes(sprintf "D:\\Temp\\Images\\Image%d.tmp" i, pixels)
    printfn "Done"

let processImageRepeats = 20

let transformImage pixels path =
    printfn "transformImage %s" path
    for i in 1 .. processImageRepeats do
        pixels |> Array.map (fun b -> b + 1uy) |> ignore
    pixels |> Array.map (fun b -> b + 1uy)

let processImage path = async {

    let readPixels path = async {
        use inStream = File.OpenRead(path)
        let pixels = Array.zeroCreate numberOfPixels
        let! _ = Async.AwaitTask (inStream.ReadAsync(pixels, 0, numberOfPixels)) 
        return pixels
    }

    let writePixels path pixels = async {
        use outStream = File.OpenWrite(path + ".done")
        do! Async.AwaitTask (outStream.WriteAsync(pixels, 0, numberOfPixels).ContinueWith(fun _ -> ()))
    }

    let! pixels = readPixels path
    let pixels' = transformImage pixels path
    do! writePixels path pixels'
}

let processImagesSync () =
    Directory.GetFiles("D:\\Temp\\Images")
        |> Seq.map processImage
        |> Async.Parallel
        |> Async.RunSynchronously
