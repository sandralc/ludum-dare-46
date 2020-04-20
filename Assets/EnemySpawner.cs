using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] doors;

    private List<Transform> doorsThisWave;

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    private GameController gameController;


    [System.Serializable]
    public class Wave
    {
        public string name = "Default";
        public WaveElement[] waveElements;
        public int[] doorIndexes;
    }

    [System.Serializable]
    public class WaveElement
    {
        public Transform enemy;
        public int count;
        public float delay;
    }

    public Wave[] waves;
    public int nextWave = 0;

    public float timeBetweenWaves = 5f;
    private float waveCountdown = 0f;
    public SpawnState state = SpawnState.COUNTING;

    private float searchCountdown = 1f;

    Coroutine spawnWaveCoroutine;

    private void Awake()
    {
        gameController = GetComponent<GameController>();
    }

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {

        if (state == SpawnState.WAITING)
        {
            if (!AreThereEnemiesAlive())
                WaveCompleted();
            else
                return;
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
                spawnWaveCoroutine = StartCoroutine(SpawnWave(waves[nextWave]));
        }
        else
            waveCountdown -= Time.deltaTime;
    }

    bool AreThereEnemiesAlive()
    {
        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
        return true;
    }

    void WaveCompleted()
    {

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("Completed all waves");
        }
        else
        {
            nextWave++;
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {

        Debug.Log("Spawning Wave " + wave.name);
        state = SpawnState.SPAWNING;

        doorsThisWave = new List<Transform>();
        for (int i = 0; i < wave.doorIndexes.Length; i++)
        {
            doorsThisWave.Add(doors[wave.doorIndexes[i]]);
            doors[wave.doorIndexes[i]].GetComponent<DoorController>().ActivateDoor();
        }

        foreach (WaveElement elem in wave.waveElements)
        {
            for (int i = 0; i < elem.count; i++)
            {
                SpawnEnemy(elem.enemy);
                yield return new WaitForSeconds(elem.delay);
            }
        }

        Debug.Log("State changed to waiting");

        DeactivateAllDoors();

        state = SpawnState.WAITING;

        yield break;
    }

    void DeactivateAllDoors()
    {
        foreach (Transform door in doors)
        {
            door.GetComponent<DoorController>().DeactivateDoor();
        }
    }

    void SpawnEnemy(Transform enemy)
    {
        int randomDoor = Random.Range(0, doorsThisWave.Count);
        Transform selectedDoor = doorsThisWave[randomDoor];
        Instantiate(enemy, selectedDoor.position, Quaternion.identity);
    }

    public void Restart()
    {

        StopCoroutine(spawnWaveCoroutine);
        DeactivateAllDoors();
        nextWave = 0;
        state = SpawnState.COUNTING;

        DestroyAllObjectsWithTag("Enemy");
        DestroyAllObjectsWithTag("Bullet");
        DestroyAllObjectsWithTag("Item");

    }

    void DestroyAllObjectsWithTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject go in gameObjects)
        {
            Destroy(go);
        }
    }
}
