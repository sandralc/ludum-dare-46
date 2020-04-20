using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class RespawnSystemController : MonoBehaviour
{

    public enum GameState { PLAYING, RESPAWNING, WAITING};

    private GameState state = GameState.PLAYING;

    public TextMeshProUGUI respawnText;

    float respawnTime = 5f;

    public CinemachineVirtualCamera camera;

    public GameObject gameOverPanel;

    public ScoreController scoreController;


    private void Start()
    {
        respawnText.gameObject.SetActive(false);

        gameOverPanel.SetActive(false);
    }

    public void Update()
    {

        if (state == GameState.WAITING)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {

                scoreController.Restart();
                GameController.instance.player.Respawn();
                GameController.instance.pet.Restart();
                GameController.instance.enemySpawner.Restart();

                gameOverPanel.gameObject.SetActive(false);
                SoundManager.instance.UnMuteSFX();

                respawnText.gameObject.SetActive(false);
                respawnTime = 5f;

                state = GameState.PLAYING;

            }

            return;
        }

        Debug.Log("Pet health " + GameController.instance.pet.health);
        Debug.Log("Pet is dead? " + PetIsDead());

        if ((state == GameState.PLAYING || state == GameState.RESPAWNING) && PetIsDead())
        {
            GameOver();
        }

        if (state == GameState.PLAYING)
        {
            if (PlayerIsDead())
            {
                Debug.Log("Player is dead");
                PrepareRespawning();
                MovePlayerNextToPet();
            }
            else
                return;
        }

        if (state == GameState.RESPAWNING)
        {
            respawnTime -= Time.deltaTime;
            respawnText.text = "Respawn in " + Mathf.RoundToInt(respawnTime);

            if (respawnTime <= 0)
            {
                respawnText.gameObject.SetActive(false);
                RespawnPlayer();
            }
        }

       
    }

    void MovePlayerNextToPet()
    {
        GameController.instance.player.transform.position = GameController.instance.player.startPosition;
        GameController.instance.followPointerCamera.transform.position = GameController.instance.player.transform.position;
    }

    bool PlayerIsDead()
    {
        return GameController.instance.player.dead;
    }

    bool PetIsDead()
    {
        return GameController.instance.pet.health <= 0f;
    }

    void PrepareRespawning()
    {
        respawnText.gameObject.SetActive(true);
        state = GameState.RESPAWNING;
    }

    void RespawnPlayer()
    {
        state = GameState.PLAYING;
        respawnTime = 5f;

        GameController.instance.player.Respawn();
    }

    void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        state = GameState.WAITING;

        scoreController.StopCounting();

        SoundManager.instance.MuteSFX();
    }


}
