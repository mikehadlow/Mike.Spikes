module Mike.Spikes.Fs.Generics

type Tree<'T> = 
    | Node of 'T * Tree<'T> * Tree<'T>
    | Leaf of 'T

let rec flatten (t:Tree<_>) =
    seq {
        match t with
        | Leaf x -> yield x
        | Node (x, left, right) ->
            yield x
            yield! flatten left
            yield! flatten right
    }

let myIntTree = Node (1, Node(2, Leaf 4, Leaf 5), Node(3, Leaf 6, Leaf 7))

let myStringTree = Node ("Mike", Node("Jim", Leaf "Sally", Leaf "Jez"), Node("Tim", Leaf "Jack", Leaf "Greg"))

let ``try flattening`` () =
    flatten myIntTree |> Seq.fold (+) 0

let ``try flattening 2`` () =
    flatten myStringTree

