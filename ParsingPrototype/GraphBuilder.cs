using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPrototype
{
    public static class GraphBuilder
    {
        public static Graph BuildGraphFromGrid(List<GridCell> grid)
        {
            var graph = new Graph();
            var nodesByPosition = new Dictionary<(int row, int col), Node>();

            foreach (GridCell cell in grid)
            {
                Node node = new Node($"{cell.Row}_{cell.Column}")
                {
                    Row = cell.Row,
                    Column = cell.Column,
                    Easting = cell.Easting,
                    Northing = cell.Northing
                };
                graph.AddNode(node);
                nodesByPosition[(cell.Row, cell.Column)] = node;
            }

            foreach (GridCell cell in grid)
            {
                Node current = nodesByPosition[(cell.Row, cell.Column)];

                if (nodesByPosition.TryGetValue((cell.Row, cell.Column + 1), out Node? right))
                    ConnectIfTraversable(graph, current, right);

                if (nodesByPosition.TryGetValue((cell.Row + 1, cell.Column), out Node? below))
                    ConnectIfTraversable(graph, current, below);
            }

            return graph;
        }

        private static void ConnectIfTraversable(Graph graph, Node a, Node b)
        {
            if (!a.IsTraversable || !b.IsTraversable)
                return;

            double dx = a.Easting - b.Easting;
            double dy = a.Northing - b.Northing;
            double flatDistance = Math.Sqrt(dx * dx + dy * dy);

            graph.AddEdge(a, b, ComputeWeight(a, b, flatDistance));
            graph.AddEdge(b, a, ComputeWeight(b, a, flatDistance));
        }

        private static double ComputeWeight(Node from, Node to, double flatDistance)
        {
            double elevationChange = 0;
            if (from.ElevationMetres.HasValue && to.ElevationMetres.HasValue)
                elevationChange = to.ElevationMetres.Value - from.ElevationMetres.Value;

            double climbPenalty = Math.Max(0, elevationChange) * 2.0; // placeholder multiplier
            return flatDistance + climbPenalty;
        }
    }
}
