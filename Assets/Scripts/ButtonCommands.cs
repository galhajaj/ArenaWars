using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonCommands : MonoBehaviour 
{
    public void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
