using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour 
{
    public float TimeBetweenWaves = 10.0F;
    public int SizeOfMaxWave = 4;
    public GameObject Enemy;

    private float _time = 0.0F;
    public Text TimeText;

    private float _spawnTimer = 0.0F;
    // =============================================================================================================== //
	void Start () 
    {
	
	}
    // =============================================================================================================== //
	void Update () 
    {
        updateTime();
        checkEndGame();


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

                int rand = Random.Range(0, 3);
                if (rand == 0) enemy.GetComponent<Battalion>().Type = ForceType.INFANTRY;
                else if (rand == 1) enemy.GetComponent<Battalion>().Type = ForceType.KNIGHTS;
                else if (rand == 2) enemy.GetComponent<Battalion>().Type = ForceType.PIKES;
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
    private void checkEndGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_time > PlayerPrefs.GetFloat("Player Score"))
                PlayerPrefs.SetFloat("Player Score", _time);
            SceneManager.LoadScene("mainScene");
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Battalion"))
        {
            if (obj.GetComponent<Battalion>() == null)
                continue;

            if (obj.GetComponent<Battalion>().Aligment == ForceAligment.FRIENDLY)
                return;
        }

        if (_time > PlayerPrefs.GetFloat("Player Score"))
            PlayerPrefs.SetFloat("Player Score", _time);
        SceneManager.LoadScene("mainScene");
    }
    // =============================================================================================================== //
    private void updateTime()
    {
        _time += Time.deltaTime;
        TimeText.text = _time.ToString("F1");
    }
    // =============================================================================================================== //
}
