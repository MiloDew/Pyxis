using System;
using System.Collections.Generic;
using System.Text;

namespace ParsingPrototype
{
    public class Graph
    {


        public List<Node> Nodes = new List<Node>();
        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }


        public void AddEdge(Node from, Node to, double weight)
        {
            from.Neighbours.Add(new Edge(to, weight));

        }


        private Node? GetClosestNode(List<Node> unvisited, Dictionary<Node, double> distances) //?Null allowed because no closest node COULD happen
        {
            Node? closest = null; //Question mark makes sure that if Node is null, it isn't set to null which shouldn't happen
            double minDistance = double.PositiveInfinity;

            foreach (Node node in unvisited)
            {
                if (distances[node] < minDistance)
                {
                    minDistance = distances[node];
                    closest = node;
                }
            }
            return closest;
        }


        public List<Node> Dijkstra(Node start, Node end) //Final shortest path between Node start, Node end
        {
            Dictionary<Node, double> distances = new Dictionary<Node, double>(); //This is for the current shortest distance from the starting node
            Dictionary<Node, Node?> previous = new Dictionary<Node, Node?>(); //This is for the previous node in the current shortest path and will then be used for reconstruction of final shortest path //? to make it nullable for Djikstra
            List<Node> unvisited = new List<Node>(); //Unvisited set


            foreach (Node node in Nodes) //Initialises all nodes ready for Djikstra
            {
                distances[node] = double.PositiveInfinity; //This sets every node to infinity as it is initially presumed unreachable until proved otherwise
                previous[node] = null; //This is because no nodes have been reached so there is no path to reconstruct yet
                unvisited.Add(node); //Sets all nodes to unvisited
            }

            distances[start] = 0; //First node is 0 distance from itself

            bool endUnreached = true; //This breaks the loop if the end is reached, replacing an if break that was in the loop
            bool notNull = true; // Same as endUnreached but if node is null which means end is unreachable, also replacing a break statement
            while ((unvisited.Count > 0) && endUnreached && notNull) //While there are still unvisited nodes
            {
                Node? current = GetClosestNode(unvisited, distances); //Greedy principle

                if (current != end)
                {
                    if (current != null) //If null then the path is unreachable, because there have been no found unvisited nodes. This is because you can't validate the feasability of a path until Dijkstra has been run.
                    {
                        unvisited.Remove(current);

                        foreach (Edge edge in current.Neighbours)
                        {
                            Node neighbour = edge.Target;
                            if (unvisited.Contains(neighbour)) // Replacing continue with if, this makes it so that if something hasn't been checked it'll run but if it hasnt then there is no point
                            {
                                double newDistance = distances[current] + edge.Weight;

                                if (newDistance < distances[neighbour])
                                {
                                    distances[neighbour] = newDistance;
                                    previous[neighbour] = current;
                                }
                            }

                        }
                    }
                    else
                    {
                        notNull = false; //Loop break
                    }
                }
                else
                {
                    endUnreached = false; //Loop break
                }

            }


            List<Node> path = new List<Node>(); //Final path for reconstruction

            if (distances[end] == Double.PositiveInfinity) //If the distances are still PositiveInfinity then there is no path between the start and end nodes, therefore nothing can be returned.
            {
                return path; //Return the path empty before the reconstruction can attempt to occur
            }

            Node? step = end; //? nullability
            while (step != null) //Path reconstruction (backwards because we worked the other way figuring out distances)
            {
                path.Insert(0, step);
                step = previous[step];
            }

            return path; //Bingo bango bongo, bish bash bosh
        }
    }

}
