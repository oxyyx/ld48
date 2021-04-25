using System.Collections.Generic;
using UnityEngine;

public class WaterVolume : MonoBehaviour
{
	[SerializeField]
	private float gravityScale = 0.25f;
	[SerializeField]
	private float linearDrag = 1;

	private class RigidBodyConfig {
		public float GravityScale { get; set; }
		public float LinearDrag { get; set; }

		public RigidBodyConfig() {

		}

		public RigidBodyConfig(Rigidbody2D rigidBody) {
			GravityScale = rigidBody.gravityScale;
			LinearDrag = rigidBody.drag;
		}

		public void Apply(Rigidbody2D rigidBody) {
			rigidBody.gravityScale = GravityScale;
			rigidBody.drag = LinearDrag;
		}
	}

	private RigidBodyConfig underwaterConfig;
	private Dictionary<GameObject, RigidBodyConfig> storedConfigs;

	private void Awake() {
		underwaterConfig = new RigidBodyConfig { GravityScale = gravityScale, LinearDrag = linearDrag };
		storedConfigs = new Dictionary<GameObject, RigidBodyConfig>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		Rigidbody2D rigidBody = collision.GetComponent<Rigidbody2D>();
		if(rigidBody == null) {
			return;
		}

		StoreRigidBodyConfig(rigidBody);

		underwaterConfig.Apply(rigidBody);
	}

	private void OnTriggerExit2D(Collider2D collision) {
		GameObject collisionObject = collision.gameObject;

		if(storedConfigs.ContainsKey(collisionObject)) {
			RigidBodyConfig config = storedConfigs[collisionObject];
			config.Apply(collisionObject.GetComponent<Rigidbody2D>());

			storedConfigs.Remove(collisionObject);
		}
	}

	private void StoreRigidBodyConfig(Rigidbody2D rigidBody) {
		if(storedConfigs.ContainsKey(rigidBody.gameObject)) {
			return;
		}

		RigidBodyConfig config = new RigidBodyConfig(rigidBody);
		storedConfigs.Add(rigidBody.gameObject, config);
	}
}
