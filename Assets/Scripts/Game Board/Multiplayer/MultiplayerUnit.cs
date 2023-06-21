using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class MultiplayerUnit : MonoBehaviour
{
    //settings
    [SerializeField] private GameObject tile;
    private MultiplayerGameBoard Board;

    //sprites
    private string rock = "Closed_Rock";
    private string paper = "Closed_Paper";
    private string scissors = "Closed_Scissors";
    private string rockOpen = "Open_Rock";
    private string paperOpen = "Open_Paper";
    private string scissorsOpen = "Open_Scissors";
    private string flag = "Flag";
    private string decoy = "Fake";
    private string empty = "Empty";
    private string outline = "Empty_Outline";

    
    //permanent statement
    public bool playerUnit = true;

    //condition
    private int tweenId;
    public bool isOpen = false;
    public string type = "Empty";
    public bool isOverTheUnit = false;
    public bool movedOn;

    //spine stuff
    SkeletonAnimation skeletonAnimation;
    Spine.AnimationState animationState;
    Spine.Skeleton skeleton;

    //attached objects
    public TrailRenderer trail;


    //Unit initiation
    public void Init()
    {
        TrailSwitch(false);
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
                skeletonAnimation.Skeleton.SetSkin(empty);
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

            case "outline":
                skeletonAnimation.Skeleton.SetSkin(outline);
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
                if (GetGameBoard().gameStage == 4 && GetGameBoard().TurnCheck() && playerUnit)
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

    public void PlayOneShotAnimation(string oneShotAnimationName, float mixDuration = 0.1f)
    {
        // Get the current track entry.
        var currentTrackEntry = animationState.GetCurrent(0);

        // Store the current animation name and track time.
        string previousAnimationName = currentTrackEntry.Animation.Name;
        float previousTrackTime = currentTrackEntry.TrackTime;

        // Play the one-shot animation.
        animationState.SetAnimation(0, oneShotAnimationName, false);

        // Create a new track entry with the previous animation and set it to loop.
        var returnToPreviousAnimation = animationState.AddAnimation(0, previousAnimationName, true, 0);
        returnToPreviousAnimation.MixDuration = mixDuration;

        // Set the track time of the new track entry to the stored time.
        returnToPreviousAnimation.TrackTime = previousTrackTime;
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

            skeletonAnimation.Skeleton.SetSkin(outline);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();    
        }
    }

    //Unit highlight exit
    void OnMouseExit()
    {
        isOverTheUnit = false;
    }

    MultiplayerGameBoard GetGameBoard()
    {
        if (Board == null)
            Board = GameObject.Find("Game Board").GetComponent<MultiplayerGameBoard>();
        return Board;
    }

    public void TrailSwitch(bool state)
    {
        trail.enabled = state;
    }

}
