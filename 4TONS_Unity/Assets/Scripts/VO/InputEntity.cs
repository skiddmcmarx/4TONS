using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputEntity : MonoBehaviour{
    public Player input;
    public int inputIndex;
    public bool isOccupied = false;
    public int playerNumber = 0;
    public GameObject wizardInstance;

    public void InitializeInputEntity(int inputIndex)
    {
        input = ReInput.players.GetPlayer(inputIndex);
        this.inputIndex = inputIndex;
    }
  
    public void joinPlayer(JoinPlayerInfo joinPlayerInfo)
    {
        inputIndex = joinPlayerInfo.inputIndex;
        print("joining player input entity. index: " + inputIndex);
        playerNumber = joinPlayerInfo.playerNumber;
        wizardInstance = joinPlayerInfo.wizardPrefab;
        isOccupied = true;
    }
   
    public void leavePlayer(int inputIndex)
    {
        print("LeavePlayerInputEntity " + inputIndex);
        playerNumber = 0;
        if (wizardInstance != null)
        {
            wizardInstance = null;
        }
        isOccupied = false;
    }
    public void endScene(int inputIndex)
    {
        print("input entity - end scene. index: " + inputIndex);
        if (wizardInstance != null)
        {
            wizardInstance = null;
        }
    }
    public void reportPlayerInputEntity()
    {

        
    }
}
