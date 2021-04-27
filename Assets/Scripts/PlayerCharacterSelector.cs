using UnityEngine;

public class PlayerCharacterSelector : MonoBehaviour
{
	[SerializeField]
	private PlayerCharacter indianaDeeper;
	[SerializeField]
	private PlayerCharacter diverDeeper;

	private PlayerCharacter activePlayerCharacter;

	private void Awake() {
		activePlayerCharacter = indianaDeeper;
	}

	private void Update() {
		if(Input.GetButtonDown("SwitchCharacter")) {
			SwitchActiveCharacter();
		}

		float horizontalMovement = Input.GetAxisRaw("Horizontal");
		activePlayerCharacter.TranslateHorizontal(Time.deltaTime * horizontalMovement);
		float verticalMovement = Input.GetAxisRaw("Vertical");
		activePlayerCharacter.TranslateVertical(Time.deltaTime * verticalMovement);

		if(Input.GetButtonDown("Jump")) {
			activePlayerCharacter.Jump();
		}

		if(Input.GetButtonDown("Interact")) {
			activePlayerCharacter.Interact();
		}

		if (Input.GetButtonDown("Attack"))
		{
			activePlayerCharacter.Attack();
		}

		if(Input.GetButtonDown("Exit")) {
			Application.Quit();
		}
	}

	private void SwitchActiveCharacter() {
		RelativePositionIndicator positionIndicator = activePlayerCharacter.GetComponentInChildren<RelativePositionIndicator>(true);
		if(positionIndicator != null) {
			positionIndicator.gameObject.SetActive(false);
		}

		if(activePlayerCharacter == indianaDeeper) {
			activePlayerCharacter = diverDeeper;
		}
		else {
			activePlayerCharacter = indianaDeeper;
		}

		positionIndicator = activePlayerCharacter.GetComponentInChildren<RelativePositionIndicator>(true);
		if(positionIndicator != null) {
			positionIndicator.gameObject.SetActive(true);
		}
	}
}
