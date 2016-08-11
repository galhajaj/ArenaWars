using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ForceType
{
    Infantry,
    Knights,
    Archers
}

public enum ForceAligment
{
    FRIENDLY,
    OPPOSING,
    NEUTRAL
}

public class Battalion : MonoBehaviour 
{
    public ForceType Type;
    public ForceAligment Aligment;
    public int NumberOfUnits;
    public float MoveSpeed;

    private bool _isHandled = false;
    private List<Vector3> _waypoints = new List<Vector3>();

    // =============================================================================================================== //
	void Start () 
    {
	
	}
    // =============================================================================================================== 
	void Update () 
    {
        updateSize();

        // if is handled collect waypoints from mouse position
        if (_isHandled)
        {
            Vector3 waypoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            _waypoints.Add(waypoint);
        }

        // mouse release
        if (Input.GetMouseButtonUp(0))
        {
            _isHandled = false;
        }

	    // move towards stored waypoints and remove them while arrived
        if (_waypoints.Count > 0)
        {
            var targetPos = Camera.main.ScreenToWorldPoint(_waypoints[0]);
            targetPos.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed * Time.deltaTime);
            // if close to waypoint remove it
            if (Vector3.Distance(targetPos, transform.position) <= 0.01F)
                _waypoints.RemoveAt(0);
        }
	}
    // =============================================================================================================== //
    void OnMouseDown() 
    {
        _waypoints.Clear();
        _isHandled = true;
    }
    // =============================================================================================================== //
    private void updateSize()
    {
        // number of units is between 250 to 1000
        int units = NumberOfUnits;
        if (units < 250)
            units = 250;
        if (units > 1000)
            units = 1000;
        float scale = units / 1000.0F;
        transform.localScale = new Vector3(scale, scale, scale);
    }
    // =============================================================================================================== //
}
