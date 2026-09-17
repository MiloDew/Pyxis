using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPrototype
{
    public class Edge
    {
        public Node Target;
        public double Weight;
        public Edge(Node target, double weight)
        {
            Target = target;
            Weight = weight;
        }
    }
}
