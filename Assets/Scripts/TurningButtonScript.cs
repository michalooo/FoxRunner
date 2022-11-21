using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurningButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
  bool pointerDown = false;
  bool turningRight = false;
  bool turningLeft = false;
  PlayerScript playerScript;

  private void Awake()
  {
    playerScript = FindObjectOfType<PlayerScript>();
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    pointerDown = true;
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    pointerDown = false;
    turningRight = false;
    turningLeft = false;
    playerScript.PlayerStop();
  }

  public void TurnLeft()
  {
    turningLeft = true;
  }

  public void TurnRight()
  {
    turningRight = true;
  }

  void Update()
  {
    if (pointerDown)
    {
      if (turningRight)
      {
        playerScript.PlayerGoRight();
      }
      else if (turningLeft)
      {
        playerScript.PlayerGoLeft();
      }
    }
  }
}
