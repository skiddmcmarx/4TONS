using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PoolManager : MonoBehaviour
    {
        /*
    ===================================================================================
    the PoolManager script does the following.
    1 - creates spell object pools for each relevant spell in the scene
    2 - childs those spell objects to a transform named and indexed by spell.
    ===================================================================================
    */
        // Dictionary Queue to hold the Unique InstanceID of an object and the Queue of the object type
        Dictionary<int, Queue<ObjectInstance>>[] playerPoolDictionarys = new Dictionary<int, Queue<ObjectInstance>>[4];
        Dictionary<string, Queue<ObjectInstance>> genericPoolDictionary = new Dictionary<string, Queue<ObjectInstance>>();
        Dictionary<string, Queue<ObjectInstance>> spellPoolDictionary = new Dictionary<string, Queue<ObjectInstance>>();
        Transform[] playerPoolParents = new Transform[4];
        Transform genericPoolParent;
        public static int poolIndex = 0;
        // Singleton Pattern to access this script with ease
        public static PoolManager instance;

        void Awake()
        {
        print("pool manager awake");
            instance = GetComponent<PoolManager>();
            genericPoolParent = new GameObject().transform;
            genericPoolParent.name = "GenericPool";
            genericPoolParent.parent = transform;
            for (int i = 0; i < 4; i++)
            {
                playerPoolDictionarys[i] = new Dictionary<int, Queue<ObjectInstance>>();
                playerPoolParents[i] = new GameObject().transform;
                playerPoolParents[i].name = "Player" + (i + 1) + "Pool";
                playerPoolParents[i].parent = transform;
            }
        }
    //used for generic objects that don't favor one player, like static blocks.
    public Transform CreatePool(GameObject prefab, int poolSize)
    {
        print("creating pool for object " + prefab.name);
        string poolKey = prefab.name; // GetInstanceID is a unique ID for every game object

        if (!genericPoolDictionary.ContainsKey(poolKey))
        {

            // Creates an empty game object to help keep track of your pool objects and parents it to the PoolManager
            Transform poolHolder = new GameObject(poolKey + " pool").transform;
            poolHolder.parent = genericPoolParent;
            genericPoolDictionary.Add(poolKey, new Queue<ObjectInstance>());
            // Create the amount of specified prefabs
            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject); // Defaults to Vector3.zero for position and Quaternion.Identity for rotation
                genericPoolDictionary[poolKey].Enqueue(newObject);
                newObject.SetParent(poolHolder);
            }
            return poolHolder;
        }
        else
        {
            Transform poolHolder = GameObject.Find(poolKey + " pool").transform;

            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                genericPoolDictionary[poolKey].Enqueue(newObject);
                newObject.SetParent(poolHolder.transform);
            }
            return poolHolder;
        }

    }


    //generic object pools.
    public GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        string poolKey = prefab.name;

        if (genericPoolDictionary.ContainsKey(poolKey))
        {
            // Dequeue then requeue the object then call the Objects Reuse function
            ObjectInstance objectToReuse = genericPoolDictionary[poolKey].Dequeue();
            genericPoolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.Reuse(position, rotation);
            return objectToReuse.gameObject;
        }
        else
        {
            return null;
        }
    }


    // Custom GameObject class to keep track of the objects transform and other values
    public class ObjectInstance
        {
            public GameObject gameObject;
            Transform transform;

            bool hasPoolObjectComponent;
            PoolObject poolObjectScript;

            public ObjectInstance(GameObject objectInstance)
            {
                gameObject = objectInstance;
                transform = gameObject.transform;
                gameObject.SetActive(false);

                // Keep track if any of the objects scripts inherit the PoolObject script
                if (gameObject.GetComponent<PoolObject>())
                {
                    hasPoolObjectComponent = true;
                    poolObjectScript = gameObject.GetComponent<PoolObject>();
                }
            }

            public ObjectInstance(GameObject objectInstance, PlayerController _playerController)
            {
                gameObject = objectInstance;
                transform = gameObject.transform;
                gameObject.SetActive(false);

                // Keep track if any of the objects scripts inherit the PoolObject script
                if (gameObject.GetComponent<PoolObject>())
                {
                    hasPoolObjectComponent = true;
                    poolObjectScript = gameObject.GetComponent<PoolObject>();
                    //poolObjectScript.loadObject(_playerController);
                }
            }

            // Method called when an ObjectInstance is being reused
            public void Reuse(Vector3 position, Quaternion rotation)
            {
                // Reset the object as specified within it's own class and the PoolObject class


                // Move to desired position then set it active
                gameObject.transform.position = position;
                gameObject.transform.rotation = rotation;

                if (hasPoolObjectComponent)
                {
                    poolObjectScript.ResetObject();
                }
                gameObject.SetActive(true);
            }

            // Set the parent of the Object to help group objects properly
            public void SetParent(Transform parent)
            {
                transform.parent = parent;
            }
        }
    }
