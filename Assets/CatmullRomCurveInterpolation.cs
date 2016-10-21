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
	
	/* Returns a point on a cubic Catmull-Rom/Blended Parabolas curve
	 * u is a scalar value from 0 to 1
	 * segment_number indicates which 4 points to use for interpolation
	 */
	Vector3 ComputePointOnCatmullRomCurve(float u, int segmentNumber)
	{
		
		// TODO - compute and return a point as a Vector3		
		// Hint: Points on segment number 0 start at controlPoints[0] and end at controlPoints[1]
		//		 Points on segment number 1 start at controlPoints[1] and end at controlPoints[2]
		//		 etc...

		Vector3 point = new Vector3();
		
		//The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
		Vector3 a = 2f * controlPoints[1 + segmentNumber];
		Vector3 b = controlPoints[2 + segmentNumber] - controlPoints[2 + segmentNumber];
		Vector3 c = 2f * controlPoints[0 + segmentNumber] - 5f * controlPoints[1 + segmentNumber] + 4f * controlPoints[2 + segmentNumber] - controlPoints[3 + segmentNumber];
		Vector3 d = -controlPoints[0 + segmentNumber] + 3f * controlPoints[1 + segmentNumber] - 3f * controlPoints[2 + segmentNumber] + controlPoints[3 + segmentNumber];

		//The cubic polynomial: a + b * t + c * t^2 + d * t^3
		point = 0.5f * (a + (b * u) + (c * u * u) + (d * u * u * u));
		
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
		

		if(time >= 2){
			time = 0;
		}else{
			time += DT;
		}
		
			
		// TODO - use time to determine values for u and segment_number in this function call
		
		Vector3 temp = ComputePointOnCatmullRomCurve(time,0);
		transform.position = temp;
	}
}
