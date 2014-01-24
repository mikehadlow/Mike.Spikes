using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mike.Spikes.DataStructures
{
    public class Play
    {
        // a tree in a database is usually represented
        // with a self referential foreign key
        const string input = 
@"0,  -1, Root
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
13, 10, Check";

        // load rows from a 'database'. Here we're just loading
        // from a string, so lots of 'Split' operations
        public IDictionary<int, InputItem> LoadItems()
        {
            return (
                from line in input.Split('\n')
                let rows = line.Split(',')
                select new InputItem(
                        int.Parse(rows[0].Trim()),
                        int.Parse(rows[1].Trim()),
                        new Node<string>(rows[2].Trim()))
                ).ToDictionary(x => x.Id);
        }

        // Create a tree datastructure with the nodes.
        // After this, we can discard the database ids.
        public Node<string> WireUp(IDictionary<int, InputItem> items)
        {
            Node<string> root = null;

            foreach (var item in items)
            {
                if (item.Value.ParentId == -1)
                {
                    root = item.Value.Node;
                }
                else
                {
                    var parent = items[item.Value.ParentId].Node;
                    parent.Children.Add(item.Value.Node);
                }
            }

            return root;
        }

        // 1. just print out root name.
        // 2. print out all names
        // 3. print out all names indented
        // 4. print as XML elements
        public void PrintTree(Node<string> root)
        {
            root.Accept((children, node) =>
                {
                });
        }

        public void Traverse(Node<string> root, Action<Node<string>> action)
        {
            action(root);
            foreach (var child in root.Children)
            {
                Traverse(child, action);
            }
        }

        public void DemoTreeVisitor()
        {
            var root = WireUp(LoadItems());
            //Traverse(root, x => Console.WriteLine(x.Value));
            var indent = 0;
            var count = 0;
            root.Accept((traverseChildren, x) =>
                {
                    if(x.Children.Any())
                    {
                        Console.WriteLine("{0}<{1}>", new string('\t', indent), x.Value);
                        indent++;
                        traverseChildren();
                        indent--;
                        Console.WriteLine("{0}</{1}>", new string('\t', indent), x.Value);
                    }
                    else
                    {
                        Console.WriteLine("{0}<{1}/>", new string('\t', indent), x.Value);
                    }
                    count++;
                });
            Console.Out.WriteLine("count = {0}", count);    
        }

        public void TreeEqualityWithRecursiveHashCode()
        {
            var root = WireUp(LoadItems());

            Console.Out.WriteLine("root =          {0}", root.GetHashCode());

            root.Children[0].Children.Add(new Node<string>("New One"));

            Console.Out.WriteLine("modified root = {0}", root.GetHashCode());

            var root2 = WireUp(LoadItems());

            Console.Out.WriteLine("root2 =         {0}", root2.GetHashCode());
        }
    }

    public class Node<T>
    {
        public T Value { get; private set; }
        public IList<Node<T>> Children { get; private set; }

        public Node(T value)
        {
            Value = value;
            Children = new List<Node<T>>();
        }

        public void Accept2(Action<Node<T>> visit)
        {
            visit(this);
            foreach (var child in Children)
            {
                child.Accept2(visit);
            }
        }

        public void Accept(Action<Action, Node<T>> visit)
        {
            visit(() =>
                {
                    foreach (var child in Children)
                    {
                        child.Accept(visit);
                    }
                }, this);
        }

        public override int GetHashCode()
        {
            var builder = new StringBuilder();
            builder.Append(Value);
            foreach (var child in Children)
            {
                builder.Append(child.GetHashCode().ToString());
            }
            return builder.ToString().GetHashCode();
        }
    }

    public class InputItem
    {
        public int Id { get; private set; }
        public int ParentId { get; private set; }
        public Node<string> Node { get; private set; }

        public InputItem(int id, int parentId, Node<string> node)
        {
            Id = id;
            ParentId = parentId;
            Node = node;
        }
    }
}