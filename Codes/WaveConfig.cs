using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]

public class WaveConfig : ScriptableObject
{

    [SerializeField] GameObject enemyprefab;
    [SerializeField] GameObject pathprefab;
    [SerializeField] float spawnTime = 0.7f;
    [SerializeField] float randomSpawnTime = 0.4f;
    [SerializeField] int numberOfEnemies = 7;
    [SerializeField] float moveSpeed = 3f;

    public GameObject Getenemyprefab() { return enemyprefab; }

    public List<Transform> GetWayPoints()
    {
        var wavePoints = new List<Transform>();

        foreach(Transform child in pathprefab.transform)
        {
            wavePoints.Add(child);
        }

        return wavePoints;
    }

    public float GetspawnTime() { return spawnTime; }
    public float GetrandomSpawnTime() { return randomSpawnTime; }
    public int GetnumberOfEnemies() { return numberOfEnemies; }
    public float GetmoveSpeed() { return moveSpeed; }
}
