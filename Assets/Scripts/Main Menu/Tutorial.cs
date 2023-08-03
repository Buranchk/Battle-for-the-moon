using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public int tutorialState = 0;


    public GameObject NextButton;

    public GameObject SkipTutorialButton;

    public GameObject TutorialObject;
    public GameObject TutorialOption0;
    public GameObject TutorialState1;
    public GameObject TutorialState2;
    public GameObject TutorialState3;
    public GameObject TutorialState4;
    public GameObject TutorialState5;
    public GameObject TutorialState6;
    public GameObject TutorialState7;

    public GameObject PlayOption;

    public Button StartSolo;

    public void StartTutorial()
    {
        //init
        TutorialObject.SetActive(true);
        NextStage();
    }

    public void NextStage()
    {
        switch(tutorialState)
        {
        case 0:
            TutorialOption0.SetActive(true);
            tutorialState++;
        return;

        case 1:
            NextButton.SetActive(true);
            SkipTutorialButton.SetActive(true);
            TutorialOption0.SetActive(false);
            TutorialState1.SetActive(true);
            tutorialState++;
        return;

        case 2:
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
            PlayOption.SetActive(true);
            //animatePlayMB
            tutorialState++;
        return;

        case 8:
            TutorialState7.SetActive(false);
            tutorialState = 0;
            PlayerPrefs.SetInt("TutorialStates", 10);
            print(PlayerPrefs.GetInt("TutorialStates"));
            StartSolo.onClick.Invoke();
        return;
        }
    }

    public void SkipTutorial()
    {

        NextButton.SetActive(false);
        TutorialOption0.SetActive(false);
        TutorialState1.SetActive(false);
        TutorialState2.SetActive(false);
        TutorialState3.SetActive(false);
        TutorialState4.SetActive(false);
        TutorialState5.SetActive(false);
        TutorialState6.SetActive(false);
        TutorialState7.SetActive(false);
        SkipTutorialButton.SetActive(false);
        TutorialObject.SetActive(false);

        tutorialState = 0;
    }
}
