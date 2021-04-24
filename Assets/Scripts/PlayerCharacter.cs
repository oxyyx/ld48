using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField]
	private float movementSpeed = 1;

	public void TranslateHorizontal(float amount) {
		transform.position = (Vector2)transform.position + new Vector2 { x = amount * movementSpeed, y = 0 };
	}

	public void TranslateVertical(float amount) {
		transform.position = (Vector2)transform.position + new Vector2 { x = 0, y = amount * movementSpeed };
	}
}
