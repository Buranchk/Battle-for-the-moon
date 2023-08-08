using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public int tutorialState = 0;


    public GameObject NextButton;

    public GameObject SkipTutorialButton;


    public GameObject Hole1;
    public GameObject Hole2;
    public GameObject Hole2_0;
    public GameObject Hole2_1;
    public GameObject Hole3;
    public GameObject Hole3_0;
    public GameObject Hole3_1;
    public GameObject Hole3_2;
    public GameObject Hole4;
    public GameObject Hole5;
    public GameObject Hole6;
    public GameObject Hole7;

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
            Hole1.SetActive(true);
            HoleApperance(Hole1);
            NextButton.SetActive(true);
            SkipTutorialButton.SetActive(true);
            TutorialOption0.SetActive(false);
            TutorialState1.SetActive(true);
            tutorialState++;
        return;

        case 2:
            Hole1.SetActive(false);
            Hole2.SetActive(true);
            HoleApperance(Hole2_0);
            HoleApperance(Hole2_1);
            TutorialState1.SetActive(false);
            TutorialState2.SetActive(true);
            tutorialState++;
        return;

        case 3:
            Hole2.SetActive(false);
            Hole3.SetActive(true);
            HoleApperance(Hole3_0);
            HoleApperance(Hole3_1);
            HoleApperance(Hole3_2);
            TutorialState2.SetActive(false);
            TutorialState3.SetActive(true);
            tutorialState++;
        return;

        case 4:
            Hole3.SetActive(false);
            Hole4.SetActive(true);
            HoleApperance(Hole4);
            TutorialState3.SetActive(false);
            TutorialState4.SetActive(true);
            tutorialState++;
        return;

        case 5:
            Hole4.SetActive(false);
            Hole5.SetActive(true);
            HoleApperance(Hole5);
            TutorialState4.SetActive(false);
            TutorialState5.SetActive(true);
            tutorialState++;
        return;

        case 6:
            Hole5.SetActive(false);
            Hole6.SetActive(true);
            HoleApperance(Hole6);
            TutorialState5.SetActive(false);
            TutorialState6.SetActive(true);
            tutorialState++;
        return;

        case 7:
            Hole6.SetActive(false);
            Hole7.SetActive(true);
            HoleApperance(Hole7);
            TutorialState6.SetActive(false);
            TutorialState7.SetActive(true);
            PlayOption.SetActive(true);
            //animatePlayMB
            tutorialState++;
        return;

        case 8:
            Hole7.SetActive(false);
            TutorialState7.SetActive(false);
            tutorialState = 0;
            PlayerPrefs.SetInt("TutorialStates", 10);
            print(PlayerPrefs.GetInt("TutorialStates"));
            StartSolo.onClick.Invoke();
        return;
        }
    }

    public void HoleApperance(GameObject hole)
    {
        hole.transform.localScale = Vector2.one * 0.5f;
        LeanTween.scale(hole, new Vector2(1f, 1f), 0.6f).setEaseOutBounce();
    }

    public void SkipTutorial()
    {
        Hole1.SetActive(false);
        Hole2.SetActive(false);
        Hole3.SetActive(false);
        Hole4.SetActive(false);
        Hole5.SetActive(false);
        Hole6.SetActive(false);
        Hole7.SetActive(false);

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
