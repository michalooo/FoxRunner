using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTileScript : MonoBehaviour
{
    [SerializeField] private GameObject leftGate;
    [SerializeField] private GameObject rightGate;
    [SerializeField] private TimeGateScript leftGateScript;
    [SerializeField] private TimeGateScript rightGateScript;
    private int lowestTimeBonus = 1;
    private int highestTimeBonus = 5;
    private int leftGateTimeBonus;
    private int rightGateTimeBonus;

    void Start()
    {
        DisableEverything();
        ActivateRandomTimeGate();
    }

    private void OnEnable()
    {
        ActivateRandomTimeGate();
    }
    private void OnDisable()
    {
        DisableEverything();
    }

    private void DisableEverything()
    {
        leftGate.SetActive(false);
        rightGate.SetActive(false);
    }

    private void ActivateRandomTimeGate()
    {
        int randomNumber = Random.Range(0, 3);
        if (randomNumber == 0)
        {
            leftGate.SetActive(true);
            leftGateTimeBonus = Random.Range(lowestTimeBonus, highestTimeBonus + 1);
            leftGateScript.SetTimeBonus(leftGateTimeBonus);
        }
        else if (randomNumber == 1)
        {
            rightGate.SetActive(true);
            rightGateTimeBonus = Random.Range(lowestTimeBonus, highestTimeBonus + 1);
            rightGateScript.SetTimeBonus(rightGateTimeBonus);
        }
        else
        {
            leftGate.SetActive(true);
            rightGate.SetActive(true);
            leftGateTimeBonus = Random.Range(lowestTimeBonus, highestTimeBonus + 1);
            do rightGateTimeBonus = Random.Range(lowestTimeBonus, highestTimeBonus + 1);
            while (leftGateTimeBonus == rightGateTimeBonus);
            leftGateScript.SetTimeBonus(leftGateTimeBonus);
            rightGateScript.SetTimeBonus(rightGateTimeBonus);
        }
    }
}
