using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPrototype
{
    public class Node
    {
        public string Name;
        public List<Edge> Neighbours = new List<Edge>();

        public int Row;
        public int Column;
        public double Easting;
        public double Northing;
        public string? Terrain;
        public double? ElevationMetres;
        public bool IsTraversable = true;

        public Node(string name)
        {
            Name = name;
        }
    }
}
