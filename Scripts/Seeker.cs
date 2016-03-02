using UnityEngine;
using System.Collections;

public class Seeker : Vehicle {

    //-----------------------------------------------------------------------
    // Class Fields
    //-----------------------------------------------------------------------
    public GameObject seekerTarget;
	public Vector3 obAvoidance;
	public Vector3 obAvoidancetotal;
	public Vector3 direction;
	public Vector3 cohension;
	public Vector3 seperation;
    public Vector3 seekingforce;
	public float safeDistance=10.0f;
    public Vector3 total = Vector3.zero;
    public Vector3 test;
    public float distse;
	public bool closeEnough=false;
	public bool leader;
	public bool alternatepath=false;

	//tells me what path they are on 
	public enum Path
	{
		goingtodorms,
		goingtoacademia,
		engineeringcollege,
		boothcollege,
		sciencecollege,
		gracies,
		dorm
	};

	//keeps track of where i am
	public Path location;

	public enum flockgroup
	{
		flockgroup1,
		flockgroup2
	}
	
	public flockgroup group;

    //Seeker's steering force (will be added to acceleration)
    private Vector3 force;

    //WEIGHTING!!!!
    public float SeekWeight = 25.0f;
	public float AvoidWeight=30.0f;
	public float alignmentweight=5.0f;
	public float Cohensionweight=5.0f;
	public float Seperationweight=100.5f;
    public float scaledmax=5f;

    //-----------------------------------------------------------------------
    // Start - No Update
    //-----------------------------------------------------------------------
	// Call Inherited Start and then do our own
	override public void Start () {
        //call parent's start
		base.Start();

        //initialize
        force = Vector3.zero;
	}

    //-----------------------------------------------------------------------
    // Class Methods
    //-----------------------------------------------------------------------

