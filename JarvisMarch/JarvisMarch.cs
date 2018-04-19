// submitted by Julian Schacher (jspp)
using System.Collections.Generic;

namespace JarvisMarch
{
    public static class JarvisMarch
    {
        private class Vector
        {
            public int XValue { get; set; }
            public int YValue { get; set; }

            public Vector(int xValue, int yValue)
            {
                this.XValue = xValue;
                this.YValue = yValue;
            }
        }

        public static List<Point> RunJarvisMarch(List<Point> points)
        {
            // Set the intial point to the first point of the list.
            var initialPoint = points[0];
            // Search for a better initial point. One where the x-position is the lowest.
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].XPosition < initialPoint.XPosition)
                {
                    initialPoint = points[i];
                }
            }
            // Add the initial point as the first point of the gift wrap.
            var giftWrap = new List<Point>()
            {
                initialPoint
            };

            // Set previous point first to some point below the first initial point.
            var previousPoint = new Point(initialPoint.XPosition, initialPoint.YPosition - 1);
            var currentPoint = initialPoint;

            var notWrapped = true;
            // Continue searching for the next point of the wrap until the wrap is completed.
            while (notWrapped)
            {
                // Search for next Point.
                // Set the first vector, which is currentPoint -> previousPoint.
                var firstVector = new Vector(previousPoint.XPosition - currentPoint.XPosition, previousPoint.YPosition - currentPoint.YPosition);

                Point nextPoint = null;
                int scalarProduct = 0;
                for (int i = 1; i < points.Count; i++)
                {
                    // Set the second vector, which is currentPoint -> points[i](potential nextPoint).
                    var secondVector = new Vector(points[i].XPosition - currentPoint.XPosition, points[i].YPosition - currentPoint.YPosition);

                    // Calculate the current scalar product.
                    var tempScalarProduct = (firstVector.XValue * secondVector.XValue) + (firstVector.YValue * secondVector.YValue);

                    // If there's currently no next Point or the current scalar product is smaller, set nextPoint to point[i].
                    if (nextPoint == null || tempScalarProduct < scalarProduct)
                    {
                        nextPoint = points[i];
                        scalarProduct = tempScalarProduct;
                    }
                }

                // Shift points and add/remove them from lists.
                previousPoint = currentPoint;
                currentPoint = nextPoint;
                points.Remove(nextPoint);
                giftWrap.Add(nextPoint);
                // Check if the gift wrap is completed.
                if (nextPoint == giftWrap[0])
                {
                    notWrapped = false;
                }
            }

            return giftWrap;
        }
    }

    public class Point
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public Point(int xPosition, int yPosition)
        {
            this.XPosition = xPosition;
            this.YPosition = yPosition;
        }
    }
}
