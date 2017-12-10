using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour {
	GameScript scriptReference ;
	public int IndividualX, IndividualY; 
	public bool Active; 
	 

	// Use this for initialization

	void Start () {

 
		scriptReference = GameObject.Find ("Controller").GetComponent<GameScript> ();

	}

	void OnMouseDown (){

		scriptReference.ProcessClick (gameObject, IndividualX, IndividualY,  gameObject.GetComponent<Renderer>().material.color,Active);

	}  
	// Update is called once per frame
	void Update () {

	}
}

 
 