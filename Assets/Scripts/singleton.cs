using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class singleton {
	public const int CELL_MIN = 5;
	public const int CELL_MAX = 10;

	public const int ENEMY_MIN = 1;
	public const int ENEMY_MAX = 5;

	private static singleton instance;

	public Navigate navigate { get; private set; }
	public int cellCount { get; private set; }
	public int enemyCount { get; private set; }
	public HashSet<Node> SelectedNodes { get; private set; }
	public List<EnemyBehaviour> enemys { get; private set; }

	private singleton() {
		cellCount = Random.Range(CELL_MIN, CELL_MAX);
		enemyCount = Random.Range(ENEMY_MIN, ENEMY_MAX);
		navigate = new Navigate(cellCount, cellCount);
		SelectedNodes = new HashSet<Node>();
		enemys = new List<EnemyBehaviour>();
	}

	public static singleton getInstance() {
		if (instance == null) {
			instance = new singleton();
		}
		return instance;
	}

	public Node GetRandom() {
		if (SelectedNodes.Count == 0) {
			throw new System.Exception("empty");
		}
		int skip = Random.Range(0, SelectedNodes.Count);
		return SelectedNodes.Skip(skip).First();
	}
}

public class NodeSet {

}