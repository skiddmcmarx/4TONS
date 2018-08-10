using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum OrthoSizes
    {
       CLOSE,
       MEDIUM,
        LONG
    }

public class CameraManager : MonoBehaviour, IManager {

 
    public static CameraManager instance;
    public bool active;
    public List<Transform> targets = new List<Transform>();
    [SerializeField]
    private Camera cam;
    private Transform trans;
    private Transform cameraTrans;
    private Vector3 restingPosition;
    private Transform backgroundTransform;
    private float parallaxSpeed = 0.2f;
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.2f;
    public OrthoSizes orthoSize;

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

    }
    public void Start()
    {
        print("camera start.");
        //initialize();
    }
    public void initialize()
    {

        trans = GetComponent<Transform>();
        cam = transform.GetChild(0).GetComponent<Camera>();
        cam.orthographicSize = GetOrthoSize(orthoSize);
        cameraTrans = transform.GetChild(0).GetComponent<Transform>();
        restingPosition = cameraTrans.position;
        active = false;
        subscribeToEvents();
    }
    public void subscribeToEvents()
    {
        GameEventManager.instance.joinPlayer += joinPlayer;
        GameEventManager.instance.leavePlayer += leavePlayer;
        GameEventManager.instance.gameUpdate += update;
        GameEventManager.instance.endScene += endScene;
        GameEventManager.instance.changeScene += changeScene;
        GameEventManager.instance.load += load;

    }
    public void unsubscribeFromEvents()
    {
        GameEventManager.instance.joinPlayer -= joinPlayer;
        GameEventManager.instance.leavePlayer -= leavePlayer;
        GameEventManager.instance.gameUpdate -= update;
        GameEventManager.instance.endScene -= endScene;
        GameEventManager.instance.changeScene -= changeScene;
        GameEventManager.instance.load -= load;
    }
    public void load(int sceneIndex)
    {
        print("loading Camera Manager");
        backgroundTransform = GameObject.Find("Background").GetComponent<Transform>();
        //subscribeToEvents();
    }
    public void endScene()
    {
        active = false;
        targets.Clear();
        //unsubscribeFromEvents();
    }
    public void changeScene(SceneInfo changeSceneInfo)
    {
        switch (changeSceneInfo.sceneIndex)
        {
            case 0:
                cam.orthographicSize = GetOrthoSize(OrthoSizes.CLOSE);
                break;
            case 1:
                cam.orthographicSize = GetOrthoSize(OrthoSizes.CLOSE);
                break;
            case 2:
                cam.orthographicSize = GetOrthoSize(OrthoSizes.MEDIUM);
                break;
            default:
                break;
        }
    }
    public void joinPlayer(JoinPlayerInfo joinPlayerInfo)
    {
        print("cameraManager adding target");
        targets.Add(joinPlayerInfo.trans);
        targets.Add(joinPlayerInfo.trans.Find("cursorAnchor").GetChild(0).GetComponent<Transform>());
        active = true;
    }
    public void leavePlayer(LeavePlayerInfo leavePlayerInfo)
    {
        targets.Remove(leavePlayerInfo.trans);
        targets.Remove(leavePlayerInfo.trans.Find("cursorAnchor").GetChild(0).GetComponent<Transform>());
    }
   
    public float GetOrthoSize(OrthoSizes size)
    {
        switch (size)
        {
            case OrthoSizes.CLOSE:
                return 4.21875f;
            case OrthoSizes.MEDIUM:
                return 8.4375f;
            case OrthoSizes.LONG:
                return 16.875f;
        }
        
        return 0;
    }

    public void update()
    {
        if (active)
        {
            trans.position = Vector3.SmoothDamp(trans.position, FollowPlayers(), ref velocity, smoothTime);
            //CorrectCameraAlignment();
            backgroundTransform.position = new Vector3(trans.position.x * parallaxSpeed, trans.position.y * parallaxSpeed, 1);
        }
    }
    public void changeCameraOrtho(float zoomDirection)
    {
        print("zoom dir = " + zoomDirection);
        if (zoomDirection > 0)
        {
                if (orthoSize == OrthoSizes.CLOSE)
                {
                    orthoSize = OrthoSizes.MEDIUM;
                    cam.orthographicSize = GetOrthoSize(OrthoSizes.MEDIUM);
                }
                else if (orthoSize == OrthoSizes.MEDIUM)
                {
                    orthoSize = OrthoSizes.LONG;
                    cam.orthographicSize = GetOrthoSize(OrthoSizes.LONG);
                }
        }
        else
        {
                if (orthoSize == OrthoSizes.LONG)
                {
                    orthoSize = OrthoSizes.MEDIUM;
                    cam.orthographicSize = GetOrthoSize(OrthoSizes.MEDIUM);
                }
                else if (orthoSize == OrthoSizes.MEDIUM)
                {
                    orthoSize = OrthoSizes.CLOSE;
                    cam.orthographicSize = GetOrthoSize(OrthoSizes.CLOSE);
                }
        }
    }
    private void LateUpdate()
    {
        Vector3 position = cameraTrans.localPosition;

        position.x = (Mathf.Round(trans.position.x * 32) / 32) - trans.position.x;
        position.y = (Mathf.Round(trans.position.y * 32) / 32) - trans.position.y;

        cameraTrans.localPosition = position;
    }
    public Vector3 FollowPlayers()
    {
        Vector3 sum = Vector3.zero;
        
            foreach (Transform targetTrans in targets)
            {
                sum += targetTrans.position;
            }
            sum /= targets.Count;
            sum.z = -10;
            return sum;
    }
}
