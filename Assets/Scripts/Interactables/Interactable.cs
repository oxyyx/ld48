using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	[SerializeField]
	private Interactable[] connectedInteractables;

	protected Interactable[] ConnectedInteractables {
		get {
			if(connectedInteractables == null) {
				return new Interactable[0];
			}
			return connectedInteractables;
		}
	}

	public virtual void Activate() {
		foreach(Interactable interactable in ConnectedInteractables) {
			interactable.Activate();
		}
	}
}
