using System;
using System.Collections.Generic;
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
	[SerializeField]
	private Weapon weapon;

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

	private int swimmingRegistrations;
	private bool IsSwimming {
		get { return swimmingRegistrations > 0; }
	}

	private int headInWaterRegistrations;
	private bool HeadIsInWater {
		get { return headInWaterRegistrations > 0; }
	}

	private new SpriteRenderer renderer;
	private new Rigidbody2D rigidbody;
	private Animator animator;

	private bool isJumping;
	private bool isFalling;
	private int jumpObstacles;

	private List<Interactable> interactablesInRange;

	private void Awake() {
		swimmingRegistrations = 0;
		headInWaterRegistrations = 0;
		isJumping = false;
		isFalling = false;
		jumpObstacles = 0;

		interactablesInRange = new List<Interactable>();

		renderer = GetComponent<SpriteRenderer>();
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		ceilingSensor.CollisionStart += (collision) => { jumpObstacles++; };
		ceilingSensor.CollisionEnd += (collision) => { jumpObstacles--; };
		ceilingSensor.WaterEnter += (collision) => { headInWaterRegistrations++; };
		ceilingSensor.WaterExit += (collision) => { headInWaterRegistrations--; };
		floorSensor.CollisionStart += (collision) => { isJumping = false; };
		floorSensor.WaterEnter += (collision) => { swimmingRegistrations++; };
		floorSensor.WaterExit += (collision) => { swimmingRegistrations--; };
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
			renderer.flipX = false;
			weapon.FlipX(false);

		}
		else if(movementVector.x < 0 && renderer.flipX == false) {
			renderer.flipX = true;
			weapon.FlipX(true);
		}

		const string walkingAnimationParameter = "HasHorizontalMovement";
		if(Array.Exists(animator.parameters, element =>  element.name == walkingAnimationParameter)) {
			animator.SetBool(walkingAnimationParameter, amount != 0);
		}		
	}

	public void TranslateVertical(float amount) {
		if(!IsSwimming) {
			return;
		}

		transform.position = (Vector2)transform.position + new Vector2 { x = 0, y = amount * verticalMovementSpeed };
	}

	public void Jump() {
		// Special case: jumping out of water. Yucky duplicate code but I'm tired and lazy :(
		if(IsSwimming && !HeadIsInWater && !isJumping && jumpObstacles == 0) {
			rigidbody.AddForce(new Vector2(0, jumpingForce), ForceMode2D.Impulse);
			isJumping = true;
			Debug.Log("Jumping out of water!");
			return;
		}

		if(isJumping || isFalling || jumpObstacles > 0 || IsSwimming) {
			return;
		}

		Debug.Log("Jumping!");
		rigidbody.AddForce(new Vector2(0, jumpingForce), ForceMode2D.Impulse);
		isJumping = true;
	}

	public void Interact() {
		foreach(Interactable interactable in interactablesInRange) {
			interactable.Activate();
		}
	}

	public void Attack()
	{
		weapon.Fire();
	}

	public void RegisterInteractableInRange(PlayerInteractable interactable) {
		if(interactablesInRange.Contains(interactable)) {
			return;
		}

		interactablesInRange.Add(interactable);
	}

	public void UnregisterInteractableInRange(PlayerInteractable interactable) {
		interactablesInRange.Remove(interactable);
	}
}
