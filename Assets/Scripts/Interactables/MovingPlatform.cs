using UnityEngine;

public class MovingPlatform : Interactable
{
    [SerializeField]
    [Tooltip("If possible, defaults to a child object with the name \"MovementTarget\" when left empty/null")]
    private Transform targetPositionIndicator;
    [SerializeField]
    private float movementSpeed = 1.0f;
    [SerializeField]
    private bool startsActive = false;

    private Vector2[] movementPositions;
    private bool isActive;
    private int currentTargetPositionIndex;

    private Vector2 CurrentTargetPosition {
		get { return movementPositions[currentTargetPositionIndex]; }
	}

	private void Awake() {
		if(targetPositionIndicator == null) {
            Transform defaultMovementTarget = transform.Find("MovementTarget");
			if(defaultMovementTarget != null) {
                targetPositionIndicator = defaultMovementTarget;
			}
		}

        isActive = startsActive ? true : false;
        movementPositions = new Vector2[2] { transform.position, targetPositionIndicator.position };
        currentTargetPositionIndex = 1;
    }

	private void Update() {
		if(!isActive) {
            return;
		}

        Vector2 movementDirection = Vector3.Normalize(CurrentTargetPosition - (Vector2)transform.position);
        float movementScalar = Time.deltaTime * movementSpeed;
        float currentX = transform.position.x;
        float currentY = transform.position.y;
        float newX = Mathf.Clamp(currentX + (movementScalar * movementDirection.x), Mathf.Min(currentX, CurrentTargetPosition.x), Mathf.Max(currentX, CurrentTargetPosition.x));
        float newY = Mathf.Clamp(currentY + (movementScalar * movementDirection.y), Mathf.Min(currentY, CurrentTargetPosition.y), Mathf.Max(currentY, CurrentTargetPosition.y));
        transform.position = new Vector2(newX, newY);

		if(Mathf.Approximately(newX, CurrentTargetPosition.x) && Mathf.Approximately(newY, CurrentTargetPosition.y)) {
            currentTargetPositionIndex = currentTargetPositionIndex == 0 ? 1 : 0;
		}
	}

	public override void Activate() {
        isActive = !isActive;
	}
}
