using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CatmullRomCurveInterpolation : MonoBehaviour {
	
	const int NumberOfPoints = 8;
	Vector3[] controlPoints;
	ArrayList table = new ArrayList();
	Dictionary<float, Vector2> map = new Dictionary<float, Vector2>();
	List<float> numbers = new List<float> ();
	
	const int MinX = -5;
	const int MinY = -5;
	const int MinZ = 0;

	const int MaxX = 5;
	const int MaxY = 5;
	const int MaxZ = 5;
	
	float time = 0f;
	const float DT = 0.001f;
	public static int segmentCount = 2;
	public static float tau = 0.5f;
	
	/* Returns a point on a cubic Catmull-Rom/Blended Parabolas curve
	 * u is a scalar value from 0 to 1
	 * segment_number indicates which 4 points to use for interpolation
	 */
	Vector3 ComputePointOnCatmullRomCurve(float u, int segmentNumber)
	{
    	// TODO - compute and return a point as a Vector3       
    	// Points on segment number 1 start at controlPoints[1] and end at controlPoints[2] etc...
    	Vector3 point = new Vector3();

    	Vector3 p0 = controlPoints[(segmentNumber - 2) % NumberOfPoints];
    	Vector3 p1 = controlPoints[(segmentNumber - 1) % NumberOfPoints];
    	Vector3 p2 = controlPoints[segmentNumber % NumberOfPoints];
    	Vector3 p3 = controlPoints[(segmentNumber + 1) % NumberOfPoints];



    	Vector3 c3 = new Vector3(); 
    	Vector3 c2 = new Vector3();
    	Vector3 c1 = new Vector3();
    	Vector3 c0 = new Vector3();

    	//x components of c values:
    	c3.x = (-1f*tau*p0.x) + (2f-tau)*p1.x + (tau-2f)*p2.x + tau*p3.x;
    	c2.x = 2f*tau*p0.x + (tau-3f)*p1.x + (3f-(2f*tau))*p2.x + tau*(-1f)*p3.x;
    	c1.x = -1f*tau*p0.x + tau*p2.x;
    	c0.x = p1.x;

    	//y components of c values:
    	c3.y = (-1f*tau*p0.y) + (2f-tau)*p1.y + (tau-2f)*p2.y + tau*p3.y;
    	c2.y = 2f*tau*p0.y + (tau-3f)*p1.y + (3f-(2f*tau))*p2.y + tau*(-1f)*p3.y;
    	c1.y = -1f*tau*p0.y + tau*p2.y;
    	c0.y = p1.y;

    	//z components of c values:
    	c3.z = (-1f*tau*p0.z) + (2f-tau)*p1.z + (tau-2f)*p2.z + tau*p3.z;
    	c2.z = 2f*tau*p0.z + (tau-3f)*p1.z + (3f-(2f*tau))*p2.z + tau*(-1f)*p3.z;
    	c1.z = -1f*tau*p0.z + tau*p2.z;
    	c0.z = p1.z;

    	//Parabolic curve equation:
		//x(u) = c3xu3 + c2xu2 + c1xu + c0x
		//y(u) = c3yu3 + c2yu2 + c1yu + c0y
		//z(u) = c3zu3 + c2zu2 + c1zu + c0z 
    	point.x = c3.x*u*u*u + c2.x*u*u + c1.x*u + c0.x;
    	point.y = c3.y*u*u*u + c2.y*u*u + c1.y*u + c0.y;
    	point.z = c3.z*u*u*u + c2.z*u*u + c1.z*u + c0.z;
    	
    	return point;
	}
	
	void GenerateControlPointGeometry()
	{
		for(int i = 0; i < NumberOfPoints; i++)
		{
			GameObject tempcube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			tempcube.transform.localScale -= new Vector3(0.8f,0.8f,0.8f);
			tempcube.transform.position = controlPoints[i];
		}	
	}
	
	// Use this for initialization
	void Start () {

		controlPoints = new Vector3[NumberOfPoints];

		
		// set points randomly:
		controlPoints[0] = new Vector3(0,0,0);
		for(int i = 1; i < NumberOfPoints; i++)
		{
			controlPoints[i] = new Vector3(Random.Range(MinX,MaxX),Random.Range(MinY,MaxY),Random.Range(MinZ,MaxZ));
		}

		GenerateControlPointGeometry();
		Dictionary<float, Vector2> tempMap = new Dictionary<float, Vector2>();
		float totalDistance = 0.0f;
		Vector3 temporaryPoint = controlPoints[0];
		float prevDistance = -1f;
		for (int n = 0; n < controlPoints.Length; n++) {
			for(float u = 0.0f; u < 1.000f; u = u + DT){
				Vector2 value = new Vector2 (n, u);
				Vector3 newPoint = ComputePointOnCatmullRomCurve(u, n + 2);
				//original implementation
				float tempDistance = Vector3.Distance (temporaryPoint, newPoint);
				//single cubic ease-in
				//tempDistance = -2 * Mathf.Pow (u, 3) + 3 * Mathf.Pow (u, 2);
				//sinusoidal ease-in
				//tempDistance = (Mathf.Sin (Mathf.PI * u - Mathf.PI / 2) + 1) / 2;
				totalDistance += tempDistance;
				if (Mathf.Abs(totalDistance - prevDistance) < .0001 ) {
					//tempMap.Add (totalDistance + .0001f, value);
					//temporaryPoint = newPoint;
				} else {
					tempMap.Add (totalDistance, value);
					temporaryPoint = newPoint;
				}

				prevDistance = totalDistance;
			}
		}
		foreach(KeyValuePair<float, Vector2> entry in tempMap) {
			map.Add (entry.Key / totalDistance, entry.Value);
			numbers.Add (entry.Key / totalDistance);
		}
/*		foreach (KeyValuePair<float, Vector2> entry in map){
			print ("(" + entry.Key + "," + entry.Value + ")");
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		// TODO - use time to determine values for u and segment_number in this function call
   		
   		if(time >= 1f){
   			segmentCount++;
   			time = 0f;
   			//transform.rotation = Quaternion.Slerp(transform.rotation,
    		//	Quaternion.LookRotation(controlPoints[(segmentCount + 1) % NumberOfPoints] - transform.position), 6f*Time.deltaTime);
   			//print("Segment #"+segmentCount%8); //debug
   			//transform.LookAt(controlPoints[(segmentCount + 1) % NumberOfPoints]);
   		}else{
   			//print(time); //debug
   			time += DT;
   		}
		Vector2 fetched = new Vector2();
		float s = (Mathf.Sin (Mathf.PI * time - Mathf.PI / 2) + 1) / 2;
		//finds closest to time
		float closest = numbers.OrderBy (item => Mathf.Abs (s - item)).First ();
		bool test = map.TryGetValue (closest, out fetched);
		int segment = Mathf.RoundToInt(fetched.x);
		//returns point
		Vector3 temp = ComputePointOnCatmullRomCurve(fetched.y, segment + 2);
		//Vector3 temp = ComputePointOnCatmullRomCurve(time, segmentCount);
    	transform.LookAt(temp);
    	transform.position = temp;
		//print (closest);
	}
}
