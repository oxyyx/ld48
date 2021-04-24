using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField]
	private float movementSpeed = 1;
	[SerializeField]
	private float jumpingForce = 10;

	[SerializeField]
	private PlayerCollisionSensor ceilingSensor;
	[SerializeField]
	private PlayerCollisionSensor floorSensor;

	private SpriteRenderer renderer;
	private Rigidbody2D rigidbody;

	private bool isJumping;
	private bool isFalling;
	private int jumpObstacles;

	private void Awake() {
		isJumping = false;
		isFalling = false;
		jumpObstacles = 0;

		renderer = GetComponent<SpriteRenderer>();
		rigidbody = GetComponent<Rigidbody2D>();

		ceilingSensor.CollisionStart += (collision) => { jumpObstacles++; };
		ceilingSensor.CollisionEnd += (collision) => { jumpObstacles--; };
		floorSensor.CollisionStart += (collision) => { isJumping = false; };

	}

	private void Update() {
		float verticalVelocity = rigidbody.velocity.y;

		if(verticalVelocity < 0.0f) {
			isFalling = true;

			if(isJumping) {
				isJumping = false;
			}
		}
		else {
			isFalling = false;
		}
	}

	public void TranslateHorizontal(float amount) {
		Vector2 movementVector = new Vector2 { x = amount * movementSpeed, y = 0 };
		transform.position = (Vector2)transform.position + movementVector;
		
		if(movementVector.x > 0 && renderer.flipX == true) {
			GetComponent<SpriteRenderer>().flipX = false;
		}
		else if(movementVector.x < 0 && renderer.flipX == false) {
			GetComponent<SpriteRenderer>().flipX = true;
		}
	}

	public void TranslateVertical(float amount) {
		transform.position = (Vector2)transform.position + new Vector2 { x = 0, y = amount * movementSpeed };
	}

	public void Jump() {
		if(isJumping || isFalling || jumpObstacles > 0) {
			return;
		}

		rigidbody.AddForce(new Vector2(0, jumpingForce), ForceMode2D.Impulse);
		isJumping = true;
	}
}
