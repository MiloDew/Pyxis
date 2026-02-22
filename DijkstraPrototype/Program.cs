//interactive bit
Graph graph = new Graph();

Node A = new Node("A");//Add nodes
Node B = new Node("B");
Node C = new Node("C");
Node D = new Node("D");
Node E = new Node("E");
Node F = new Node("F");
Node G = new Node("G");

graph.Nodes.AddRange(new Node[]{A,B,C,D,E,F,G}); //Add nodes to graph

void AddEdge(Node n1, Node n2, double w) //This makes the edges two way as this implementation of Dijkstra does not account for directionality, more on that later ;)
{
    n1.Neighbours.Add(new Edge(n2, w));
    n2.Neighbours.Add(new Edge(n1, w));
}

AddEdge(D, E, 2); //Add edges
AddEdge(D, A, 4);
AddEdge(E, A, 4);
AddEdge(E, C, 4);
AddEdge(E, G, 5);
AddEdge(A, C, 3);
AddEdge(C, G, 5);
AddEdge(C, F, 5);
AddEdge(C, B, 2);
AddEdge(G, F, 5);
AddEdge(B, F, 2);

List<Node> path = graph.Dijkstra(D,F);
foreach (Node n in path)
{
    Console.Write(n.Name + " ");
}

//Result SHOULD be 10 (decbf)



//Classes blah blah blah
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


/* The result was correct, output as:
dotnet run
D E C B F 
Very happy with this result as it is correct
*/
