using UnityEngine;
using System.Collections;

public class BackButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown () 
	{
		//Change this after compilation
		Application.LoadLevel("Game_AvianCounter_Select");
		Debug.Log ("Hello");
	}
}
