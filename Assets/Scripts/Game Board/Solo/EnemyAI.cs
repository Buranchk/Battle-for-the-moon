using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class EnemyAI : MonoBehaviour
{

    private string hidden = "Enemy_wait";
    private string outlined = "Enemy_wait_outline";
    private string rockOpen = "Enemy_O_stone";
    private string paperOpen = "Enemy_O_paper";
    private string scissorsOpen = "Enemy_O_scissors";
    private string rockOpen_outline = "Enemy_O_stone_outline";
    private string paperOpen_outline = "Enemy_O_paper_outline";
    private string scissorsOpen_outline = "Enemy_O_scissors_outline";

    //permanent statement
    public bool playerUnit = false;
    private GameBoard board; 

    //condition
    public bool isOpen = false;
    public string type = "hidden";
    public bool isOverTheUnit = false;
    public bool movedOn;

    //spine stuff
    SkeletonAnimation skeletonAnimation;
    Spine.AnimationState animationState;
    Spine.Skeleton skeleton;

    //attached objects
    public TrailRenderer trail;


   void Awake () {
      skeletonAnimation = GetComponent<SkeletonAnimation>();
      skeleton = skeletonAnimation.Skeleton;
      animationState = skeletonAnimation.AnimationState;
   }


    public void Init()
    {
        TrailSwitch(false);
        
    }

    void OnMouseOver()
    {
        if(GetGameBoard().windowRPS.activeInHierarchy == false)
        {
            isOverTheUnit = true;
            if (Input.GetMouseButtonDown(0))
            {
                if (movedOn && GetGameBoard().gameStage == 4)
                    GetGameBoard().AttackEnemy(gameObject);
            }
        }
    }

    public void ChangeType(string newType)
    {
        type = newType;
        if(isOpen)
        {
            switch (newType)
            {

                case "rock":
                    skeletonAnimation.Skeleton.SetSkin(rockOpen);
                    skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                    break;

                case "paper":
                    skeletonAnimation.Skeleton.SetSkin(paperOpen);
                    skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                    break;
                
                case "scissors":
                    skeletonAnimation.Skeleton.SetSkin(scissorsOpen);
                    skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                    break;
                
                default:
                    skeletonAnimation.Skeleton.SetSkin(hidden);
                    skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                    break;

            }
        }else if (!isOpen)
        {
            skeletonAnimation.Skeleton.SetSkin(hidden);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        }
    }

    //Unit highlight exit
    GameBoard GetGameBoard()
    {
        if(board == null)
            board = GameObject.Find("Game Board").GetComponent<GameBoard>();
        return board;
    }

    public void highlightFX(bool state)
    {
        if(state)
        {
            if(isOpen)
            {
                switch (type)
                {
                    case "rock":
                        skeletonAnimation.Skeleton.SetSkin(rockOpen_outline);
                        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                        break;

                    case "paper":
                        skeletonAnimation.Skeleton.SetSkin(paperOpen_outline);
                        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                        break;

                    case "scissors":
                        skeletonAnimation.Skeleton.SetSkin(scissorsOpen_outline);
                        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                        break;

                    default:
                        skeletonAnimation.Skeleton.SetSkin(hidden);
                        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                        break;
                }
            }
            else if (!isOpen)
            {
                skeletonAnimation.Skeleton.SetSkin(outlined);
                skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            }
            skeletonAnimation.AnimationState.SetAnimation(0, "aggressive", true);
        }
        else
        {
            ChangeType(type);
            skeletonAnimation.AnimationState.SetAnimation(0, "animation", true);
        }
    }

    public void setAlpha(float opacity)
    {

    }

    public void SpawnAnimation()
    {
        //setAlpha(0);
        transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        LeanTween.scale(gameObject, new Vector3(0.32f, 0.32f), 0.7f).setEaseOutElastic();

        //gameObject.LeanAlpha(1, 0.75f).setEaseOutBack();
        //gameObject.LeanScale(new Vector3(1f, 1f), 0.5f).setEaseInCubic();
        //gameObject.LeanScale(new Vector3(0.85f, 0.85f), 0.5f).setEaseOutBack();
    }

    public void TrailSwitch(bool state)
    {
        trail.enabled = state;
    }

}
