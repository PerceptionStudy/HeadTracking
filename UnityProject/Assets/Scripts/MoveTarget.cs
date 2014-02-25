using UnityEngine;
using UnityEditor;
using System.Collections;

public class MoveTarget : MonoBehaviour 
{
	private UDPReceive faceAPI;
	
	void Start () 
	{
		GameObject contoller = GameObject.FindGameObjectWithTag("GameController");
		faceAPI = contoller.GetComponentInChildren<UDPReceive>();
	}
	
	void Update () 
	{
		if(Camera.main != null)
		{
			Camera.main.orthographicSize = Screen.height * 0.5f;
		}

		float yMaxAngle = 20.0f;
		float xPosition = Mathf.Clamp(-faceAPI.yaw, -yMaxAngle, yMaxAngle) / yMaxAngle * Screen.width * 0.5f ;

		float xMaxAngle = 30.0f;
		float yPosition = Mathf.Clamp(faceAPI.pitch, -xMaxAngle, xMaxAngle) / xMaxAngle * Screen.height * 0.5f ;

		Vector3 pos = new Vector3(xPosition, 0, 0);
		//Vector3 pos = new Vector3(xPosition, yPosition, 0);

		this.transform.position = pos;
	}
}