// Declare classes
public class Node
{
    public string Name; // get set means I can get to read the variable and set to write it
    public List<Edge> Neighbours = new List<Edge>(); //list of neighbours(other nodes connected by edge)
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
}

