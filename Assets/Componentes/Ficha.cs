using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour {

	public GameObject estrella;
	public bool activa;
	Vector2Int posEnMatriz, nuevaPos;

    int tipoAntiguo = 0;


	// Use this for initialization
	void Start () {
		activa = false;
		cambiaEstrella (false);

	}

    public int getTipoAntiguo()
    {
        return tipoAntiguo;
    }

    public void setTipoAntiguo(int nuevoTipoAntiguo)
    {
        tipoAntiguo = nuevoTipoAntiguo;
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

	public Vector2Int getPosMatriz(){
		return posEnMatriz;
	}


	void OnMouseDown() {

		activa = true;
		cambiaEstrella (true);
		GameManager.instance.setFichaActiva (this.gameObject);
		
	}

    public void mueveFicha(Vector2Int np)
    {
        nuevaPos = np;
        posEnMatriz = np;      //Actualizo la referencia a la matriz local
        Invoke("mueve", 0.7f); //Llama a mueve con un breveRetraso
    }

    void mueve()
    {
        transform.position.Set(nuevaPos.x, -nuevaPos.y, 0);
    }
}
