
using UnityEngine;
using Photon.Pun;

public class ButtonScript : MonoBehaviour
{
    private PhotonView photonView;
    
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
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
//        AudioManager.Instance.PlayButton();
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
    public void ExitGame()
    {
        if(PhotonNetwork.InRoom)
        {
            Debug.Log("Exit button clicked");
            GameObject.Find("Multiplayer Game Board").GetComponent<MultiplayerGameBoard>().CallOpponentLeft();
        }
        PhotonNetwork.LoadLevel("Main Menu");
        AudioMusic.Instance.MainMenuMusic();
    }

}
