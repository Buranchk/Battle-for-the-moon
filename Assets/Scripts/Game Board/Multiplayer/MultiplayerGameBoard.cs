using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerGameBoard : MonoBehaviour
{
 //settings
    public int width, height;
    [SerializeField] private MultiplayerTile tilePrefab;
    [SerializeField] private Camera cam;
    [SerializeField] private MultiplayerUnit spaceMan;
    [SerializeField] private MultiplayerUnit XP;
    [SerializeField] private MultiplayerUnit gold;
    [SerializeField] private MultiplayerUnit ruby;


    [SerializeField] private MultiplayerEUnit spaceManE;
    [SerializeField] private MultiplayerEUnit XPE;
    [SerializeField] private MultiplayerEUnit goldE;
    [SerializeField] private MultiplayerEUnit rubyE;


//In game Data process
    private MultiplayerUnit gameVarUnit;
    public GameObject selectedUnit;
    public MultiplayerEUnit eUnit;
    public MultiplayerUnit fUnit;
    private string [ , ] map;
    public bool turn = false;
    public bool suggestSystem = false;
    public int gameStage = 0;
    private Dictionary<Vector2, MultiplayerTile> tiles;
    private GameObject lastSelectedUnit;
    public List<GameObject> units = new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();
    public bool gameWin = true;

//UI & Visuals
    public RPSMatch frameRPS;
    [SerializeField] private GameObject explosion;
    [SerializeField] private ParticleSystem winParticles;
    [SerializeField] private ParticleSystem oopsParticles;
    public GameObject windowRPS;
    [SerializeField] private GameObject buttonShuffle;
    [SerializeField] private GameObject arrowShuffle;
    [SerializeField] private GameObject buttonDone;
    [SerializeField] private GameObject decoyText;
    [SerializeField] private GameObject flagText;
    [SerializeField] private GameObject reshuffleText;
    [SerializeField] private GameObject startGameText;
    [SerializeField] private GameObject gradient;

    [SerializeField] private GameObject soundIcon;
    [SerializeField] private GameObject noSoundIcon;

    [SerializeField] private GameObject UnitDecoy;
    [SerializeField] private GameObject UnitFlag;

    [SerializeField] private Sprite buttonDoneActive;
    [SerializeField] private Sprite buttonDoneInactive;
    private GameObject PermFlag = null;
    private GameObject PermDecoy = null;

//Utility Items
    private DataManager DataMan;
    public Tweens tweens;
    private PhotonView photonView;
    public Timer timer;


    void Start()
    {
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();
        photonView = GetComponent<PhotonView>();
        SoundSwitch();
        SoundSwitch();


        GenerateGrid();
        gradient.SetActive(true);
        StartCoroutine(SpawnUnits());
    }

    private void Update()
    {
        //Should use different shit here, maybe... someday.....
        //Deselect Unit
        if (Input.GetMouseButtonDown(0))
            if(selectedUnit != null)
                if(!selectedUnit.GetComponent<MultiplayerUnit>().isOverTheUnit)
                {
                    DeselectUnit();
                }
    }

/* Start */
    void GenerateGrid() 
    {
        tiles = new Dictionary<Vector2, MultiplayerTile>();
        
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

    IEnumerator SpawnUnits()
    {
        
        SelectUnitSkin();
        map = new string [width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                map[x, y] = "empty"; 

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < 2; y++) {
                var spawnedUnit = Instantiate(fUnit, new Vector2(x, y), Quaternion.identity, transform);

                spawnedUnit.name = $"Unit {x} {y}";

                spawnedUnit.Init();

                units.Add(GetTileAtPosition(new Vector2(x, y)).unitLinked = GameObject.Find($"Unit {x} {y}"));

                map[x, y] = "myUnit";
                
                yield return new WaitForSeconds(0.05f);
            }
        }
        NewStage();
    }

    public void SendSpawnedUnits()
    {
        string[] matrix = new string[width * 2];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < 2; y++) {
                matrix[x * 2 + y] = GetTileAtPosition(new Vector2(x,y)).unitLinked.GetComponent<MultiplayerUnit>().type;
  
            }
        }
        photonView.RPC("EnemySpawn", RpcTarget.Others, GameObject.Find("Data Manager").GetComponent<DataManager>().GetSelectedSkin(), matrix);
        print("sent RPC prekols!!!!!!");
    }

    private void SelectUnitSkin()
    {
        int selectedskin = GameObject.Find("Data Manager").GetComponent<DataManager>().GetSelectedSkin();
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
        AudioManager.Instance.FlagDecoyAppereance();
        if(gameStage == 1)
        {
            PermFlag = Unit;
            Vector2 flagPos = Unit.transform.position;
            GameObject.Find("Flag").transform.position = flagPos;
            SelectUnselectUnit(Unit.GetComponent<MultiplayerUnit>(), true);
            setDoneActive();
        } 
        
        else if (gameStage == 2)
        {
            if(Unit.transform.position != PermFlag.transform.position)
            {
                PermDecoy = Unit;
                Vector2 decayPos = Unit.transform.position;
                GameObject.Find("Decoy").transform.position = decayPos;
                SelectUnselectUnit(Unit.GetComponent<MultiplayerUnit>(), false);
                setDoneActive();
            }
        }
    }

    private void SelectUnselectUnit(MultiplayerUnit unit, bool isFlag)
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

    public static string[,] RotateMatrix180(string[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        string[,] rotatedMatrix = new string[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                rotatedMatrix[i, j] = matrix[rows - i - 1, cols - j - 1];
            }
        }
        return rotatedMatrix;
    }


    public string[,] ProcessArray(string[] flatArray, int rows, int cols)
    {
        string[,] receivedArray = new string[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                receivedArray[i, j] = flatArray[i * cols + j];
            }
        }
        return receivedArray;
    }

    [PunRPC]
    public void EnemySpawn(int skin, string[] matrix)
    {
        print("recive RPC prekols!!!!!!");
        switch (skin)
        {
            case 0:
            eUnit = spaceManE;
            break;

            case 1:
            eUnit = XPE;
            break;

            case 2:
            eUnit = goldE;
            break;

            case 3:
            eUnit = rubyE;
            break;
        }

        string[,] positionsArr = new string[width, 2];

        positionsArr = RotateMatrix180(ProcessArray(matrix, width, 2));


        for (int x = 0; x < width; x++) {
            for (int y = height - 2; y < height; y++) {

                var spawnedUnit = Instantiate(eUnit, new Vector2(x, y), Quaternion.identity, transform);

                spawnedUnit.name = $"Enemy {x} {y}";

                spawnedUnit.Init();

                GameObject newEnemy = GameObject.Find($"Enemy {x} {y}");

                enemyUnits.Add(newEnemy);

                GetTileAtPosition(new Vector2(x, y)).unitLinked = newEnemy;

                MultiplayerEUnit enMult = newEnemy.GetComponent<MultiplayerEUnit>();

                enMult.SpawnAnimation();

                enMult.ChangeType(positionsArr[x, y - (height - 2)]);

                map[x, y] = "enemyUnit";

            }
        }

    }

