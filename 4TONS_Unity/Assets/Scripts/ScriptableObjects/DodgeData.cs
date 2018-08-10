using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dodge Data", menuName = "Custom Assets/Dodge Data", order = 3)]
public class DodgeData : ScriptableObject
{
    public float cooldown;
    public float invulnTime;
    public float power;
}