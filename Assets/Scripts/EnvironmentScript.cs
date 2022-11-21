using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnvironmentScript : MonoBehaviour
{
  private Transform tr;
  private float currentEnvironmentSpeed;
  private float normalEnvironmentSpeed = 15;
  private float highEnvironmentSpeed = 23;
  private int maxNumberOfTiles = 15;
  private int numberOfEmptyTiles = 15;
  private int numberOfScoreTiles = 8;
  private float tileLength;
  private ObjectPool<GameObject> _emptyTilesPool;
  private ObjectPool<GameObject> _scoreTilesPool;
  private ObjectPool<GameObject> _obstacleTilesPool;
  private GameManager gameManager;
  [SerializeField] private GameObject emptyTilePrefab;
  [SerializeField] private GameObject scoreTilePrefab;
  [SerializeField] private GameObject obstacleTilePrefab;
  [SerializeField] private Transform tilesParent;
  private Vector3 tileCreationPosition = new Vector3(0, 0, -100);
  private Vector3 nextTilePosition = new Vector3(0, 0, -20);

  void Awake()
  {
    tr = transform;
    tileLength = emptyTilePrefab.transform.localScale.z;
    gameManager = FindObjectOfType<GameManager>();
    _emptyTilesPool = new ObjectPool<GameObject>(CreateEmptyTile, (obj) => obj.SetActive(true), (obj) => ReleaseTile(obj), (obj) => Destroy(obj), true, numberOfEmptyTiles, maxNumberOfTiles);
    _scoreTilesPool = new ObjectPool<GameObject>(CreateScoreTile, (obj) => obj.SetActive(true), (obj) => ReleaseTile(obj), (obj) => Destroy(obj), true, numberOfScoreTiles, maxNumberOfTiles);
    _obstacleTilesPool = new ObjectPool<GameObject>(CreateObstacleTile, (obj) => obj.SetActive(true), (obj) => ReleaseTile(obj), (obj) => Destroy(obj), true, maxNumberOfTiles, maxNumberOfTiles);
  }

  private void Start()
  {
    SpawnEmptyTile();
    SpawnEmptyTile();
    SpawnEmptyTile();
    SpawnEmptyTile();
    for (int i = 0; i < maxNumberOfTiles - 2; i++)
    {
      SpawnNewRandomTile();
    }
  }

  public void StartMoving()
  {
    currentEnvironmentSpeed = normalEnvironmentSpeed;
  }

  public void StopMoving()
  {
    currentEnvironmentSpeed = 0;
  }

  void Update()
  {
    tr.Translate(Vector3.back * Time.deltaTime * currentEnvironmentSpeed);
    gameManager.SetDistance(Mathf.FloorToInt(-tr.position.z));
  }

  private GameObject CreateEmptyTile()
  {
    GameObject obj = Instantiate(emptyTilePrefab, tileCreationPosition, Quaternion.identity, tilesParent);
    obj.SetActive(false);
    return obj;
  }

  private GameObject CreateScoreTile()
  {
    GameObject obj = Instantiate(scoreTilePrefab, tileCreationPosition, Quaternion.identity, tilesParent);
    obj.SetActive(false);
    return obj;
  }

  private GameObject CreateObstacleTile()
  {
    GameObject obj = Instantiate(obstacleTilePrefab, tileCreationPosition, Quaternion.identity, tilesParent);
    obj.SetActive(false);
    return obj;
  }

  private void ReleaseTile(GameObject obj)
  {
    obj.transform.localPosition = tileCreationPosition;
    obj.SetActive(false);
  }

  public GameObject SpawnEmptyTile()
  {
    GameObject obj;
    obj = _emptyTilesPool.Get();
    obj.transform.localPosition = nextTilePosition;
    nextTilePosition += new Vector3(0, 0, tileLength);
    return obj;
  }

  public GameObject SpawnNewRandomTile()
  {
    GameObject obj;
    int randomNumber = Random.Range(0, 4);
    if (randomNumber == 0) obj = _scoreTilesPool.Get();
    else if (randomNumber < 3) obj = _obstacleTilesPool.Get();
    else obj = _emptyTilesPool.Get();
    obj.transform.localPosition = nextTilePosition;
    nextTilePosition += new Vector3(0, 0, tileLength);
    return obj;
  }

  public void DeactivateEmptyTile(GameObject obj)
  {
    _emptyTilesPool.Release(obj);
  }

  public void DeactivateScoreTile(GameObject obj)
  {
    _scoreTilesPool.Release(obj);
  }

  public void DeactivateObstacleTile(GameObject obj)
  {
    _obstacleTilesPool.Release(obj);
  }

  public void SpeedUp(int boostTime)
  {
    StopAllCoroutines();
    StartCoroutine(SpeedUpCoroutine(boostTime));
  }

  private IEnumerator SpeedUpCoroutine(int boostTime)
  {
    currentEnvironmentSpeed = highEnvironmentSpeed;
    yield return new WaitForSeconds(boostTime);
    currentEnvironmentSpeed = normalEnvironmentSpeed;
  }

  // private IEnumerator SmoothSpeedChange(float newSpeed)
  // {
  //     while (Mathf.Abs(tr.position.x - target.x) > 0.1f)
  //     {
  //         currentEnvironmentSpeed = Mathf.Lerp(currentEnvironmentSpeed, newSpeed, Time.deltaTime * 20);
  //         yield return new WaitForEndOfFrame();
  //     }
  // }
}
