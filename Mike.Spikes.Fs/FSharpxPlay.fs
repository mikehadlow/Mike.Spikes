module Mike.Spikes.Fs.FSharpxPlay

open FSharpx

let ``play with monoids`` () =
    let sumInt = Monoid.sumInt
    let result = sumInt.Combine (4, 4)
    result

