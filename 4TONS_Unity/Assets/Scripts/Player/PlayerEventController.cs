using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventController : MonoBehaviour {

    private PlayerBehaviours playerBehaviours;

    public delegate void Spawn();
    public event Spawn spawn;

    public delegate void Die();
    public event Die die;

    public delegate void RollDodge(DodgeInfo dodgeInfo);
    public event RollDodge rollDodge;
    // Use this for initialization
    public void InitializePlayerEventController(PlayerBehaviours playerBehaviours)
    {
        this.playerBehaviours = playerBehaviours;
    }
	
	// Update is called once per frame
	void Update () {
    }
    public void SpawnEvent()
    {
        if (spawn != null)
            spawn();
    }
    public void DieEvent()
    {
        if (die != null)
            die();
    }
    public void RollDodgeEvent(DodgeInfo dodgeInfo)
    {
        if (rollDodge != null)
            rollDodge(dodgeInfo);
    }
}

