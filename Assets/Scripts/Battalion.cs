using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ForceType
{
    INFANTRY,
    KNIGHTS,
    ARCHERS,
    PIKES,
    NONE
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
    //public float MoveSpeed;
    public bool IsCollided = false;
    public GameObject FrayObject;

    public Sprite InfantrySprite;
    public Sprite KnightsSprite;
    public Sprite PikesSprite;

    private float _speed = 0.0F;

    private List<Vector3> _waypoints = new List<Vector3>();

    public GameObject WaypointObject;
    private List<GameObject> _waypointsObjects = new List<GameObject>();

    private GameObject _prey = null;

    readonly Color PURPLE_COLOR = new Color(1.0F, 0.0F, 1.0F);
    readonly Color BLUE_COLOR   = new Color(0.4F, 0.4F, 1.0F);
    readonly Color RED_COLOR    = new Color(1.0F, 0.3F, 0.3F);

    // =============================================================================================================== //
	void Start () 
    {
	
	}
    // =============================================================================================================== 
	void Update () 
    {
        updateSize();
        updateColor();
        updateSpeed();
        updateSprite();

        updateFindPrey();
        updateLostPrey();
        updateChasePrey();

	    // move towards stored waypoints and remove them while arrived
        if (_waypoints.Count > 0)
        {
            Vector3 targetPos = _waypoints[0];
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _speed * Time.deltaTime);
            // if close to waypoint remove it
            if (Vector3.Distance(targetPos, transform.position) <= 0.01F)
            {
                _waypoints.RemoveAt(0);
                Destroy(_waypointsObjects[0]);
                _waypointsObjects.RemoveAt(0);
            }
        }
	}
    // =============================================================================================================== //
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Fray>() != null)
            return;

        if (other.gameObject.GetComponent<Battalion>().Aligment == Aligment)
            return;

        if (IsCollided)
            return;

        GameObject fray = Instantiate(FrayObject, other.transform.position, Quaternion.identity) as GameObject;
        fray.GetComponent<Fray>().Init(this.gameObject, other.gameObject);

        int mySign = (Aligment == ForceAligment.FRIENDLY) ? 1 : -1;
        int otherSign = (other.gameObject.GetComponent<Battalion>().Aligment == ForceAligment.FRIENDLY) ? 1 : -1;
        transform.position = new Vector3(mySign * 10000.0F, mySign * 10000.0F);
        other.transform.position = new Vector3(otherSign * 10000.0F, otherSign * 10000.0F);

        IsCollided = true;
        other.gameObject.GetComponent<Battalion>().IsCollided = true;

        this.ClearWaypoints();
        other.gameObject.GetComponent<Battalion>().ClearWaypoints();
    }
    // =============================================================================================================== //
    private void updateSize()
    {
        // number of units is between 250 to 1000
        int units = NumberOfUnits;
        if (units < 200)
            units = 200;
        /*if (units > 1000)
            units = 1000;*/
        float scale = units / 1000.0F / 1.5F;
        transform.localScale = new Vector3(scale, scale, scale);
    }
    // =============================================================================================================== //
    private void updateColor()
    {
        if (Aligment == ForceAligment.FRIENDLY)
            this.GetComponent<SpriteRenderer>().color = BLUE_COLOR;//Color.blue;
        else if (Aligment == ForceAligment.OPPOSING)
            this.GetComponent<SpriteRenderer>().color = RED_COLOR;//Color.red;
        else
            this.GetComponent<SpriteRenderer>().color = PURPLE_COLOR;
    }
    // =============================================================================================================== //
    private void updateSpeed()
    {
        // TODO: in future make that 1000 will be 0.2 and 100 will be 0.8
        if (Type == ForceType.INFANTRY)
        {
            _speed = 0.6F - (NumberOfUnits - 100.0F) / 9000.0F;
        }
        else if (Type == ForceType.KNIGHTS)
        {
            _speed = 0.8F - (NumberOfUnits - 100.0F) / 9000.0F;
        }
        else if (Type == ForceType.PIKES)
        {
            _speed = 0.5F - (NumberOfUnits - 100.0F) / 9000.0F;
        }
    }
    // =============================================================================================================== //
    private void updateSprite()
    {
        if (Type == ForceType.INFANTRY)
            this.GetComponent<SpriteRenderer>().sprite = InfantrySprite;
        else if (Type == ForceType.KNIGHTS)
            this.GetComponent<SpriteRenderer>().sprite = KnightsSprite;
        else if (Type == ForceType.PIKES)
            this.GetComponent<SpriteRenderer>().sprite = PikesSprite;
    }
    // =============================================================================================================== //
    private void updateFindPrey()
    {
        if (Aligment != ForceAligment.OPPOSING)
            return;

        if (_prey != null)
            return;

        _prey = findRandomPrey();
    }
    // =============================================================================================================== //
    private void updateChasePrey()
    {
        if (_prey == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _prey.transform.position, _speed * Time.deltaTime);
    }
    // =============================================================================================================== //
    private void updateLostPrey()
    {
        if (_prey == null)
            return;

        float distanceToPrey = Vector3.Distance(_prey.transform.position, this.transform.position);
        //Debug.Log(distanceToPrey);
        if (distanceToPrey > 100.0F)
            _prey = null;
    }
    // =============================================================================================================== //
    public void AddWaypoint(Vector3 waypoint)
    {
        _waypoints.Add(waypoint);
        _waypointsObjects.Add( Instantiate(WaypointObject, waypoint, Quaternion.identity) as GameObject );
    }
    // =============================================================================================================== //
    public void ClearWaypoints()
    {
        _waypoints.Clear();
        foreach (GameObject obj in _waypointsObjects)
            Destroy(obj);
        _waypointsObjects.Clear();
    }
    // =============================================================================================================== //
    private GameObject findRandomPrey()
    {
        List<GameObject> preys = new List<GameObject>();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Battalion"))
        {
            if (obj.GetComponent<Battalion>().Aligment == ForceAligment.FRIENDLY)
                preys.Add(obj);
        }

        if (preys.Count <= 0)
            return null;

        int rand = Random.Range(0, preys.Count);

        return preys[rand];
    }
    // =============================================================================================================== //
}
