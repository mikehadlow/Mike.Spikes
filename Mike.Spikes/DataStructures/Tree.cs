using System.Collections.Generic;

namespace Mike.Spikes.DataStructures
{
    public class Play
    {
        const string input = 
@"
0,  -1, Root
1,  0,  Shirts
2,  0,  Trousers
3,  1,  Stripey
4,  1,  Plain
5,  3,  Blue
6,  3,  Green
7,  4,  Blue
8,  4,  Red
9,  2,  Jeans
10, 2,  Slacks
11, 9,  Blue
12, 9,  Black
13, 10, Check 
";

        public void Create()
        {
            
        }
    }

    public class Tree<T>
    {
        public Node<T> Root { get; private set; }

        public Tree(T rootValue)
        {
            Root = new Node<T>(rootValue);
        }
    }

    public class Node<T>
    {
        private readonly List<Node<T>> children = new List<Node<T>>();
        public T Value { get; private set; }

        public Node(T value)
        {
            Value = value;
        }

        public Node<T> AddChild(T value)
        {
            var child = new Node<T>(value);
            children.Add(child);
            return child;
        }

        public IEnumerable<Node<T>> Children
        {
            get { return children; }
        }
    }
}