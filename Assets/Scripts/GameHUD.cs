using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacter mrDeeper;

    [SerializeField]
    private PlayerCharacter msDeeper;

    [SerializeField]
    private Text mrDeeperHealthText;

    [SerializeField]
    private Text msDeeperHealthText;

    void Awake()
    {
        msDeeper.HealthChanged += OnMsDeeperHealthChanged;
        mrDeeper.HealthChanged += OnMrDeeperHealthChanged;
    }

    void OnMsDeeperHealthChanged(int playerHealth)
    {
        msDeeperHealthText.text = playerHealth.ToString();
    }

    void OnMrDeeperHealthChanged(int playerHealth)
    {
        mrDeeperHealthText.text = playerHealth.ToString();
    }
}
