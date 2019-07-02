using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controll : MonoBehaviour {
	public GameObject world;
	public CellBehaviour prefub;
	public EnemyBehaviour walker;
	public Text info;

	private IEnumerable<Node> way;
	private CellBehaviour[,] arr;

	void Awake() {
		singleton data = singleton.getInstance();

		int cellCount = data.cellCount;
		float size = prefub.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * prefub.transform.localScale.x;
		Vector3 start = new Vector3(cellCount * size / 2.0f - size / 2, 0.1f, cellCount * size / 2.0f - size / 2);

		arr = new CellBehaviour[cellCount, cellCount];

		for (int i = 0; i<cellCount; i++) {
			for (int j = 0; j<cellCount; j++) {
				GameObject obj = Instantiate(prefub.gameObject);
				arr[i, j] = obj.GetComponent<CellBehaviour>();
				obj.transform.SetParent(world.transform);
				obj.transform.localPosition = start;

				data.navigate.SetPoint(i, j, obj.transform);
				start.x -= size;
			}
			start.z -= size;
			start.x = cellCount* size / 2 - size/2;
		}

		data.navigate.CreateNavMesh();

		for (int i = 0; i<cellCount; i++) {
			for (int j = 0; j<cellCount; j++) {
				arr[i, j].SetNode(data.navigate.GetNode(i, j));
			}
		}

		for (int i = 0; i<data.enemyCount; i++) {
			Vector3 position = data.navigate.GetNode(Random.Range(0, cellCount - 1), Random.Range(0, cellCount - 1)).position;
			EnemyBehaviour agent = Instantiate(walker.gameObject, position, Quaternion.identity).GetComponent<EnemyBehaviour>();
			data.enemys.Add(agent);
		}

		info.text = string.Format("world side {0}\nenemy count {1}", data.cellCount, data.enemyCount);
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {//emerge cube
			RaycastHit hit;
			CellBehaviour select = FindGameObjectUnderMouse<CellBehaviour>(out hit);
			if (select != null) {
				select.SetBarrier();
			}
		}

		if (Input.GetMouseButtonDown(1)) {//set as point
			RaycastHit hit;
			CellBehaviour select = FindGameObjectUnderMouse<CellBehaviour>(out hit);
			if (select != null) {
				select.Select();
			}
		}
	}

	private T FindGameObjectUnderMouse<T>(out RaycastHit hit) {		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);   
		Physics.Raycast(ray, out hit);
		if (hit.collider != null) {
			return hit.collider.gameObject.GetComponent<T>();
		}
		else {
			return default(T);
		}
	}
}
