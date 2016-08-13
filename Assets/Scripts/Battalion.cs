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
    public float MoveSpeed;
    public bool IsCollided = false;
    public GameObject FrayObject;

    private List<Vector3> _waypoints = new List<Vector3>();
    public List<Vector3> Waypoints
    {
        get { return _waypoints; }
        set { _waypoints = value; }
    }

    readonly Color PURPLE_COLOR = new Color(1.0F, 0.0F, 1.0F);

    // =============================================================================================================== //
	void Start () 
    {
	
	}
    // =============================================================================================================== 
	void Update () 
    {
        updateSize();
        updateColor();

	    // move towards stored waypoints and remove them while arrived
        if (_waypoints.Count > 0)
        {
            Vector3 targetPos = _waypoints[0];
            transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed * Time.deltaTime);
            // if close to waypoint remove it
            if (Vector3.Distance(targetPos, transform.position) <= 0.01F)
                _waypoints.RemoveAt(0);
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

        other.gameObject.GetComponent<Battalion>().IsCollided = true;
    }
    // =============================================================================================================== //
    private void updateSize()
    {
        // number of units is between 250 to 1000
        int units = NumberOfUnits;
        if (units < 100)
            units = 100;
        /*if (units > 1000)
            units = 1000;*/
        float scale = units / 1000.0F;
        transform.localScale = new Vector3(scale, scale, scale);
    }
    // =============================================================================================================== //
    private void updateColor()
    {
        if (Aligment == ForceAligment.FRIENDLY)
            this.GetComponent<SpriteRenderer>().color = Color.blue;
        else if (Aligment == ForceAligment.OPPOSING)
            this.GetComponent<SpriteRenderer>().color = Color.red;
        else
            this.GetComponent<SpriteRenderer>().color = PURPLE_COLOR;
    }
    // =============================================================================================================== //
}
