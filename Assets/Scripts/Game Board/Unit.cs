using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Unit : MonoBehaviour
{
    //settings
    [SerializeField] private GameObject tile;
    private GameBoard Board;

    //sprites
    private string rock = "SC_stone";
    private string paper = "SC_paper";
    private string scissors = "SC_scissors";
    private string rockOpen = "SO_stone";
    private string paperOpen = "SO_paper";
    private string scissorsOpen = "SO_scissors";
    private string flag = "SM_FLAG";
    private string decoy = "SM_FAKE";
    private string nothing = "MARINE";

    
    //permanent statement
    public bool playerUnit = true;

    //condition
    private int tweenId;
    public bool isOpen = false;
    public string type = "Nothing";
    public bool isOverTheUnit = false;
    public bool movedOn;

    //spine stuff
    SkeletonAnimation skeletonAnimation;
    Spine.AnimationState animationState;
    Spine.Skeleton skeleton;


    //Unit initiation
    public void Init()
    {
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        movedOn = false;
        //highlight.SetActive(false);
        gameObject.LeanScale(new Vector3(0.4f, 0.4f, 0.4f), 0.4f).setEaseOutCirc();
    }

   void Awake () {
      skeletonAnimation = GetComponent<SkeletonAnimation>();
      //skeletonGraphic = GetComponent<SkeletonGraphic>();
      skeleton = skeletonAnimation.Skeleton;

      animationState = skeletonAnimation.AnimationState;
   }

    public void ChangeType(string newType)
    {
        type = newType;
        
        switch (newType)
        {
            case "Nothing":
                skeletonAnimation.Skeleton.SetSkin(nothing);
                skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            break;

            case "rock":
                if(isOpen)
                {
                    skeletonAnimation.Skeleton.SetSkin(rockOpen);
                    skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                } else
                {
                    skeletonAnimation.Skeleton.SetSkin(rock);
                    skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                }
            break;

            case "paper":
                if(isOpen)
                {
                    skeletonAnimation.Skeleton.SetSkin(paperOpen);
                    skeletonAnimation.Skeleton.SetSlotsToSetupPose();                    
                } else
                {
                    skeletonAnimation.Skeleton.SetSkin(paper);
                    skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                }

            break;

            case "scissors":
                if(isOpen)
                {
                    skeletonAnimation.Skeleton.SetSkin(scissorsOpen);
                    skeletonAnimation.Skeleton.SetSlotsToSetupPose();                    
                } else
                {
                    skeletonAnimation.Skeleton.SetSkin(scissors);
                    skeletonAnimation.Skeleton.SetSlotsToSetupPose();                    
                }
            break;

            case "flag":
                skeletonAnimation.Skeleton.SetSkin(flag);
                skeletonAnimation.Skeleton.SetSlotsToSetupPose();            
            break;

                case "decoy":
                skeletonAnimation.Skeleton.SetSkin(decoy);
                skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            break;
        }
    }

    //Unit selection
    void OnMouseOver()
    {
        if(GetGameBoard().windowRPS.activeInHierarchy == false)
        {
            isOverTheUnit = true;
            if (Input.GetMouseButtonDown(0) && gameObject.name != "DecoyUnit" && gameObject.name != "FlagUnit")
            {
                if (GetGameBoard().gameStage == 4 && GetGameBoard().turn)
                    GetGameBoard().SelectUnit(gameObject);
                else if (GetGameBoard().gameStage <= 2)
                {
                    GetGameBoard().SetFlagDecoy(gameObject);
                }
            }
        }
    }

    public void setAnimation(string animationInstance)
    {
        skeletonAnimation.AnimationName = animationInstance;
    }

    public void FlagDecoySelected(bool isFlag, bool selected)
    {
        if(selected)
        {
            if(isFlag)
            {
                skeletonAnimation.Skeleton.SetSkin(flag);
                skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            } 
            else if (!isFlag)
            {
                skeletonAnimation.Skeleton.SetSkin(decoy);
                skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            }


            // Get the SkeletonGraphic component
            SkeletonGraphic skeletonGraphic = GetComponent<SkeletonGraphic>();

            // Animate the alpha value in a loop
            tweenId = LeanTween.value(gameObject, 1f, 0f, 1f)
                .setEase(LeanTweenType.easeInOutSine)
                .setLoopType(LeanTweenType.pingPong)
                .setOnUpdate((float value) =>
                {
                    skeleton.A = value;
                })
                .id;

        } else if (!selected)
        {
            LeanTween.cancel(tweenId);
            skeleton.A = 1f;

            skeletonAnimation.Skeleton.SetSkin(nothing);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();    
        }
    }

    //Unit highlight exit
    void OnMouseExit()
    {
        isOverTheUnit = false;
    }

    GameBoard GetGameBoard()
    {
        if (Board == null)
            Board = GameObject.Find("Game Board").GetComponent<GameBoard>();
        return Board;
    }

}
