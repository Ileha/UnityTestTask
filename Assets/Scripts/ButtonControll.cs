using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControll : MonoBehaviour {
	private Button switchButton;
	private Text title;

	// Use this for initialization
	void Start () {
		switchButton = GetComponent<Button>();
		title = switchButton.GetComponentInChildren<Text>();
		SetFreeWalk();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void SetFreeWalk() {
		switchButton.onClick.RemoveAllListeners();
		title.text = "Stay";
		foreach (EnemyBehaviour enemy in singleton.getInstance().enemys) {
			enemy.FreeWalk();
		}
		switchButton.onClick.AddListener(SetStay);
	}

	private void SetStay() {
		switchButton.onClick.RemoveAllListeners();
		title.text = "Free walk";

		IEnumerator<Node> nodes = singleton.getInstance().SelectedNodes.GetEnumerator();
		foreach (EnemyBehaviour enemy in singleton.getInstance().enemys) {
			if (nodes.MoveNext()) {
				enemy.StayAt(nodes.Current);
			}
			else {
				enemy.StayAt();
			}
		}

		switchButton.onClick.AddListener(SetFreeWalk);
	}
}
