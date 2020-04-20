using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnPointController : MonoBehaviour
{

    public enum SpawnPointState { WAITING, COUNTING, SPAWNING };

    public float timeBetweenSpawns = 5f;
    public SpawnPointState state = SpawnPointState.COUNTING;

    private float spawnCountdown = 0f;
    private float searchCountdown = 1f;

    private void Start()
    {
        spawnCountdown = timeBetweenSpawns;

        spawnCountdown = Random.Range(1f, timeBetweenSpawns); //Give it a headstart so that not all of them appear at once.
    }

    private void Update()
    {
        if (state == SpawnPointState.WAITING)
        {
            if (!IsThereAnItem())
                ItemPicked();
            else
                return;
        }

        if (spawnCountdown <= 0)
        {
            if (state != SpawnPointState.SPAWNING)
                StartCoroutine(SpawnItem());

        }
        else
        {
            spawnCountdown -= Time.deltaTime;
        }
    }

    bool IsThereAnItem()
    {
        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;

            Debug.Log("I have " + transform.childCount + " children");

            if (transform.childCount == 0)
                return false;
        }

        return true;
    }

    void ItemPicked()
    {
        state = SpawnPointState.COUNTING;

        spawnCountdown = timeBetweenSpawns;

    }

    IEnumerator SpawnItem()
    {

        state = SpawnPointState.SPAWNING;

        int randomItemIndex = Random.Range(0, 3);

        ItemController itemController = (ItemController)Instantiate(GameController.instance.itemPrefab, new Vector2(0f, 0f), Quaternion.identity).GetComponent<ItemController>();
        ItemController.Type itemType = (ItemController.Type)randomItemIndex;

        itemController.transform.SetParent(this.transform, false);

        itemController.Init(itemType);

        state = SpawnPointState.WAITING;

        yield break;
    }
}
