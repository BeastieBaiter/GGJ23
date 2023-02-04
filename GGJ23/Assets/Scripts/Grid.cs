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

    // Array of Upgrade objects to spawn
    public GameObject[] upgrades = new GameObject[3];
    // Array of Vector2 to spawn upgrades
    Vector2[] upgradeSpawnPos = new Vector2[3];

    void Start()
    {
        GridBuilder();
    }
    void GridBuilder(){
        Start_Grid();
        Func_Grid();
    }
    void Start_Grid(){
        parent = new GameObject("Parent");
        parent.transform.position = new Vector2(gridStartPos.x, gridStartPos.y);
        for (int i = 0; i < upgrades.Length+1; i++){
            int randX = Random.Range(0, gridWidth);
            int randY = Random.Range(0, gridHeight);
            upgradeSpawnPos[i] = new Vector2(randX, randY);
        }
    }
    void Func_Grid(){
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                double pos = VerifyUpgradePos(upgradeSpawnPos, x, y);
                if(pos == -1)
                    SpawnDirt(x, y);
                else 
                    SpawnUpgrade(x, y, int.Parse(pos.ToString()));
            }
        }
    }
    void SpawnDirt(int x,int y){
        var dirtClone=Instantiate(dirt, new Vector2(x,y), Quaternion.identity);
        dirtClone.name = "Dirt" + x + y;
        dirts.Add(dirtClone);
        dirtClone.transform.parent=parent.transform;
    }
    void SpawnUpgrade(int x,int y,int pos){
        var upgradeClone=Instantiate(upgrades[pos], new Vector2(x,y), Quaternion.identity);
        upgradeClone.name = "Upgrade" + x + y;
        upgradeClone.transform.parent=parent.transform;
    }
    int VerifyUpgradePos(Vector2[] upgradeSpawnPos , int x, int y){
        for (int i = 0; i < upgradeSpawnPos.Length; i++){
            if(upgradeSpawnPos[i].x == x && upgradeSpawnPos[i].y == y)
                return i;
        }
        return -1;
    }
}
