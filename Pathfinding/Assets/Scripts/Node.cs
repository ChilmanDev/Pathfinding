using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{

	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;

	public int weight;
	private int weightMultip = 50;

	public int gCost;
	public int hCost;
	public Node parent;

	int heapIndex;

	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _weight)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
		weight = _weight;
	}

	public int fCost
	{
		get
		{
			return (gCost + hCost) + weight * weightMultip;
		}
	}

	public int HeapIndex
    {
        get
        {
			return heapIndex;
        }
        set
        {
			heapIndex = value;
		}
    }

	public int CompareTo(Node nodeToCompare)
    {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if(compare == 0)
        {
			compare = hCost.CompareTo(nodeToCompare.hCost);
        }
		return -compare;
    }
}