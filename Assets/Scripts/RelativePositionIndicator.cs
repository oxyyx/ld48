using System;
using UnityEngine;

public class RelativePositionIndicator : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Defaults to parent object if left empty.")]
	private Transform source;
    [SerializeField]
    private Transform target;
	[SerializeField]
	private float displayDistance = 1.5f;

	private void Awake() {
		if(source == null && transform.parent != null) {
			source = transform.parent;
		}
	}

	private void Update() {
		Vector2 differenceVector = target.position - source.position;
		Vector2 direction = Vector3.Normalize(target.position - source.position);

		float magnitude = displayDistance;

		if(differenceVector.magnitude < 2 * displayDistance) {
			magnitude = 0.5f * differenceVector.magnitude;
		}

		transform.localPosition = magnitude * direction;
		transform.up = direction;
	}
}
