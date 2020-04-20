using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CharacterController2D player;
    public PetController pet;

    public HUDController HUD;
    public PlayerItemsManager playerItemsManager;
    public EnemySpawner enemySpawner;

    public static GameController instance;

    public Sprite medicine;
    public Sprite food;
    public Sprite toy;
    public Sprite gem;

    public Sprite normalPanel;
    public Sprite halvedPanel;

    public Transform itemPrefab;

    public AudioClip backgroundMusic;

    public AudioClip shootBulletSound;
    public AudioClip shootBulletEnemySound;
    public AudioClip circularBulletSound;
    public AudioClip impactSound;
    public AudioClip hurtSound;
    public AudioClip hurtPlayerSound;

    public Transform followPointerCamera;

    public GameObject tutorial;

    public enum TutorialState { TUTORIAL, DONE };
    public TutorialState state = TutorialState.TUTORIAL;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        Init();
    }

    private void Init()
    {
        player = GameObject.Find("Player").GetComponent<CharacterController2D>();
        pet = GameObject.Find("Pet").GetComponent<PetController>();

        HUD = GetComponent<HUDController>();
        playerItemsManager = GetComponent<PlayerItemsManager>();
        enemySpawner = GetComponent<EnemySpawner>();
    }

    private void Start()
    {
        SoundManager.instance.PlayBackgroundMusic(backgroundMusic);

        tutorial.SetActive(true);
    }

    private void Update()
    {
        if (state == TutorialState.TUTORIAL)
        {
            if (Input.anyKey)
            {
                state = TutorialState.DONE;
                tutorial.SetActive(false);
            }
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

}
