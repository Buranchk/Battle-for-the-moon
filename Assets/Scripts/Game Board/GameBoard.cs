using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    //settings
    public int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Camera cam;
    [SerializeField] private Unit spaceMan;
    [SerializeField] private Unit XP;
    [SerializeField] private Unit gold;
    [SerializeField] private Unit ruby;
    [SerializeField] private EnemyAI enemyPrefab;
    

    //in game stuff
    //selectedUnit suggestSystem windowRPS
    public GameObject selectedUnit;
    public EnemyAI eUnit;
    public Unit fUnit;
    private string [ , ] map;
    public bool turn = false;
    public bool suggestSystem = false;
    public GameObject windowRPS;
    public GameObject buttonShuffle;
    public GameObject buttonDone;
    public GameObject decoyText;
    public GameObject flagText;
    public GameObject reshuffleText;
    public GameObject StartGame;
    public GameObject gradient;
    public GameObject UnitDecoy;
    public GameObject UnitFlag;
    public RPSMatch frameRPS;
    public Tweens tweens;
    public Timer timer;

    public int gameStage = 0;

    //private
    private Unit gameVarUnit;
    [SerializeField] private Sprite buttonDoneActive;
    [SerializeField] private Sprite buttonDoneInactive;
    private GameObject PermFlag = null;
    private GameObject PermDecoy = null;
    private bool DecoyAlive = true;
    private Dictionary<Vector2, Tile> tiles;
    private GameObject lastSelectedUnit;
    [HideInInspector] public List<GameObject> units = new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();
    public bool gameWin = true;

    void Start()
    {
        GenerateGrid();
        gradient.SetActive(true);
        StartCoroutine(SpawnUnits());
    }

    private void Update()
    {
        //Deselect Unit
        if (Input.GetMouseButtonDown(0))
            if(selectedUnit != null)
                if(!selectedUnit.GetComponent<Unit>().isOverTheUnit)
                {
                    DeselectUnit();
                }
//Should use different shit here, maybe... someday.....
    }

