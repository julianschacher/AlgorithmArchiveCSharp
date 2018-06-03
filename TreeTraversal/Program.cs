// submitted by Julian Schacher (jspp)
using System;

namespace TreeTraversal
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TreeTraversal");
            var tree = new Tree(3, 3);
            Console.WriteLine("StartDFSRecursive:");
            tree.StartDFSRecursive();
            Console.WriteLine("DFSStack:");
            tree.DFSStack();
            Console.WriteLine("DFSQueue:");
            tree.BFSQueue();
            Console.WriteLine("StartDFSRecursivePostorder");
            tree.StartDFSRecursivePostorder();

            // Console.WriteLine("StartDFSRecursiveInorder (fail)");
            // tree.StartDFSRecursiveInorderBinary();
            tree = new Tree(3, 2);
            Console.WriteLine("StartDFSRecursiveInorder (succeed)");
            tree.StartDFSRecursiveInorderBinary();
        }
    }
}