/* UI related functions */
    public void SoundSwitch()
    {
        if(DataMan.GetSound())
        {
            noSoundIcon.transform.localScale = noSoundIcon.transform.localScale * 1.3f;
            DataMan.SetSound(false);

            soundIcon.SetActive(false);
            noSoundIcon.SetActive(true);

            LeanTween.scale(noSoundIcon,new Vector3(0.85f, 0.85f, 0.85f), 0.1f).setEaseOutCirc();
        } 
        else if(!(DataMan.GetSound()))
        {
            
            soundIcon.transform.localScale = noSoundIcon.transform.localScale * 1.3f;
            DataMan.SetSound(true);

            soundIcon.SetActive(true);
            noSoundIcon.SetActive(false);

            LeanTween.scale(soundIcon, new Vector3(0.85f, 0.85f, 0.85f), 0.1f).setEaseOutCirc();
        }
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
                    units[x].GetComponent<MultiplayerUnit>().ChangeType("rock");
                if (i == 1)
                    units[x].GetComponent<MultiplayerUnit>().ChangeType("paper");
                if (i == 2)
                    units[x].GetComponent<MultiplayerUnit>().ChangeType("scissors");
            }
        }
    }

    private void UnitOutline()
    {
        foreach (GameObject obj in units)
        {
            obj.GetComponent<MultiplayerUnit>().ChangeType("outline");
        }
        
    }

    void Shuffle(List<GameObject> a)
    {
        AudioManager.Instance.ShuffleUnit();
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
        AudioMusic.Instance.AmbientMusic();
    }

    IEnumerator StartGameFX()
    {
        LeanTween.alpha(gradient, 0f, 0.4f);

        startGameText.SetActive(true);
        LeanTween.scale(startGameText, new Vector3(70f, 70f, 70f), 0.3f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.alpha(startGameText, 1.0f, 0.3f);

        for(int i = 0; i < width; i++)
        {
            StartCoroutine(TileRawAppearence(i));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.4f);

        LeanTween.scale(startGameText, new Vector3(30.3f, 0.3f, 30.3f), 0.2f);
        LeanTween.alpha(startGameText, 0f, 0.2f);

        yield return new WaitForSeconds(0.2f);
        startGameText.SetActive(false);
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
        MultiplayerTile tile;
        tile = GetTileAtPosition(tilePos);
        tile.MakeTileVisible();
        yield return new WaitForSeconds(0.05f);
    }


/* Usage functions */
    public MultiplayerTile GetTileAtPosition(Vector2 pos) 
    {
        if (tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public MultiplayerUnit GetUnitAtPosition(int x, int y)
    {
        return GetTileAtPosition(new Vector2(x, y)).unitLinked.GetComponent<MultiplayerUnit>();
    }
    
    public MultiplayerEUnit GetEnemyAtPosition(int x, int y)
    {
        return GetTileAtPosition(new Vector2(x, y)).unitLinked.GetComponent<MultiplayerEUnit>();
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
            tweens.AppearScale(arrowShuffle);
            
            buttonShuffle.SetActive(true);
            UnitRandomize();
            setDoneActive();
            ApplyUnitSelection("Decoy");
            timer.ResetTimer15();
            break;

            case 4:
            SendSpawnedUnits();
            
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
        AudioManager.Instance.SelectionSoundFX();

        newSelectedUnit.GetComponent<MultiplayerUnit>().PlayOneShotAnimation("jump", 0.6f);
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
            selectedUnit.GetComponent<MultiplayerUnit>().setAnimation("animation");
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
            else if (map[x, y] == "enemyUnit")
            {
                GetEnemyAtPosition(x, y).movedOn = isActiveSelection;
                GetEnemyAtPosition(x, y).highlightFX(isActiveSelection);
            }
        }
    }

    public void TileStep(int x, int y, int xe, int ye)
    {
        StartCoroutine(UnitStep(x, y, xe, ye));
        photonView.RPC("SendDataStep", RpcTarget.Others, x,y,xe,ye);
        x = width - x - 1;
        y = height - y - 1;
        xe = width - xe - 1;
        ye = height - ye - 1;
        print("Data modify Host send = " + x + " " + y + " " + xe + " " + ye);

    }

    [PunRPC]
    public void SendDataStep (int x, int y, int xe, int ye)
    {
        x = width - x - 1;
        y = height - y - 1;
        xe = width - xe - 1;
        ye = height - ye - 1;

        print("Data modify send = " + x + " " + y + " " + xe + " " + ye);

        UnitEStep(x,y,xe,ye);
    }

    IEnumerator UnitEStep(int x, int y, int xe, int ye)
    {
        //Make PUNCall of UnitStepEnemy

        AudioManager.Instance.AirWhistleSoundFX();
        turn = !turn;
        MultiplayerEUnit unitScript = GetUnitObjectAt(xe, ye).GetComponent<MultiplayerEUnit>();
        unitScript.TrailSwitch(true);
        //Unit link to a new Tile
        GetTileAtPosition(new Vector2 (x, y)).GetComponent<MultiplayerTile>().unitLinked = unitScript.gameObject;

        //move Unit
        LeanTween.move(GetUnitObjectAt(xe, ye), new Vector2(x, y), 0.4f).setEaseInOutQuint();
        yield return new WaitForSeconds(0.4f);

        //delete old Unit link
        GetTileAtPosition(new Vector2 (xe, ye)).GetComponent<MultiplayerTile>().unitLinked = null;

        //Map Update
        map[x, y] = (string)map[xe, ye];
        map[xe, ye] = "empty";

        print(GetTileAtPosition(new Vector2 (x, y)).GetComponent<MultiplayerTile>().unitLinked.name + " makes a step");

        //switch the turn
        yield return new WaitForSeconds(0.15f);
        unitScript.TrailSwitch(false);
        EnemyTurn();
    }


    IEnumerator UnitStep(int x, int y, int xe, int ye)
    {
        //Make PUNCall of UnitStepEnemy

        AudioManager.Instance.AirWhistleSoundFX();
        turn = !turn;
        MultiplayerUnit unitScript = GetUnitObjectAt(xe, ye).GetComponent<MultiplayerUnit>();
        unitScript.TrailSwitch(true);
        //Unit link to a new Tile
        GetTileAtPosition(new Vector2 (x, y)).GetComponent<MultiplayerTile>().unitLinked = selectedUnit;

        //Selected Unit = null, SuggestMoves(false)
        DeselectUnit();

        //move Unit
        LeanTween.move(GetUnitObjectAt(xe, ye), new Vector2(x, y), 0.4f).setEaseInOutQuint();
        yield return new WaitForSeconds(0.4f);

        //delete old Unit link
        GetTileAtPosition(new Vector2 (xe, ye)).GetComponent<MultiplayerTile>().unitLinked = null;

        //Map Update
        map[x, y] = (string)map[xe, ye];
        map[xe, ye] = "empty";

        print(GetTileAtPosition(new Vector2 (x, y)).GetComponent<MultiplayerTile>().unitLinked.name + " makes a step");

        //switch the turn
        yield return new WaitForSeconds(0.15f);
        unitScript.TrailSwitch(false);
        EnemyTurn();
    }


/* Fight functions */
    public void DestroyUnit(GameObject Unit)
    {
        if(Unit != null)
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
    }

    public void UnitFight()
    {
        AudioManager.Instance.UnitFight();
        print("Units duel now");
        GameObject fUnitObj = GameObject.Find(fUnit.name); //f
        GameObject eUnitObj = GameObject.Find(eUnit.name); //e

        print("Enemy was - " + eUnit.type + "    Friend was - " + fUnit.type);

        if(eUnit.type == "flag")
        {
            print("Enemy Flag is fucked");
            DestroyUnit(eUnitObj);
            gameWin = true;
            NewStage();
        }

        if(fUnit.type == "flag")
        {
            print("Friendly Flag is fucked");
            DestroyUnit(fUnitObj);
            gameWin = false;
            NewStage();
        }

        if(eUnit.type == "decoy" || fUnit.type == "decoy")
        {
            DestroyUnit(fUnitObj);
            DestroyUnit(eUnitObj);
            turn = !turn;
            if(!turn)
                EnemyTurn();
        }

        if (eUnit.type == fUnit.type)
        {
            AudioManager.Instance.UnitMatch();
            windowRPS.SetActive(true);
            frameRPS.Appear();
        }

        if(RPS(eUnit.type, fUnit.type) && eUnit.type != fUnit.type) //e
        {
            AudioManager.Instance.UnitDeath();
            frameRPS.RegularRPS();
            eUnit.isOpen = true;
            eUnit.ChangeType(eUnit.type);
            eUnit.movedOn = false;
            if(turn){
                turn = !turn;
            }
            else
            StartCoroutine(FightAnimation(eUnitObj, fUnitObj, eUnit.gameObject.transform.position.x, eUnit.gameObject.transform.position.y, fUnit.gameObject.transform.position.x, fUnit.gameObject.transform.position.y, false));
        }
        else if(!RPS(eUnit.type, fUnit.type) && eUnit.type != fUnit.type) //f
        {
            AudioManager.Instance.UnitDeath();
            frameRPS.RegularRPS();
            fUnit.isOpen = true;
            fUnit.ChangeType(fUnit.type);
            fUnit.movedOn = false;
            if(turn){
                turn = !turn;
            } 
            else
            StartCoroutine(FightAnimation(fUnitObj, eUnitObj, eUnit.gameObject.transform.position.x, eUnit.gameObject.transform.position.y, fUnit.gameObject.transform.position.x, fUnit.gameObject.transform.position.y, true));
        }

    }

    public void AttackEnemy(GameObject UnitOn)
    {
        eUnit = UnitOn.GetComponent<MultiplayerEUnit>(); //e
        fUnit = selectedUnit.GetComponent<MultiplayerUnit>(); //f

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
                win = true;
                break;
            }
            break;
        }
        return win;
    }

    IEnumerator FightAnimation(GameObject unit1, GameObject unit2, float pos1X, float pos1Y, float pos2X, float pos2Y, bool win)
    {

        Vector2 place = new Vector2(0, 15f);
        Vector2 fightPlace = new Vector2(((pos1X + pos2X)/2), ((pos1Y + pos2Y)/2));

        if(turn)
            tweens.UnitsMeet(fUnit.gameObject, eUnit.gameObject, win);
        else
            tweens.UnitsMeet(eUnit.gameObject, fUnit.gameObject, !win);

        print("Code is past Animation sector");

        if(win)
        {
            winParticles.gameObject.transform.position = fightPlace;
            winParticles.Play();
        }
        else
        {
            oopsParticles.gameObject.transform.position = fightPlace;
            oopsParticles.Play();
        }

            
        print("BeforeTimePause");
        yield return new WaitForSeconds(0.6f);
        print("AfterTimePause");
        DeselectUnit();
        DestroyUnit(unit2);
        
        yield return new WaitForSeconds(1f);
        oopsParticles.gameObject.transform.position = place;
        winParticles.gameObject.transform.position = place;
    }


