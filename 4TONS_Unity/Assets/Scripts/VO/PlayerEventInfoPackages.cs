using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventInfoPackages : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
[System.Serializable]
public class DodgeInfo
{
    public float invulnTime;
    public float dodgePower;
    public float cooldown;
}
public class SpawnInfo
{
    public Vector3 spawnPoint;
    public float spawnTime;
}
public class DieInfo
{
    public float dieTime;
    public int livesLeft;
}
