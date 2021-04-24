using System.Collections.Generic;
using UnityEngine;

public class WaterVolume : MonoBehaviour
{
	[SerializeField]
	private float gravityScale = 0.25f;

	private class RigidBodyConfig {
		public float GravityScale { get; set; }

		public RigidBodyConfig() {

		}

		public RigidBodyConfig(Rigidbody2D rigidBody) {
			GravityScale = rigidBody.gravityScale;
		}

		public void Apply(Rigidbody2D rigidBody) {
			rigidBody.gravityScale = GravityScale;
		}
	}

	private RigidBodyConfig underwaterConfig;
	private Dictionary<GameObject, RigidBodyConfig> storedConfigs;

	private void Awake() {
		underwaterConfig = new RigidBodyConfig { GravityScale = gravityScale };
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
		if(storedConfigs.ContainsKey(collision.gameObject)) {
			storedConfigs.Remove(collision.gameObject);
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