/*(UI related)*/
    public void pickRock()
    {
        frameRPS.Match();
        windowRPS.SetActive(false);
        fUnit.ChangeType("rock");
        EnemyPickTurn();
    }

    public void pickPaper()
    {
        frameRPS.Match();
        windowRPS.SetActive(false);
        fUnit.ChangeType("paper");
        EnemyPickTurn();
    }

    public void pickScissors()
    {
        frameRPS.Match();
        windowRPS.SetActive(false);
        fUnit.ChangeType("scissors");
        EnemyPickTurn();
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


/* EnemyPlayer */
    public void EnemyPickTurn()
    {
        // Request 
        photonView.RPC("SendData", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    public void SendData()
    {
        UnitFight();
    }

    public void EnemyTurn()
    {
        // Get the list of players in the room
        Player[] players = PhotonNetwork.PlayerList;

        if (players.Length > 1)
       { 
            Debug.Log("Ownership was: "+ photonView.Owner);
            // Find the index of the current owner in the player list
            int currentOwnerIndex = System.Array.IndexOf(players, photonView.Owner);
            Debug.Log ("Current owner index " +currentOwnerIndex);
            // Calculate the index of the next player in a circular manner
            int nextOwnerIndex = (currentOwnerIndex + 1) % players.Length;
            Debug.Log ("Next owner index "+nextOwnerIndex);

            // Transfer ownership to the next player
            photonView.TransferOwnership(players[nextOwnerIndex]);
            Debug.Log("Ownership was transfered to: "+ photonView.Owner);
       }
    }

    public bool TurnCheck() 
    {
        return (photonView.IsMine);
    }

}
