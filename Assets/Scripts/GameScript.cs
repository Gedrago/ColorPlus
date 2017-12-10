using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {
	//define the variables
	int Score = 0; 
	float GameLength = 60; 
	float TurnLength= 2;
	int Turn= 0;
	int gridLenY, gridLenX; 
	Vector3 NextCubePosition = new Vector3 (8, 10, 0);
	public GameObject[,] CubeArray; 
	public GameObject CubePre; 
	public GameObject NextCubePrefab; 
	GameObject NextCube; 
	new Vector3 CubePosition;
	Color[] ColorArray = { Color.blue, Color.green, Color.red, Color.yellow, Color.magenta };
	int row; 
	bool CubeActive; 
	bool NextCubeDisplaced;
	GameObject ActiveCube = null; 
	int ActiveCubeX, ActiveCubeY; 
	List<int> iList = new List<int>();
	float BreakTime;

	// Use this for initialization
	void Start () {
		//start values of variables 
		BreakTime = 4 ;
		gridLenX = 8;
		gridLenY = 5; 
		CreateCubeArray ();

		 
		 
	}
	void CreateCubeArray (){
		CubeArray = new GameObject[gridLenX, gridLenY];
		//make grid
		for (int y = 0; y < gridLenY ; y ++){
			for (int x = 0; x < gridLenX  ; x++) {
				CubePosition = new Vector3 (x *2, y * 2, 0);
				CubeArray[x,y]= Instantiate (CubePre, CubePosition, Quaternion.identity);
				CubeArray [x, y].GetComponent<CubeScript> ().IndividualX = x;
				CubeArray [x, y].GetComponent<CubeScript> ().IndividualY = y;

			}
		}
	}

	void GetNextCube(){
		if(NextCube == null){
			NextCube = Instantiate (NextCubePrefab, NextCubePosition, Quaternion.identity);
		}
		NextCube.GetComponent<Renderer> ().material.color = ColorArray [Random.Range (0, ColorArray.Length)];
		 
	}
	void EndGame(bool win){
		if (win) {
			print ("congratulations! you won");
		} else {
			print ("loser haha");
		}

	}
	GameObject FindAvailableCube(int y){
		List<GameObject> whiteCubes = new List<GameObject> () ;
		 
			//checking which one is white on the row 
		//int NumWhiteCubes =0;
		for (int x= 0; x< gridLenX; x++){
			if(CubeArray[x,y].GetComponent<Renderer>().material.color == Color.white){
				whiteCubes.Add(CubeArray[x, y]);	
				
			}
			
		}
		return PickWhiteCubes (whiteCubes);

	}
	GameObject PickWhiteCubes (List<GameObject> whiteCubes){
		if (whiteCubes.Count == 0){
			return null; 

		}
		//pick a random cube 

		return whiteCubes[Random.Range(0,whiteCubes.Count)]; 


	}

	GameObject FindAvailableCube(){
		List<GameObject> whiteCubes = new List<GameObject> () ;

		//checking which one is white on the row 
		//int NumWhiteCubes =0;
		for (int y = 0; y < gridLenY; y++) {
			for (int x = 0; x < gridLenX; x++) {
				if (CubeArray [x, y].GetComponent<Renderer> ().material.color == Color.white) {
					whiteCubes.Add (CubeArray [x, y]);	

				}

			}
		}
		return PickWhiteCubes (whiteCubes);



	}
	void SetCubeColor (GameObject mycube, Color color ){
		if ( mycube == null) {
			EndGame (false);
		} 
		else {
			mycube.GetComponent<Renderer>().material.color =  color;
			Destroy (NextCube);
			NextCube = null; 

		}

	}
	void TransportCube(int y){
		//goes over each of the columns of a specific and checks which one is white and which one is black
		GameObject WhiteCube = FindAvailableCube(y);
		SetCubeColor (WhiteCube, NextCube.GetComponent<Renderer>().material.color ); 


	}
	void AddBlackCube (){
		GameObject WhiteCube = FindAvailableCube();
		SetCubeColor (WhiteCube, Color.black ); 
	}  



	void ProcessKeyboard() {
		//Detects what keyboard input was given and returns the number 
		int Numpressed= 0; 
		if (Input.GetKeyDown (KeyCode.Alpha1) ||  Input.GetKeyDown (KeyCode.Keypad1)){
			Numpressed = 1; 
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) ||  Input.GetKeyDown (KeyCode.Keypad2)){
			Numpressed = 2; 
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)  ||  Input.GetKeyDown (KeyCode.Keypad3)){
			Numpressed = 3; 
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)  ||  Input.GetKeyDown (KeyCode.Keypad4)){ 
			Numpressed = 4; 
		}
		if (Input.GetKeyDown(KeyCode.Alpha5)  ||  Input.GetKeyDown (KeyCode.Keypad5)){
			Numpressed = 5; 
		}
		//if we still have another cube and the player pressed a key
		if (NextCube!= null && Numpressed!=0 ){
			TransportCube (Numpressed - 1);
			
		}

	}
		
	public void ProcessClick (GameObject clickedCube, int IndividualX , int IndividualY , Color CubeColor, bool Active  ){

		// Any time a player clicks a colored cube (non-white, non-black) in the grid, the cube should activate.
		//An activated cube should highlight in some way, like enlarging a little bit or having a spotlight shine on it. 
		//Only one cube can be active at a time. If a player clicks an active cube, it should deactivate.
		if (CubeColor!= Color.white && CubeColor!= Color.black) {
			if (Active  == false) { 
				if(ActiveCube != null){
					ActiveCube.transform.localScale /= 1.5f;
					ActiveCube.GetComponent<CubeScript> ().Active = false;

				}
				clickedCube.transform.localScale *= 1.5f;
				clickedCube.GetComponent<CubeScript> ().Active = true; 
				ActiveCube = clickedCube; 
				 
			} else   {
				
				clickedCube.transform.localScale /= 1.5f;
				clickedCube.GetComponent<CubeScript> ().Active = false;
				ActiveCube = null; 
			}
		} 
		//If a player clicks a white cube that is adjacent to the active cube (including diagonals),
		//the active cube moves to that location instantly (and remains active). 
		//The location that the active cube just vacated should become a white cube.
		else if (CubeColor == Color.white && ActiveCube!= null) {
			if (Mathf.Abs(clickedCube.GetComponent<CubeScript>().IndividualX - ActiveCube.GetComponent<CubeScript>().IndividualX) <= 1 && Mathf.Abs(clickedCube.GetComponent<CubeScript>().IndividualY - ActiveCube.GetComponent<CubeScript>().IndividualY) <= 1){
				clickedCube.GetComponent<Renderer> ().material.color = ActiveCube.GetComponent<Renderer>().material.color;
				ActiveCube.GetComponent<Renderer> ().material.color = Color.white; 
				clickedCube.transform.localScale *= 1.5f;
				ActiveCube.transform.localScale /= 1.5f;
				ActiveCube.GetComponent<CubeScript>().Active = false; 
				// change the active cube to the new position 
				clickedCube.GetComponent<CubeScript>().Active = true; 

				//keep track of the new active cube 
				ActiveCube = clickedCube;
			}	 
		} 
		//Cube movement due to clicking happens instantly and not on turn boundaries. 
		//Therefore, with fast clicking, it’s possible to make many moves in a single turn.
		//Note: The players can only move a colored cube to a white cube, 
		//so if the players click a colored cube and then another colored cube, no switching happens, 
		//and the second colored cube ends up being the active one. For example:
		}  

	//create a method to detect the color plus ...first to detect a same color plus 
	void DetectPlus (){
		
		// detects if there is plus of the same color OR if there is a plus of rainbow color
		//check the color of the adjacent color and return a bool of whether or not a plus has been made 
	}
	void CheckGameEnd(){
		//did we detect a game plus 

	}

	 
	
	// Update is called once per frame
	void Update () { 
		ProcessKeyboard (); 

		
		if(Time.time > TurnLength*Turn){
			Turn++;
			if (NextCube != null){
				Score -= 1;
				AddBlackCube (); 
			}
			 
			GetNextCube ();
		 
			
		}
		 
		 
	}
}
