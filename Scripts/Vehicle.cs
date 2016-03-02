using UnityEngine;
using System.Collections;

//use the Generic system here to make use of a Flocker list later on
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]

abstract public class Vehicle : MonoBehaviour {

    //-----------------------------------------------------------------------
    // Class Fields
    //-----------------------------------------------------------------------

    //movement
    protected Vector3 acceleration;
    protected Vector3 velocity;
    protected Vector3 desired;
	protected GameManager gm;
	protected GameObject[] awaypointslist;
	protected GameObject[] dwaypointslist;
	protected GameObject[] boothwaypointslist;
	protected GameObject[] ewaypointslist;
	protected GameObject[] swaypointslist;
	protected GameObject[] gwaypointslist;
	protected GameObject[] dormswaypointslist;
	protected int awaypointnumber;
	protected int dwaypointnumber;
	protected int boothwaypointnumber;
	protected int ewaypointnumber;
	protected int swaypointnumber;
	protected int gwaypointnumber;
	protected int dormswaypointnumber;
	protected Vector3 behind;
    protected bool queueing;


	public Vector3 Behind{
		get {return behind;}
	}
    public Vector3 Velocity {
        get { return velocity; }
    }
    public bool Queueing
    {
        get { return queueing; }
        set { queueing = value; }
    }


    //public for changing in Inspector
    //define movement behaviors
    public float maxSpeed = 2.0f;
    public float maxForce = 12.0f;
    public float mass = 1.0f;
    public float radius = 1.0f;

    //access to Character Controller component
    CharacterController charControl;
    

    abstract protected void CalcSteeringForces();


    //-----------------------------------------------------------------------
    // Start and Update
    //-----------------------------------------------------------------------
	virtual public void Start(){
        //acceleration = new Vector3 (0, 0, 0);     
        acceleration = Vector3.zero;
        velocity = transform.forward;
        charControl = GetComponent<CharacterController>();
		gm = GameObject.Find ("GameManagerGO").GetComponent<GameManager> ();
		awaypointslist = gm.Awaypoints;
		awaypointnumber = gm.Awaypointnum;
		dwaypointnumber = gm.Dwaypointnum;
		dwaypointslist = gm.Dwaypoints;
		boothwaypointnumber = gm.Bwaypointnum;
	    boothwaypointslist=gm.BoothWaypoints;
		ewaypointnumber=gm.Ewaypointnum;
		ewaypointslist=gm.Ewaypoints;
		swaypointnumber=gm.Swaypointnum;
		swaypointslist=gm.Swaypoints;
		gwaypointnumber=gm.gwaypointnum;
		gwaypointslist=gm.Gracieswaypoints;
		dwaypointnumber = gm.Dwaypointnum;
		dwaypointslist = gm.Dwaypoints;
		dormswaypointslist = gm.Dormswaypoints;
	}

	
	// Update is called once per frame
	protected void Update () {
		//calculate all necessary steering forces
		CalcSteeringForces ();

		//add accel to vel
		velocity += acceleration * Time.deltaTime;
		velocity.y = 0;
		//limit vel to max speed
		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
		//move the character based on velocity
		charControl.Move (velocity * Time.deltaTime);
		//reset acceleration to zero
		acceleration = Vector3.zero;
		//turn the dude 
		transform.forward = velocity.normalized;
		behind = this.transform.position+(-1 * transform.forward.normalized);
	}


    //-----------------------------------------------------------------------
    // Class Methods
    //-----------------------------------------------------------------------

    protected void ApplyForce(Vector3 steeringForce) {
        acceleration += steeringForce / mass;
    }

    protected Vector3 Seek(Vector3 targetPos) {
        desired = targetPos - transform.position;
        desired = desired.normalized * maxSpeed;
        desired -= velocity;
        desired.y = 0;
        return desired;
    }
	//Cohension
	protected Vector3 Cohension(Vector3 Centroid)
	{
		
		desired = Centroid - this.transform.position;
		desired = desired.normalized * maxSpeed;
		desired -= velocity;
		desired.y = 0;
		return desired;
	}
	//Alignment
	protected Vector3 Alignment(Vector3 direction)
	{
		
		desired = direction; 
		desired = desired.normalized * maxSpeed;
		desired -= velocity;
		desired.y = 0;
		//Debug.Log (desired);
		return desired;
		
	}
	//seperation
	protected Vector3 Seperation(GameObject[] seekers)
	{
		desired = Vector3.zero;
		float dist = -1;
		float newdist;
		int numinlist=0;
		//FLEE FROM CLOSEST
		//find closest neighbor
		for(int x =0;x<seekers.Length;x++)
		{
			if(dist==-1)
			{
				dist = Vector3.Magnitude(this.transform.position-seekers[x].transform.position);
				numinlist=x;
			}
			else
			{
				newdist = Vector3.Magnitude(this.transform.position-seekers[x].transform.position);
				if(newdist<dist&&newdist!=0.0f)
				{
					dist=newdist;
					numinlist=x;
				}
			}
		}
		if (dist < 2.8f) 
		{
			desired = seekers[numinlist].transform.position-this.transform.position;
			desired = desired.normalized*maxSpeed;
			desired-=velocity;
			desired.y=0;
			desired = desired*-1;
		}
		return desired;
		
		
		
		
		//FLEE FROM EVERYONE
		//    total = Vector3.zero;
		//
		//	for (int x =0; x<seekers.Length; x++) {
		//		 test = seekers[x].transform.position-transform.position;
		//		 distse = test.magnitude;
		//        
		//		Debug.Log (distOb);
		//		if(distse<3.0f&&distOb!=0)
		//		{
		//			test.Normalize();
		//			test=test*maxSpeed;
		//			test=test*-1;
		//			test=test/distse;
		//			total=total+test;
		//		}
		//
		//	}
		//	total.Normalize();
		//	total=total*maxSpeed;
		//	total-=velocity;
		//	return total;
		
	}

	protected Vector3 AvoidObstacle(GameObject ob, float safe) {
		
		//reset desired velocity
		desired = Vector3.zero;
		//get radius from obstacle's script
		float obRad = ob.GetComponent<ObstacleScript>().Radius;
		//get vector from vehicle to obstacle
		Vector3 vecToCenter = ob.transform.position - transform.position;
		//zero-out y component (only necessary when working on X-Z plane)
		vecToCenter.y = 0;
		//if object is out of my safe zone, ignore it
		if(vecToCenter.magnitude > safe){
			return Vector3.zero;
		}
		//if object is behind me, ignore it
		if(Vector3.Dot(vecToCenter, transform.forward) < 0){
			return Vector3.zero;
		}
		//if object is not in my forward path, ignore it
		if(Mathf.Abs(Vector3.Dot(vecToCenter, transform.right)) > obRad + radius){
			return Vector3.zero;
		}
		
		//if we get this far, we will collide with an obstacle!
		//object on left, steer right
		if (Vector3.Dot(vecToCenter, transform.right) < 0) {
			desired = transform.right * maxSpeed;
			//debug line to see if the dude is avoiding to the right
			Debug.DrawLine(transform.position, ob.transform.position, Color.red);
		}
		else {
			desired = transform.right * -maxSpeed;
			//debug line to see if the dude is avoiding to the left
			Debug.DrawLine(transform.position, ob.transform.position, Color.green);
		}
		return desired;
	}

	protected bool CloseEnough(Vector3 waypointpos)
	{
		bool CE = false;
		float distance = 0f;
		distance = Vector3.Magnitude (waypointpos - this.transform.position);

		if (distance < 2.5f) {
			CE=true;
			return CE;
		}
		return CE;
	}


}
