using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wizard Data", menuName = "Custom Assets/Wizard Data", order = 0)]
public class WizardData : ScriptableObject
{

    public string wizardName;
    public Sprite wizardSprite;
    [TextArea(15, 25)]
    public string wizardDescription;
    public GameObject wizardPrefab;
    
    [SerializeField]
    public DodgeInfo dodgeInfo;
    public SpellData defaultSpell;
    public SpellData[] spells;

}
