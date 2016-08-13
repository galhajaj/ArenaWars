using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour 
{
    public float TimeBetweenWaves = 10.0F;
    public int SizeOfMaxWave = 4;
    public GameObject Enemy;

    private float _spawnTimer = 0.0F;
    // =============================================================================================================== //
	void Start () 
    {
	
	}
    // =============================================================================================================== //
	void Update () 
    {
        if (_spawnTimer <= 0.0F)
        {
            _spawnTimer = TimeBetweenWaves;

            int sizeOfWave = Random.Range(1, SizeOfMaxWave + 1);

            for (int i = 0; i < sizeOfWave; ++i)
            {
                GameObject spawnPoint = getRandomSpawnPoint();
                GameObject enemy = Instantiate(Enemy, spawnPoint.transform.position, Quaternion.identity) as GameObject;
                enemy.GetComponent<Battalion>().Aligment = ForceAligment.OPPOSING;
                enemy.GetComponent<Battalion>().NumberOfUnits = Random.Range(100, 1000);
            }
        }
        else
        {
            _spawnTimer -= Time.deltaTime;
        }
	}
    // =============================================================================================================== //
    private GameObject getRandomSpawnPoint()
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        int rand = Random.Range(0, spawns.Length);
        return spawns[rand];
    }
    // =============================================================================================================== //
}
