
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void OnClicButton1()
    {
        AudioManager.Instance.OnClicButton1();
    }
    public void OnClicButton2()
    {
        AudioManager.Instance.OnClicButton2();
    }
    public void OnPlayButton()
    {
        AudioManager.Instance.PlayButton();
    }
    public void OnBuyButton()
    {
        AudioManager.Instance.BuyButton();
    }
    public void OnSelectCharacter()
    {
        AudioManager.Instance.SelectionSoundFX();
    }
    public void OnSwipeCharacter()
    {
        AudioManager.Instance.AirWhistleSoundFX();
    }
    public void OnMuteButton()
    {
        AudioManager.Instance.Mutebutton();
        AudioMusic.Instance.Mutebutton();
    }
}
