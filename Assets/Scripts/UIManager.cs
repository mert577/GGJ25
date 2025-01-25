using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
   
            ToggleUpgradePanel(!upgradePanel.activeSelf);
        }
    }


    public void UpdateJumpCooldown(float percent)
    {
        jumpCooldownImage.fillAmount = 1f- percent;
    }
    public void UpdateCoinText(int coins)
    {
        coinText.text = "Coins: " + coins;
    }
    public void ToggleUpgradePanel(bool state)
    {
        upgradePanel.SetActive(state);
        if(state)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
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
    


    public void UpdateHealthText(int health)
    {
        healthText.text = "Health: " + health;
    }
}
