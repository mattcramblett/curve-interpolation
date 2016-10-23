using UnityEngine;
using System.Collections;

public class CatmullRomCurveInterpolation : MonoBehaviour {
	
	const int NumberOfPoints = 8;
	Vector3[] controlPoints;
	
	const int MinX = -5;
	const int MinY = -5;
	const int MinZ = 0;

	const int MaxX = 5;
	const int MaxY = 5;
	const int MaxZ = 5;
	
	float time = 0f;
	const float DT = 0.01f;
	public static int segmentCount = 1;
	public static float curveSpeed = 100;
	
	/* Returns a point on a cubic Catmull-Rom/Blended Parabolas curve
	 * u is a scalar value from 0 to 1
	 * segment_number indicates which 4 points to use for interpolation
	 */
	Vector3 ComputePointOnCatmullRomCurve(float u, int segmentNumber)
	{
    	// TODO - compute and return a point as a Vector3       
    	// Points on segment number 1 start at controlPoints[1] and end at controlPoints[2] etc...
    	Vector3 point = new Vector3();

    	float c0 = ((-u + 2f) * u - 1f) * u * 0.5f;
    	float c1 = (((3f * u - 5f) * u) * u + 2f) * 0.5f;
    	float c2 = ((-3f * u + 4f) * u + 1f) * u * 0.5f;
    	float c3 = ((u - 1f) * u * u) * 0.5f;

    	Vector3 p0 = controlPoints[(segmentNumber - 1) % NumberOfPoints];
    	Vector3 p1 = controlPoints[segmentNumber % NumberOfPoints];
    	Vector3 p2 = controlPoints[(segmentNumber + 1) % NumberOfPoints];
    	Vector3 p3 = controlPoints[(segmentNumber + 2) % NumberOfPoints];

    	point.x = (p0.x * c0) + (p1.x * c1) + (p2.x * c2) + (p3.x * c3);
    	point.y = (p0.y * c0) + (p1.y * c1) + (p2.y * c2) + (p3.y * c3);
    	point.x = (p0.z * c0) + (p1.z * c1) + (p2.z * c2) + (p3.z * c3);

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

		/*
		//Hard coded control points:
		controlPoints[0] = new Vector3(0,0,0);
		controlPoints[1] = new Vector3(0,0,0);
		controlPoints[2] = new Vector3(0,0,0);
		controlPoints[3] = new Vector3(0,0,0);
		controlPoints[4] = new Vector3(0,0,0);
		controlPoints[5] = new Vector3(0,0,0);
		controlPoints[6] = new Vector3(0,0,0);
		controlPoints[7] = new Vector3(0,0,0);
		*/

		GenerateControlPointGeometry();
	}
	
	// Update is called once per frame
	void Update () {
		// TODO - use time to determine values for u and segment_number in this function call
   		time += DT * Time.deltaTime * curveSpeed;
   		if(time >= 1){
   			segmentCount++;
   			time = 0;
   		}
    	
    	Vector3 temp = ComputePointOnCatmullRomCurve(time, segmentCount);
    	transform.position = temp;
	}
}
