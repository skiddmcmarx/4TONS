using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is responsible for handling animations on a per-scene basis.

//usually takes requests from the GameManager.

public class AnimationManager : MonoBehaviour {

    private Animator transitionAnim;
	
    public void InitializeAnimationManager()
    {
        transitionAnim = GetComponent<Animator>();
    }
	
    public void FadeIn()
    {
        transitionAnim.SetTrigger("fadeIn");
    }
    public void FadeOut()
    {
        transitionAnim.SetTrigger("fadeOut");
    }
}
