using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField]
	private int health = 100;
	[SerializeField]
	private float horizontalMovementSpeed = 3;
	[SerializeField]
	private float verticalMovementSpeed = 0;
	[SerializeField]
	private float jumpingForce = 10;

	[SerializeField]
	private PlayerCollisionSensor ceilingSensor;
	[SerializeField]
	private PlayerCollisionSensor floorSensor;

	private event Action<int> healthChanged;
	public event Action<int> HealthChanged {
		add {
			healthChanged += value;
		}
		remove {
			healthChanged -= value;
		}
	}

	public int Health {
		get {
			return health;
		}
		set {
			health = value;

			if(healthChanged != null) {
				healthChanged(health);
			}
		}
	}

	private SpriteRenderer renderer;
	private Rigidbody2D rigidbody;
	private Animator animator;

	private bool isJumping;
	private bool isFalling;
	private int jumpObstacles;
	private bool isSwimming;

	private void Awake() {
		isJumping = false;
		isFalling = false;
		jumpObstacles = 0;
		isSwimming = false;

		renderer = GetComponent<SpriteRenderer>();
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		ceilingSensor.CollisionStart += (collision) => { jumpObstacles++; };
		ceilingSensor.CollisionEnd += (collision) => { jumpObstacles--; };
		floorSensor.CollisionStart += (collision) => { isJumping = false; };
		floorSensor.WaterEnter += (collision) => { isSwimming = true; };
		floorSensor.WaterExit += (collision) => { isSwimming = false; };
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
		Vector2 movementVector = new Vector2 { x = amount * horizontalMovementSpeed, y = 0 };
		transform.position = (Vector2)transform.position + movementVector;
		
		if(movementVector.x > 0 && renderer.flipX == true) {
			GetComponent<SpriteRenderer>().flipX = false;
		}
		else if(movementVector.x < 0 && renderer.flipX == false) {
			GetComponent<SpriteRenderer>().flipX = true;
		}

		const string walkingAnimationParameter = "HasHorizontalMovement";
		if(Array.Exists(animator.parameters, element =>  element.name == walkingAnimationParameter)) {
			animator.SetBool(walkingAnimationParameter, amount != 0);
		}		
	}

	public void TranslateVertical(float amount) {
		if(!isSwimming) {
			return;
		}

		transform.position = (Vector2)transform.position + new Vector2 { x = 0, y = amount * verticalMovementSpeed };
	}

	public void Jump() {
		if(isJumping || isFalling || jumpObstacles > 0 || isSwimming) {
			return;
		}

		rigidbody.AddForce(new Vector2(0, jumpingForce), ForceMode2D.Impulse);
		isJumping = true;
	}
}
