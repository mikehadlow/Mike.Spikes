module Mike.Spikes.Fs.AsyncPlay

open System
open System.Net
open System.IO

let museums =   [
                "MOMA", "http://moma.org/"
                "British Museum", "http://www.thebritishmuseum.ac.uk/"
                "Prado", "http://www.museodelprado.es/"
                ]

let tprintfn format =
    printf "[.NET Thread %d]" System.Threading.Thread.CurrentThread.ManagedThreadId
    printfn format

let fetch(name, url:string) = async {
    tprintfn "Creating request for %s..." name
    let request = WebRequest.Create(url)
    let! response = request.AsyncGetResponse()

    use stream = response.GetResponseStream()

    tprintfn "Reading response for %s..." name
    let reader = new StreamReader(stream)
    let! html = Async.AwaitTask (reader.ReadToEndAsync())

    return sprintf "Read %i characters for %s..." html.Length name
}

let ``fetch museum web pages`` () =
    museums 
        |> List.map fetch 
        |> Async.Parallel 
        |> Async.RunSynchronously


