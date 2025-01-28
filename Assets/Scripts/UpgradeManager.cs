using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{



    public static UpgradeManager instance;

    private int coins;


    public int numberOfUpgrades;


    public List<Upgrade> upgradesList = new List<Upgrade>();



    public List<GameObject> upgradeContainers = new List<GameObject>();



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

    public void AddCoins(int amount)
    {
        coins += amount;
        UIManager.instance.UpdateCoinText(coins);
    }


    // Start is called before the first frame update
    void Start()
    {
        PopulateMenuWithUpgrades();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public bool CanBuyUpgrade(Upgrade upgrade)
    {
        return coins >= upgrade.upgradeCost;
    }

    public void DestroyUpgradeContainers(bool addUpgradeBackToPool = false)
    {
        if (addUpgradeBackToPool)
        {
            foreach (GameObject upgradeContainer in upgradeContainers)
            {
                upgradesList.Add(upgradeContainer.GetComponent<UpgradeButton>().upgradeData);
            }
        }

        foreach (GameObject upgradeContainer in upgradeContainers)
        {
            Destroy(upgradeContainer);
        }
        upgradeContainers.Clear();
    }


    public void SpawnUpgradeContainer()
    {
        if (upgradesList.Count == 0)
        {
            Debug.Log("All upgrades used");
            return;

        }
        Upgrade upgrade = upgradesList[Random.Range(0, upgradesList.Count)];


        GameObject upgradeContainer = Instantiate(UIManager.instance.upgradeContainer, UIManager.instance.upgradePanelLayout.transform);
        upgradeContainer.GetComponent<UpgradeButton>().InitWithData(upgrade);
        upgradeContainers.Add(upgradeContainer);
        upgradesList.Remove(upgrade);
    }



    public void RerollUpgrades()
    {

        if (coins < 5)
        {
            Debug.Log("Not enough coins");
            return;
        }
        coins -= 5;
        UIManager.instance.UpdateCoinText(coins);
        DestroyUpgradeContainers(true);
        PopulateMenuWithUpgrades();
        UIManager.instance.UpdateUpgradeButtonStates();
    }

    public void PopulateMenuWithUpgrades()
    {
        for (int i = 0; i < numberOfUpgrades; i++)
        {


            SpawnUpgradeContainer();
        }

         UIManager.instance.UpdateUpgradeButtonStates();
    }


    public void TryBuyUpgrade(UpgradeButton upgradeContainer)
    {
        Upgrade upgrade = upgradeContainer.upgradeData;
        Debug.Log("Trying to buy upgrade: " + upgrade.upgradeName);
        if (coins >= upgrade.upgradeCost)
        {
            coins -= upgrade.upgradeCost;
            UIManager.instance.UpdateCoinText(coins);
            upgrade.ApplyUpgrade();

            upgradeContainers.Remove(upgradeContainer.gameObject);
            Destroy(upgradeContainer.gameObject);
            SpawnUpgradeContainer();
            UIManager.instance.UpdateUpgradeButtonStates();

        }
    }
}
