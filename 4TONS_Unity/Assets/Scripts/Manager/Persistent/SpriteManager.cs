using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;
    private float spriteUpdateInterval = 0.05f;

    public Material pixelPerfectMat;
    public delegate void UpdateSpriteLayers();
    public event UpdateSpriteLayers updateSpriteLayers;

    private void Awake()
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

        InvokeRepeating("UpdateSprites", spriteUpdateInterval, spriteUpdateInterval);
    }
   
    public void LateUpdate()
    {
        if (updateSpriteLayers != null)
            updateSpriteLayers();
    }
	
}