    protected override void CalcSteeringForces() {
		//reset value to (0, 0, 0)

		closeEnough = false;
		force = Vector3.zero;
		obAvoidancetotal = Vector3.zero;
        
		if (this.Queueing == true) {
			//got a seeking force
			if (leader == true) {
				//leader will always seek a point
				force += Seek (seekerTarget.transform.position) * SeekWeight;
			} else {
				//non-leaders will queue
				force += Arrival () * SeekWeight;
			}
			//or if not queue they will seek and flock leader's target 
		} else if (this.Queueing == false) {
			force += Seek (seekerTarget.transform.position) * SeekWeight;
		}
        
		//obstacle avoidance
		for(int x =0;x<gm.Obstacles.Length;x++)
		{
			obAvoidance = this.AvoidObstacle(gm.Obstacles[x],safeDistance);
			obAvoidancetotal=obAvoidance+obAvoidancetotal;
		}
		force += obAvoidancetotal*AvoidWeight;
		//alignment
		if (group == flockgroup.flockgroup1) {
			direction = this.Alignment (gm.Flockdirection);
			force += direction * alignmentweight;
			//cohension
			//cohension = this.Cohension (gm.Centroid);
			//force += cohension * Cohensionweight;
			//seperation
			seperation = this.Seperation (gm.Flockers);
			force += seperation * Seperationweight;
			//limited the seeker's steering force
			force = Vector3.ClampMagnitude (force, maxForce);
		} else if (group == flockgroup.flockgroup2) {
			direction = this.Alignment (gm.Flockdirection2);
			force += direction * alignmentweight;
			//cohension
			//cohension = this.Cohension (gm.Centroid2);
			//force += cohension * Cohensionweight;
			//seperation
			seperation = this.Seperation (gm.Flockers2);
			force += seperation * Seperationweight;
			//limited the seeker's steering force
			force = Vector3.ClampMagnitude (force, maxForce);
		}
		//applied the steering force to this Vehicle's acceleration (ApplyForce)
		ApplyForce (force);
		pathfollowing ();
		//for my leader, once it reaches a point close enough, it will change targets
	}
	protected void pathfollowing()
	{
		if (leader == true) {
			//see if it is close enough to a waypoint
			closeEnough = CloseEnough (seekerTarget.transform.position);
			//Debug.Log (closeEnough);
			//se if it is on going to dorm waypoint or going to academia waypoint
			if (closeEnough == true && this.location == Path.goingtoacademia || closeEnough == true && this.location == Path.goingtodorms) {
				//see if it should go to the alertanate path
				if (dwaypointnumber == 3 || awaypointnumber == 3) {
					int rando = Random.Range (0, 100);
					//Debug.Log (rando);
					if (rando < 50) {
						alternatepath = true;

					}
					
				}
				//reset dorm heading path and change desire to Dorms
				if (dwaypointnumber == 3 && alternatepath == false) {
					dwaypointnumber = 0;

					this.location = Path.dorm;

				} 
				//reset academic heading path and change desire to engineering college
				if (awaypointnumber == 3 && alternatepath == false) {
					awaypointnumber = 0;

					this.location = Path.engineeringcollege;
				}

				//go to next point
				else if (this.location == Path.goingtoacademia && alternatepath == false) {
					awaypointnumber += 1;
					this.seekerTarget = awaypointslist [awaypointnumber];

				}
				//go to next point
				else if (this.location == Path.goingtodorms && alternatepath == false) {
					dwaypointnumber += 1;
					this.seekerTarget = dwaypointslist [dwaypointnumber];
				}
			}
			//make them go to the location
			if (this.location == Path.engineeringcollege && ewaypointnumber == 0) {
				this.seekerTarget = ewaypointslist [ewaypointnumber];
				closeEnough = CloseEnough (seekerTarget.transform.position);
			}
			if (this.location == Path.dorm && dormswaypointnumber == 0) {
				this.seekerTarget = dormswaypointslist [dormswaypointnumber];
				closeEnough = CloseEnough (seekerTarget.transform.position);
			}
			//going through the engineering college or dorms
			if (closeEnough == true && this.location == Path.engineeringcollege || closeEnough == true && this.location == Path.dorm) {
				//reset engineering path and go to goingtodorm
				if (this.location == Path.engineeringcollege && ewaypointnumber == 3) {
					ewaypointnumber = 0;
					this.seekerTarget = dwaypointslist [dwaypointnumber];
					closeEnough = CloseEnough (seekerTarget.transform.position);
					this.location = Path.goingtodorms;
				}
				//reset dorm path and go to goingtoacademia
				if (this.location == Path.dorm && dormswaypointnumber == 3) {
					dormswaypointnumber = 0;
					this.seekerTarget = awaypointslist [awaypointnumber];
					closeEnough = CloseEnough (seekerTarget.transform.position);
					this.location = Path.goingtoacademia;
				}
				//go to next point
				if (this.location == Path.engineeringcollege) {
					ewaypointnumber += 1;
					this.seekerTarget = ewaypointslist [ewaypointnumber];
				}
				if (this.location == Path.dorm) {
					dormswaypointnumber += 1;
					this.seekerTarget = dormswaypointslist [dormswaypointnumber];
				}
			}
			//if they should go to alternate path......
			if (alternatepath == true) {
				closeEnough = CloseEnough (seekerTarget.transform.position);
				//whcih one?
				if (this.location == Path.goingtoacademia) {
					int random = Random.Range (0, 100);
					if (random < 50) {
						this.location = Path.boothcollege;
						this.seekerTarget = boothwaypointslist [boothwaypointnumber];
					} else {
						this.location = Path.sciencecollege;
						this.seekerTarget = swaypointslist [swaypointnumber];
					}
				} else if (this.location == Path.goingtodorms) {

					this.location = Path.gracies;
					this.seekerTarget = gwaypointslist [gwaypointnumber];


				}
				//get to the path
				if (this.location == Path.boothcollege && boothwaypointnumber == 0) {
					awaypointnumber = 0;
					seekerTarget = boothwaypointslist [boothwaypointnumber];
					closeEnough = CloseEnough (seekerTarget.transform.position);

				}
				if (this.location == Path.sciencecollege && swaypointnumber == 0) {
					awaypointnumber = 0;
					seekerTarget = swaypointslist [swaypointnumber];
					closeEnough = CloseEnough (seekerTarget.transform.position);
				}
				if (this.location == Path.gracies && gwaypointnumber == 0) {
					dwaypointnumber = 0;
					seekerTarget = gwaypointslist [gwaypointnumber];
					closeEnough = CloseEnough (seekerTarget.transform.position);
				}
				//if they reach the end, go back to other path
				if (this.location == Path.boothcollege && boothwaypointnumber == 3 && this.closeEnough == true) {
					boothwaypointnumber = 0;
					seekerTarget = dwaypointslist [dwaypointnumber];
					this.location = Path.goingtodorms;
					alternatepath = false;
					closeEnough = CloseEnough (seekerTarget.transform.position);
				}
				if (this.location == Path.sciencecollege && swaypointnumber == 3 && this.closeEnough == true) {
					swaypointnumber = 0;
					seekerTarget = dwaypointslist [swaypointnumber];
					this.location = Path.goingtodorms;
					alternatepath = false;
					closeEnough = CloseEnough (seekerTarget.transform.position);
				}
				if (this.location == Path.gracies && gwaypointnumber == 3 && this.closeEnough == true) {
					gwaypointnumber = 0;
					seekerTarget = awaypointslist [awaypointnumber];
					closeEnough = CloseEnough (seekerTarget.transform.position);
					this.location = Path.goingtoacademia;
					alternatepath = false;
				}
				//move along
				if (this.location == Path.boothcollege && this.closeEnough == true) {
					boothwaypointnumber += 1;
					seekerTarget = boothwaypointslist [boothwaypointnumber];
				}
				if (this.location == Path.sciencecollege && this.closeEnough == true) {
					swaypointnumber += 1;
					seekerTarget = swaypointslist [swaypointnumber];
				}
				if (this.location == Path.gracies && this.closeEnough == true) {
					gwaypointnumber += 1;
					seekerTarget = gwaypointslist [gwaypointnumber];
				}



			}


			Debug.Log (this.location);



			this.Changing ();
		}


		if (Queueing == false && leader == false && group == flockgroup.flockgroup1) {
			this.seekerTarget = gm.Flockers [0].GetComponent<Seeker> ().seekerTarget;
		}
		if (Queueing == false && leader == false && group == flockgroup.flockgroup2) {
			this.seekerTarget = gm.Flockers2 [0].GetComponent<Seeker> ().seekerTarget;
		}

	}
	protected Vector3 Arrival()
	{
		Vector3 desired = Vector3.zero;
		 desired = this.seekerTarget.GetComponent<Vehicle>().Behind-this.transform.position;
         Debug.DrawLine(this.transform.position,this.transform.position+desired);
		float arrivaldistance = 0f;
		arrivaldistance=desired.magnitude;
		//Debug.Log (arrivaldistance);
		desired.Normalize ();
		if (arrivaldistance < 2.5f) {
			float m = arrivaldistance/10*maxSpeed;
			desired = desired * m;

		}
		else {
			desired = desired*maxSpeed;
		}
		desired -= velocity;
		desired.y = 0;
		return desired;
	}
   protected void Changing()
   {
		//Debug.Log("HERE");
		if (group == flockgroup.flockgroup1) {
			GameObject leader = gm.Flockers [0];
			float dist = 0;
			//make them flock
			if (leader.GetComponent<Seeker> ().awaypointnumber == 0 && leader.GetComponent<Seeker> ().location == Path.goingtoacademia || leader.GetComponent<Seeker> ().dwaypointnumber == 0 && leader.GetComponent<Seeker> ().location == Path.goingtodorms) {
				dist = Vector3.Magnitude (leader.transform.position - leader.GetComponent<Seeker> ().seekerTarget.transform.position);
				Debug.Log (dist);
				if (dist < 20) {
					for (int x = 1; x < gm.Flockers.Length; x++) {

						GameObject changer = gm.Flockers [x];
						changer.GetComponent<Seeker> ().seekerTarget = gm.Flockers [0].GetComponent<Seeker> ().seekerTarget;
						changer.GetComponent<Vehicle> ().Queueing = false;
						gm.Flockers [x] = changer;
					}
				}
			}
       //make them queue 
       else if (leader.GetComponent<Seeker> ().boothwaypointnumber == 0 && leader.GetComponent<Seeker> ().location == Path.boothcollege || leader.GetComponent<Seeker> ().ewaypointnumber == 0 && leader.GetComponent<Seeker> ().location == Path.engineeringcollege || leader.GetComponent<Seeker> ().swaypointnumber == 0 && leader.GetComponent<Seeker> ().location == Path.sciencecollege || leader.GetComponent<Seeker> ().dormswaypointnumber == 0 && leader.GetComponent<Seeker> ().location == Path.dorm || leader.GetComponent<Seeker> ().gwaypointnumber == 0 && leader.GetComponent<Seeker> ().location == Path.gracies) {
				dist = Vector3.Magnitude (leader.transform.position - leader.GetComponent<Seeker> ().seekerTarget.transform.position);
				Debug.Log (dist);
				if (dist < 30) {
					for (int x = 1; x < gm.Flockers.Length; x++) {

						GameObject changer = gm.Flockers [x];
						changer.GetComponent<Seeker> ().seekerTarget = gm.Flockers [x - 1];
						changer.GetComponent<Vehicle> ().Queueing = true;
						gm.Flockers [x] = changer;
					}
				}
 
			}
		}
		else if(group==flockgroup.flockgroup2){
			GameObject leader2 = gm.Flockers2 [0];
			float dist = 0;
			//make them flock
			if (leader2.GetComponent<Seeker> ().awaypointnumber == 0 && leader2.GetComponent<Seeker> ().location == Path.goingtoacademia || leader2.GetComponent<Seeker> ().dwaypointnumber == 0 && leader2.GetComponent<Seeker> ().location == Path.goingtodorms) {
				dist = Vector3.Magnitude (leader2.transform.position - leader2.GetComponent<Seeker> ().seekerTarget.transform.position);
				Debug.Log (dist);
				if (dist < 20) {
					for (int x = 1; x < gm.Flockers2.Length; x++) {
						
						GameObject changer = gm.Flockers2 [x];
						changer.GetComponent<Seeker> ().seekerTarget = gm.Flockers2 [0].GetComponent<Seeker> ().seekerTarget;
						changer.GetComponent<Vehicle> ().Queueing = false;
						gm.Flockers2 [x] = changer;
					}
				}
			}
			//make them queue 
			else if (leader2.GetComponent<Seeker> ().boothwaypointnumber == 0 && leader2.GetComponent<Seeker> ().location == Path.boothcollege || leader2.GetComponent<Seeker> ().ewaypointnumber == 0 && leader2.GetComponent<Seeker> ().location == Path.engineeringcollege || leader2.GetComponent<Seeker> ().swaypointnumber == 0 && leader2.GetComponent<Seeker> ().location == Path.sciencecollege || leader2.GetComponent<Seeker> ().dormswaypointnumber == 0 && leader2.GetComponent<Seeker> ().location == Path.dorm || leader2.GetComponent<Seeker> ().gwaypointnumber == 0 && leader2.GetComponent<Seeker> ().location == Path.gracies) {
				dist = Vector3.Magnitude (leader2.transform.position - leader2.GetComponent<Seeker> ().seekerTarget.transform.position);
				Debug.Log (dist);
				if (dist < 50) {
					for (int x = 1; x < gm.Flockers2.Length; x++) {
						
						GameObject changer = gm.Flockers2 [x];
						changer.GetComponent<Seeker> ().seekerTarget = gm.Flockers2 [x - 1];
						changer.GetComponent<Vehicle> ().Queueing = true;
						gm.Flockers2 [x] = changer;
					}
				}
				
			}
		}

	}
}
