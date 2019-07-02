using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Agent : MonoBehaviour {
	public float speed = 6.0F;
	public float rotationSpeed = 3.0F;
	public float gravity = 20.0F;
	public float accuracy = 0.1f;

	private Coroutine currentMove;

	// Use this for initialization
	void Start () {
		OnStart();
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Vector3 startPoint = transform.position + transform.up;
		Physics.Raycast(startPoint, transform.InverseTransformDirection(Vector3.down), out hit);

		Vector3 movieDirection = (hit.point - transform.position);
		if (movieDirection.magnitude > 1) {
			movieDirection.Normalize();
		}
		transform.position += movieDirection * Time.deltaTime * gravity;
		OnUpdate();
	}

	protected virtual void OnUpdate() {}
	protected virtual void OnStart() {}
	protected virtual void OnReachDestination() {}
	protected virtual void OnWayBlocked() {}

	private IEnumerator MoveByPoints(Func<IEnumerator<Node>> getWay) {
		Vector3 distance;
		IEnumerator<Node> points = getWay();

		while(points != null && points.MoveNext()) {
			do {
				distance = points.Current.position - transform.position;
				distance.y = 0;
				transform.position += (distance.normalized* Time.deltaTime * speed);

				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance), Time.deltaTime*rotationSpeed);
				yield return null;
			} while (distance.magnitude > accuracy);
			points = getWay();
		}

		currentMove = null;

		if (points != null) {
			OnReachDestination();
		}
		else {
			OnWayBlocked();
		}
	}

	private IEnumerator<Node> GetWay(Node target) {
		try {
			return singleton.getInstance().navigate.GetWay(transform, target).GetEnumerator();
		}
		catch (Exception Err) {
			return null;
		}
	}

	public void SetPosition(Node target) {
		if (currentMove != null) {
			Stop();
		}
		currentMove = StartCoroutine(MoveByPoints(() => GetWay(target)));
	}

	public void Stop() {
		StopCoroutine(currentMove);
		currentMove = null;
	}
}
