using System;
using System.Collections;
using System.Collections.Generic;

// When you implement IEnumerable, you must also implement IEnumerator. 
public class GraphEnum<T> : IEnumerator<T>
{
	public NodeList<T> _nodes;
	int position = -1;

	public GraphEnum(NodeList<T> list) {
		_nodes = list;
	}

	public bool MoveNext() {
		position++;
		return (position < _nodes.Count);
	}

	public void Reset() {
		position = -1;
	}

    void IDisposable.Dispose() { }

	object IEnumerator.Current
	{
		get
		{
			return Current;
		}
	}

	public T Current
	{
		get
		{
			try
			{
				return _nodes[position].Value;
			}
			catch (IndexOutOfRangeException)
			{
				throw new InvalidOperationException();
			}
		}
	}
}


public class Graph<T> : IEnumerable<T>
{
	private NodeList<T> nodeSet;

	public Graph() : this(null) {}
	public Graph(NodeList<T> nodeSet)
	{
		if (nodeSet == null)
			this.nodeSet = new NodeList<T>();
		else
			this.nodeSet = nodeSet;
	}

	public GraphNode<T> AddNode(GraphNode<T> node)
	{
		nodeSet.Add(node);
		return node;
	}

	public GraphNode<T> AddNode(T value)
	{
		return AddNode(new GraphNode<T>(value));
	}

	public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, float cost)
	{
		from.Neighbors.Add(to);
		from.Costs.Add(cost);
	}

	public void AddDirectedEdge(T from, T to, float cost)
	{
		AddDirectedEdge((GraphNode<T>)nodeSet.FindByValue(from),(GraphNode<T>)nodeSet.FindByValue(to),cost);
	}

	public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, float cost)
	{
		from.Neighbors.Add(to);
		from.Costs.Add(cost);

		to.Neighbors.Add(from);
		to.Costs.Add(cost);
	}

	public void AddUndirectedEdge(T from, T to, float cost)
	{
		AddUndirectedEdge((GraphNode<T>)nodeSet.FindByValue(from),(GraphNode<T>)nodeSet.FindByValue(to),cost);
	}

	public IEnumerator<T> GetEnumerator() {
		return new GraphEnum<T>(nodeSet);
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
		return this.GetEnumerator();
	}

    public bool Contains(T value)
    {
		return nodeSet.FindByValue(value) != null;
    }

	public bool Remove(T value)
	{
		 // first remove the node from the nodeset
		 GraphNode<T> nodeToRemove = (GraphNode<T>) nodeSet.FindByValue(value);
		 if (nodeToRemove == null)
			 // node wasn't found
			 return false;

		 // otherwise, the node was found
		 nodeSet.Remove(nodeToRemove);

		 // enumerate through each node in the nodeSet, removing edges to this node
		 foreach (GraphNode<T> gnode in nodeSet)
		 {
			 int index = gnode.Neighbors.IndexOf(nodeToRemove);
			 if (index != -1)
			 {
				 // remove the reference to the node and associated cost
				 gnode.Neighbors.RemoveAt(index);
				 gnode.Costs.RemoveAt(index);
			 }
		 }

		 return true;
	}

	public void Clear() {
		nodeSet.Clear();
	}

	public NodeList<T> Nodes
	{
		get
		{
			return nodeSet;
		}
	}

	public int Count
	{
		get { return nodeSet.Count; }
	}
}