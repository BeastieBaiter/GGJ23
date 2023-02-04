using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // starting position of the grid
    public Vector2 gridStartPos;
    public int gridWidth;
    public int gridHeight;
    // dirt prefab
    public GameObject dirt; 
    //Parent object
    GameObject parent;
    // List of dirt objects
    public List<GameObject> dirts = new List<GameObject>();

    void Start()
    {
        parent = new GameObject("Parent");
        parent.transform.position = new Vector2(gridStartPos.x, gridStartPos.y);
        Func_Grid();
    }
    void Func_Grid(){
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                SpawnDirt(x,y);
            }
        }
    }
    void SpawnDirt(int x,int y){
        var dirtClone=Instantiate(dirt, new Vector2(x,y), Quaternion.identity);
        dirtClone.name = "Dirt" + x + y;
        dirts.Add(dirtClone);
        dirtClone.transform.parent=parent.transform;
    }
}
