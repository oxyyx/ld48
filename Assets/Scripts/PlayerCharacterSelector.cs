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

		if(Input.GetButtonDown("Jump")) {
			activePlayerCharacter.Jump();
		}
	}

	private void SwitchActiveCharacter() {
		if(activePlayerCharacter == indianaDeeper) {
			activePlayerCharacter = diverDeeper;
		}
		else {
			activePlayerCharacter = indianaDeeper;
		}
	}
}
