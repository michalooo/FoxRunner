using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerScript : MonoBehaviour
{
    private EnvironmentScript environmentScript;
    private void Start()
    {
        environmentScript = FindObjectOfType<EnvironmentScript>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "EmptyTile")
        {
            environmentScript.DeactivateEmptyTile(other.gameObject);
            environmentScript.SpawnNewRandomTile();
        }
        else if (other.tag == "TimeTile")
        {
            environmentScript.DeactivateScoreTile(other.gameObject);
            environmentScript.SpawnNewRandomTile();
        }
        else if (other.tag == "ObstacleTile")
        {
            environmentScript.DeactivateObstacleTile(other.gameObject);
            environmentScript.SpawnNewRandomTile();
        }
    }
}
