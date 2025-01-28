using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{


    public static TutorialManager instance;

    public List<GameObject> tutorialPanels = new List<GameObject>();


    public int currentPanelIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ActivateTutorialPanel(int panelIndex)
    {
        Time.timeScale = 0;
        tutorialPanels[panelIndex].SetActive(true);
    }


    public void SkipTutorialPanel(GameObject panel)
    {
        panel.SetActive(false);
        Time.timeScale = 1;
    }
  
}
