using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{
    private GameObject _handledObject = null;
    private Vector3 _deltaOfClickFromObject;

    readonly float MIN_DISTANCE_TO_MANAGE_OBJECT = 1.0F;
    // =============================================================================================================== //
	void Start () 
    {
	
	}
    // =============================================================================================================== //
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            _handledObject = findClosestObjectToMousePosition();
            if (_handledObject != null)
            {
                _handledObject.GetComponent<Battalion>().ClearWaypoints();
            }
        }

        // if handled not null, collect waypoints from mouse position
        if (_handledObject != null)
        {
            if (!_handledObject.GetComponent<Battalion>().IsCollided)
            {
                Vector3 waypoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
                Vector3 waypointWorld = Camera.main.ScreenToWorldPoint(waypoint);
                waypointWorld.z = 0.0F;
                //_handledObject.GetComponent<Battalion>().Waypoints.Add(waypointWorld + _deltaOfClickFromObject);
                _handledObject.GetComponent<Battalion>().AddWaypoint(waypointWorld + _deltaOfClickFromObject);
            }
        }

        // mouse release
        if (Input.GetMouseButtonUp(0))
        {
            _handledObject = null;
        }
	}
    // =============================================================================================================== //
    private GameObject findClosestObjectToMousePosition()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseWorldPosition.z = 0.0F;

        GameObject closestObject = null;
        float closestDistance = 100000.0F;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Battalion"))
        {
            if (obj.GetComponent<Battalion>() == null)
                continue;

            if (obj.GetComponent<Battalion>().Aligment != ForceAligment.FRIENDLY)
                continue;

            float distance = Vector3.Distance(obj.transform.position, mouseWorldPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
                _deltaOfClickFromObject = obj.transform.position - mouseWorldPosition;
            }
        }

        if (closestDistance <= MIN_DISTANCE_TO_MANAGE_OBJECT)
            return closestObject;

        return null;
    }
    // =============================================================================================================== //
}
