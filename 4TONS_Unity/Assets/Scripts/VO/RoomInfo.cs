using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomInfo : MonoBehaviour {

    public Transform roomBody;
    public TextAsset roomCsv;
    public Color blockTint;
    public Vector3 originPoint;
    public int roomIndex;
    public int[] connectingRooms;
    public int[] blockPoolDepths = new int[10];


    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(originPoint, new Vector3(0.4f, 0.4f, 0.4f));
    }
}
