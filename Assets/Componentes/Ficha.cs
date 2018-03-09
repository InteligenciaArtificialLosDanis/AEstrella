using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour {

	public GameObject estrella;
	Vector2 posEnMatriz;


	// Use this for initialization
	void Start () {
		estrella.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){
	}

	public void cambiaActivo(bool activo){
		estrella.SetActive (activo);
	}

	public void setPosMatriz(int x, int y){
		posEnMatriz.x = x;
		posEnMatriz.y = y;
	}

	public Vector2 getPosMatriz(){
		return posEnMatriz;
	}
}
