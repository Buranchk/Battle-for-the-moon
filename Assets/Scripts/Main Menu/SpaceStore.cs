using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpaceStore : MonoBehaviour
{
    public TextMeshProUGUI goldSpace;
    public TextMeshProUGUI rubySpace;


    public TextMeshProUGUI price1;
    public TextMeshProUGUI price2;
    public TextMeshProUGUI price3;
    public TextMeshProUGUI price4;
    public TextMeshProUGUI price5;

    public GameObject buttonLvlUP;
    public GameObject buttonGold;
    public GameObject buttonEnergy;
    public GameObject buttonNoAds;
    public GameObject buttonRuby;

    public int goldPriceForLvlUp;
    public int rubyPriceForGold = 10;
    public int rubyPriceForEnergy = 5;
    public float moneyPriceForNoAds = 1.99f;
    public float moneyPriceForRuby = 0.99f;
    private float updXP;
    public Slider XPSlider;

    public GameObject powerSpace;
    public GameObject EnergyBar1;
    public GameObject EnergyBar2;
    public GameObject EnergyBar3;
    public GameObject lvlSpace;

    public ParticleSystem particleCoins;
    public ParticleSystem particleRubins;
    
    private DataManager DataMan;
    private SceneLoader SceneLoad;

    void Start()
    {
        
        //moneyPriceForNoAds GET

        //moneyPriceForRuby
        SceneLoad = GameObject.Find("Scene loader").GetComponent<SceneLoader>();
        DataMan = GameObject.Find("Data Manager").GetComponent<DataManager>();

        UpdateStats();
        print("stats are updated");
    }


//Buttons
    public void GoldToXP()
    {
        if(DataMan.CheckResources("gold", goldPriceForLvlUp) && !(DataMan.GetLvl() == 10))
        {

            LeanTween.value(gameObject, UpdateGoldValue, DataMan.GetGold(), DataMan.GetGold() - goldPriceForLvlUp, 1.0f).setOnComplete(() =>
                {
                    UpdateStats();
                }
            );

            DataMan.TakeResources("gold", goldPriceForLvlUp);

            buttonLvlUP.GetComponent<Button>().interactable = false;

            LeanTween.scale(buttonLvlUP, Vector3.one / 2.2f, 0.1f).setLoopPingPong(1);
            buttonLvlUP.GetComponent<Image>().color = new Color(0, 1, 0, 1);
            LeanTween.scale(buttonLvlUP, Vector3.one / 2.2f, 0.1f).setLoopPingPong(1);

            LeanTween.value(gameObject, UpdatePrice1Value, goldPriceForLvlUp, 0.0f, 1.0f);

            LeanTween.color(buttonLvlUP.GetComponent<Image>().rectTransform, new Color(1f, 1f, 1f, 1f), 1f);
            
            NextLevel();
        }
        else if (DataMan.GetLvl() == 10)
        {
            buttonLvlUP.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            LeanTween.color(buttonLvlUP.GetComponent<Image>().rectTransform, new Color(1, 1, 1, 1), 0.3f);
            
            lvlSpace.GetComponent<TMPro.TextMeshProUGUI>().color = new Color (0.0f, 1.0f, 0.0f, 1.0f);
            lvlSpace.transform.localScale = new Vector3(3.2f, 3.2f, 3.2f);
            LeanTween.scale(lvlSpace, new Vector2(4.2f, 4.2f), 0.1f).setEaseOutBounce().setLoopPingPong(1);

        }
        else if (!DataMan.CheckResources("gold", goldPriceForLvlUp))
        {
            ButtonPress(buttonLvlUP, false);
        }
    }

    public void RubyToGold()
    {
        if(DataMan.CheckResources("ruby", rubyPriceForGold))
        {
            int initialGold = DataMan.GetGold();
            int endGold = initialGold + (rubyPriceForGold * 10);

            LeanTween.value(gameObject, UpdateGoldValue, initialGold, endGold, 1.5f).setOnComplete(() =>
                {
                    UpdateStats();
                }
            );

            DataMan.TakeResources("ruby", rubyPriceForGold);
            DataMan.GiveGold(rubyPriceForGold * 10);

            ButtonPress(buttonGold, true);
            particleCoins.Play();

            UpdateStats();
        }
        else
        {
            ButtonPress(buttonGold, false);
        }
    }

    public void RubyToEnergy()
    {
        if(DataMan.CheckResources("ruby", 5) && !DataMan.CheckResources("energy", 60))
        {
            DataMan.TakeResources("ruby", 5); 
            DataMan.FillEnergy();
            ButtonPress(buttonEnergy ,true);
            StartCoroutine(FillEnergy());
            print("stats are updated");
        }
        else if (DataMan.CheckResources("energy", 60))
        {
            buttonEnergy.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            LeanTween.color(buttonEnergy.GetComponent<Image>().rectTransform, new Color(1, 1, 1, 1), 0.3f);
            StartCoroutine(FillEnergy());
            print("ur energy is full");
        }
        else if (!DataMan.CheckResources("ruby", 5))
        {
            ButtonPress(buttonGold, false);
            print("not enough ruby");
        }
        ShowEnergy();

    }

    public void MonyNoAd()
    {
        //some function
    }

    public void MoneyToRuby()
    {
        if(true)
        {
            int initialRubin = DataMan.GetRuby();
            int endRubin = initialRubin + (rubyPriceForGold * 10);

            LeanTween.value(gameObject, UpdateRubinValue, initialRubin, endRubin, 1.5f);

            DataMan.GiveRuby(100);

            ButtonPress(buttonRuby, true);
            particleRubins.Play();

            UpdateStats();
        }
        else if(false)
        {
            ButtonPress(buttonGold, false);
        }
    }


//function helpers
    private void UpdateStats()
    {
        lvlSpace.GetComponent<TMPro.TextMeshProUGUI>().text = (DataMan.GetLvl()).ToString();

        goldSpace.text = DataMan.GetGold().ToString();
        rubySpace.text = DataMan.GetRuby().ToString();

        goldPriceForLvlUp = (DataMan.GetXP() * 2);

        price1.text = (goldPriceForLvlUp).ToString();
        price2.text = (rubyPriceForGold).ToString();
        price3.text = (rubyPriceForEnergy).ToString();
        price4.text = (moneyPriceForNoAds).ToString() + " $";
        price5.text = (moneyPriceForRuby).ToString() + " $";

        FillXP();
        ShowEnergy();
    }

    private void NextLevel()
    {
        LeanTween.scale(lvlSpace, new Vector2(4f, 4f), 0.5f).setEaseOutBounce().setLoopPingPong(1);
        lvlSpace.GetComponent<TMPro.TextMeshProUGUI>().color = new Color (0.0f, 1.0f, 0.0f, 1.0f);
        LeanTween.value(XPSlider.gameObject, updXP, 1.0f, 1.0f).setEase(LeanTweenType.easeInOutQuad).setOnUpdate((float val) => {
                updXP = val;
            }).setOnComplete(() => {
                buttonLvlUP.GetComponent<Button>().interactable = true;
                DataMan.NextLvl();
                lvlSpace.transform.localScale = new Vector3(3.2f, 3.2f, 3.2f);
                LeanTween.scale(lvlSpace, new Vector2(4.2f, 4.2f), 0.1f).setEaseOutBounce().setLoopPingPong(1);
                lvlSpace.GetComponent<TMPro.TextMeshProUGUI>().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
                UpdateStats();
                print("stats are updated");
                //animatedPart.GetComponent<Animator>().Play(newLevelAnimation.name);
                //lvlUpAnimation.Play(true);

            });
    }

    private void ShowEnergy()
    {
        int energy = DataMan.GetPower();
        if(energy > 60)
            energy = 60;
        //powerSpace.GetComponent<TMPro.TextMeshProUGUI>().text = (energy.ToString() + "/60");
        switch (energy)
        {
            case int val when val < 20:
                print("ur shit is empty");
                break;
            case int val when val >= 20 && val < 40:
                EnergyBar1.SetActive(true);
                break;
            case int val when val >= 40 && val < 60:
                EnergyBar1.SetActive(true);
                EnergyBar2.SetActive(true);
                break;
            case int val when val == 60:
                EnergyBar1.SetActive(true);
                EnergyBar2.SetActive(true);
                EnergyBar3.SetActive(true);
                break;
            default:
                print("something is wrong");
                break;
        }
    }

    private void FillXP()
    {
        int[] lvls = new int[12]{0, 5, 10, 15, 25, 35, 50, 70, 90, 120, 150, 100000};

        if(DataMan.GetLvl() == 10)
        {
            //ShowFullXPBar + MaxLVL
        }
        else
        {
            float first = (DataMan.GetXP() - lvls[DataMan.GetLvl()]);
            float second = (lvls[DataMan.GetLvl() + 1] - lvls[DataMan.GetLvl()]);

            updXP = first / second;
            print(first);
            print(second);
            print(updXP);
        }
    }

    private void ButtonPress(GameObject button, bool state)
    {
        button.transform.localScale = Vector3.one * 0.4f;
        if(state)
        {
            LeanTween.scale(button, Vector3.one / 2.2f, 0.1f).setLoopPingPong(1);
            button.GetComponent<Image>().color = new Color(0, 1, 0, 1);
            LeanTween.color(button.GetComponent<Image>().rectTransform, new Color(1, 1, 1, 1), 0.1f);
        } 
            else if (!state)
        {
            LeanTween.scale(button, Vector3.one / 2.2f, 0.1f).setLoopPingPong(1);
            button.GetComponent<Image>().color = new Color(1, 0, 0, 1);
            LeanTween.color(button.GetComponent<Image>().rectTransform, new Color(1, 1, 1, 1), 0.1f);
        }

    }

    IEnumerator FillEnergy()
    {
        EnergyBar1.SetActive(true);
        LeanTween.scale(EnergyBar1.GetComponent<Image>().rectTransform, Vector3.one * 1.1f, 0.1f).setLoopPingPong(1);
        yield return new WaitForSeconds(0.15f);

        EnergyBar2.SetActive(true);
        LeanTween.scale(EnergyBar2.GetComponent<Image>().rectTransform, Vector3.one * 1.1f, 0.1f).setLoopPingPong(1);
        yield return new WaitForSeconds(0.15f);

        EnergyBar3.SetActive(true);
        LeanTween.scale(EnergyBar3.GetComponent<Image>().rectTransform, Vector3.one * 1.1f, 0.1f).setLoopPingPong(1);

        UpdateStats();
    }

//value's animations
    void UpdatePrice1Value(float value)
    {
        price1.text = Mathf.RoundToInt(value).ToString();
    }

    void UpdateGoldValue(float value)
    {
        goldSpace.text = Mathf.RoundToInt(value).ToString();
    }

    void UpdateRubinValue(float value)
    {
        rubySpace.text = Mathf.RoundToInt(value).ToString();
    }

    private void Update()
    {
        XPSlider.value = updXP;
    }

//other
    public void CoinAdd()
    {
        goldSpace.color = new Color (1.0f, 1.0f, 0.0f, 1.0f);
        goldSpace.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        LeanTween.scale(goldSpace.gameObject, new Vector2(1.2f, 1.2f), 0.05f).setEaseOutBounce().setLoopPingPong(1).setOnComplete(() =>
            {
                goldSpace.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
                goldSpace.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                
            }
        );
    }

    public void RubinAdd()
    {
        rubySpace.color = new Color (1.0f, 0.0f, 1.0f, 1.0f);
        rubySpace.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        LeanTween.scale(rubySpace.gameObject, new Vector2(1.2f, 1.2f), 0.05f).setEaseOutBounce().setLoopPingPong(1).setOnComplete(() =>
            {
                rubySpace.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
                rubySpace.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        );
    }
}
