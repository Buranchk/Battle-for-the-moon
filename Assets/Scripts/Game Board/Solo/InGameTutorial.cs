using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTutorial : MonoBehaviour
{
    public GameObject TutorialObject;

    public GameObject NextButton;

    public GameObject TutorialState0;
    public GameObject TutorialState1;
    public GameObject TutorialState2;
    public GameObject TutorialState3;
    public GameObject TutorialState4;
    public GameObject TutorialState5;
    public GameObject TutorialState6;
    public GameObject TutorialState7;
    public GameObject TutorialState81;
    public GameObject TutorialState82;
    public GameObject TutorialState9;
    public GameObject TutorialState10;

    public int tutorialState = 0;

    private bool flag = true;
    private bool decoy = true;

    public void StartTutorial()
    {
        TutorialObject.SetActive(true);
        NextStage();
    }

    public void NextStage()
    {
        switch(tutorialState)
        {
        case 0:
            NextButton.SetActive(true);
            TutorialState0.SetActive(true);
            tutorialState++;
        return;

        case 1:
            TutorialState0.SetActive(false);
            TutorialState1.SetActive(true);
            tutorialState++;
        return;

        case 2:
            NextButton.SetActive(false);
            //fingerPlay
            GameObject.Find("Game Board").GetComponent<GameBoard>().StartFingerPointing();
            GameObject.Find("Game Board").GetComponent<GameBoard>().SetTutorialState(false);
            
            TutorialState1.SetActive(false);
            TutorialState2.SetActive(true);
            tutorialState++;
        return;

        case 3:
            TutorialState2.SetActive(false);
            TutorialState3.SetActive(true);
            tutorialState++;
        return;

        case 4:
            TutorialState3.SetActive(false);
            TutorialState4.SetActive(true);
            tutorialState++;
        return;

        case 5:
            TutorialState4.SetActive(false);
            TutorialState5.SetActive(true);
            tutorialState++;
        return;

        case 6:
            TutorialState5.SetActive(false);
            TutorialState6.SetActive(true);
            tutorialState++;
        return;

        case 7:
            TutorialState6.SetActive(false);
            TutorialState7.SetActive(true);
            tutorialState++;
        return;

        case 8:
            TutorialState7.SetActive(false);
            SkipTutorial();
        return;

        }
    }

    public void NextStageFlag()
    {
        if(flag)
        {
            NextStage();
            flag = false;
        }
    }

    public void NextStageDecoy()
    {
        if(decoy)
        {
            NextStage();
            decoy = false;
        }
    }

    public void SkipTutorial()
    {

        NextButton.SetActive(false);
        TutorialState0.SetActive(false);
        TutorialState1.SetActive(false);
        TutorialState2.SetActive(false);
        TutorialState3.SetActive(false);
        TutorialState4.SetActive(false);
        TutorialState5.SetActive(false);
        TutorialState6.SetActive(false);
        TutorialState7.SetActive(false);
        TutorialObject.SetActive(false);

        tutorialState = 0;
    }

}
