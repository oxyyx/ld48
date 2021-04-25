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

	private event Action<Collider2D> waterEnter;
	public event Action<Collider2D> WaterEnter {
		add {			
			waterEnter += value;
		}
		remove {
			waterEnter -= value;
		}
	}

	private event Action<Collider2D> waterExit;
	public event Action<Collider2D> WaterExit {
		add {			
			waterExit += value;
		}
		remove {
			waterExit -= value;
		}
	}


	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.GetComponent<WaterVolume>() != null) {
			if(waterEnter != null) {
				waterEnter(collision);
			}
			return;
		}

		if(collisionStart != null) {
			collisionStart(collision);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if(collision.GetComponent<WaterVolume>() != null) {
			if(waterExit != null) {
				waterExit(collision);
			}
			return;
		}

		if(collisionEnd != null) {
			collisionEnd(collision);
		}
	}
}
