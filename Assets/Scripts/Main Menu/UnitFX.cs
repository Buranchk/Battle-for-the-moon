using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class UnitFX : MonoBehaviour
{
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

    

    //condition
    private int tweenId;
    public bool isOpen = false;
    public string type = "Empty";


    //spine stuff
    SkeletonAnimation skeletonAnimation;
    Spine.AnimationState animationState;
    Spine.Skeleton skeleton;



   void Awake () {
      skeletonAnimation = GetComponent<SkeletonAnimation>();
      //skeletonGraphic = GetComponent<SkeletonGraphic>();
      skeleton = skeletonAnimation.Skeleton;

      animationState = skeletonAnimation.AnimationState;
   }

    public void ChangeType(string newType, bool open)
    {
        type = newType;
        isOpen = open;
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
}