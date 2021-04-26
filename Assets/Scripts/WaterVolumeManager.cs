using System.Collections.Generic;
using System;
using UnityEngine;

public class WaterVolumeManager : MonoBehaviour
{
	public class RigidBodyConfig {
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

		public override string ToString() {
			return String.Format("GravityScale: {0}, LinearDrag: {1}", GravityScale, LinearDrag);
		}
	}

	private Dictionary<GameObject, RigidBodyConfig> storedConfigs;
	private Dictionary<GameObject, Queue<RigidBodyConfig>> queuedRegistrations;

	private void Awake() {
		storedConfigs = new Dictionary<GameObject, RigidBodyConfig>();
		queuedRegistrations = new Dictionary<GameObject, Queue<RigidBodyConfig>>();
	}

	public void RegisterVolumeEntered(Collider2D collision, RigidBodyConfig underwaterConfig) {
		Rigidbody2D rigidBody = collision.GetComponent<Rigidbody2D>();
		if(rigidBody == null) {
			return;
		}

		bool isAlreadyRegistered = storedConfigs.ContainsKey(rigidBody.gameObject);
		if(isAlreadyRegistered) {
			queuedRegistrations[rigidBody.gameObject].Enqueue(underwaterConfig);
		}
		else {
			StoreRigidBodyConfig(rigidBody);
			CreateQueueForRigidBody(rigidBody);

			underwaterConfig.Apply(rigidBody);
		}		
	}

	public void RegisterVolumeExited(Collider2D collision) {
		Rigidbody2D rigidBody = collision.GetComponent<Rigidbody2D>();
		if(rigidBody == null || !storedConfigs.ContainsKey(rigidBody.gameObject)) {
			return;
		}

		RigidBodyConfig config = storedConfigs[rigidBody.gameObject];
		config.Apply(rigidBody);
			

		if(queuedRegistrations[rigidBody.gameObject].Count > 0) {
			RigidBodyConfig queuedConfig = queuedRegistrations[rigidBody.gameObject].Dequeue();
			queuedConfig.Apply(rigidBody);
		}
		else {
			storedConfigs.Remove(rigidBody.gameObject);
			queuedRegistrations.Remove(rigidBody.gameObject);
		}
	}

	private void StoreRigidBodyConfig(Rigidbody2D rigidBody) {
		if(storedConfigs.ContainsKey(rigidBody.gameObject)) {
			return;
		}

		RigidBodyConfig config = new RigidBodyConfig(rigidBody);
		storedConfigs.Add(rigidBody.gameObject, config);
	}

	private void CreateQueueForRigidBody(Rigidbody2D rigidBody) {
		if(queuedRegistrations.ContainsKey(rigidBody.gameObject)) {
			return;
		}

		queuedRegistrations.Add(rigidBody.gameObject, new Queue<RigidBodyConfig>());
	}
}
