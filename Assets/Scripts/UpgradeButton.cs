using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeButton : MonoBehaviour
{

    public Upgrade upgradeData;
    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI upgradeCostText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClicked()
    {
        UpgradeManager.instance.TryBuyUpgrade(this);
    }

    public void OnButtonHover(){
        Debug.Log("Hovering over upgrade: " + upgradeData.upgradeName);
        UIManager.instance.UpdateUpgradeDescription(upgradeData.upgradeDescription);

    }

    public void OnButtonExit(){
        Debug.Log("Exiting upgrade: " + upgradeData.upgradeName);
        UIManager.instance.DisableUpgradeDescription();

    }

    public void InitWithData(Upgrade upgrade)
    {
        upgradeData = upgrade;
        upgradeNameText.text = upgrade.upgradeName;
        upgradeCostText.text = upgrade.upgradeCost.ToString();
     
    }
}
