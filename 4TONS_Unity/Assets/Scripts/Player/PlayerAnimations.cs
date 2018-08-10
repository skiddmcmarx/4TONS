using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    [SerializeField]
    private PlayerEventController playerEventController;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private SpriteRenderer playerSprite;
    [SerializeField]
    private SpriteRenderer weaponSprite;
    [SerializeField]
    private SpriteRenderer cursorSprite;

    public Coroutine currentAnimation;

    [SerializeField]
    private Color defaultColor = new Color(1, 1, 1, 1);
    [SerializeField]

    private Color opaqueColor = new Color(1, 1, 1, 0.3f);
    
    [SerializeField]
    private Animator anim;

	void Start () {
		
	}
	public void InitializePlayerAnimations(PlayerBehaviours playerBehaviours)
    {
        playerSprite = transform.Find("playerFeet").GetChild(0).GetComponent<SpriteRenderer>();
        weaponSprite = transform.Find("staffPivot").GetChild(1).GetComponent<SpriteRenderer>();
        cursorSprite = transform.Find("cursorAnchor").GetChild(0).GetComponent<SpriteRenderer>();
        anim = playerSprite.GetComponent<Animator>();
        playerController = playerBehaviours.playerController;
        playerEventController = playerBehaviours.playerEventController;
        playerEventController.rollDodge += playDodgeAnimation;
    }
    public void AnimationUpdate(float movementSpeed, float xDirection)
    {
        anim.SetFloat("speed", movementSpeed);
        if (xDirection > 0 && playerSprite.flipX == true)
        {
            playerSprite.flipX = false;
        }
        else if (xDirection < 0 && playerSprite.flipX == false){
            playerSprite.flipX = true;
        }
    }
    public void overrideAnimations(Coroutine routine)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);
        currentAnimation = routine;
    }
    public void playDodgeAnimation(DodgeInfo dodgeInfo)
    {
        StartCoroutine(DodgeRoutine(dodgeInfo));

    }
    public IEnumerator DodgeRoutine(DodgeInfo dodgeInfo)
    {
        anim.SetTrigger("Dodge");
        weaponSprite.enabled = false;
        cursorSprite.color = opaqueColor;
        playerSprite.color = opaqueColor;
        float invulnTime = dodgeInfo.invulnTime;
        float cooldown = dodgeInfo.cooldown;
        yield return new WaitForSeconds(invulnTime);
        playerSprite.color = defaultColor;
        yield return new WaitForSeconds(cooldown - invulnTime);
        currentAnimation = null;
        weaponSprite.enabled = true;
        cursorSprite.color = defaultColor;
    }
}
