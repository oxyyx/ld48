using System.Collections;
using UnityEngine;

public class Button : PlayerInteractable {
	[SerializeField]
	[Tooltip("Time between allowed uses in seconds. Only works if not a single use button.")]
	private float resetTime = 0.5f;
	[SerializeField]
	private bool singleUse = true;

	private Animator animator;

	private bool isUsable;
	private bool IsUsable {
		get {
			return isUsable;
		}
		set {
			isUsable = value;
			UpdateAnimator();
		}
	}

	private void Awake() {
		animator = GetComponent<Animator>();

		IsUsable = true;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		PlayerCharacter playerCharacter = collision.GetComponent<PlayerCharacter>();
		if(playerCharacter != null) {
			playerCharacter.RegisterInteractableInRange(this);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		PlayerCharacter playerCharacter = collision.GetComponent<PlayerCharacter>();
		if(playerCharacter != null) {
			playerCharacter.UnregisterInteractableInRange(this);
		}
	}

	public override void Activate() {
		if(!IsUsable) {
			return;
		}

		base.Activate();

		IsUsable = false;

		if(!singleUse) {
			StartCoroutine(ResetUseCoroutine());
		}
	}	

	private void UpdateAnimator() {
		animator.SetBool("IsPressed", IsUsable);
	}

	private IEnumerator ResetUseCoroutine() {
		yield return new WaitForSeconds(resetTime);

		IsUsable = true;
	}
}
