using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Use this for initialization
    public static GameManager instance;
    public GameObject gameEventManagerGO;
    public CameraManager cameraManager;
    public GameEventManager gameEventManager;
    public PlayerInputManager playerInputManager;
    public LevelManager levelManager;
    public SpriteManager spriteManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        gameEventManager = GameObject.Find("GameEventManager").GetComponent<GameEventManager>();
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        playerInputManager = GameObject.Find("PlayerInputManager").GetComponent<PlayerInputManager>();
        gameEventManager.initialize();
        cameraManager.initialize();
        playerInputManager.initialize();

    }

    private void OnEnable()
    {
        print("game manager onenable.");
        SceneManager.sceneLoaded += OnLevelLoaded;
    }
    private void OnDisable()
    {
        print("game manager onDisable.");
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    public virtual void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        GameEventManager.instance.loadEvent(scene.buildIndex);
        loadGameManager(scene, mode);
    }
    
    public void loadGameManager(Scene scene, LoadSceneMode mode)
    {
        print("load game manager");
        spriteManager = GameObject.Find("SpriteManager").GetComponent<SpriteManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
}