/* Start */

    void GenerateGrid() 
    {
        tiles = new Dictionary<Vector2, Tile>();
        
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y, 1), Quaternion.identity, transform);
                spawnedTile.name = $"Tile {x} {y}";

                bool isOffset = (x % 2 == 0 && y % 2 != 0 || x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                tiles[new Vector2(x, y)] = spawnedTile;
                if(y >= 2){
                    spawnedTile.GetComponent<SpriteRenderer>().color = isOffset ? new Color(0.69f, 0.69f, 0.69f, 1f) : new Color(0.5f, 0.5f, 0.5f, 1f);
                }
            }
        }
        cam.transform.position = new Vector3((float)width/2 -0.5f, (float)height / 2 -1f,-5);
        cam.orthographicSize = ((width > height * cam.aspect) ? (float)width + 1 / (float)cam.pixelWidth * cam.pixelHeight : height + 1) / 2;
    }

    public void SpawnEnemies()
    {
        // fix this later, i shuffle shit in here
        int x = 0;
        for (x = 0; x < width; x++) {
            for (int y = height - 2; y < height; y++) {
                var spawnedUnit = Instantiate(enemyPrefab, new Vector2(x, y), Quaternion.identity, transform);

                spawnedUnit.name = $"Enemy {x} {y}";

                spawnedUnit.Init();

                GameObject newEnemy = GameObject.Find($"Enemy {x} {y}");

                //enemyUnits.Add(GameObject.Find($"Tile {x} {y}").GetComponent<Tile>().unitLinked = GameObject.Find($"Enemy {x} {y}")); 
                enemyUnits.Add(newEnemy);

                GetTileAtPosition(new Vector2(x, y)).unitLinked = newEnemy;

                LeanTween.alpha(newEnemy, 1, 0.75f).setEaseOutBack();
                newEnemy.GetComponent<EnemyAI>().SpawnAnimation();
                
                map[x, y] = "enemyUnit";
            }
        }
        EnemyFlagDecoy();
        Shuffle(enemyUnits);
        int temp = 0;
        x = 0;
        int[] types = {enemyUnits.Count / 3 + enemyUnits.Count % 3, enemyUnits.Count / 3, enemyUnits.Count / 3};
        for (int i = 0; i < 3; i++)
        {
            temp += types[i];
            for (; x < temp; x++)
            {
                if (i == 0)
                    enemyUnits[x].GetComponent<EnemyAI>().ChangeType("rock");
                if (i == 1)
                    enemyUnits[x].GetComponent<EnemyAI>().ChangeType("paper");
                if (i == 2)
                    enemyUnits[x].GetComponent<EnemyAI>().ChangeType("scissors");
            }
        }
        turn = true;
    }

    public void EnemyFlagDecoy()
    {
        int rndF = UnityEngine.Random.Range(0, width - 1);
        enemyUnits.Remove(GetTileAtPosition(new Vector2(rndF, height-1)).unitLinked);
        GetUnitObjectAt(rndF, height - 1).GetComponent<EnemyAI>().type = "flag";
        int rndD = UnityEngine.Random.Range(0, width-1);
        if (rndD == rndF)
        {
            if(rndD == width-1)
                rndD -= 1;
            else
                rndD += 1;
        }
        enemyUnits.Remove(GetTileAtPosition(new Vector2(rndD, height-1)).unitLinked);
        GetUnitObjectAt(rndD, height-1).GetComponent<EnemyAI>().type = "decoy";
    }

    IEnumerator SpawnUnits()
    {
        //Setup the right unit prefab from the DataManager!!!!!
        SelectUnitSkin();
        //Unit map
        map = new string [width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                map[x, y] = "empty"; 

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < 2; y++) {
                var spawnedUnit = Instantiate(fUnit, new Vector2(x, y), Quaternion.identity, transform);

                spawnedUnit.name = $"Unit {x} {y}";

                spawnedUnit.Init();

                //units.Add(GameObject.Find($"Tile {x} {y}").GetComponent<Tile>().unitLinked = GameObject.Find($"Unit {x} {y}"));

                //GameObject.Find($"Tile {x} {y}").GetComponent<Tile>().unitLinked = GameObject.Find($"Unit {x} {y}");

                units.Add(GetTileAtPosition(new Vector2(x, y)).unitLinked = GameObject.Find($"Unit {x} {y}"));

                //GetTileAtPosition(new Vector2(x, y)).unitLinked = GetUnitObjectAt(x, y);

                map[x, y] = "myUnit";
                
                yield return new WaitForSeconds(0.05f);
            }
        }
        NewStage();
    }

    private void SelectUnitSkin()
    {
        int selectedskin = GameObject.Find("Data Manager").GetComponent<DataManager>().GiveSelectedSkin();
        switch (selectedskin){
            case 0:
            fUnit = spaceMan;
            break;

            case 1:
            fUnit = XP;
            break;

            case 2:
            fUnit = gold;
            break;

            case 3:
            fUnit = ruby;
            break;
        }
    }

    public void SetFlagDecoy(GameObject Unit)
    {
        if(gameStage == 1)
        {
            PermFlag = Unit;
            Vector2 flagPos = Unit.transform.position;
            GameObject.Find("Flag").transform.position = flagPos;
            SelectUnselectUnit(Unit.GetComponent<Unit>(), true);
            setDoneActive();
        } 
        
        else if (gameStage == 2)
        {
            if(Unit.transform.position != PermFlag.transform.position)
            {
                PermDecoy = Unit;
                Vector2 decayPos = Unit.transform.position;
                GameObject.Find("Decoy").transform.position = decayPos;
                SelectUnselectUnit(Unit.GetComponent<Unit>(), false);
                setDoneActive();
            }
        }
    }

    private void SelectUnselectUnit(Unit unit, bool isFlag)
    {
        if(gameVarUnit == null)
        {
            if(gameStage == 1)
                tweens.ScaleDownDisappear(GameObject.Find("UnitFlag"));
            else if (gameStage == 2)
                tweens.ScaleDownDisappear(GameObject.Find("UnitDecoy"));

            unit.FlagDecoySelected(isFlag, true);
            gameVarUnit = unit;
        } else
        {
            gameVarUnit.FlagDecoySelected(isFlag, false);
            unit.FlagDecoySelected(isFlag, true);
            gameVarUnit = unit;
        }

    }

    private void ApplyUnitSelection(string state)
    {
        gameVarUnit.FlagDecoySelected(true, false);
        gameVarUnit = null;
        //set by property
        if (state == "Flag")
        {
            GameObject Flag = GameObject.Find("Flag");
            Vector2 flagPos = Flag.transform.position;
            GetUnitAtPosition((int)flagPos.x, (int)flagPos.y).ChangeType("flag");
            units.Remove(GetUnitObjectAt((int)flagPos.x, (int)flagPos.y));
            GetUnitAtPosition((int)flagPos.x, (int)flagPos.y).name = "FlagUnit";
            Destroy(Flag);
        }
        else if (state == "Decoy")
        {
            GameObject Decoy = GameObject.Find("Decoy");
            Vector2 decoyPos = Decoy.transform.position;
            GetUnitAtPosition((int)decoyPos.x, (int)decoyPos.y).ChangeType("decoy");
            units.Remove(GetUnitObjectAt((int)decoyPos.x, (int)decoyPos.y));
            GetUnitAtPosition((int)decoyPos.x, (int)decoyPos.y).name = "DecoyUnit";
            Destroy(Decoy);
        }
    }

