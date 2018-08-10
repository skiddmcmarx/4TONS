using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour, IManager {

    public static PlayerInputManager instance;
    private int leaderInputIndex;
    public bool joinable;
    public int currentPlayerCount = 0;
    public List<InputEntity> inputEntities = new List<InputEntity>();
    public GameObject defaultWizard;
    public GameObject tutorialWizard;
    public GameObject playerOneCrown;
    public GameObject[] standardWizards;
    public GameObject[] flummoxWizards;



    public delegate void UpdateInput();
    public event UpdateInput updateInput;

    public delegate void FixedUpdateInput();
    public event FixedUpdateInput fixedUpdateInput;

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
    }

    public void initialize()
    {
        for (int i = 0; i < 5; i++)
        {
            inputEntities[i].InitializeInputEntity(i);
        }
        InputEntityReport();
        subscribeToEvents();
    }
    
    public void changeScene(SceneInfo sceneInfo)
    {
        //subscribeToEvents();
        int initiatorIndex = sceneInfo.initiatorIndex;
        leaderInputIndex = initiatorIndex;
        currentPlayerCount = 0;
        switch (sceneInfo.sceneIndex)
        {
            case 0:
                joinable = true;
                break;
            case 1:
                joinable = false;
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }
    public void load(int sceneIndex)
    {
        print("playerInputManager load. ");
        InputEntityReport();
        switch (sceneIndex)
        {
            case 0:
                joinable = true;
                break;
            case 1:
                joinable = false;
                createPlayer(leaderInputIndex);
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }
    public void subscribeToEvents()
    {
        //subscriber to these events.
        GameEventManager.instance.joinPlayer += joinPlayer;
        GameEventManager.instance.leavePlayer += leavePlayer;
        GameEventManager.instance.gameUpdate += update;
        GameEventManager.instance.endScene += endScene;
        GameEventManager.instance.changeScene += changeScene;
        GameEventManager.instance.load += load;
    }
    public void unsubscribeFromEvents()
    {
        //subscriber to these events.
        GameEventManager.instance.joinPlayer -= joinPlayer;
        GameEventManager.instance.leavePlayer -= leavePlayer;
        GameEventManager.instance.gameUpdate -= update;
        GameEventManager.instance.endScene -= endScene;
        GameEventManager.instance.changeScene -= changeScene;
        GameEventManager.instance.load -= load;
    }
    
    public void joinPlayer(JoinPlayerInfo joinPlayerInfo)
    {
        int inputIndex = joinPlayerInfo.inputIndex;
        currentPlayerCount += 1;
        inputEntities[joinPlayerInfo.inputIndex].joinPlayer(joinPlayerInfo);
        print("current player count: " + currentPlayerCount);
    }
    public void createPlayer(int inputIndex)
    {
        print("create player.");
        JoinPlayerInfo joinPlayerInfo = new JoinPlayerInfo();
        joinPlayerInfo.inputIndex = inputIndex;
        joinPlayerInfo.playerNumber = currentPlayerCount + 1;
        joinPlayerInfo.wizardPrefab = defaultWizard;
        GameObject wizardInstance;
        Vector3 spawnPoint = GameObject.Find("Player " + joinPlayerInfo.playerNumber + " spawnPoint").transform.position;
        if (inputEntities[inputIndex].wizardInstance == null)
            wizardInstance = Instantiate(defaultWizard, spawnPoint, Quaternion.identity);
        else
            wizardInstance = inputEntities[inputIndex].wizardInstance;

        if (joinPlayerInfo.playerNumber == 1)
        {
            leaderInputIndex = inputIndex;
               GameObject go = Instantiate(playerOneCrown, wizardInstance.transform.position, Quaternion.identity);
            go.transform.parent = wizardInstance.transform;
        }

        joinPlayerInfo.wizardPrefab = wizardInstance;
        joinPlayerInfo.trans = wizardInstance.transform;
        //initializes player
        PlayerBehaviours playerBehaviours = wizardInstance.GetComponent<PlayerBehaviours>();
        playerBehaviours.joinPlayer(joinPlayerInfo);
        GameEventManager.instance.joinPlayerEvent(joinPlayerInfo);
    }

    public void leavePlayer(LeavePlayerInfo leavePlayerInfo)
    {
        int inputIndex = leavePlayerInfo.inputIndex;
        inputEntities[inputIndex].leavePlayer(inputIndex);
        currentPlayerCount -= 1;
    }
  
    public void InputEntityReport()
    {
        print("InputEntityReport. current entities: " + inputEntities.Count +", occupied: " +currentPlayerCount);
        foreach (InputEntity inputEntity in inputEntities)
        {
            inputEntity.reportPlayerInputEntity();
        }
    }
    public void joinGameUpdate()
    {
            foreach (InputEntity inputEntity in inputEntities)
            {
                    if (inputEntity.isOccupied == false && inputEntity.wizardInstance == null && inputEntity.input.GetButton("Interact"))
                    {
                        createPlayer(inputEntity.inputIndex);
                    }
            }
    }

  
    public void endScene()
    { 
        joinable = false;
        endScenePlayers();
        //unsubscribeFromEvents();
        InputEntityReport();
    }
    public void endScenePlayers()
    {
        print("ending scene for players. cleans up InputEntities. ");
        for (int i = 0; i < inputEntities.Count; i++)
        {
            inputEntities[i].endScene(i);
        } 
    }
    public void update()
    {
        //print("player input manager update");
        if (Input.GetKeyDown(KeyCode.U))
        {
            print("h current inputs + " + inputEntities.Count);
            InputEntityReport();
        }
        if (updateInput != null && currentPlayerCount != 0)
        {
            print("player input manager update");
            updateInput();
        }
        if (joinable)
        {
            joinGameUpdate();
        }
    }
    public virtual void FixedUpdate()
    {
        if (fixedUpdateInput != null)
        {
            fixedUpdateInput();
        }
    }

}
