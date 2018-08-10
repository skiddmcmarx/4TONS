using UnityEngine;

//********************** responsible for ordering player layers and adjusting position to be pixel perfect.
public class SpriteObject : MonoBehaviour {

    public bool moves;
    [SerializeField]
    private Transform spriteTrans;
    protected SpriteRenderer spriteRenderer;
    private int layerOffset;
    private const int IsometricRangePerYUnit = 100;
    private const int pixelsPerUnit = 32;
    private float positionYOffsetFromGround;
    private Transform groundPos;
    
   
    // Use this for initialization
    private void Start () {
        //ground position is the theoretical position of where the object meets the ground.
        groundPos = this.transform;

        //IMPORTANT: Trans must be set in inspector.
        if (spriteTrans == null)
        {
            print("Object " + gameObject.name + " has no sprite transform assigned. destroying object.");
            Destroy(gameObject);
        }
        else
        {
            spriteRenderer = spriteTrans.GetComponent<SpriteRenderer>();
            spriteRenderer.material = SpriteManager.instance.pixelPerfectMat;
            positionYOffsetFromGround = spriteTrans.localPosition.y;
            layerOffset = (int)(positionYOffsetFromGround * 100);
            //set in inspector
            if (moves)
                subscribeToEvents();
            updateSprite();
        }

	}
    public void subscribeToEvents()
    {
        SpriteManager.instance.updateSpriteLayers += updateSprite;
    }

    public void unsubscribeFromEvents()
    {
        SpriteManager.instance.updateSpriteLayers -= updateSprite;
    }
public void updateSprite()
    {
        correctSpriteOrderLayer();
        CorrectPixelAlignment();
    }
    public void OnDestroy()
    {
        if (moves)
            unsubscribeFromEvents();
    }
    protected virtual void correctSpriteOrderLayer()
    {
            spriteRenderer.sortingOrder = -(int)(groundPos.position.y * IsometricRangePerYUnit);
    }
    private void CorrectPixelAlignment()
    {
        Vector3 newLocalPosition = Vector3.zero;
        newLocalPosition.x = (Mathf.Round(groundPos.position.x * pixelsPerUnit) / pixelsPerUnit) - groundPos.position.x;
        newLocalPosition.y = (Mathf.Round(groundPos.position.y * pixelsPerUnit) / pixelsPerUnit) - groundPos.position.y + positionYOffsetFromGround;
        spriteTrans.localPosition = newLocalPosition;
    }

}
