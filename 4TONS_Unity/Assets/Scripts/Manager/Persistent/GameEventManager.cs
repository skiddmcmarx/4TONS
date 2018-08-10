using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventManager : MonoBehaviour {

    public static GameEventManager instance;

    public delegate void JoinPlayer(JoinPlayerInfo joinPlayerInfo);
    public event JoinPlayer joinPlayer;

    public delegate void LeavePlayer(LeavePlayerInfo leavePlayerInfo);
    public event LeavePlayer leavePlayer;

    public delegate void ChangeScene(SceneInfo sceneChangeInfo);
    public event ChangeScene changeScene;

    public delegate void Load(int sceneIndex);
    public event Load load;

    public delegate void GameUpdate();
    public event GameUpdate gameUpdate;

    public delegate void EndScene();
    public event EndScene endScene;
   

    private void OnDisable()
    {
        instance = null;
    }
    public void initialize()
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
    void Update()
    {
        //print("game event manager update");
        gameUpdateEvent();
    }
    public void gameUpdateEvent()
    {
        if (gameUpdate != null)
            gameUpdate();
    }
    public void joinPlayerEvent(JoinPlayerInfo joinPlayerInfo)
    {
        if (joinPlayer != null)
        {
            joinPlayer(joinPlayerInfo);
        }
    }
    public void leavePlayerEvent(LeavePlayerInfo leavePlayerInfo)
    {
        if (leavePlayer !=  null)
        {
            leavePlayer(leavePlayerInfo);
        }
    }
    public void endSceneEvent()
    {
        if (endScene!= null)
            endScene();
    }
    public void changeSceneEvent(SceneInfo sceneChangeInfo)
    {
        print("changing scene.");
        if (changeScene != null)
            changeScene(sceneChangeInfo);
        SceneManager.LoadScene(sceneChangeInfo.sceneIndex);
    }
    public void loadEvent(int sceneIndex)
    {
        print("LOADING LEVEL!");
        if (load != null)
            load(sceneIndex);
    }
}