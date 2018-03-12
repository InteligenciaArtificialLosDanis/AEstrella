using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour {

	public GameObject estrella;
	public bool activa;
	Vector2 posEnMatriz;


	// Use this for initialization
	void Start () {
		activa = false;
		cambiaEstrella (false);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void cambiaEstrella(bool actEstr){
		if (actEstr) {

			Instantiate (estrella, this.transform);
		} 
		else { //CUANDO ES FALSO
			//estrella.renderer.enabled = false;
		}
	}

	public void setPosMatriz(int x, int y){
		posEnMatriz.x = x;
		posEnMatriz.y = y;
	}

	public Vector2 getPosMatriz(){
		return posEnMatriz;
	}


	void OnMouseDown() {

		activa = true;
		cambiaEstrella (true);
		GameManager.instance.setFichaActiva (this.gameObject);
		
	}
}
