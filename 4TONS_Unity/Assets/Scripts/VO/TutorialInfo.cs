using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialInfo : MonoBehaviour {

    public int tutorialIndex;
    public Transform baseTrans;
    public string textPrompt;
    public TextAsset tutorialFile;
    public Vector3 csvStartPoint;
}
