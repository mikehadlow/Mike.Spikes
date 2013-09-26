module Mike.Spikes.Fs.Euler

open MathNet.Numerics

// 234168
let ``problem 1, multiples of 3 and 5`` () =
    
    Set.union (Set.ofSeq (seq { 0 .. 3 .. 1000 })) (Set.ofSeq (seq { 0 .. 5 .. 1000 }))
        |> List.ofSeq |> List.sum


let rec fib a b =
    seq {
        let next = a + b
        yield next
        yield! fib b next
    }

let even x = x % 2 = 0

// 4613732
let ``problem 2, even fibonacci numbers`` () =

    (fib 0 1) 
        |> Seq.filter even
        |> Seq.takeWhile ( (>) 4000000 ) 
        |> Seq.sum


let rec factors (n:bigint) (f:bigint) =
    seq {
        if n % f = 0I then
            yield f
        if f < (n / 2I) then
            yield! factors n (f + 1I)
    }

let isPrime (n:bigint) = factors n 2I |> Seq.length = 0

let primeFactors n = factors n 2I |> Seq.filter isPrime

let ``problem 3, largest prime factor`` () =
    
    primeFactors 600851475143I


let ``aside, playing with some palindrome ideas`` () =

    let digits = seq { 0 .. 9 } 
    let rec combine s n = 
        seq {
            match n with
            | 0 -> yield! s
            | _ -> yield! combine (s |> Seq.collect (fun x -> digits |> Seq.map (fun y -> y :: x))) (n - 1)
        }
    
    combine (digits |> Seq.map (fun x -> [x])) 1 
        |> Seq.filter (fun x -> x = (List.rev x))

// 906609
let ``problem 4, largest palindrome product`` () =
    let digits = 3.0
    let numbers = seq { 1 .. int (10.0 ** digits - 1.0) }
    numbers 
        |> Seq.collect (fun x -> numbers |> Seq.map (fun y -> x * y))
        |> Seq.filter (fun x -> (List.ofSeq (x.ToString())) = List.rev (List.ofSeq (x.ToString())))
        |> Seq.max