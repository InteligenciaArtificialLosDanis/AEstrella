using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Flecha : MonoBehaviour {

	Vector2Int positionInMatrix;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setPosMatriz(int x, int y){
		positionInMatrix.x = x;
		positionInMatrix.y = y;
	}

	public Vector2Int getPosMatriz(){
		return positionInMatrix;
	}
}
