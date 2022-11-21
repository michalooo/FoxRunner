using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
  [SerializeField] GameObject leftObstacle;
  [SerializeField] GameObject rightObstacle;
  [SerializeField] Transform leftObstacleTransform;
  [SerializeField] Transform rightObstacleTransform;
  [SerializeField] GameObject leftBonus;
  [SerializeField] GameObject rightBonus;
  // Start is called before the first frame update
  void Start()
  {
    DisableEverything();
    ActivateBonusAndObstacles();
  }

  private void OnEnable()
  {
    ActivateBonusAndObstacles();
  }
  private void OnDisable()
  {
    DisableEverything();
  }

  private void DisableEverything()
  {
    leftObstacle.SetActive(false);
    rightObstacle.SetActive(false);
    leftBonus.SetActive(false);
    rightBonus.SetActive(false);
  }

  private void ActivateBonusAndObstacles()
  {
    if (Random.Range(0, 2) == 0)
    {
      if (Random.Range(0, 3) == 0)
      {
        leftBonus.SetActive(true);
      }
      leftObstacleTransform.Rotate(Vector3.up, Random.Range(0, 360));
      leftObstacleTransform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);
      leftObstacle.SetActive(true);
    }
    else
    {
      if (Random.Range(0, 3) == 0)
      {
        rightBonus.SetActive(true);
      }
      rightObstacleTransform.Rotate(Vector3.up, Random.Range(0, 360));
      rightObstacleTransform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);
      rightObstacle.SetActive(true);
    }
  }
}
