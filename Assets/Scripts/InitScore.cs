using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InitScore : MonoBehaviour 
{
    public Text TimeText;
	// Use this for initialization
	void Start () 
    {
        TimeText.text = "Best Time: " + PlayerPrefs.GetFloat("Player Score").ToString("F1");
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
