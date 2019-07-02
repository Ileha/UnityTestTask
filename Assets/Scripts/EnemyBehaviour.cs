using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Agent
{
	public const string WALK_VALUE = "walk";

	public Animator animator;

	private Node selectNode;
	private bool walk = true;

	protected override void OnUpdate() {
		if (walk && (selectNode == null || !singleton.getInstance().SelectedNodes.Contains(selectNode)))
		{
			try {
				selectNode = singleton.getInstance().GetRandom();
				SetPosition(selectNode);
				animator.SetBool(WALK_VALUE, true);
			}
			catch (System.Exception err) {}
		}
	}

	public void StayAt(Node node) {
		walk = false;
        SetPosition(node);
		animator.SetBool(WALK_VALUE, true);
	}
	public void StayAt() {
		walk = false;
		Stop();
		animator.SetBool(WALK_VALUE, false);
	}

	public void FreeWalk() {
		selectNode = null;
		walk = true;
	}

	protected override void OnReachDestination() {
		selectNode = null;
		if (!walk) {
			animator.SetBool(WALK_VALUE, false);
		}
	}

	protected override void OnWayBlocked() {
		selectNode = null;
		if (!walk) {
			animator.SetBool(WALK_VALUE, false);
		}
	}
}

