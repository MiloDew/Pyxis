public class Node
{
    public string Name;
    public List<Edge> Neighbours = new List<Edge>();
    public Node(string name)
    {
        Name = name;
    }
}






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








public class Graph
{


    public List<Node> Nodes = new List<Node>();
    public void AddNode (Node node)
    {
        Nodes.Add(node);
    }


    
    public void AddEdge(Node from, Node to, double weight)
    {
        from.Neighbours.Add(new Edge(to,weight));

    }


    private Node GetClosestNode(List<Node> unvisited, Dictionary<Node, double> distances)
    {
        Node closest = null;
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
        Dictionary<Node, Node>  previous = new Dictionary<Node, Node>(); //This is for the previous node in the current shortest path and will then be used for reconstruction of final shortest path
        List<Node> unvisited = new List<Node>(); //Unvisited set


        foreach (Node node in Nodes) //Initialises all nodes ready for Djikstra
        {
            distances[node] = double.PositiveInfinity; //This sets every node to infinity as it is initially presumed unreachable until proved otherwise
            previous[node] = null; //This is because no nodes have been reached so there is no path to reconstruct yet
            unvisited.Add(node); //Sets all nodes to unvisited
        }

        distances[start] = 0; //First node is 0 distance from itself


        while(unvisited.Count > 0)
        {
            Node current = GetClosestNode(unvisited, distances);

            if (current == end)
                break;

            unvisited.Remove(current);

            foreach (Edge edge in current.Neighbours)
            {
                Node neighbour = edge.Target;

                if (!unvisited.Contains(neighbour))
                    continue;
                
                double newDistance = distances[current] + edge.Weight;

                if (newDistance < distances[neighbour])
                {
                    distances[neighbour] = newDistance;
                    previous[neighbour] = current;
                }
            }
        }

        List<Node> path = new List<Node>();
        Node step = end;

        while (step != null)
        {
            path.Insert(0, step);
            step = previous[step];
        }

        return path;
    }
}

