using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour, IManager {

    private Transform trans;
    public static LevelManager instance;
    public Material pixelPerfectMat;
    public Color blockTint;
    public float spawnSpeed;
    public int roomNumber;
    public TextAsset levelFile;
    public RoomInfo[] rooms;
    [SerializeField]
    private GameObject[] blockSet;
    [SerializeField]
    public int[] currentBlockPoolDepths;
    //public static Dictionary<int, int> currentBlockPoolDepths = new Dictionary<int, int>();
    public static Dictionary<int, RowInfo> rowDatabase = new Dictionary<int, RowInfo>();
    public static List<TileInfo> primaryRoomBlocks = new List<TileInfo>();
    public static List<TileInfo> secondaryRoomBlocks = new List<TileInfo>();
    private int rowCount = 0; // X
    private int maxRowCount = 28;
    private Vector2 maxCoords = new Vector2(13, 13);// array index (1-28)
    private int columnCount;
    private Transform breakPoint;
    private Transform originPoint;
    private Text cursorCoordinateText;

    public bool movingSomething;
    public Transform puppetTransform;
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.2f;



    private CompositeCollider2D compCol;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //we start at one to skip the empty tile (or 0)
        
        trans = GetComponent<Transform>();
        breakPoint = trans.Find("breakPoint").GetComponent<Transform>();
        breakPoint.parent = null;
        originPoint = trans.Find("originPoint").GetComponent<Transform>();
        originPoint.parent = null;
        compCol = GetComponent<CompositeCollider2D>();
        loadRoom(0);
    }
    void Start()
    {
        //calculatePoolDepths();
        for (int i = 1; i < blockSet.Length; i++)
        {
                print("creating pool for block " + i + ", pool Depth: " + currentBlockPoolDepths[i] + ", ");
                PoolManager.instance.CreatePool(blockSet[i], currentBlockPoolDepths[i]);
        }
        foreach (RoomInfo room in rooms)
        {
            StartCoroutine(ParseLevelFile(room.roomCsv, room.originPoint, room.blockTint));
        }
        subscribeToEvents();
    }
    public void calculatePoolDepths()
    {
        for(int a = 0; a < rooms.Length; a++)
        {
            LevelUtils.countBlocks(rooms[a]);
        }

        //this loop goes through each room, adds up pool depths on all matching setpiece indexes, 
        //and compares that to the current global pool depth for that setpiece index to obtain a highest possible count for that particular block.
        foreach (RoomInfo room in rooms)
        {
            for (int i = 0; i < room.connectingRooms.Length; i++)
            {
                RoomInfo connectingRoom = rooms[room.connectingRooms[i]];
                print("comparing room " + room.roomIndex + " and room " + room.connectingRooms[i]);
                for (int j = 0; j < room.blockPoolDepths.Length; j++)
                {
                   int possibleMaxPoolDepth = room.blockPoolDepths[j] + connectingRoom.blockPoolDepths[j];
                            if (possibleMaxPoolDepth > currentBlockPoolDepths[j])
                            {
                                currentBlockPoolDepths[j] = possibleMaxPoolDepth;
                            }
                            else if (room.blockPoolDepths[j] > currentBlockPoolDepths[j])
                            {
                                currentBlockPoolDepths[j] = room.blockPoolDepths[j];
                            }
                }
            }
        }

      
    }
    //roomNumber = index of room (in sequence) this is since the launch of hub center.
    public List<TileInfo> buildRoomPreparation()
    {
        roomNumber++;
        List<TileInfo> thisRoom = new List<TileInfo>();
        switch (roomNumber - 1)
        {
            case 0:
                //first room. There is nothing to destroy, and there is no current 
                //primary or secondary at this state.
                break;
            case 1:
                //secondary room. There is only a primary, no secondary. Load current primary into secondary,
                // and load these tiles into primary.
                secondaryRoomBlocks = primaryRoomBlocks;
                primaryRoomBlocks.Clear();
                break;
            default:
                //After the first two rooms, this is a consistent process.
                //clear out the secondary to make room, load primary into secondary
                foreach (TileInfo tile in secondaryRoomBlocks)
                {
                    print("Destroying block");
                    tile.destroyTile();
                }
                secondaryRoomBlocks.Clear();
                secondaryRoomBlocks = primaryRoomBlocks;
                primaryRoomBlocks.Clear();
                break;
        }

        return thisRoom = primaryRoomBlocks;
    }
    public IEnumerator ParseLevelFile(TextAsset csv,  Vector3 startingPoint, Color blockTint)
    {
        buildRoomPreparation();
        Vector3 startingPointRounded = IsometricUtils.TranslateSceneToIso(startingPoint);
        print("csv starting point: " + startingPointRounded);
        int xOffset = Mathf.RoundToInt(startingPointRounded.x);
        int yOffset = Mathf.RoundToInt(startingPointRounded.y);
        breakPoint.position = startingPoint;
        originPoint.position = startingPoint;
        string[] rows = csv.text.Split("\n"[0]);

        //starts on bottom row of csv.
        print("rows.length: " + rows.Length);
        //i = Y value
        for (int i = 0; i <= rows.Length - 2; i++)
        {
            int yCoord = i + yOffset;
            RowInfo row;
            if (!rowDatabase.ContainsKey(yCoord))
            {
                print("creating new row. " + yCoord);
                row = new RowInfo();
                row.rowIndex = yCoord;
                rowDatabase.Add(yCoord, row);
            }
            else
            {
                print("appending block to existing row.");
                row = rowDatabase[i];
            }
            print("row (yValue): " + i);
            string[] rowTiles = rows[(rows.Length - 2) - i].Split(',');
            //j = X value
            for (int j = 0; j < rowTiles.Length; j++)
            {
                print("parsing "+ rowTiles[j] + " to int");
                int setPieceIndex = int.Parse(rowTiles[j]);
                if (setPieceIndex != 0)
                {
                    yield return new WaitForSeconds(spawnSpeed);
                    int xCoord = j + xOffset;
                    print(xCoord + ", " + yCoord + " tile value: " + setPieceIndex);
                    TileInfo tile = new TileInfo();
                    if (setPieceIndex > 0 && blockSet[setPieceIndex] != null)
                    {
                        print("blockset[setpieceIndex] = " + blockSet[setPieceIndex]);
                    GameObject GO = PoolManager.instance.ReuseObject(blockSet[setPieceIndex], originPoint.position, Quaternion.identity);
                    GO.transform.parent = this.transform;
                    GO.transform.name += (" (" + xCoord + ", " + yCoord + ")");
                    tile = GO.GetComponent<TileInfo>();
                    tile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = blockTint;

                    }
                    tile.initializeTile(new Vector2 (xCoord, yCoord), originPoint.position);
                    row.tiles.Add(xCoord, tile);
                }
                originPoint.position += new Vector3(0.5f, 0.25f, 0);
                compCol.GenerateGeometry();
            }
            LineBreak();
        }
        originPoint.position = Vector3.zero;
    }
    public void loadRoom(int roomIndex)
    {
        RoomInfo room = rooms[roomIndex];
        movingSomething = true;
        puppetTransform = room.roomBody;
        targetPosition = room.originPoint;
    }
    public void LineBreak()
    {
        print("line break!");
        originPoint.position = new Vector3((breakPoint.position.x - 0.5f), (breakPoint.position.y + 0.25f), 0);
        breakPoint.position = originPoint.position;
    }
    public void subscribeToEvents()
    {
        //subscriber to these events.
        GameEventManager.instance.joinPlayer += joinPlayer;
        GameEventManager.instance.leavePlayer += leavePlayer;
        GameEventManager.instance.gameUpdate += update;
        GameEventManager.instance.endScene += endScene;
        //GameEventManager.instance.changeScene += changeScene;
        GameEventManager.instance.load += load;
    }
    public void unsubscribeFromEvents()
    {
        //subscriber to these events.
        GameEventManager.instance.joinPlayer -= joinPlayer;
        GameEventManager.instance.leavePlayer -= leavePlayer;
        GameEventManager.instance.gameUpdate -= update;
        GameEventManager.instance.endScene -= endScene;
        //GameEventManager.instance.changeScene -= changeScene;
        GameEventManager.instance.load -= load;
    }
    public void initialize()
    {
    }
    public void load(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    
    }
    public void update()
    {

        if (movingSomething)
        {
            puppetTransform.position = Vector3.SmoothDamp(puppetTransform.position, targetPosition, ref velocity, smoothTime);
            if (Vector3.Distance(puppetTransform.position, targetPosition) < 0.1f)
            {
                puppetTransform.position = targetPosition;
                puppetTransform = null;
                movingSomething = false;
            }
        }
    }
    public void endScene()
    {
        print("level manager ending scene. Clearing row db");
        rowDatabase.Clear();
    }
    public void changeScene(SceneInfo sceneChangeInfo)
    {
    }
    public void joinPlayer(JoinPlayerInfo joinPlayerInfo)
    {
    }
    public void leavePlayer(LeavePlayerInfo leaverPlayerInfo)
    {
    }

    public class RowInfo
    {
        public int rowIndex;
        public Dictionary<int, TileInfo> tiles = new Dictionary<int, TileInfo>();
    }

   
}