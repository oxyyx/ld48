using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

public class WaterVolume : Interactable
{
	[SerializeField]
	private float gravityScale = 0.25f;
	[SerializeField]
	private float linearDrag = 1;

	[SerializeField]
	private bool startsEmpty = false;
	[SerializeField]
	private bool allowFilling = true;
	[SerializeField]
	private float fillingSpeed = 1.0f;
	[SerializeField]
	private bool allowDraining = true;
	[SerializeField]
	private WaterVolume[] drainageTargets = new WaterVolume[0];
	[SerializeField]
	private float drainingSpeed = 1.0f;

	private event Action drainingEvent;
	public event Action Draining {
		add {
			drainingEvent += value;
		}
		remove {
			drainingEvent -= value;
		}
	}

	private event Action fillingEvent;
	public event Action Filling {
		add {
			fillingEvent += value;
		}
		remove {
			fillingEvent -= value;
		}
	}

	private event Action drainedEvent;
	public event Action Drained {
		add {
			drainedEvent += value;
		}
		remove {
			drainedEvent -= value;
		}
	}

	private event Action filledEvent;
	public event Action Filled {
		add {
			filledEvent += value;
		}
		remove {
			filledEvent -= value;
		}
	}

	private float halfSpriteHeight;
	private float bottomY;
	private float originalScale;
	private bool isEmpty;
	private bool isFull;

	private WaterVolumeManager waterVolumeManager;
	private WaterVolumeManager.RigidBodyConfig underwaterConfig;
	private List<Collider2D> collidersInVolume;

	private void Awake() {
		waterVolumeManager = FindObjectOfType<WaterVolumeManager>();
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		halfSpriteHeight = renderer.sprite.bounds.max.y;
		bottomY = renderer.bounds.min.y;
		originalScale = transform.localScale.y;
		isEmpty = startsEmpty ? true : false;
		isFull = startsEmpty ? false : true;

		underwaterConfig = new WaterVolumeManager.RigidBodyConfig { GravityScale = gravityScale, LinearDrag = linearDrag };
		collidersInVolume = new List<Collider2D>();

		if(startsEmpty) {
			InitializeAsEmpty();
		}
	}

	public override void Activate() {
		if(isFull) {
			Drain();
		}
		else if(isEmpty) {
			Fill();
		}
	}	

	private void OnTriggerEnter2D(Collider2D collision) {
		collidersInVolume.Add(collision);
		waterVolumeManager.RegisterVolumeEntered(collision, underwaterConfig);
	}

	private void OnTriggerExit2D(Collider2D collision) {
		collidersInVolume.Remove(collision);
		waterVolumeManager.RegisterVolumeExited(collision);
	}

	public void Drain() {
		if(!allowDraining || !isFull) {
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
		if(!allowFilling || !isEmpty) {
			return;
		}

		StartCoroutine(FillCoroutine());
	}

	private IEnumerator DrainCoroutine() {
		isFull = false;
		if(drainingEvent != null) {
			drainingEvent();
		}

		const float drainingSpeedModifier = 0.1f;
		while(transform.localScale.y - (drainingSpeed * drainingSpeedModifier) > 0) {
			UpdateTransformForScale(transform.localScale.y - (drainingSpeed * drainingSpeedModifier));
			yield return null;
		}

		UpdateTransformForScale(0);
		isEmpty = true;
		if(drainedEvent != null) {
			drainedEvent();
		}

		foreach(Collider2D collider in collidersInVolume) {
			waterVolumeManager.RegisterVolumeExited(collider);
		}
	}

	private IEnumerator FillCoroutine() {
		isEmpty = false;
		if(fillingEvent != null) {
			fillingEvent();
		}

		const float fillingSpeedModifier = 0.1f;
		while(transform.localScale.y + (fillingSpeed * fillingSpeedModifier) < originalScale) {
			UpdateTransformForScale(transform.localScale.y + (fillingSpeed * fillingSpeedModifier));
			yield return null;
		}

		UpdateTransformForScale(originalScale);
		isFull = true;
		if(filledEvent != null) {
			filledEvent();
		}
	}	

	private void InitializeAsEmpty() {
		UpdateTransformForScale(0);
	}

	private void UpdateTransformForScale(float scale) {
		transform.localScale = new Vector3(transform.localScale.x, scale, transform.localScale.z);
		float positionY = bottomY + (scale * halfSpriteHeight);
		transform.position = new Vector2(transform.position.x, positionY);
	}
}
