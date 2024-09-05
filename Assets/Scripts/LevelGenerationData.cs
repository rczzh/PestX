using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelGenerationData.asset", menuName = "LevelGenerationData/Level Data")]

public class LevelGenerationData : ScriptableObject
{
    public int numberOfLevelGeneration;
    public int iterationMin;
    public int iterationMax;
}
