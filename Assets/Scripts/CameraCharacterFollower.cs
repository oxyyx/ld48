using UnityEngine;

public class CameraCharacterFollower : MonoBehaviour
{
	[SerializeField]
	private PlayerCharacter target;

	private void Update() {
		Vector2 position = transform.position;
		transform.position = new Vector3(position.x, target.transform.position.y, -10);
	}
}
