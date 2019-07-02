using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node {
	public int value;
	public bool isBarrier;
	public Vector3 position { 
		get {
			return target.position;	
		}
	}

	public IEnumerable<Node> neighbours { get; private set; }

	private Transform target;

	public Node(Transform position) {
		target = position;
		isBarrier = false;
		value = 0;
	}

	public void SetNeighbours(IEnumerable<Node> neighbours) {
		this.neighbours = neighbours;
	}

	public override int GetHashCode() {
		return position.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj == null) { return false; }
		if (!(obj is Node)) { return false; }

		Node other = obj as Node;
		if (other == this) { return true; }

		return position.x == other.position.x
			&& position.y == other.position.y
			&& position.z == other.position.z;
	}

	public override string ToString() {
		return string.Format("[Node: position={0}, neighbours={1}]", position, neighbours.Count());
	}
}
