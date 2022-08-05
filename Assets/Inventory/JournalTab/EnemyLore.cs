using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemyLore", menuName = "ScriptableObjects/EnemyLore")]
public class EnemyLore : ScriptableObject
{
    public string displayName;
    [TextArea(4, 10)]
    public string enemyDescription; // The useful info
    [TextArea(4, 10)]
    public string flavourText; // funny and useless info
    public Sprite sprite;

    public enum EnemyType
    { crawling, dipsa, twistedDipsa, crabber, lapisLazer, bossBatter, batterRing, greatCrawler }
    public EnemyType thisEnemyType;
}
