using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeGateScript : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    private GameManager gameManager;
    private int timeBonus;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void SetTimeBonus(int timeBonusValue)
    {
        timeBonus = timeBonusValue;
        timeText.text = "+" + timeBonus.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") gameManager.AddSeconds(timeBonus);
    }
}
