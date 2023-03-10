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

    public int waveCounter { get; private set;}
    private UIManager _uiManager;
    private AudioManager _audioManager;
    private TimeManager _timeManager;
    private CombatManager _combatManager;
    
    public int counter=0;
    public List<GameObject> carrotPhases;
    public GameObject rain;
    
    public int currTreeHealth;
    public int maxTreeHealth;
    public List<GameObject> monsterPrefabs;
    public List<Monster> monsterArmy { get; private set;}
    
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

    public bool battleStarted { get; private set;}
    public bool dmgBuff { get; private set; }
    public bool dmgResistance { get; private set; }

    [SerializeField] private int healthBuff;
    [SerializeField] private float dmgResistanceValue;

    private void Start()
    {
        GridBuilder();
        waveCounter = 1;
        currTreeHealth = maxTreeHealth;
        _audioManager = AudioManager.Instance;
        _uiManager = UIManager.Instance;
        _timeManager = TimeManager.Instance;
        _combatManager = CombatManager.Instance;
        monsterArmy = new List<Monster>();
        battleStarted = false;
        dmgBuff = false;
    }

    public void Update()
    {

        for (int i = 0; i < carrotPhases.Count; i++)
        {
            if (counter == i)
            {
                carrotPhases[i].SetActive(true);
            }
            else
            {
                carrotPhases[i].SetActive(false);
            }
        }
        
        if(Input.GetButtonDown("Fire1")){
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider!= null && hit.collider.gameObject.tag == "Upgrade"){
                _audioManager.Play("Dig");
                counter++;
                Debug.Log(hit.collider.gameObject.name +"clicked");
                hit.collider.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
                hit.collider.gameObject.tag = "Broken";
                GiveBuff(hit.collider.gameObject.GetComponent<Upgrade>().index);
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
                _audioManager.Play("Dig");
                Debug.Log(hit.collider.gameObject.name +"clicked");
                hit.collider.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
                
                hit.collider.gameObject.tag = "Broken";
                hit.collider.gameObject.GetComponent<Dirt>().ChangeRootSprite();

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
            //test
        }

        if (!_timeManager.TimerRunning && !battleStarted)
        {
            battleStarted = true;
            BattleStart();
        }
    }

    private void GiveBuff(int upgrade)
    {
        switch (upgrade)
        {
            case 1:
                maxTreeHealth += healthBuff;
                currTreeHealth = maxTreeHealth;
                _uiManager.ResetHealthBar();
                break;
            case 2:
                dmgBuff = true;
                break;
            case 3:
                dmgResistance = true;
                break;
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
        for (var x = gridStartPos.x; x < gridWidth + gridStartPos.x; x++)
        {
            for (var y = gridStartPos.y; y > -1 * (Math.Abs(gridStartPos.y) + gridHeight); y--)
            {
                //double pos = VerifyUpgradePos(upgradeSpawnPos, x, y);
                if(x == -4.5 && y == -9.5)
                    SpawnUpgrade(x, y, 0);
                else if(x == 0.5 && y == -20.5) 
                    SpawnUpgrade(x, y, 1);
                else if(x == 4.5 && y == -30.5)
                    SpawnUpgrade(x, y, 2);
                else 
                    SpawnDirt(x, y);
            }
        }
        for (float x = gridStartPos.x; x < gridWidth + gridStartPos.x; x++){
            SpawnBedrock(x, (-1 * (Math.Abs(gridStartPos.y) + gridHeight)));
        }

        StartCoroutine(DelayRainStart());
    }
    void SpawnBedrock(float x,float y){
        var bedrockClone=Instantiate(bedrock, new Vector2(x,y), Quaternion.identity);
        bedrockClone.name = "Bedrock" + x + y;
        bedrockClone.transform.parent=parent.transform;
    }
    void SpawnDirt(float x,float y){
        var dirtClone=Instantiate(dirt, new Vector2(x,y), Quaternion.identity);

        if(y==-5.5){
            dirtClone.GetComponent<Dirt>().canBeBroken=true;
        }
        dirtClone.name = "Dirt" + x + y;
        dirts.Add(dirtClone.GetComponent<Dirt>());
        dirtClone.transform.parent=parent.transform;
    }
    void SpawnUpgrade(float x,float y,int pos){
        var upgradeClone=Instantiate(upgrades[pos], new Vector2(x,y), Quaternion.identity);
        upgradeClone.name = "Upgrade" + x + y;
        upgradeClone.transform.parent=parent.transform;
    }
    int VerifyUpgradePos(Vector2[] upgradeSpawnPos , float x, float y){
        for (int i = 0; i < upgradeSpawnPos.Length; i++){
            if(upgradeSpawnPos[i].x == x && upgradeSpawnPos[i].y == y)
                return i;
        }
        return -1;
    }

    IEnumerator DelayRainStart()
    {
        yield return new WaitForSeconds(1f);
        MakeItRain();
    }
    
    /*End of Grid Builder*/
    void MakeItRain(){
        _audioManager.Play("Rain");
        foreach (var dirt in dirts)
        {
            StartCoroutine(Rain());
            dirt.UpdateWaterLevel();
        }
    }

    IEnumerator Rain()
    {
        rain.SetActive(true);
        yield return new WaitForSeconds(3f);
        rain.SetActive(false);
    }

    public void HurtTree(int damage)
    {
        float dmg = damage;
        
        if (dmgResistance)
        {
            dmg *= dmgResistanceValue;
        }
        
        currTreeHealth -= (int) dmg;
        _audioManager.Play("Damage");
        _uiManager.UpdateHealthBar();
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
        battleStarted = false;
        _timeManager.ResetTimer();
        _audioManager.Play("TimerStart");
        MakeItRain();
        //pan da camara para baixo
        //reenable das setas
    }
    
    public void GameOver()
    {
        battleStarted = false;
        Debug.Log("The game is over.");
        _audioManager.Play("Fail");
        _uiManager.OpenEndScreenPanel("GAME OVER\n" + " THE GREAT CARROOT WAS KILLED");
        Time.timeScale = 0;
    }
    public void EndGame()
    {

        battleStarted = false;
        Debug.Log("The game is over.");
        _audioManager.Play("Win");
        _uiManager.OpenEndScreenPanel("VICTORY\n" + " LONG LIVE THE GREAT CARROOT");
        Time.timeScale = 0;
    }   
}
