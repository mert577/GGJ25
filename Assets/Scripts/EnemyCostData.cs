using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCostData", menuName = "EnemyCostData")]
public class EnemyCostData : ScriptableObject
{
    public int cost;
    public GameObject enemyPrefab;
}
