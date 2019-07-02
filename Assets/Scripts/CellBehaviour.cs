using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum state {
	normal,
	selected,
	barrier
}

public class CellBehaviour : MonoBehaviour {
	public Material normal;
	public Material target;

	public GameObject barrier;

	private Renderer render;
	private state currentState = state.normal;
	private Node currentNode;

	// Use this for initialization
	void Start () {
		render = gameObject.GetComponent<Renderer>();
		render.sharedMaterial = normal;
		barrier.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetNode(Node node) {
		currentNode = node;
	}

	public void Select() {
		if (currentState == state.normal) {
			render.sharedMaterial = target;
			currentState = state.selected;
			singleton.getInstance().SelectedNodes.Add(currentNode);
		}
		else if(currentState == state.selected) {
			render.sharedMaterial = normal;
			currentState = state.normal;
			singleton.getInstance().SelectedNodes.Remove(currentNode);
		}
	}

	public void SetBarrier() {
		if (currentState == state.normal) {
			currentState = state.barrier;
			barrier.SetActive(true);
			currentNode.isBarrier = true;
		}
		else if(currentState == state.barrier) {
			currentState = state.normal;
			barrier.SetActive(false);
			currentNode.isBarrier = false;
		}
	}
}
