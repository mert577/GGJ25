using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIManager : MonoBehaviour
{

    public static UIManager instance;



    [Header("Health")]
    public TextMeshProUGUI healthText;

    [Header("Upgrades")]
    public TextMeshProUGUI upgradeDescriptionText;


    public GameObject upgradePanel;
    public GameObject upgradePanelLayout;

    public GameObject upgradeContainer;


    public TextMeshProUGUI coinText;

    public Image jumpCooldownImage;


    public GameObject deathPanel;

    public GameObject rerollButton;

    public float shakeRotation = 10f;

    public float punchScale = 1.1f;


    public GameObject inGamePanel;

    public GameObject titlePanel;

    public GameObject tutorialPanel;


    public GameObject upgradePanelPrompt;


    public float timeToShowUpgradePanelPrompt;

    public float timerForUpgradePanelPrompt = 10f;

    public TextMeshProUGUI scoreText;


    public List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();

    public float rerollPunchScale  = 1.1f;
    public float rerollPunchRotation = 10f;





    public void RerollButtonAnim()
    {
        Debug.Log("Rerolling button");
        rerollButton.transform.DORewind();
      //  rerollButton.transform.DOPunchRotation(new Vector3(0, 0, shakeRotation), 0.5f, 10, 1).SetUpdate(true);
        rerollButton.transform.DOPunchScale(Vector3.one * punchScale, 0.5f, 10, 1).SetUpdate(true);

        //shake all upgrade buttons
        upgradeButtons = new List<UpgradeButton>(FindObjectsOfType<UpgradeButton>(true));
        foreach(UpgradeButton button in upgradeButtons){
            //do rewinds to prevent stacking
            button.transform.DORewind();
            button.transform.DOPunchRotation(new Vector3(0, 0, rerollPunchRotation), 0.5f, 10, 1).SetUpdate(true);
            button.transform.DOPunchScale(Vector3.one * rerollPunchScale, 0.5f, 10, 1).SetUpdate(true);
        }

    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        timerForUpgradePanelPrompt = timeToShowUpgradePanelPrompt;
        DisableUpgradePrompt();

        
    }

    // Update is called once per frame
    void Update()
    {   





        if(GameManager.instance.isGameActive)
        {
                 timerForUpgradePanelPrompt -= Time.deltaTime;
        }
 
        if(timerForUpgradePanelPrompt <= 0)
        {
            if(!upgradePanel.activeSelf){

            TriggerUpgradePanelPrompt();
            }
        }
        if (Input.GetKeyDown(KeyCode.U) && GameManager.instance.isGameActive)
        {

            timerForUpgradePanelPrompt = timeToShowUpgradePanelPrompt;
            DisableUpgradePrompt();
            ToggleUpgradePanel(!upgradePanel.activeSelf);
        }
    }



    public void UpdateUpgradeButtonStates(){
        UpgradeButton[] buttons    = FindObjectsOfType<UpgradeButton>(true);
        Debug.Log("Updating upgrade button states");
        Debug.Log("Found " + buttons.Length + " buttons");
        foreach(UpgradeButton button in buttons){
            
            button.UpdateButtonState();
        }
    }

    public void DisableUpgradePrompt()
    {
        upgradePanelPrompt.SetActive(false);

        upgradePanelPrompt.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 75f);
    }
    public void EnableInGamePanel()
    {
        inGamePanel.SetActive(true);
    }

    public void UpdateJumpCooldown(float percent)
    {
        jumpCooldownImage.fillAmount = 1f - percent;
    }
    public void UpdateCoinText(int coins)
    {
        coinText.text = "" + coins;
    }
    public void ToggleUpgradePanel(bool state)
    {
        upgradePanel.SetActive(state);
        if (state)
        {
            Time.timeScale = 0;
             UpdateUpgradeButtonStates();
        }
        else
        {
            Time.timeScale = 1;
            UpdateUpgradeButtonStates();
        
        }
    }


    public void TriggerUpgradePanelPrompt()
    {
        upgradePanelPrompt.SetActive(true);
        upgradePanelPrompt.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f);

        //looping punch scale
        upgradePanelPrompt.transform.DOPunchScale(Vector3.one * .15f, 0.5f, 10, 1).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        timerForUpgradePanelPrompt = timeToShowUpgradePanelPrompt*2f;
    }

    public void DisableTitlePanel()
    {
        titlePanel.SetActive(false);
    }

    public void UpdateUpgradeDescription(string upgradeDescription)
    {
        Debug.Log("Updating upgrade description");
        Debug.Log(upgradeDescription);
        upgradeDescriptionText.text = upgradeDescription;
    }

    public void DisableUpgradeDescription()
    {
        upgradeDescriptionText.text = "";
    }



    public void EnableDeathPanel()
    {
        IEnumerator _()
        {
            scoreText.text = "Score: " + GameManager.instance.playerScore;
            yield return new WaitForSeconds(1.5f);
            deathPanel.SetActive(true);
            deathPanel.transform.DOPunchScale(Vector3.one * punchScale, 0.5f, 10, 1).SetUpdate(true);
        }

        StartCoroutine(_());
    }


    public void EnableTutorialPanel()
    {
        tutorialPanel.SetActive(true);
    }

    public void DisableTutorialPanel()
    {
        tutorialPanel.SetActive(false);
    }

    public void UpdateHealthText(int health)
    {
        healthText.text = "Health: " + health;
    }
}
