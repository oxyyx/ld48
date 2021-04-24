using System;
using UnityEngine;

public class PlayerCollisionSensor : MonoBehaviour
{
	private event Action<Collider2D> collisionStart;
	public event Action<Collider2D> CollisionStart {
		add {
			collisionStart += value;
		}
		remove {
			collisionStart -= value;
		}
	}

	private event Action<Collider2D> collisionEnd;
	public event Action<Collider2D> CollisionEnd {
		add {
			collisionEnd += value;
		}
		remove {
			collisionEnd -= value;
		}
	}


	private void OnTriggerEnter2D(Collider2D collision) {
		if(collisionStart != null) {
			collisionStart(collision);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if(collisionEnd != null) {
			collisionEnd(collision);
		}
	}
}
