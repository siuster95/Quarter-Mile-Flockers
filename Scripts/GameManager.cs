
using UnityEngine;
using System.Collections;

//add using System.Collections.Generic; to use the generic list format
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    //-----------------------------------------------------------------------
    // Class Fields
    //-----------------------------------------------------------------------
    public GameObject dude;
    public GameObject target;
	public GameObject obstacle;
    public GameObject dudePrefab;
    public GameObject targetPrefab;
    public GameObject obstaclePrefab;
	public GameObject femalePrefab;
	public GameObject malePrefab;
	public GameObject flocker2cube;
	//make game Cameras for people to cycle through and number to keep track
	public Camera[] cameras;
	private int currentCameraIndex;
	private float dist;
	private GameObject[] flockers;
	private GameObject[] flockers2;
	//Academic Heading path
	private GameObject[] awaypoints;
	//Dorm Heading path
	private GameObject[] dwaypoints;
	//path to booth college
	private GameObject[] boothwaypoints;
	//path to engineering college
	private GameObject[] ewaypoints;
	//path to science college
	private GameObject[] swaypoints;
	//path to Gracies
	private GameObject[] gracieswaypoints;
	//path to Dorms 
	private GameObject[] dormswaypoints;
	//obstacles
    private GameObject[] obstacles;
	//array to hold all wavepoints
	private GameObject[] waypoints;

	private bool wpactive;
	private Vector3 centroid;
	private Vector3 flockdirection;
	private Vector3 centroid2;
	private Vector3 flockdirection2;
	private int awaypointnumber;
	private int dwaypointnumber;
	private int bwaypointnumber;
	private int ewaypointnumber;
	private int swaypointnumber;
	private int gwaypointnumber;
	private int dormswaypointnumber;

	public GameObject[] Dormswaypoints
	{
		get{return dormswaypoints;}
	}
	public GameObject[] Gracieswaypoints
	{
		get{return gracieswaypoints;}
	}

	public GameObject[] Swaypoints
	{
		get{return swaypoints;}
	}

	public GameObject[] Ewaypoints
	{
		get{return ewaypoints;}
	}

	public GameObject[] BoothWaypoints
	{
		get{return boothwaypoints;}
	}

	public GameObject[] Awaypoints
	{
		get{return awaypoints;}
	}

	public GameObject[] Dwaypoints
	{
		get {return dwaypoints;}
	}
   
	public Vector3 Centroid
	{
		get {return centroid;}
	}

	public Vector3 Flockdirection
	{
		get { return flockdirection;}
	}

	public GameObject[] Flockers{
		get {return flockers;}
	}

	public Vector3 Centroid2
	{
		get{return centroid2;}
	}

	public Vector3 Flockdirection2
	{
		get{return flockdirection2;}
	}

	public GameObject[] Flockers2
	{
		get {return flockers2;}
	}
	public GameObject[] Obstacles {
		get {return obstacles;}
	}
	public int Awaypointnum{
		get {return awaypointnumber;}
	}

	public int Dwaypointnum{
		get {return dwaypointnumber;}
	}
	public int Bwaypointnum{
		get {return bwaypointnumber;}
	}
	public int Ewaypointnum{
		get {return ewaypointnumber;}
	}
	public int Swaypointnum{
		get {return swaypointnumber;}
	}
	public int gwaypointnum{
		get {return gwaypointnumber;}
	}
	public int Dormswaypointnum{
		get {return dormswaypointnumber;}
	}
	public bool Wpactive{
		get{ return wpactive;}
	}


    //-----------------------------------------------------------------------
    // Start and Update
    //-----------------------------------------------------------------------
	void Start () {
		wpactive = false;
		centroid = Vector3.zero;
		flockdirection = Vector3.zero;
		centroid2 = Vector3.zero;
		flockdirection2 = Vector3.zero;
		flocker2cube = GameObject.Find ("flockgroup2cube");
		awaypointnumber = 0;
		dwaypointnumber = 0;
		bwaypointnumber = 0;
		ewaypointnumber = 0;
		swaypointnumber = 0;
		gwaypointnumber = 0;
		dormswaypointnumber = 0;
		//instantiate the arrays
		//list for Academic sided path
		awaypoints = new GameObject[4];
		awaypoints [0] = GameObject.Find ("AwaypointA");
		awaypoints [1] = GameObject.Find ("AwaypointB");
		awaypoints [2] = GameObject.Find ("AwaypointC");
		awaypoints [3] = GameObject.Find ("AwaypointD");
		//list for Dorms sided path
		dwaypoints = new GameObject[4];
		dwaypoints [0] = GameObject.Find ("DwaypointA");
		dwaypoints [1] = GameObject.Find ("DwaypointB");
		dwaypoints [2] = GameObject.Find ("DwaypointC");
		dwaypoints [3] = GameObject.Find ("DwaypointD");
		//list for Booth  path
		boothwaypoints = new GameObject[4];
		boothwaypoints [0] = GameObject.Find ("Boothwaypoint0");
		boothwaypoints [1] = GameObject.Find ("Boothwaypoint1");
		boothwaypoints [2] = GameObject.Find ("Boothwaypoint2");
		boothwaypoints [3] = GameObject.Find ("Boothwaypoint3");
		//list for Engineering path
		ewaypoints = new GameObject[4];
		ewaypoints [0] = GameObject.Find ("Ewaypoint0");
		ewaypoints [1] = GameObject.Find ("Ewaypoint1");
		ewaypoints [2] = GameObject.Find ("Ewaypoint2");
		ewaypoints [3] = GameObject.Find ("Ewaypoint3");
		//list for science path 
		swaypoints = new GameObject[4];
		swaypoints [0] = GameObject.Find ("Swaypoint0");
		swaypoints [1] = GameObject.Find ("Swaypoint1");
		swaypoints [2] = GameObject.Find ("Swaypoint2");
		swaypoints [3] = GameObject.Find ("Swaypoint3");
		//list for gracies path
		gracieswaypoints = new GameObject[4];
		gracieswaypoints [0] = GameObject.Find ("Gwaypoint0");
		gracieswaypoints [1] = GameObject.Find ("Gwaypoint1");
		gracieswaypoints [2] = GameObject.Find ("Gwaypoint2");
		gracieswaypoints [3] = GameObject.Find ("Gwaypoint3");
		//list for dorm path
		dormswaypoints = new GameObject[4];
		dormswaypoints [0] = GameObject.Find ("Dormswaypoint0");
		dormswaypoints [1] = GameObject.Find ("Dormswaypoint1");
		dormswaypoints [2] = GameObject.Find ("Dormswaypoint2");
		dormswaypoints [3] = GameObject.Find ("Dormswaypoint3");
		//put all wavepoints into array
		waypoints = new GameObject[28];
		waypoints[0]=GameObject.Find ("AwaypointA");
		waypoints[1]=GameObject.Find ("AwaypointB");
		waypoints[2]=GameObject.Find ("AwaypointC");
		waypoints[3]=GameObject.Find ("AwaypointD");
		waypoints[4]=GameObject.Find ("DwaypointA");
		waypoints[5]= GameObject.Find ("DwaypointB");
		waypoints[6]= GameObject.Find ("DwaypointC");
		waypoints[7]= GameObject.Find ("DwaypointD");
		waypoints[8] = GameObject.Find ("Boothwaypoint0");
		waypoints[9]= GameObject.Find ("Boothwaypoint1");
		waypoints[10]= GameObject.Find ("Boothwaypoint2");
		waypoints[11] = GameObject.Find ("Boothwaypoint3");
		waypoints[12]= GameObject.Find ("Ewaypoint0");
		waypoints[13]= GameObject.Find ("Ewaypoint1");
		waypoints[14] = GameObject.Find ("Ewaypoint2");
		waypoints[15]= GameObject.Find ("Ewaypoint3");
		waypoints[16] = GameObject.Find ("Swaypoint0");
		waypoints[17]= GameObject.Find ("Swaypoint1");
		waypoints[18]= GameObject.Find ("Swaypoint2");
		waypoints[19] = GameObject.Find ("Swaypoint3");
		waypoints[20]= GameObject.Find ("Gwaypoint0");
		waypoints[21]= GameObject.Find ("Gwaypoint1");
		waypoints[22]= GameObject.Find ("Gwaypoint2");
		waypoints[23]= GameObject.Find ("Gwaypoint3");
		waypoints[24] = GameObject.Find ("Dormswaypoint0");
		waypoints[25]= GameObject.Find ("Dormswaypoint1");
		waypoints[26] = GameObject.Find ("Dormswaypoint2");
		waypoints[27]= GameObject.Find ("Dormswaypoint3");
		//turn the wavepoints off
		for (int f =0; f<waypoints.Length; f++) {
			waypoints[f].gameObject.SetActive(false);
		}
		//obstacles 
		obstacles = new GameObject[6];
		obstacles[0]=GameObject.Find ("obstacle");
		obstacles[1]=GameObject.Find ("obstacle1");
		obstacles[2]=GameObject.Find ("obstacle2");
		obstacles[3]=GameObject.Find ("obstacle3");
		obstacles[4]=GameObject.Find ("obstacle4");
		obstacles[5]=GameObject.Find ("obstacle5");
		//initailize the camareas list and first camera


		currentCameraIndex = 0;
		//turn off all cameras off, except the first one
		for (int i =1; i<cameras.Length; i++) 
		{
			cameras[i].gameObject.SetActive(false);
		}

		//if any cameras were added to the controller,enable the first one
		if (cameras.Length > 0) 
		{
			cameras[0].gameObject.SetActive(true);
			//write to console which camera is enabled
			Debug.Log ("Camera with name:" + cameras[0].name+"is now enabled");
		}
		//make the first dude 
		Vector3 pos = new Vector3 (0, 4.0f, 0);
		int num = Random.Range (0, 100);
		if (num < 50) 
		{
			dudePrefab = femalePrefab;
		}
		else 
		{
			dudePrefab=malePrefab;
		}
		pos = new Vector3 (Random.Range (20, 30), 30, 340);
		dude = (GameObject)Instantiate (dudePrefab, pos, Quaternion.identity);
		//set the googly eye guy's target
		target = awaypoints [0];
		dude.GetComponent<Seeker> ().seekerTarget = target;
		dude.GetComponent<Seeker> ().leader = true;
		dude.GetComponent<Seeker> ().Queueing = false;
		dude.GetComponent<Seeker> ().location = Seeker.Path.goingtoacademia;
		dude.GetComponent<Seeker>().group=Seeker.flockgroup.flockgroup1;
		//set the camera's target 
		cameras[1].GetComponent<SmoothFollow>().target = GameObject.Find("flockgroup1cube").transform;
		cameras[2].GetComponent<SmoothFollow>().target=GameObject.Find ("flockgroup2cube").transform;






		flockers = new GameObject[4];
		//make 20 new obstacles and put them in the Obstacle Array
		//for (int x=0; x<20; x++) {
		//	pos = new Vector3 (Random.Range (-30, 30), 4f, Random.Range (-30, 30));
		//	Quaternion rot = Quaternion.Euler (Random.Range (0, 180), Random.Range (0, 180), Random.Range (0, 180));
		//	obstacle=(GameObject)Instantiate(obstaclePrefab,pos,rot);
		//	obstacles[x]=obstacle;
		//}
		flockers [0] = dude;
		//make 5 flockers
		for (int x =1; x<flockers.Length; x++) {
			//Create the GooglyEye Guys at (random from 1 to 10, 1,random from 0 to 10)
			num=Random.Range(0,100);
			if (num < 50) 
			{
				dudePrefab = femalePrefab;
			}
			else 
			{
				dudePrefab=malePrefab;
			}
				

			pos = new Vector3 (Random.Range (20, 30),30f, Random.Range (345, 335));
			dude = (GameObject)Instantiate (dudePrefab, pos, Quaternion.identity);
			//set the googly eye guy's target
			dude.GetComponent<Seeker> ().seekerTarget = target;
			dude.GetComponent<Seeker> ().leader = false;
			dude.GetComponent<Seeker> ().Queueing = false;
			dude.GetComponent<Seeker> ().location = Seeker.Path.goingtoacademia;
			dude.GetComponent<Seeker>().group=Seeker.flockgroup.flockgroup1;
			flockers [x] = dude;
		}

		flockers2 = new GameObject[4];
		pos = new Vector3 (Random.Range (0, 10), 30f, Random.Range (-60, -50));
		num=Random.Range(0,100);
		if (num < 50) 
		{
			dudePrefab = femalePrefab;
		}
		else 
		{
			dudePrefab=malePrefab;
		}
		//make 2nd group first guy
		dude = (GameObject)Instantiate (dudePrefab, pos, Quaternion.identity);
		target = dwaypoints [0];
		dude.GetComponent<Seeker> ().seekerTarget = target;
		dude.GetComponent<Seeker> ().leader = true;
		dude.GetComponent<Seeker> ().Queueing = false;
		dude.GetComponent<Seeker> ().location = Seeker.Path.goingtodorms;
		dude.GetComponent<Seeker>().group=Seeker.flockgroup.flockgroup2;
		Flockers2 [0] = dude;
		for (int x =1; x<flockers2.Length; x++) {
			//Create the 2nd group of flockers
			num=Random.Range(0,100);
			if (num < 50) 
			{
				dudePrefab = femalePrefab;
			}
			else 
			{
				dudePrefab=malePrefab;
			}
			pos = new Vector3 (Random.Range (0, 10), 30f, Random.Range (-60, -50));
			dude = (GameObject)Instantiate (dudePrefab, pos, Quaternion.identity);
			//set the googly eye guy's target
			dude.GetComponent<Seeker> ().seekerTarget = target;
			dude.GetComponent<Seeker> ().leader = false;
			dude.GetComponent<Seeker> ().Queueing = false;
			dude.GetComponent<Seeker> ().location = Seeker.Path.goingtodorms;
			dude.GetComponent<Seeker>().group=Seeker.flockgroup.flockgroup2;
			flockers2 [x] = dude;
		}
	
	}
	void Update () {

		centroid = CalcCentroid ();
		flockdirection = CalcFlockDirection ();
		this.transform.forward = Flockdirection;
		this.transform.position = centroid;
		centroid2 = CalcCentroid2 ();
		flockdirection2 = CalcFlockDirection2 ();
		flocker2cube.transform.position = centroid2;
		flocker2cube.transform.forward = flockdirection2;
		Debug.DrawLine (this.transform.position, this.transform.position+this.transform.forward, Color.red);
		//cycle through cameras
		if (Input.GetKeyDown (KeyCode.C)) {
			currentCameraIndex++;
			Debug.Log ("C button has been pressed, switching to next camera");
			if (currentCameraIndex < cameras.Length) {
				cameras [currentCameraIndex - 1].gameObject.SetActive (false);
				cameras [currentCameraIndex].gameObject.SetActive (true);
				Debug.Log ("Camera with name:" + cameras [currentCameraIndex].name + "is now enabled");
			} else {
				cameras [currentCameraIndex - 1].gameObject.SetActive (false);
				currentCameraIndex = 0;
				cameras [currentCameraIndex].gameObject.SetActive (true);
				Debug.Log ("Camera with name:" + cameras [currentCameraIndex].name + "is now enabled");
			}
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			if(wpactive==false)
			{
				wpactive=true;
				for(int h =0;h<waypoints.Length;h++)
				{
					waypoints[h].gameObject.SetActive(true);
				}
			}
			else if(wpactive==true)
			{
				wpactive=false;
				for(int u=0;u<waypoints.Length;u++)
				{
					waypoints[u].gameObject.SetActive(false);
				}
			}
		}
		//Debug.Log (centroid);
      

		//get the distance between the noodle and googly eye guy
		//for (int z=0; z<flockers.Length; z++) {
		//	 dist = Vector3.Distance(target.transform.position, flockers[z].transform.position);
		//
		//	//if they get too close randomize the position
		//	if (dist < 5f) {
		//		{
		//			do
		//			target.transform.position = new Vector3 (Random.Range (-30, 30), 4f, Random.Range (-30, 30));
		//			while(NearAnObstacle());
		//		} 
		//
		//	
		//	}
		//
		//
		//}

		cameras[1].GetComponent<SmoothFollow>().target = GameObject.Find("flockgroup1cube").transform;
		cameras[2].GetComponent<SmoothFollow>().target=GameObject.Find ("flockgroup2cube").transform;

	}
	//NEAR AN OBSTACLE METHOD
	//bool NearAnObstacle(){
	//	//iterate through all obstacles and compare the distance between each obstacle and the noodle
	//	//if the noodle is within a 4 unit distance of the noodle,return true
	//	for(int y =0;y<obstacles.Length;y++)
	//	    {
	//		if(Vector3.Distance(target.transform.position,obstacles[y].transform.position)< 5.0f){
	//			return true;
	//		}
	//
	//	}
	//	//otherwise return false
	//	return false;
	//} 

	//CalcCentroid

	Vector3 CalcCentroid()
	{
		Vector3 centroidamount = Vector3.zero;
		for (int y =0; y<this.flockers.Length;y++) {
		centroidamount=centroidamount+flockers[y].transform.position;
		}
		centroidamount = centroidamount / flockers.Length;
		return centroidamount;
	}

	Vector3 CalcCentroid2()
	{
		Vector3 centroidamount = Vector3.zero;
		for (int y =0; y<this.flockers2.Length;y++) {
			centroidamount=centroidamount+flockers2[y].transform.position;
		}
		centroidamount = centroidamount / flockers2.Length;
		return centroidamount;
	}

	//CalcFlockDirection

	Vector3 CalcFlockDirection()
	{
		Vector3 direction = Vector3.zero;
		for (int y =0; y<this.Flockers.Length;y++) {
			direction+=Flockers[y].transform.forward;
		}
		direction = direction / Flockers.Length;
		return direction;
	}

	Vector3 CalcFlockDirection2()
	{
		Vector3 direction = Vector3.zero;
		for (int y =0; y<this.Flockers2.Length;y++) {
			direction+=Flockers2[y].transform.forward;
		}
		direction = direction / Flockers2.Length;
		return direction;
	}

    //-----------------------------------------------------------------------
    // Flocking Methods
    //-----------------------------------------------------------------------



}
