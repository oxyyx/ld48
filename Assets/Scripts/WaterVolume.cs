using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

public class WaterVolume : MonoBehaviour
{
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

	[SerializeField]
	private float gravityScale = 0.25f;
	[SerializeField]
	private float linearDrag = 1;

	[SerializeField]
	private bool startsEmpty = false;
	[SerializeField]
	private float fillingSpeed = 1.0f;
	[SerializeField]
	private WaterVolume[] drainageTargets = new WaterVolume[0];
	[SerializeField]
	private float drainingSpeed = 1.0f;

	private float halfSpriteHeight;
	private float bottomY;
	private float originalScale;
	private bool isEmpty;
	private bool isFull;	

	private RigidBodyConfig underwaterConfig;
	private Dictionary<GameObject, RigidBodyConfig> storedConfigs;

	private void Awake() {
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		halfSpriteHeight = renderer.sprite.bounds.max.y;
		bottomY = renderer.bounds.min.y;
		originalScale = transform.localScale.y;
		isEmpty = false;
		isFull = true;

		underwaterConfig = new RigidBodyConfig { GravityScale = gravityScale, LinearDrag = linearDrag };
		storedConfigs = new Dictionary<GameObject, RigidBodyConfig>();

		if(startsEmpty) {
			isEmpty = true;
			isFull = false;
			InitializeAsEmpty();
		}
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

	public void Drain() {
		if(!isFull) {
			return;
		}

		StartCoroutine(DrainCoroutine());
		foreach(WaterVolume drainageTarget in drainageTargets){
			if(drainageTarget != null) {
				drainageTarget.Fill();
			}
		}		
	}	

	public void Fill() {
		if(!isEmpty) {
			return;
		}

		StartCoroutine(FillCoroutine());
	}

	private IEnumerator DrainCoroutine() {
		isFull = false;

		const float drainingSpeedModifier = 0.1f;
		while(transform.localScale.y - (drainingSpeed * drainingSpeedModifier) > 0) {
			UpdateTransformForScale(transform.localScale.y - (drainingSpeed * drainingSpeedModifier));
			yield return null;
		}

		UpdateTransformForScale(0);
		isEmpty = true;
	}

	private IEnumerator FillCoroutine() {
		isEmpty = false;

		const float fillingSpeedModifier = 0.1f;
		while(transform.localScale.y + (fillingSpeed * fillingSpeedModifier) < originalScale) {
			UpdateTransformForScale(transform.localScale.y + (fillingSpeed * fillingSpeedModifier));
			yield return null;
		}

		UpdateTransformForScale(originalScale);
		isFull = true;
	}

	private void StoreRigidBodyConfig(Rigidbody2D rigidBody) {
		if(storedConfigs.ContainsKey(rigidBody.gameObject)) {
			return;
		}

		RigidBodyConfig config = new RigidBodyConfig(rigidBody);
		storedConfigs.Add(rigidBody.gameObject, config);
	}

	private void InitializeAsEmpty() {
		transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
		transform.position = new Vector2(transform.position.x, bottomY);
	}

	private void UpdateTransformForScale(float scale) {
		transform.localScale = new Vector3(transform.localScale.x, scale, transform.localScale.z);
		float positionY = bottomY + (scale * halfSpriteHeight);
		transform.position = new Vector2(transform.position.x, positionY);
	}
}
