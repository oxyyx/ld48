using UnityEngine;

public class Hatch : Interactable
{
	[SerializeField]
	private bool startOpen = false;

	private Animator animator;

	private bool isOpen;

	private void Awake() {
		animator = GetComponent<Animator>();

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
	}
}
