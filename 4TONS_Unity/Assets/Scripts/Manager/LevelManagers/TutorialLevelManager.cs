using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelManager : LevelManager {


    public List<TutorialInfo> tutorials = new List<TutorialInfo>();

    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.2f;



    public void loadTutorial(int tutorialIndex)
    {
        TutorialInfo tut = tutorials[tutorialIndex];
        Transform baseTrans = tut.baseTrans;
        while (baseTrans.localPosition != Vector3.zero)
        {
            baseTrans.localPosition = Vector3.SmoothDamp(baseTrans.localPosition, Vector3.zero, ref velocity, smoothTime);
        }
        loadRoom(tutorialIndex);
    }

    
}
