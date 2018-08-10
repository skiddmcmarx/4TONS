using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//holds all game-relevant player information as well as all associated player scripts.

public class PlayerBehaviours : MonoBehaviour {

    public WizardData wizardData;

    public int inputIndex;
    public int playerNumber;

    public PlayerEventController playerEventController;
    public PlayerController playerController;
    public PlayerAnimations playerAnimations;
    public void joinPlayer(JoinPlayerInfo joinPlayerInfo)
    {
        this.inputIndex = joinPlayerInfo.inputIndex;
        this.playerNumber = joinPlayerInfo.playerNumber;
        this.tag = "Player" + (playerNumber);
        this.gameObject.layer = playerNumber + 7;
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = 7 + playerNumber;
            transform.GetChild(i).gameObject.tag = "Player" + (playerNumber);
        }
        playerEventController = GetComponent<PlayerEventController>();
        playerController = GetComponent<PlayerController>();
        playerAnimations = GetComponent<PlayerAnimations>();
        initializePlayerBehaviours();
    }
    public void initializePlayerBehaviours()
    {
        print("initializing player behaviours");
        playerEventController.InitializePlayerEventController(this);
        playerController.initialize(this);
        playerAnimations.InitializePlayerAnimations(this);


    }
  
}
