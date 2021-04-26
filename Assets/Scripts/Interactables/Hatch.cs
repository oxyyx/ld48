using System.Collections;
using UnityEngine;

public class Hatch : Interactable
{
	[SerializeField]
	private bool startOpen = false;
	[SerializeField]
	private bool closesAutomatically = false;
	[SerializeField]
	private float autoCloseTime = 1.0f;

	private Animator animator;
	private new Collider2D collider;

	private bool isOpen;

	private void Awake() {
		animator = GetComponent<Animator>();
		collider = GetComponent<Collider2D>();

		isOpen = startOpen;

		UpdateAnimation();
	}

	public override void Activate() {
		base.Activate();

		isOpen = !isOpen;
		UpdateAnimation();

		if(isOpen && closesAutomatically) {
			StartCoroutine(AutoCloseCoRoutine());
		}
	}

	private IEnumerator AutoCloseCoRoutine() {
		yield return new WaitForSeconds(autoCloseTime);

		if(isOpen) {
			Activate();
		}
	}

	private void UpdateAnimation() {
		animator.SetBool("IsOpen", isOpen);
		collider.enabled = !isOpen;
	}
}
