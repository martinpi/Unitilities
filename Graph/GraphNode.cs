using System.Collections.Generic;

public class GraphNode<T> : Node<T>
{
    private List<float> costs;

    public GraphNode() : base() { }
    public GraphNode(T value) : base(value) { }
    public GraphNode(T value, NodeList<T> neighbors) : base(value, neighbors) { }

    new public NodeList<T> Neighbors
    {
        get
        {
            if (base.Neighbors == null)
                base.Neighbors = new NodeList<T>();

            return base.Neighbors;
        }            
    }

    public List<float> Costs
    {
        get
        {
            if (costs == null)
                costs = new List<float>();

            return costs;
        }
    }
}
