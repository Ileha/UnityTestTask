using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigate {
	private Node[,] world;
	private Transform[,] positions;

	public Navigate(int x, int y) {
		world = new Node[x,y];
		positions = new Transform[x, y];
	}

	public void SetPoint(int x, int y, Transform point) {
		positions[x, y] = point;
	}

	public void CreateNavMesh() {
		int x = world.GetLength(0);
		int y = world.GetLength(1);

		for (int i = 0; i<x; i++) {
			for (int j = 0; j<y; j++) {
				world[i, j] = new Node(positions[i, j]);
			}
		}

		for (int i = 0; i<x; i++) {
			for (int j = 0; j<y; j++) {
				List<Node> neighbours = new List<Node>();
				if (i > 0) {
					neighbours.Add(world[i - 1, j]);
				}
				if (i<x-1) {
					neighbours.Add(world[i + 1, j]);
				}
				if (j > 0) {
					neighbours.Add(world[i, j - 1]);
				}
				if (j<y-1) {
					neighbours.Add(world[i, j + 1]);
				}
				world[i, j].SetNeighbours(neighbours);
			}
		}
	}

	public Node GetNode(int x, int y) {
		return world[x, y];
	}

	private void ResetWorld() {
		for (int i = 0; i < world.GetLength(0); i++) {
			for (int j = 0; j < world.GetLength(1); j++) {
				world[i, j].value = 0;
			}
		}
	}
	private Node GetNiarestNode(Transform point) {
		Node node = world[0, 0];
		float distance = (node.position - point.position).magnitude;

		for (int i = 0; i<world.GetLength(0); i++) {
			for (int j = 0; j<world.GetLength(1); j++) {
				float dist = (world[i, j].position - point.position).magnitude;
				if (dist<distance) {
					node = world[i, j];
					distance = dist;
				}
			}
		}
		return node;
	}

	public IEnumerable<Node> GetWay(Transform start, Transform end) {
		Node target = GetNiarestNode(end);
		return GetWay(start, target);
	}

	public IEnumerable<Node> GetWay(Transform start, Node target) {
		Node startNode = GetNiarestNode(start);

		Stack<Node> way = new Stack<Node>();
		if (startNode.Equals(target)) {
			return way;
		}

		ResetWorld();

		int currentBorder = 0;
		int nextBorder = 1;
		List<Node>[] border = new List<Node>[2];
		border[0] = new List<Node>();
		border[1] = new List<Node>();
		border[currentBorder].Add(startNode);
		int nodeDistance = 1;

		while (border[currentBorder].Count > 0) {
			foreach (Node node in border[currentBorder]) {
				node.value = nodeDistance;
				if (node == target) {
					goto LoopEnd;
				}
				foreach (Node neighbour in node.neighbours) {
					if (neighbour.value == 0 && neighbour.isBarrier == false) {
						border[nextBorder].Add(neighbour);
					}
				}
			}
			border[currentBorder].Clear();
			nodeDistance++;
			currentBorder = nextBorder;
			nextBorder = (nextBorder + 1) % border.Length;
		}
		throw new Exception("way not found");
	LoopEnd:;


		way.Push(target);

		while (true) {
			nodeDistance = way.Peek().value;
			foreach (Node neighbour in way.Peek().neighbours) {
				if (nodeDistance > neighbour.value && neighbour.value != 0) {
					nodeDistance = neighbour.value;
				}
			}
			double dist = double.MaxValue;
			Node forAdd = null;
			foreach (Node neighbour in way.Peek().neighbours) {
				if (nodeDistance == neighbour.value) {
					double timeDist = (neighbour.position - start.position).magnitude;
					if (timeDist < dist) {
						dist = timeDist;
						forAdd = neighbour;
					}
				}
			}
			if (forAdd == startNode) {
				break;
			}
			way.Push(forAdd);
		}
		return way;
	}
}
