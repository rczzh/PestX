using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 0, down = 1, left = 2, right = 3
}

public class LevelGenerationController : MonoBehaviour
{
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();
    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.up, Vector2Int.up },
        {Direction.down, Vector2Int.down },
        {Direction.left, Vector2Int.left },
        {Direction.right, Vector2Int.right }
    };

    public static List<Vector2Int> GenerateLevel(LevelGenerationData levelGenerationData)
    {
        List<LevelGeneration> levelGenerations = new List<LevelGeneration>();

        for (int i = 0; i < levelGenerationData.numberOfLevelGeneration; i++)
        {
            levelGenerations.Add(new LevelGeneration(Vector2Int.zero));
        }

        int iterations = Random.Range(levelGenerationData.iterationMin, levelGenerationData.iterationMax);

        for (int i = 0; i < iterations; i++)
        {
            foreach (LevelGeneration levelGeneration in levelGenerations)
            {
                Vector2Int newPosition = levelGeneration.Move(directionMovementMap);
                positionsVisited.Add(newPosition);
            }
        }

        return positionsVisited;
    }

}
