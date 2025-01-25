using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper;
using AYellowpaper.SerializedCollections;

public class ModifierManager : MonoBehaviour
{

    public static ModifierManager instance;


    public SerializedDictionary<string, int> modifierDictionary = new SerializedDictionary<string, int>();


    public SerializedDictionary<string, List<float>> modifierLevelValues = new SerializedDictionary<string, List<float>>();




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
        
    }


    public void UpgradeModifierLevel(string modifierName)
    {
        if(modifierDictionary.ContainsKey(modifierName))
        {
            modifierDictionary[modifierName] += 1;
        }
        else
        {
           Debug.LogError("Modifier " + modifierName + " does not exist in the dictionary");
        }
    }


    public float TryGetModifierValue(string modifierName)
    {
        if(modifierDictionary.ContainsKey(modifierName))
        {
            if(modifierDictionary[modifierName] >= modifierLevelValues[modifierName].Count)
            {
                Debug.LogError("Modifier " + modifierName + " is at max level");
                return modifierLevelValues[modifierName][modifierLevelValues[modifierName].Count - 1];
            }

            return modifierLevelValues[modifierName][modifierDictionary[modifierName]];
        }
        else
        {
            Debug.LogError("Modifier " + modifierName + " does not exist in the dictionary");
            return 0;
        }
    }
}
