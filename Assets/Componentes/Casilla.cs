using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour {
	 
	public Vector2 positionInMatrix;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnMouseDown() {
		
		GameManager.instance.onClick (this.gameObject);
	}
}
