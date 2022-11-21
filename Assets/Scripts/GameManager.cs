using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
  private float currentTime = 20;
  private int currentDistance = 0;
  private int restartDelay = 4;
  private int boostTime = 4;
  private bool isGameOver = false;
  private bool isStarted = false;
  private bool isFinishedStarting = false;
  private EnvironmentScript environmentScript;
  private PlayerScript playerScript;
  [SerializeField] private CanvasGroup timeAndDistance;
  [SerializeField] private CanvasGroup turningButtons;
  [SerializeField] private CanvasGroup startTextGroup;
  [SerializeField] private CanvasGroup tutorial;
  [SerializeField] private Transform camTransform;
  [SerializeField] private Transform playingCameraTransfrom;
  // [SerializeField] private Transform companyName;
  [SerializeField] private Animator playerAnim;
  [SerializeField] private TMP_Text timeText;
  [SerializeField] private TMP_Text distanceText;
  [SerializeField] private TMP_Text gameOverText;
  [SerializeField] private TMP_Text gameTitle;

  private void Awake()
  {
    environmentScript = FindObjectOfType<EnvironmentScript>();
    playerScript = FindObjectOfType<PlayerScript>();
    playerAnim.SetBool("gameOver", false);
  }

  private void Update()
  {
    if (Input.GetKey(KeyCode.Space) && !isStarted)
    {
      StartGame();
    }
    if (isStarted)
    {

      if (currentTime > 0)
      {
        if (isFinishedStarting)
        {
          currentTime -= Time.deltaTime;
          if (currentTime < 0) currentTime = 0;
        }
      }
      else if (!isGameOver) GameOver();
    }
    DisplayDistance();
    DisplayTime(currentTime);
  }

  public void AddSeconds(int extraSeconds = 1)
  {
    currentTime += extraSeconds;
  }

  private void DisplayTime(float timeToDisplay)
  {
    int minutes = Mathf.FloorToInt(timeToDisplay / 60);
    int seconds = Mathf.FloorToInt(timeToDisplay % 60);
    timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
  }

  private void DisplayDistance()
  {
    distanceText.text = currentDistance.ToString() + " m";
  }

  public void EnableBonusSpeed()
  {
    playerScript.SpeedUp(boostTime);
    environmentScript.SpeedUp(boostTime);
  }

  private void GameOver()
  {
    isGameOver = true;
    turningButtons.interactable = false;
    Debug.Log("Game Over");
    playerAnim.SetBool("gameOver", true);
    playerAnim.Play("Fox_Sit");
    currentTime = 0;
    environmentScript.StopMoving();
    gameOverText.alpha = 1;
    StartCoroutine(RestartGame(restartDelay));
  }

  private IEnumerator RestartGame(int delay = 0)
  {
    yield return new WaitForSeconds(delay);
    DOTween.KillAll();
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  public void SetDistance(int newDistance)
  {
    currentDistance = newDistance;
  }

  public bool IsGameOver()
  {
    return isGameOver;
  }

  public bool IsGameStarted()
  {
    return isStarted;
  }

  public void StartGame()
  {
    StartCoroutine(StartGameCoroutine());
  }

  private IEnumerator StartGameCoroutine()
  {
    isStarted = true;
    turningButtons.interactable = true;
    startTextGroup.interactable = false;
    startTextGroup.blocksRaycasts = false;
    startTextGroup.DOFade(0, 0.5f);
    camTransform.DOMove(playingCameraTransfrom.position, 1);
    camTransform.DORotateQuaternion(playingCameraTransfrom.rotation, 1);
    timeAndDistance.DOFade(1, 0.5f).SetDelay(0.5f);
    playerAnim.SetTrigger("startGame");
    yield return new WaitForSeconds(1);
    // companyName.DOMoveX(-40, 16).SetEase(Ease.InOutSine).OnComplete(() => companyName.DOMoveX(40, 30).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine));
    environmentScript.StartMoving();
    isFinishedStarting = true;
  }

  public void ObstacleHit()
  {
    StartCoroutine(StopMovingCoroutine());
  }

  private IEnumerator StopMovingCoroutine()
  {
    playerAnim.Play("Fox_Collision");
    environmentScript.StopMoving();
    yield return new WaitForSeconds(1.3f);
    if (!isGameOver) environmentScript.StartMoving();
  }
}
