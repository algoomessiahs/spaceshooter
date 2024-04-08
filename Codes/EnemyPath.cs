using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{

    WaveConfig waveConfig;

    List<Transform> pathofEnemy;

    int wayPointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        pathofEnemy = waveConfig.GetWayPoints();
        transform.position = pathofEnemy[wayPointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void MoveEnemy()
    {
        if (wayPointIndex <= pathofEnemy.Count - 1)
        {
            var targetPosition = pathofEnemy[wayPointIndex].transform.position;
            var movementThisFrame = waveConfig.GetmoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                wayPointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
