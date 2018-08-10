using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFromOtherSpriteObject : SpriteObject{

    public SpriteRenderer otherSprite;
    public int offsetFromOther;
    protected override void correctSpriteOrderLayer()
    {
        spriteRenderer.sortingOrder = otherSprite.sortingOrder + offsetFromOther;
    }
}
