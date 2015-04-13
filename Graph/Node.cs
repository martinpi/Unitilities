using System.Collections;
using System.Collections.Generic;

public class NodeList<T> : List<Node<T>>
{
    public NodeList() : base() { }

    public NodeList(int initialSize)
    {
        for (int i = 0; i < initialSize; i++)
            base.Add(default(Node<T>));
    }

    public Node<T> FindByValue(T value)
    {
        for (int i = 0; i < Count; i++) {
            if (base[i].Value.Equals(value)) return base[i];
        }

        // foreach (Node<T> node in Items)
        //     if (node.Value.Equals(value))
        //         return node;

        return null;
    }
}

public class Node<T>
{
    private T data;
    private NodeList<T> neighbors = null;

    public Node() {}
    public Node(T data) : this(data, null) {}
    public Node(T data, NodeList<T> neighbors)
    {
        this.data = data;
        this.neighbors = neighbors;
    }

    public T Value
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
        }
    }

    protected NodeList<T> Neighbors
    {
        get
        {
            return neighbors;
        }
        set
        {
            neighbors = value;
        }
    }
}
