// submitted by Julian Schacher (jspp)
using System;
using System.Collections.Generic;

namespace JarvisMarch
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("JarvisMarch");
            // Example list of points.
            var points = new List<Point>()
            {
                new Point(1, 3),
                new Point(2, 4),
                new Point(4, 0),
                new Point(1, 0),
                new Point(0, 2),
                new Point(2, 2),
                new Point(3, 4),
                new Point(3, 1),
            };
            var giftWrap = JarvisMarch.RunJarvisMarch(points);
            // Print the points of the gift wrap.
            foreach (var point in giftWrap)
                System.Console.WriteLine($"{point.XPosition}, {point.YPosition}");
        }
    }
}
