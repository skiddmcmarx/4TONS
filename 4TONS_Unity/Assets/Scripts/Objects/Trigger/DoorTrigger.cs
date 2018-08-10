using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

    LevelManager levelManager;
    public int roomIndex;
    public GameObject wallCollider;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        wallCollider = transform.GetChild(0).gameObject;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!LevelManager.instance.movingSomething)
        {
            LevelManager.instance.loadRoom(roomIndex);
            print("tutorial trigger entered by " + other.gameObject.name);
            StartCoroutine(openDoor());
        }
    }
    private IEnumerator openDoor()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
