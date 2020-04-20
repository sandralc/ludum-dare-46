using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PossessionController : MonoBehaviour
{
    public GameObject firstPosession;
    public GameObject secondPosession;
    public GameObject thirdPosession;

    public TextMeshPro firstCountdown;
    public TextMeshPro secondCountdown;
    public TextMeshPro thirdCountdown;

    public int firstCountdownDuration = 10;
    public int secondCountdownDuration = 10;
    public int thirdCountdownDuration =  10;

    private PetController petController;

    private void Awake()
    {
        petController = GetComponent<PetController>();
    }

    private void Start()
    {
        firstPosession.SetActive(false);
        secondPosession.SetActive(false);
        thirdPosession.SetActive(false);
    }

    public void Possess (EnemyController.EnemyType type)
    {
        switch(type)
        {
            case EnemyController.EnemyType.GHOST_FOOD:
                StartCoroutine(CountdownPossession(firstCountdownDuration, firstCountdown, firstPosession, type));
                break;
            case EnemyController.EnemyType.GHOST_MIND:
                StartCoroutine(CountdownPossession(secondCountdownDuration, secondCountdown, secondPosession, type));
                break;
            case EnemyController.EnemyType.GHOST_HEALTH:
                StartCoroutine(CountdownPossession(thirdCountdownDuration, thirdCountdown, thirdPosession, type));
                break;
        }
    }

    private IEnumerator CountdownPossession(float duration, TextMeshPro textObject, GameObject possession, EnemyController.EnemyType type)
    {

        int totalTime = 0;
        possession.SetActive(true);

        petController.Possessed(type);

        while (totalTime < duration)
        {
            totalTime += 1;
            Debug.Log("Total time " + totalTime);
            var secondsLeft = duration - totalTime;
            textObject.text = secondsLeft.ToString() + "s";
            yield return new WaitForSeconds(1f);
        }

        possession.SetActive(false);

        petController.DePossessed(type);

        yield break;
    }

}
