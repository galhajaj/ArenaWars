using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour 
{
    public void StartGame()
    {
        SceneManager.LoadScene("manageArmyScene");
    }
}
