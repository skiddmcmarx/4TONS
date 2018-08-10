
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    [SerializeField]
    private Vector2 cartCoord;
    [SerializeField]
    private Vector3 sceneCoord;
    [SerializeField]
    private Vector2 isoCoord;

    private PoolObject poolObject;

    private void Awake()
    {
        poolObject = GetComponent<PoolObject>();
    }
    
    public void destroyTile()
    {
        poolObject.Destroy();
    }

    public void initializeTile(Vector2 coord, Vector3 sceneCoord)
    {
        cartCoord = coord;
        this.sceneCoord = sceneCoord;
        isoCoord = new Vector2(sceneCoord.x, sceneCoord.y);
    }

    public Vector2 getCartCoord()
    {
        return cartCoord;
    }
    public Vector2 getIsoCoord()
    {
        return isoCoord;
    }
    public Vector2 getSceneCoord()
    {
        return sceneCoord;
    }
}

