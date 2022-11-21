using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
  private Transform tr;
  private Rigidbody rb;
  private GameManager gameManager;
  private EnvironmentScript environmentScript;
  private Vector3 playerRightPosition = new Vector3(1.75f, 0.5f, -5);
  private Vector3 playerLeftPosition = new Vector3(-1.75f, 0.5f, -5);
  private float playerHorizontalSpeed = 8;
  private float movementAnimSpeedMultiplayer = 1.3f;
  private bool isTurning = false;
  private bool isBusy = false;
  private bool touchingRightWall = false;
  private bool touchingLeftWall = false;
  private Animator anim;
  private bool foxCanBeHit = true;

  void Awake()
  {
    gameManager = FindObjectOfType<GameManager>();
    environmentScript = FindObjectOfType<EnvironmentScript>();
    tr = transform;
    anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody>();
  }

  void Update()
  {
    if (foxCanBeHit)
    {
      if (Input.GetKey(KeyCode.LeftArrow))
      {
        PlayerGoLeft();
      }
      else if (Input.GetKey(KeyCode.RightArrow))
      {
        PlayerGoRight();
      }
      else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
      {
        PlayerStop();
      }
      else if (!isTurning || isBusy)
      {
        PlayerStop();
      }
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Obstacle" && foxCanBeHit)
    {
      PlayerStop();
      gameManager.ObstacleHit();
      rb.velocity = Vector3.zero;
      foxCanBeHit = false;
      StartCoroutine(ImmortalAfterHit());
    }
    if (other.tag == "Bonus")
    {
      other.gameObject.SetActive(false);
      gameManager.EnableBonusSpeed();
    }
    if (other.tag == "JumpTrigger")
    {
      PlayerStop();
      rb.velocity = Vector3.zero;
      anim.Play("Fox_Jump_Pivot_InPlace");
    }
  }

  private IEnumerator ImmortalAfterHit()
  {
    yield return new WaitForSeconds(1.5f);
    foxCanBeHit = true;
  }

  private void OnCollisionEnter(Collision other)
  {
    if (other.transform.tag == "RightWall")
    {
      touchingRightWall = true;
      PlayerStop();
    }
    if (other.transform.tag == "LeftWall")
    {
      touchingLeftWall = true;
      PlayerStop();
    }
  }

  private void OnCollisionExit(Collision other)
  {
    if (other.transform.tag == "RightWall")
    {
      touchingRightWall = false;
    }
    if (other.transform.tag == "LeftWall")
    {
      touchingLeftWall = false;
    }
  }

  public void SpeedUp(int boostTime)
  {
    StopCoroutine(SpeedUpCoroutine(boostTime));
    StartCoroutine(SpeedUpCoroutine(boostTime));
  }


  private IEnumerator SpeedUpCoroutine(int boostTime)
  {
    anim.speed = movementAnimSpeedMultiplayer;
    yield return new WaitForSeconds(boostTime);
    anim.speed = 1;
  }

  // private IEnumerator SmoothTranslation(Vector3 target)
  // {
  //   isTurning = true;
  //   while (Mathf.Abs(tr.position.x - target.x) > 0.1f)
  //   {
  //     tr.position = Vector3.Lerp(tr.position, target, Time.deltaTime * playerHorizontalSpeed);
  //     yield return new WaitForEndOfFrame();
  //   }
  //   isTurning = false;
  // }

  public void PlayerGoRight()
  {
    if (!gameManager.IsGameOver() && gameManager.IsGameStarted() && !isBusy)
    {
      isTurning = true;

      // if (!playerOnRight)
      // {
      //   playerOnRight = true;
      //   StartCoroutine(SmoothTranslation(playerRightPosition));
      // }
      if (!touchingRightWall)
      {
        rb.velocity = Vector3.right * playerHorizontalSpeed;
      }
    }
  }

  public void PlayerGoLeft()
  {
    if (!gameManager.IsGameOver() && gameManager.IsGameStarted() && !isBusy)
    {
      isTurning = true;
      // if (playerOnRight)
      // {
      //   playerOnRight = false;
      //   StartCoroutine(SmoothTranslation(playerLeftPosition));
      // }
      if (!touchingLeftWall)
      {
        rb.velocity = Vector3.left * playerHorizontalSpeed;
      }
    }
  }

  public void PlayerStop()
  {
    isTurning = false;
    rb.velocity = Vector3.zero;
  }

  public void SetIsBusy(int currentState)
  {
    isBusy = (currentState == 1);
  }

}
