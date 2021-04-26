using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelSign : MonoBehaviour
{
	[SerializeField]
	private String nextSceneName;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		SceneManager.LoadScene(nextSceneName);
	}
}
