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
        Dictionary<Node, Node?>  previous = new Dictionary<Node, Node?>(); //This is for the previous node in the current shortest path and will then be used for reconstruction of final shortest path //? to make it nullable for Djikstra
        List<Node> unvisited = new List<Node>(); //Unvisited set


        foreach (Node node in Nodes) //Initialises all nodes ready for Djikstra
        {
            distances[node] = double.PositiveInfinity; //This sets every node to infinity as it is initially presumed unreachable until proved otherwise
            previous[node] = null; //This is because no nodes have been reached so there is no path to reconstruct yet
            unvisited.Add(node); //Sets all nodes to unvisited
        }

        distances[start] = 0; //First node is 0 distance from itself


        while(unvisited.Count > 0) //While there are still unvisited nodes
        {
            Node? current = GetClosestNode(unvisited, distances); //Greedy principle

            if (current == end) //If you have already reached the target then there is no need to continue searching
//--------------------------------------------------------------------------------------------------
                break; // I REALISE THIS IS BAD PRACTICE SO SHALL WORK TO REWRITE WITHOUT IT, maybe
//--------------------------------------------------------------------------------------------------

            if (current == null)
//--------------------------------------------------------------------------------------------------
                break; // I REALISE THIS IS BAD PRACTICE SO SHALL WORK TO REWRITE WITHOUT IT, maybe
//--------------------------------------------------------------------------------------------------

            unvisited.Remove(current); //Mark current as visited (actually unmarking as unvisited if you want to get techy sassy with it)

            foreach (Edge edge in current.Neighbours) //Cycle through currently neighbouring nodes
            {
                Node neighbour = edge.Target;

                if (!unvisited.Contains(neighbour)) //Omit visited neighbours
//---------------------------------------------------------------------------------------------------------                
                    continue; // I REALISE THIS IS BAD PRACTICE SO SHALL WORK TO REWRITE WITHOUT IT, maybe
//---------------------------------------------------------------------------------------------------------

                double newDistance = distances[current] + edge.Weight; //Node start -> Current -> Neighbour distance

                if (newDistance < distances[neighbour]) //Edge relaxation
                {
                    distances[neighbour] = newDistance; //If a shorter route IS found, update values
                    previous[neighbour] = current;
                }
            }
        }



        List<Node> path = new List<Node>(); //Final path for reconstruction

        if (distances[end] == int.MaxValue) //If the distances are still PositiveInfinity then there is no path between the start and end nodes, therefore nothing can be returned
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

