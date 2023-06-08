using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Timer : MonoBehaviour
{
    public float timerLength = 30.0f; // The length of the timer in seconds
    public TextMeshProUGUI timerText; // A reference to the TextMeshProUGUI element that will display the timer
    public GameObject AbortedGame;
    public Tweens tween;

    private float timeLeft; // The time remaining on the timer

    private bool timeState = true;

    private void Start()
    {
        timeLeft = timerLength;
    }

    public void ExitGame()
    {
        StartCoroutine(AbortGame());
    }

    IEnumerator AbortGame()
    {
        tween.AbortGame(AbortedGame);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Main Menu");
        AudioMusic.Instance.MainMenuMusic();
    }

    private void Update()
    {
        if(timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime; // Subtract the time since last frame from the time remaining on the timer

            // Update the TextMeshProUGUI element with the time remaining on the timer
            timerText.text = ":" + (Mathf.CeilToInt(timeLeft).ToString());


            if (timeLeft < 10.0f && timeState)
            {
                TimeIsLow();
                timeState = false;
            }

            // Check if the timer has run out
            if (timeLeft <= 0)
            {
                StartCoroutine(AbortGame());
            }
        }
    }

    private void TimeIsLow()
    {
        if (timeState)
        {
            timerText.color = Color.red;
            LeanTween.scale(timerText.gameObject, Vector3.one * 1.1f, 0.3f)
                .setEase(LeanTweenType.easeInOutQuad)
                .setLoopPingPong();
        }
    }

    public void ResetTimer()
    {
        timeState = true;
        timeLeft = 30;
        timerText.color = Color.white;
        timerText.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        LeanTween.cancel(timerText.gameObject, false);
    }

    public void ResetTimer15()
    {
        timeState = true;
        timeLeft = 15;
        timerText.color = Color.white;
        timerText.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        LeanTween.cancel(timerText.gameObject, false);
    }

}
