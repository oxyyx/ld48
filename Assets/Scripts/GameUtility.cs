using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtility : MonoBehaviour
{
	private void Start() {
		PlayerCharacter[] players = FindObjectsOfType<PlayerCharacter>();
		foreach(PlayerCharacter player in players) {
			player.HealthChanged += (health) => { if(health <= 0) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); } };
		}
	}
}