/* UI related functions */

//TEMP!!!!
    public void WIN()
    {
        NewStage();
        NewStage();
        NewStage();
        NewStage();
    }

    public void UnitRandomize()
    {
        Shuffle(units);
        int temp = 0;
        int x = 0;
        int[] types = {units.Count / 3 + units.Count % 3, units.Count / 3, units.Count / 3};
        for (int i = 0; i < 3; i++)
        {
            temp += types[i];
            for (; x < temp; x++)
            {
                if (i == 0)
                    units[x].GetComponent<Unit>().ChangeType("rock");
                if (i == 1)
                    units[x].GetComponent<Unit>().ChangeType("paper");
                if (i == 2)
                    units[x].GetComponent<Unit>().ChangeType("scissors");
            }
        }
    }

    private void UnitOutline()
    {
        foreach (GameObject obj in units)
        {
            obj.GetComponent<Unit>().ChangeType("outline");
        }
        
    }

    void Shuffle(List<GameObject> a)
    {
        // Loop array
        for (int i = a.Count - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = UnityEngine.Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overwrite when we swap the values
            GameObject temp = a[i];

            // Swap the new and old values
            a[i] = a[rnd];
            a[rnd] = temp;
        }
    }

    public void GameResult()
    {
        SceneManager.LoadScene("Game Result");
    }

    IEnumerator StartGameFX()
    {
        LeanTween.alpha(gradient, 0f, 0.4f);

        StartGame.SetActive(true);
        LeanTween.scale(StartGame, new Vector3(70f, 70f, 70f), 0.3f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.alpha(StartGame, 1.0f, 0.3f);

        for(int i = 0; i < width; i++)
        {
            StartCoroutine(TileRawAppearence(i));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.4f);

        LeanTween.scale(StartGame, new Vector3(30.3f, 0.3f, 30.3f), 0.2f);
        LeanTween.alpha(StartGame, 0f, 0.2f);
        

        SpawnEnemies();
        yield return new WaitForSeconds(0.2f);
        StartGame.SetActive(false);
        
    }

    IEnumerator TileRawAppearence(int raw)
    {
        
        Vector2 tilePos;
        for(int i = 2; i < height; i++)
        {
            tilePos = new Vector2(raw, i);
            StartCoroutine(MakeTileVisible(tilePos));
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator MakeTileVisible(Vector2 tilePos)
    {
        Tile tile;
        tile = GetTileAtPosition(tilePos);
        tile.MakeTileVisible();
        yield return new WaitForSeconds(0.05f);

        // for(float opacity = 0; opacity <= 1; opacity += 0.05f)
        // {
        //     tile = GetTileAtPosition(tilePos);
        //     tile.setAlpha(opacity);
        //     yield return new WaitForSeconds(0.05f);
        // }
    }
 
/* Usage functions */

    public Tile GetTileAtPosition(Vector2 pos) 
    {
        if (tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public Unit GetUnitAtPosition(int x, int y)
    {
        return GetTileAtPosition(new Vector2(x, y)).unitLinked.GetComponent<Unit>();
    }
    
    public EnemyAI GetEnemyAtPosition(int x, int y)
    {
        return GetTileAtPosition(new Vector2(x, y)).unitLinked.GetComponent<EnemyAI>();
    }

    public GameObject GetUnitObjectAt (int x, int y)
    {
        return GetTileAtPosition(new Vector2(x, y)).unitLinked;
    }

    public void NewStage()
    {
        gameStage = gameStage + 1;
        switch (gameStage)
        {
            case 0:
            //time delay
            break;

            case 1:
            UnitOutline();
            UnitFlag.SetActive(true);
            flagText.SetActive(true);
            tweens.AppearScale(flagText);
            tweens.AppearScale(UnitFlag);
            setDoneInactive();
            break;

            case 2:
            UnitFlag.SetActive(false);
            flagText.SetActive(false);
            UnitDecoy.SetActive(true);
            decoyText.SetActive(true);
            tweens.AppearScale(decoyText);
            tweens.AppearScale(UnitDecoy);
            setDoneInactive();
            ApplyUnitSelection("Flag");
            timer.ResetTimer15();
            break;

            case 3:
            UnitDecoy.SetActive(false);
            decoyText.SetActive(false);
            reshuffleText.SetActive(true);
            tweens.AppearScale(reshuffleText);
            tweens.AppearScale(buttonShuffle);
            buttonShuffle.SetActive(true);
            UnitRandomize();
            setDoneActive();
            ApplyUnitSelection("Decoy");
            timer.ResetTimer15();
            break;

            case 4:
            reshuffleText.SetActive(false);
            buttonShuffle.SetActive(false);
            buttonDone.SetActive(false);
            StartCoroutine(StartGameFX());
            timer.ResetTimer();
            break;

            case 5:
            if(gameWin == true)
            {
                print("GameResult: Win");
                PlayerPrefs.SetInt("GameResult", 1);
            }
            if (gameWin == false)
            {
                print("GameResult: Loose");
                PlayerPrefs.SetInt("GameResult", 0);
            }

            // if(enemyUnits.Count == 0)
            // {
            //     print("GameResult: Win");
            //     PlayerPrefs.SetInt("GameResult", 1);
            // }
            // else if(units.Count == 0)
            // {
            //     print("GameResult: Loose");
            //     PlayerPrefs.SetInt("GameResult", 0);
            // }
            GameResult();
            break;
        }
    }

/* Unit functions */
    public void SelectUnit(GameObject newSelectedUnit)
    {
        //newSelectedUnit.GetComponent<Unit>().highlight.SetActive(true);
        newSelectedUnit.GetComponent<Unit>().PlayOneShotAnimation("jump", 0.6f);
        if (selectedUnit != newSelectedUnit && selectedUnit != null)
            ChangeUnit(newSelectedUnit);
        else if (selectedUnit == newSelectedUnit && selectedUnit != null)
            DeselectUnit();
        else if (selectedUnit == null){
            selectedUnit = newSelectedUnit;
            SuggestMoves(true);
        }
    }

    public void DeselectUnit()
    {   
        if(selectedUnit != null){
            //selectedUnit.GetComponent<Unit>().highlight.SetActive(false);
            selectedUnit.GetComponent<Unit>().setAnimation("animation");
            SuggestMoves(false);
            selectedUnit = null;
        }
    }

    public void ChangeUnit(GameObject newSelectedUnit)
    {
        DeselectUnit();
        selectedUnit = newSelectedUnit;
        SuggestMoves(true);
    }

/* Step functions */
    public void SuggestMoves(bool isActiveSelection)
    {
        //Check map bounds
        //Highlight possible Unit moveset
        int x = ((int)selectedUnit.transform.position.x);
        int y = ((int)selectedUnit.transform.position.y);
        TileHighlighting(x+1, y, isActiveSelection);
        TileHighlighting(x-1, y, isActiveSelection);
        TileHighlighting(x, y+1, isActiveSelection);
        TileHighlighting(x, y-1, isActiveSelection);
    }

    void TileHighlighting(int x, int y, bool isActiveSelection)
    {
        if(y < height && y >= 0 && x >= 0 && x < width){
            if (map[x, y] == "empty")
            {
                GetTileAtPosition(new Vector2(x, y)).TileHighlight(selectedUnit, isActiveSelection);
            }
            else if (map[x, y] == "enemyUnit") // fix here
            {
                GetEnemyAtPosition(x, y).movedOn = isActiveSelection;
                GetEnemyAtPosition(x, y).highlight.SetActive(isActiveSelection);
            }
        }
    }

    public void TileStep(int x, int y, int xe, int ye)
    {
        StartCoroutine(UnitStep(x, y, xe, ye));
    }

    IEnumerator UnitStep(int x, int y, int xe, int ye)
    {
        Unit unitScript = GetUnitObjectAt(xe, ye).GetComponent<Unit>();
        unitScript.TrailSwitch(true);
        //Unit link to a new Tile
        GetTileAtPosition(new Vector2 (x, y)).GetComponent<Tile>().unitLinked = selectedUnit;

        //Selected Unit = null, SuggestMoves(false)
        DeselectUnit();

        //move Unit
        LeanTween.move(GetUnitObjectAt(xe, ye), new Vector2(x, y), 0.4f).setEaseInOutQuint();
        yield return new WaitForSeconds(0.4f);

        //delete old Unit link
        GetTileAtPosition(new Vector2 (xe, ye)).GetComponent<Tile>().unitLinked = null;

        //Map Update
        map[x, y] = (string)map[xe, ye];
        map[xe, ye] = "empty";

        print(GetTileAtPosition(new Vector2 (x, y)).GetComponent<Tile>().unitLinked.name + " makes a step");

        //switch the turn
        yield return new WaitForSeconds(0.15f);
        unitScript.TrailSwitch(false);
        turn = !turn;
        EnemyAI();
    }

/* Fight functions */
    public void DestroyUnit(GameObject Unit)
    {
        GetTileAtPosition(Unit.transform.position).unitLinked = null;
        map[(int)Unit.transform.position.x, (int)Unit.transform.position.y] = "empty";
        if(Unit.tag == "Enemy")
        {
            enemyUnits.Remove(Unit);
            if(enemyUnits.Count == 0)
            {
                gameWin = true;
                NewStage();
            }

        }
        else
        {
            units.Remove(Unit);
            if(units.Count == 0)
            {
                gameWin = false;
                NewStage();
            }
        }
        Destroy(Unit);
        print (Unit.name + " was destroyed");
    }

    public void UnitFight()
    {
        print("Units duel now");
        GameObject fUnitObj = GameObject.Find(fUnit.name); //f
        GameObject eUnitObj = GameObject.Find(eUnit.name); //e

        print("Enemy was - " + eUnit.type + "    Friend was - " + fUnit.type);


        if(eUnit.type == "flag")
        {
            print("Enemy Flag is fucked");
            DestroyUnit(eUnitObj);
            //fUnit.highlight.SetActive(false);
            gameWin = true;
            NewStage();
        }

        if(fUnit.type == "flag")
        {
            print("Friendly Flag is fucked");
            DestroyUnit(fUnitObj);
            eUnit.highlight.SetActive(false);
            gameWin = false;
            NewStage();
        }

        if(eUnit.type == "decoy" || fUnit.type == "decoy")
        {
            //fUnit.highlight.SetActive(false);
            eUnit.highlight.SetActive(false);
            DestroyUnit(fUnitObj);
            DestroyUnit(eUnitObj);
            turn = !turn;
            if(!turn)
                EnemyAI();
        }

        if (eUnit.type == fUnit.type)
        {
            windowRPS.SetActive(true);
            frameRPS.Appear();
        }

        if(RPS(eUnit.type, fUnit.type) && eUnit.type != fUnit.type) //e
        {
            frameRPS.RegularRPS();
            eUnit.isOpen = true;
            eUnit.ChangeType(eUnit.type);
            eUnit.movedOn = false;
            DestroyUnit(fUnitObj);
            eUnit.highlight.SetActive(false);
            turn = !turn;
            if(!turn)
                EnemyAI();
        }
        else if(!RPS(eUnit.type, fUnit.type) && eUnit.type != fUnit.type) //f
        {
            frameRPS.RegularRPS();
            fUnit.isOpen = true;
            fUnit.ChangeType(fUnit.type);
            fUnit.movedOn = false;
            DestroyUnit(eUnitObj);
            //fUnit.highlight.SetActive(false);
            turn = !turn;
            if(!turn)
                EnemyAI();
        }

    }

    public void AttackEnemy(GameObject UnitOn)
    {
        eUnit = UnitOn.GetComponent<EnemyAI>(); //e
        fUnit = selectedUnit.GetComponent<Unit>(); //f

        print(fUnit.name + " is attacking " + eUnit.name);
        
        UnitFight();
    }

    public bool RPS(string first, string second)
    {
        bool win = false;

        switch (first)
        {
            case "rock":
            switch (second)
            {
                case "paper":
                win = false;
                break;

                case "scissors":
                win = true;
                break;

                case "decoy":
                DecoyAlive = false;
                win = true;
                break;

            }
            break;

            case "paper":
            switch (second)
            {
                case "rock":
                win = true;
                break;

                case "scissors":
                win = false;
                break;

                case "decoy":
                DecoyAlive = false;
                win = true;
                break;
            }
            break;

            case "scissors":
            switch (second)
            {
                case "rock":
                win = false;
                break;

                case "paper":
                win = true;
                break;

                case "decoy":
                DecoyAlive = false;
                win = true;
                break;
            }
            break;
        }
        return win;
    }

/*(UI related)*/
    public void pickRock()
    {
        frameRPS.Match();
        windowRPS.SetActive(false);
        //movedFromUnit.GetComponent<Unit>().isOpen = true;
        fUnit.ChangeType("rock");
        EnemyAIPickRandom();
        UnitFight();
    }

    public void pickPaper()
    {
        frameRPS.Match();
        windowRPS.SetActive(false);
        fUnit.ChangeType("paper");
        EnemyAIPickRandom();
        UnitFight();
    }

    public void pickScissors()
    {
        frameRPS.Match();
        windowRPS.SetActive(false);
        fUnit.ChangeType("scissors");
        EnemyAIPickRandom();
        UnitFight();
    }

    public void setDoneInactive()
    {
            buttonDone.GetComponent<Button>().interactable = false;
            buttonDone.GetComponent<Image>().sprite = buttonDoneInactive;
    }

    public void setDoneActive()
    {
        buttonDone.SetActive(true);
        buttonDone.GetComponent<Image>().sprite = buttonDoneActive;
        buttonDone.GetComponent<Button>().interactable = true;
    }

/* EnemyAI */
    public void EnemyAI()
    {
        timer.ResetTimer();
        //moving down effectivness
        int effectiveness = height;
        GameObject movingUnit = null;
        if(enemyUnits.Count == 0)
            return;
        
        for (int i = enemyUnits.Count - 1; i >= 0; i --)
        {
            if(SideCheck(enemyUnits[i]))
                return;

            int pY = (int)enemyUnits[i].transform.position.y;
            for(int y = pY; y >= 0; y--)
            {
                if(map[(int)enemyUnits[i].transform.position.x, y] == "enemyUnit")
                    y = 0;
                if(map[(int)enemyUnits[i].transform.position.x, y] == "myUnit" || y <= 1)
                {
                    if((height - (height - pY + 1) + y) < effectiveness)
                    {
                        effectiveness = (height - (height - pY + 1) + y);
                        movingUnit = enemyUnits[i];
                    }
                }
            }
        }

        if(turn == false)
        {
            if(movingUnit.transform.position.y <= 1)
                LookForAFlag(movingUnit);
            else
            {
                if(map[(int)movingUnit.transform.position.x, (int)movingUnit.transform.position.y - 1] == "empty")
                    StartCoroutine(EnemyStep(0, -1, movingUnit));
                else 
                {
                    if((int)movingUnit.transform.position.x + 1 < width)
                        if(map[(int)movingUnit.transform.position.x + 1, (int)movingUnit.transform.position.y] == "empty")
                        {
                            StartCoroutine(EnemyStep(+1, 0, movingUnit));
                            return;
                        }

                    if((int)movingUnit.transform.position.x - 1 >= 0)
                        if(map[(int)movingUnit.transform.position.x - 1 , (int)movingUnit.transform.position.y] == "empty")
                        {
                            StartCoroutine(EnemyStep(-1, 0, movingUnit));
                            return;
                        }
                            
                    if((int)movingUnit.transform.position.y + 1 < height)
                        if(map[(int)movingUnit.transform.position.x, (int)movingUnit.transform.position.y + 1] == "empty")
                        {
                            StartCoroutine(EnemyStep(0, 1, movingUnit));
                            return;
                        }
                }
            }
            print (movingUnit.name + " is moving");
        }

    //h - (height - posY + 1 + tarY) - distance formula
    }

    public bool SideCheck(GameObject movingUnit)
    {
        int posX =(int)movingUnit.transform.position.x;
        int posY =(int)movingUnit.transform.position.y;

        eUnit = movingUnit.GetComponent<EnemyAI>();

        if(posY + 1 < height)
        {
            if(CheckSide(posX, posY + 1, movingUnit))
                return true;
        }
        if(posX + 1 < width)
        {
            if (CheckSide(posX + 1, posY, movingUnit))
                return true;
        }
        if(posY - 1 >= 0)
        {
            if (CheckSide(posX, posY - 1, movingUnit))
                return true;
        }
        if(posX - 1 >= 0)
        {
            if (CheckSide(posX - 1, posY, movingUnit))
                return true;
        }
        return false;
    }

    public bool CheckSide(int pX, int pY, GameObject movingUnit)
    {
        if("myUnit" == $"{map[pX, pY]}")
        {
            if(GetUnitAtPosition(pX, pY).isOpen)
            {
                //win RPS prediction
                if(RPS(movingUnit.GetComponent<EnemyAI>().type, GetUnitAtPosition(pX, pY).type))
                {
                    fUnit = GetTileAtPosition(new Vector2(pX, pY)).unitLinked.GetComponent<Unit>(); //f
                    eUnit = movingUnit.GetComponent<EnemyAI>(); //e
                    UnitFight();
                    return true;
                } 
                else if(!RPS(movingUnit.GetComponent<EnemyAI>().type, GetUnitAtPosition(pX, pY).type))
                {
                    int posX =(int)movingUnit.transform.position.x;
                    int posY =(int)movingUnit.transform.position.y;
                    if(posY > 1)
                    {
                        return false;
                    }
                        else if (posY <= 1)
                    {
                        fUnit = GetTileAtPosition(new Vector2(pX, pY)).unitLinked.GetComponent<Unit>(); //f
                        eUnit = movingUnit.GetComponent<EnemyAI>(); //e
                        UnitFight();
                        return true;
                    }
                    return true;
                }
            }
            //Unknown prediction
            else if(!GetUnitAtPosition(pX, pY).isOpen)
            {
                fUnit = GetTileAtPosition(new Vector2(pX, pY)).unitLinked.GetComponent<Unit>(); //f
                eUnit = movingUnit.GetComponent<EnemyAI>(); //e
                print (eUnit.name + " is fighting");
                UnitFight();
                return true;
            }
        }
        return false;
    }

    public void LookForAFlag(GameObject movingUnit)
    {
        GameObject Flag = GameObject.Find("FlagUnit");
        GameObject Decoy = GameObject.Find("DecoyUnit");
        GameObject Target;

        if(Random.Range(0, 5) < 3 && movingUnit.transform.position.y == 1)
        {
            StartCoroutine(EnemyStep(0, -1, movingUnit));
            return;
        }

        if(DecoyAlive)
            Target = Decoy;
        else
            Target = Flag;
        

        int posX =(int)movingUnit.transform.position.x;
        int posY =(int)movingUnit.transform.position.y;



        if(movingUnit.transform.position.x < Target.transform.position.x)
        {
            StartCoroutine(EnemyStep(+1, 0, movingUnit));
        }
        else if (movingUnit.transform.position.x > Target.transform.position.x)
        {
            StartCoroutine(EnemyStep(-1, 0, movingUnit));
        }
    }

    IEnumerator EnemyStep(int x, int y, GameObject movingUnit)
    {
        GetTileAtPosition(new Vector2((int)movingUnit.transform.position.x, (int)movingUnit.transform.position.y)).unitLinked = null;
        map[(int)movingUnit.transform.position.x, (int)movingUnit.transform.position.y] = "empty";
        LeanTween.move(movingUnit, new Vector2(movingUnit.transform.position.x + x, movingUnit.transform.position.y + y), 0.4f).setEaseInOutQuint();
        yield return new WaitForSeconds(0.4f);
        GetTileAtPosition(new Vector2((int)movingUnit.transform.position.x, (int)movingUnit.transform.position.y)).unitLinked = movingUnit;
        map[(int)movingUnit.transform.position.x, (int)movingUnit.transform.position.y] = "enemyUnit";
        movingUnit.transform.position = new Vector2(movingUnit.transform.position.x, movingUnit.transform.position.y);
        turn = true;
    }

    public void EnemyAIPickRandom()
    {
        int RPS = Random.Range(0, 3);
        //int RPS = 1;
        switch (RPS)
        {
            case 0:
            eUnit.ChangeType("rock");
            break;

            case 1:
            eUnit.ChangeType("paper");
            break;

            case 2:
            eUnit.ChangeType("scissors");
            break;
        }
        print("Enemy Unit is gonna be " + eUnit.type);
    }

}
