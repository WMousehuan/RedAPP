using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GridHandle
{
    public Vector2 currentPos;
    public int currentIndex = -1;
    public List<GridPack> gridPacks;
    public GridPack currentGridPack => gridPacks[currentIndex];

    public GridPack this[int index] =>  gridPacks[index];

    public GridPack CatchGrid(Vector2 pos)
    {
        GridPack currentGridPack = null;
        for (int i = 0; i < gridPacks.Count; i++)
        {
            if (gridPacks[i].item.gameObject.activeSelf)
            {
                if (currentGridPack == null)
                {
                    currentIndex = i;
                    currentGridPack = gridPacks[i];
                }
                else
                {
                    if ((currentGridPack.pos - pos).magnitude > (gridPacks[i].pos - pos).magnitude)
                    {
                        currentIndex = i;
                        currentGridPack = gridPacks[i];
                    }
                }
            }
        }

       // this.currentGridPack = currentGridPack;
        currentPos = currentGridPack.pos;

        return currentGridPack;
    }

    public GridPack MoveCatchGrid(Vector2 direction)
    {
        return CatchGrid(currentPos + direction);
    }

    [System.Serializable]
    public class GridPack
    {
        public Vector2 pos;
        public Transform item;
    }
}
