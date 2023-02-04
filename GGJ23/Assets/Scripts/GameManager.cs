using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) 
            Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private int waveCounter;
    private UIManager _uiManager;
    private AudioManager _audioManager;
    private TimeManager _timeManager;
    private CombatManager _combatManager;
    
    public int counter=0;
    [HideInInspector] public int currTreeHealth;
    public int maxTreeHealth;
    public List<GameObject> monsterPrefabs;
    public List<Monster> monsterArmy { get; private set;}
    public List<Monster> enemyArmy { get; private set;}
    
    /* Grid Variables */
    // starting position of the grid
    public Vector2 gridStartPos;
    public int gridWidth;
    public int gridHeight;
    // dirt prefab
    public GameObject dirt; 
    // bedrock prefab
    public GameObject bedrock;
    //Parent object
    GameObject parent;
    // List of dirt objects
    public List<Dirt> dirts = new List<Dirt>();
    // Array of Upgrade objects to spawn
    public GameObject[] upgrades = new GameObject[3];
    // Array of Vector2 to spawn upgrades
    Vector2[] upgradeSpawnPos = new Vector2[3];
    
    
    private void Start()
    {
        GridBuilder();
        waveCounter = 0;
        currTreeHealth = maxTreeHealth;
        _audioManager = AudioManager.Instance;
        _uiManager = UIManager.Instance;
        _timeManager = TimeManager.Instance;
        _combatManager = CombatManager.Instance;
    }

    public void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider!= null && hit.collider.gameObject.tag == "Upgrade"){
                counter+=1;
                Debug.Log(hit.collider.gameObject.name +"clicked");
                hit.collider.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
                hit.collider.gameObject.tag = "Broken";
                hit.collider.gameObject.SetActive(false);
            }
            if (hit.collider != null && hit.collider.gameObject.tag == "Bedrock" && hit.collider.gameObject.GetComponent<Bedrock>().canBeBroken)
            {
                EndGame();
            }
            
            if (hit.collider != null && hit.collider.gameObject.tag == "Dirt" && hit.collider.gameObject.GetComponent<Dirt>().canBeBroken)
            {
                //remove Dirt if clicked
                //log unity
                Debug.Log(hit.collider.gameObject.name +"clicked");
                hit.collider.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
                
                hit.collider.gameObject.tag = "Broken";
                hit.collider.gameObject.SetActive(false);

                int waterLevel = hit.collider.gameObject.GetComponent<Dirt>().waterLevel;

                if (waterLevel > 0)
                {
                    GameObject m = Instantiate(monsterPrefabs[waterLevel - 1], hit.collider.gameObject.transform.position,
                        Quaternion.identity);
                    monsterArmy.Add(m.GetComponent<Monster>());
                }
                
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeItRain();
        }

        if (!_timeManager.TimerRunning)
        {
            BattleStart();
        }
    }
    /*Grid Builder*/
    void GridBuilder(){
        Start_Grid();
        Func_Grid();
    }
    void Start_Grid(){
        parent = new GameObject("Parent");
        parent.transform.position = new Vector2(gridStartPos.x, gridStartPos.y);
        for (int i = 1; i < upgrades.Length+1; i++){
            int randX = UnityEngine.Random.Range(gridWidth*(i-1)/upgrades.Length, gridWidth*i/upgrades.Length);
            int randY = UnityEngine.Random.Range(gridHeight*(i-1)/upgrades.Length, gridHeight*(i)/upgrades.Length);
            upgradeSpawnPos[i-1] = new Vector2(randX, randY);
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
        for (int x = 0; x < gridWidth; x++){
            SpawnBedrock(x, gridHeight);
        }
    }
    void SpawnBedrock(int x,int y){
        var bedrockClone=Instantiate(bedrock, new Vector2(x,y), Quaternion.identity);
        bedrockClone.name = "Bedrock" + x + y;
        bedrockClone.transform.parent=parent.transform;
    }
    void SpawnDirt(int x,int y){
        var dirtClone=Instantiate(dirt, new Vector2(x,y), Quaternion.identity);
        // TO DO: change to the carrot coords
        if(x==0 && y==0){
            dirtClone.GetComponent<Dirt>().canBeBroken=true;
        }
        dirtClone.name = "Dirt" + x + y;
        dirts.Add(dirtClone.GetComponent<Dirt>());
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
    /*End of Grid Builder*/
    void MakeItRain(){
        foreach (var dirt in dirts)
        {
            dirt.UpdateWaterLevel();
        }
    }

    public void HurtTree(int damage)
    {
        Debug.Log("The tree took " + damage + " damage");
    }

    public void BattleStart()
    {
        _timeManager.StopTime();
        //MOVER A CAMARA
        //dar disable das setas
        CombatManager.Instance.StartBattle();
    }
    
    public void BattleOver(List<Monster> newMonsterArmy)
    {
        _timeManager.ResetTimer();
        //pan da camara para baixo
        //reenable das setas
    }
    
    public void GameOver()
    {
        Debug.Log("The game is over.");
    }
    public void EndGame()
    {
        Debug.Log("The game is over.");
    }   
}
