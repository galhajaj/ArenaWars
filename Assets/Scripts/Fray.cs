using UnityEngine;
using System.Collections;

public class Fray : MonoBehaviour 
{
    private int _numberOfUnits;
    private GameObject _rival1;
    private GameObject _rival2;
    private Battalion _rival1Script;
    private Battalion _rival2Script;
    // =============================================================================================================== //
	void Start () 
    {
	
	}
    // =============================================================================================================== //
	void Update () 
    {
        updateBattle();
        updateSize();
	}
    // =============================================================================================================== //
    public void Init(GameObject rival1, GameObject rival2)
    {
        _rival1 = rival1;
        _rival2 = rival2;
        _rival1Script = _rival1.GetComponent<Battalion>();
        _rival2Script = _rival2.GetComponent<Battalion>();
    }
    // =============================================================================================================== //
    private void updateSize()
    {
        _numberOfUnits = _rival1Script.NumberOfUnits + _rival2Script.NumberOfUnits;

        // number of units is between 250 to 1000
        int units = _numberOfUnits;
        if (units < 100)
            units = 100;
        /*if (units > 1000)
            units = 1000;*/
        float scale = units / 1000.0F;
        transform.localScale = new Vector3(scale, scale, scale);
    }
    // =============================================================================================================== //
    private void updateBattle()
    {
        int rand = Random.Range(0, 3);
        if (rand == 0)
            _rival1Script.NumberOfUnits--;
        else if (rand == 1)
            _rival2Script.NumberOfUnits--;

        if (_rival1Script.NumberOfUnits <= 0)
        {
            _rival2.transform.position = this.transform.position;
            _rival2Script.IsCollided = false;
            Destroy(this.gameObject);
        }

        if (_rival2Script.NumberOfUnits <= 0)
        {
            _rival1.transform.position = this.transform.position;
            _rival1Script.IsCollided = false;
            Destroy(this.gameObject);
        }
    }
    // =============================================================================================================== //
}
