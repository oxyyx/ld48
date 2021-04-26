using UnityEngine;

public class Hatch : Interactable
{
	[SerializeField]
	private bool startOpen = false;

	private Animator animator;
	private Collider2D collider;

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
	}

	private void UpdateAnimation() {
		animator.SetBool("IsOpen", isOpen);
		collider.enabled = !isOpen;
	}
}
