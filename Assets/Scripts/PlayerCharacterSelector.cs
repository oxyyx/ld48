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

		float horizontalMovement = Input.GetAxis("Horizontal");
		activePlayerCharacter.TranslateHorizontal(Time.deltaTime * horizontalMovement);
		float verticalMovement = Input.GetAxis("Vertical");
		activePlayerCharacter.TranslateVertical(Time.deltaTime * verticalMovement);
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
