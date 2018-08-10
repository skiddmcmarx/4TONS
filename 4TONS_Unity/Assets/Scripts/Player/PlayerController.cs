using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour, IWizardComponent {

    protected PlayerAnimations playerAnimations;
    protected PlayerEventController playerEventController;

    protected DodgeInfo playerDodgeInfo;
    protected Player input;
    protected int inputIndex;
    protected int playerNumber;
    public float currentMovementSpeed;


    protected bool casting = false;
    protected bool canDodge = true;

    //movement variables
    protected Rigidbody2D rb;
    [SerializeField]
    private const int moveSpeed = 1000;
    private Vector2 moveDirection;

    //aiming variables
    public Vector3 cursorDirection;
    private Vector3 velocity = Vector3.zero;
    private const float cursorReach = 6f;
    private const float cursorSmooth = 0.15f;
    private const float joystickDeadzone = 0.05f;

    //hitboxes
    private BoxCollider2D hitBox;
    private CircleCollider2D footCollider;
    //transforms
    public Transform trans;
    public Transform cursorAnchor;
    public Transform cursor;
    public Transform staffPivot;
    public void Awake()
    {
        
    }
    public void initialize(PlayerBehaviours playerBehaviours)
    {
        print("player controller initialize");
        playerEventController = playerBehaviours.playerEventController;
        playerDodgeInfo = playerBehaviours.wizardData.dodgeInfo;
        this.inputIndex = playerBehaviours.inputIndex;
        input = ReInput.players.GetPlayer(inputIndex);
        rb = GetComponent<Rigidbody2D>();  
        findLocations();
        playerAnimations = playerBehaviours.playerAnimations;
        subscribeToEvents();
    }

    public void subscribeToEvents()
    {
        print("player controller sub to events");
        playerEventController.rollDodge += dodgeSubscriber;
        GameEventManager.instance.endScene += endScene;
        PlayerInputManager.instance.updateInput += update;
        PlayerInputManager.instance.fixedUpdateInput += fixedUpdate;
    }
    public void unsubscribeFromEvents()
    {
        print("player controller UNSUB from events");
        playerEventController.rollDodge -= dodgeSubscriber;
        GameEventManager.instance.endScene -= endScene;
        PlayerInputManager.instance.updateInput -= update;
        PlayerInputManager.instance.fixedUpdateInput -= fixedUpdate;
    }
    private void OnDestroy()
    {
        print("wizard ondestroy");
        //endScene();
    }
    private void findLocations()
    {
        print("finding player locations.");
        trans = transform;
        hitBox = trans.Find("hitBox").GetComponent<BoxCollider2D>();
        footCollider = trans.Find("playerFeet").GetComponent<CircleCollider2D>();
        cursorAnchor = trans.Find("cursorAnchor").transform;
        cursor = cursorAnchor.Find("cursor").transform;
        staffPivot = trans.Find("staffPivot").transform;
    }
    public void endScene()
    {
        unsubscribeFromEvents();
        print("endScene - PlayerController");
        Destroy(gameObject);
    }
    // Update is called once per frame
    public virtual void update()
    {
        print("player controller update");
       // print("player update");
        if (input.GetButtonDown("RollDodge") && !casting && canDodge)
        {
            playerEventController.RollDodgeEvent(playerDodgeInfo);
            StartCoroutine(RollDodge(playerDodgeInfo));
        }
        cameraZoomInputUpdate();
    }
    public void fixedUpdate()
    {
        Movement();
        Aiming();
    }
    public void Movement()
    {
        //input normalizing
        moveDirection.x = input.GetAxisRaw("MoveHorizontal");
        moveDirection.y = input.GetAxisRaw("MoveVertical");
        moveDirection.Normalize();
        //horizontal movement
        rb.AddForce(Vector2.right * moveSpeed * moveDirection.x);
        rb.AddForce(Vector2.up * (moveSpeed / 2) * moveDirection.y);
        currentMovementSpeed = rb.velocity.magnitude;
    }
    
    void Aiming()
    {


        cursorDirection = new Vector3(input.GetAxis("CursorX"), input.GetAxis("CursorY"), 0);
        cursorDirection = Vector3.ClampMagnitude(cursorDirection, 1);
        cursorDirection.y = cursorDirection.y / 2;
        cursorDirection *= cursorReach;
        if (inputIndex == 0)
            cursor.position = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5.0f)));
        else
        {
            print("inputIndex = " + inputIndex + ". radial cursor mode. ");
            cursor.localPosition = (Vector3.SmoothDamp(cursor.localPosition, cursorDirection, ref velocity, cursorSmooth));

        }

        float dist = Vector2.Distance(cursorAnchor.position, cursor.position);
        if (dist > joystickDeadzone)
        {
            Vector3 dir = cursor.position - staffPivot.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            staffPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            if (cursor.position.x > trans.position.x)
            {
                staffPivot.rotation = new Quaternion(0, 0, 0, 0);
            }
            else
            {
                staffPivot.rotation = new Quaternion(0, 0, 180, 0);
            }
        }
        playerAnimations.AnimationUpdate(rb.velocity.magnitude, (cursor.position.x - trans.position.x));
    }
    private void cameraZoomInputUpdate()
    {
        if (input.GetButtonDown("CameraZoomIn"))
            CameraManager.instance.changeCameraOrtho(-1);
        else if (input.GetButtonDown("CameraZoomOut"))
            CameraManager.instance.changeCameraOrtho(1);
    }
    private void dodgeSubscriber(DodgeInfo dodgeInfo)
    {
        StartCoroutine(RollDodge(dodgeInfo));
    }
    IEnumerator RollDodge(DodgeInfo dodgeInfo)
    {
        canDodge = false;
        casting = true;
        float dodgePower = dodgeInfo.dodgePower;
        float invulnTime = dodgeInfo.invulnTime;
        float cooldown = dodgeInfo.cooldown;
        rb.AddRelativeForce(Vector3.right * dodgePower * input.GetAxis("MoveHorizontal"), ForceMode2D.Impulse);
        rb.AddRelativeForce(Vector3.up * (dodgePower / 2) * input.GetAxis("MoveVertical"), ForceMode2D.Impulse);
        hitBox.enabled = false;
        //playerVitals.SetInvulnerable();
        yield return new WaitForSeconds(invulnTime);
        hitBox.enabled = true;
        //playerVitals.SetVulnerable();
        yield return new WaitForSeconds(cooldown - invulnTime);
        canDodge = true;
        casting = false;
    }
}
