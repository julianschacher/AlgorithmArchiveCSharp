using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace HuffmanCoding
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = HuffmanCoding.Encode("aaaabbbccd");
            // Print dictionary.
            foreach (var entry in result.Dictionary)
            {
                var bitString = "";
                foreach (var value in entry.Value)
                {
                    if (value)
                        bitString += "1";
                    else
                        bitString += "0";
                }
                System.Console.WriteLine(entry.Key + " " + bitString);
            }
            // Print bitString.
            var readableBitString = "";
            foreach (var boolean in result.BitString)
            {
                if (boolean)
                    readableBitString += "1";
                else
                    readableBitString += "0";
            }
            System.Console.WriteLine(readableBitString);

            var originalString = HuffmanCoding.Decode(result);
            System.Console.WriteLine(originalString);
        }
    }

    public class EncodeResult 
    {
        public List<bool> BitString { get; set; }
        public Dictionary<char, List<bool>> Dictionary { get; set; }
        public HuffmanCoding.Node Tree { get; set; }

        public EncodeResult(List<bool> bitString, Dictionary<char, List<bool>> dictionary, HuffmanCoding.Node tree)
        {
            this.BitString = bitString;
            this.Dictionary = dictionary;
            this.Tree = tree;
        }
    }

    public static class HuffmanCoding
    {
        public class Node
        {
            public Node[] Children { get; set; } = new Node[2];
            public List<bool> BitString { get; set; } = new List<bool>();
            public int Weight { get; set; }
            public string Key { get; set; }

            public Node(string key, int weight)
            {
                this.Key = key;
                this.Weight =  weight;
            }
        }

        public static EncodeResult Encode(string input)
        {
            var root = CreateTree(input);
            var dictionary = CreateDictionary(root);
            var bitString = CreateBitString(input, dictionary);

            return new EncodeResult(bitString, dictionary, root);
        }

        public static string Decode(EncodeResult result)
        {
            var output = "";
            Node currentNode = result.Tree;
            foreach (var boolean in result.BitString)
            {
                // Go down the tree.
                if (!boolean)
                    currentNode = currentNode.Children[0];
                else
                    currentNode = currentNode.Children[1];

                // Check if it's a leaf node.
                if (currentNode.Key.Count() == 1)
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
            var nodes = new List<Node>();
            foreach (var character in input)
            {
                var result = nodes.Where(n => n.Key[0] == character).SingleOrDefault();

                if (result == null)
                    nodes.Add(new Node(character.ToString(), 1));
                else
                    result.Weight++;
            }
            // Sort nodes by count in input (Weight).
            nodes = nodes.OrderByDescending(n => n.Weight).ToList();

            foreach (Node node in nodes)
            {
                System.Console.WriteLine(node.Key + " : " + node.Weight);
            }

            while (nodes.Count > 1)
            {
                var parentNode = new Node("", 0);
                for (int i = 0; i < 2; i++)
                {
                    System.Console.WriteLine("> " + nodes.Last().Key);
                    parentNode.Children[i] = nodes.Last();
                    parentNode.Key += nodes.Last().Key;
                    parentNode.Weight += nodes.Last().Weight;

                    nodes.RemoveAt(nodes.Count - 1);
                };
                Console.WriteLine(">> " + parentNode.Key + " : " + parentNode.Weight);
                nodes.Add(parentNode);

                nodes = nodes.OrderByDescending(n => n.Weight).ToList();
            }

            return nodes[0];
        }

        private static Dictionary<char, List<bool>> CreateDictionary(Node root)
        {
            var dictionary = new Dictionary<char, List<bool>>();

            var stack = new Stack<Node>();
            stack.Push(root);
            Node temp;

            while (stack.Count != 0)
            {
                temp = stack.Pop();

                if (temp.Key.Count() == 1)
                    dictionary.Add(temp.Key[0], temp.BitString);
                else
                {
                    for (int i = 0; i < temp.Children.Count(); i++)
                    {
                        if (temp.Children[i] != null)
                        {
                            if (i == 0)
                            {
                                temp.Children[i].BitString.AddRange(temp.BitString);
                                temp.Children[i].BitString.Add(false);
                            }
                            else
                            {
                                temp.Children[i].BitString.AddRange(temp.BitString);
                                temp.Children[i].BitString.Add(true);
                            }

                            stack.Push(temp.Children[i]);
                        }
                    }
                }
           }

           return dictionary;
        }

        private static List<bool> CreateBitString(string input, Dictionary<char, List<bool>> dictionary)
        {
            var bitString = new List<bool>();
            foreach (var character in input)
                bitString.AddRange(dictionary[character]);

            return bitString;
        }
    }
}