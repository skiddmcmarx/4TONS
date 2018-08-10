using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelUtils {

	public static void countBlocks(RoomInfo room)
    {
        TextAsset roomCsv = room.roomCsv;
        string[] rows = roomCsv.text.Split("\n"[0]);


        for (int i = 0; i < rows.Length - 1; i++)
        {
            string[] rowTiles = rows[i].Split(',');
            for (int j = 0; j < rowTiles.Length; j++)
            {
                int setPieceIndex = int.Parse(rowTiles[j]);
                if (setPieceIndex > 0)
                    room.blockPoolDepths[setPieceIndex]++;
            }
        }
    }
}
