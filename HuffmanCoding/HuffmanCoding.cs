// submitted by Julian Schacher (jspp) with help by gustorn.
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuffmanCoding
{
    public class EncodingResult 
    {
        public string BitString { get; set; }
        public Dictionary<char, string> Dictionary { get; set; }
        public HuffmanCoding.Node Tree { get; set; }

        public EncodingResult(string bitString, Dictionary<char, string> dictionary, HuffmanCoding.Node tree)
        {
            this.BitString = bitString;
            this.Dictionary = dictionary;
            this.Tree = tree;
        }
    }

    public static class HuffmanCoding
    {
        // The Node class used for the Huffman Tree.
        public class Node : IComparable<Node>
        {
            public Node LeftChild { get; set; }
            public Node RightChild { get; set; }
            public string BitString { get; set; } = "";
            public int Weight { get; set; }
            public string Key { get; set; }

            public bool IsLeaf => LeftChild == null && RightChild == null;
            
            // Creates a leaf. So just a node is created with the given values.
            public static Node CreateLeaf(char key, int weight) => new Node(key.ToString(), weight, null, null);
            // Creates a branch. Here a node is created by adding the keys and weights of both childs together.
            public static Node CreateBranch(Node leftChild, Node rightChild) => new Node(leftChild.Key + rightChild.Key, leftChild.Weight + rightChild.Weight, leftChild, rightChild);
            private Node(string key, int weight, Node leftChild, Node rightChild)
            {
                this.Key = key;
                this.Weight =  weight;
                this.LeftChild = leftChild;
                this.RightChild = rightChild;
            }

            public int CompareTo(Node other) => this.Weight - other.Weight;
        }

        // Node with biggest value at the top.
        class NodePriorityList
        {
            public int Count => nodes.Count;

            private List<Node> nodes = new List<Node>();

            public NodePriorityList() { }
            public NodePriorityList(List<Node> givenNodes)
            {
                this.nodes = givenNodes.ToList();
                this.nodes.Sort();
            }

            public void Add(Node newNode)
            {
                var index = ~this.nodes.BinarySearch(newNode);
                if (index == this.nodes.Count)
                {
                    this.nodes.Add(newNode);
                    return;
                }
                this.nodes.Insert(~index, newNode);
            }

            public Node Pop()
            {
                var result = this.nodes[0];
                this.nodes.RemoveAt(0);
                return result;
            }
        }

        public static EncodingResult Encode(string input)
        {
            var root = CreateTree(input);
            var dictionary = CreateDictionary(root);
            var bitString = CreateBitString(input, dictionary);

            return new EncodingResult(bitString, dictionary, root);
        }

        public static string Decode(EncodingResult result)
        {
            var output = "";
            Node currentNode = result.Tree;
            foreach (var bit in result.BitString)
            {
                // Go down the tree.
                if (bit == '0')
                    currentNode = currentNode.LeftChild;
                else
                    currentNode = currentNode.RightChild;

                if (currentNode.IsLeaf)
                {                    
                    output += currentNode.Key;
                    currentNode = result.Tree;
                }
            }
            return output;
        }

        private static Node CreateTree(string input)
        {
            // Create a List of all characters and their count in input by putting them into nodes.
            var nodes = input
                .GroupBy(c => c)
                .Select(n => Node.CreateLeaf(n.Key, n.Count()))
                .ToList();

            // Convert list of nodes to a NodePriorityList.
            var nodePriorityList = new NodePriorityList(nodes);

            // Create Tree.
            while (nodePriorityList.Count > 1)
            {
                // Pop the two nodes with the smallest weights from the nodePriorityList and create a parentNode with the CreateBranch method. (This method adds the keys and weights of the childs together.)
                var leftChild = nodePriorityList.Pop();
                var rightChild = nodePriorityList.Pop();
                var parentNode = Node.CreateBranch(leftChild, rightChild);

                nodePriorityList.Add(parentNode);
            }

            return nodePriorityList.Pop();
        }

        private static Dictionary<char, string> CreateDictionary(Node root)
        {
            // We're using a string instead of a actual bits here, since it makes the code somewhat more readable and this is an educational example. 
            var dictionary = new Dictionary<char, string>();

            var stack = new Stack<Node>();
            stack.Push(root);
            Node temp;

            while (stack.Count != 0)
            {
                temp = stack.Pop();

                if (temp.IsLeaf)
                    dictionary.Add(temp.Key[0], temp.BitString);
                else
                {
                    if (temp.LeftChild != null)
                    {
                        temp.LeftChild.BitString  += temp.BitString;
                        temp.LeftChild.BitString += 0;
                        stack.Push(temp.LeftChild);
                    }
                    if (temp.RightChild != null)
                    {
                        temp.RightChild.BitString += temp.BitString;
                        temp.RightChild.BitString += 1;
                        stack.Push(temp.RightChild);
                    }
                }
           }

           return dictionary;
        }

        private static string CreateBitString(string input, Dictionary<char, string> dictionary)
        {
            // We're using a string right here. While no compression is achieved with a string, it's the easiest way to display what the compressed result looks like. Also this is just an educational example.
            var bitString = "";
            foreach (var character in input)
                bitString += dictionary[character];

            return bitString;
        }
    }
}