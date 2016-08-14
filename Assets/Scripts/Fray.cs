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
        if (units < 200)
            units = 200;
        /*if (units > 1000)
            units = 1000;*/
        float scale = units / 1000.0F / 1.5F;
        transform.localScale = new Vector3(scale, scale, scale);
    }
    // =============================================================================================================== //
    private void updateBattle()
    {
        if (_rival1Script.Type == _rival2Script.Type)
        {
            int rand = Random.Range(0, 3);
            if (rand == 0)
                _rival1Script.NumberOfUnits--;
            else if (rand == 1)
                _rival2Script.NumberOfUnits--;
        }
        else if (
            (_rival1Script.Type == ForceType.KNIGHTS && _rival2Script.Type == ForceType.INFANTRY) || 
            (_rival1Script.Type == ForceType.INFANTRY && _rival2Script.Type == ForceType.PIKES) ||
            (_rival1Script.Type == ForceType.PIKES && _rival2Script.Type == ForceType.KNIGHTS))
        {
            int rand = Random.Range(0, 4);
            if (rand >= 0 && rand <= 2)
                _rival2Script.NumberOfUnits--;
            else
                _rival1Script.NumberOfUnits--;
        }
        else if (
            (_rival2Script.Type == ForceType.KNIGHTS && _rival1Script.Type == ForceType.INFANTRY) || 
            (_rival2Script.Type == ForceType.INFANTRY && _rival1Script.Type == ForceType.PIKES) ||
            (_rival2Script.Type == ForceType.PIKES && _rival1Script.Type == ForceType.KNIGHTS))
        {
            int rand = Random.Range(0, 4);
            if (rand >= 0 && rand <= 2)
                _rival1Script.NumberOfUnits--;
            else
                _rival2Script.NumberOfUnits--;
        }

        if (_rival1Script.NumberOfUnits <= 0)
        {
            _rival2.transform.position = this.transform.position;
            _rival2Script.IsCollided = false;
            Destroy(_rival1);
            Destroy(this.gameObject);
        }

        if (_rival2Script.NumberOfUnits <= 0)
        {
            _rival1.transform.position = this.transform.position;
            _rival1Script.IsCollided = false;
            Destroy(_rival2);
            Destroy(this.gameObject);
        }
    }
    // =============================================================================================================== //
}
