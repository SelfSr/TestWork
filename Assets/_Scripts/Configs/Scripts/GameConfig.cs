using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    public List<Wave> waves;
}

[Serializable]
public class Wave
{
    public List<Enemy> enemies;
}