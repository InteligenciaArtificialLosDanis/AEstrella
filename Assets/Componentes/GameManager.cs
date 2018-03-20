using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	public static GameManager instance;
	//public enum tipoCasilla {Normal, Embarrada, Bloqueada, Ficha, end_tipoCasilla};
    /*
	 * CASILLA VACIA = 0;
	 * CASILLA EMBARRADA = 1;
	 * CASILLA BLOQUEADA = 2;
     * CASILLA CON FICHA = 3;
	*/

	int [,] tablero = new int[10,10];
    
    //IA
    public GameObject IAManager;

	//GameObjects tablero
	public GameObject normal;
	public GameObject embarrada;
	public GameObject bloqueada;


	//Ficha
	public GameObject roja;
	public GameObject verde;
	public GameObject azul;

	GameObject fichaActiva, flechaActiva, rojaAct, verdeAct, azulAct;
	public Text seleccion;

	//Flechas
	public GameObject flechaRoja;
	public GameObject flechaVerde;
	public GameObject flechaAzul;

    const int MaxNumCasillasEmbarradas = 12, MaxNumCasillasBloqueadas = 10;
	int numCasillasEmbarradas = MaxNumCasillasEmbarradas;
	int numCasillasBloqueadas = MaxNumCasillasBloqueadas;
	// Use this for initialization
	void Start () {

		fichaActiva = null;

		instance = this;
		instanciaFichas ();
		creaTablero ();

	}
	
	// Update is called once per frame
	void Update () {
		
    }

	public void onClick(GameObject casillaPulsada){

		if (fichaActiva != null) {
			if (casillaPulsada.tag == "CasillaBloqueada") {

                if (fichaActiva.tag == "FichaRoja") Destroy(rojaAct);

                else if (fichaActiva.tag == "FichaVerde") Destroy(verdeAct);

                else if (fichaActiva.tag == "FichaAzul") Destroy(azulAct);

                fichaActiva = null;

                seleccion.color = new Color (1, 1, 1);
				seleccion.text = "Unicornio activo: ";

			} 
			//INSTANCIA DE LA FLECHA
			else {

                if (fichaActiva.tag == "FichaRoja" && rojaAct == null) instanciaFlecha(casillaPulsada, "FichaRoja");

                else if (fichaActiva.tag == "FichaVerde" && verdeAct == null) instanciaFlecha(casillaPulsada, "FichaVerde");

                else if (fichaActiva.tag == "FichaAzul" && azulAct == null) instanciaFlecha(casillaPulsada, "FichaAzul");

                else
                {

                        if (flechaActiva == rojaAct)
                            rojaAct.transform.position = casillaPulsada.transform.position;

                        else if (flechaActiva == verdeAct)
                            verdeAct.transform.position = casillaPulsada.transform.position;

                        else if (flechaActiva == azulAct)
                            azulAct.transform.position = casillaPulsada.transform.position;
                    
                }
            }

		} 
	
		else if (tablero [(int)casillaPulsada.GetComponent<Casilla> ().positionInMatrix.x, (int)casillaPulsada.GetComponent<Casilla> ().positionInMatrix.y] != 3) {
			switch (casillaPulsada.tag) {
			case "CasillaNormal":
				casillaPulsada.tag = "CasillaEmbarrada";
				tablero [(int)casillaPulsada.GetComponent<Casilla> ().positionInMatrix.x, (int)casillaPulsada.GetComponent<Casilla> ().positionInMatrix.y] = 1;
				casillaPulsada.GetComponent<SpriteRenderer> ().sprite = embarrada.GetComponent<SpriteRenderer> ().sprite;
				break;

			case "CasillaEmbarrada":
				casillaPulsada.tag = "CasillaBloqueada";
				tablero [(int)casillaPulsada.GetComponent<Casilla> ().positionInMatrix.x, (int)casillaPulsada.GetComponent<Casilla> ().positionInMatrix.y] = 2;
				casillaPulsada.GetComponent<SpriteRenderer> ().sprite = bloqueada.GetComponent<SpriteRenderer> ().sprite;
				break;

			case "CasillaBloqueada":
				casillaPulsada.tag = "CasillaNormal";
				tablero [(int)casillaPulsada.GetComponent<Casilla> ().positionInMatrix.x, (int)casillaPulsada.GetComponent<Casilla> ().positionInMatrix.y] = 0;
				casillaPulsada.GetComponent<SpriteRenderer> ().sprite = normal.GetComponent<SpriteRenderer> ().sprite;
				break;
			}
		}
	}

	void instanciaFichas(){
		int maxRange = tablero.GetLength(0);
		int randomIndex_X;
		int randomIndex_Y;

		for (int i = 0; i < 3; i++) {
			
			randomIndex_X = Random.Range(0, maxRange);
			randomIndex_Y = Random.Range(0, maxRange);

			while (tablero [randomIndex_X, randomIndex_Y] != 3) {
				
				switch (i) {

				case 0: //Ficha roja
					roja.GetComponent<Ficha> ().setPosMatriz (randomIndex_X, randomIndex_Y);
					roja.transform.position = new Vector2 (randomIndex_X, -randomIndex_Y);
					Instantiate (roja, this.transform);
					tablero [randomIndex_X, randomIndex_Y] = 3;
					break;

				case 1: //Ficha verde
					verde.GetComponent<Ficha> ().setPosMatriz (randomIndex_X, randomIndex_Y);
					verde.transform.position = new Vector2 (randomIndex_X, -randomIndex_Y);
					Instantiate (verde, this.transform);
					tablero [randomIndex_X, randomIndex_Y] = 3;
					break;

				case 2: //Ficha azul
					azul.GetComponent<Ficha> ().setPosMatriz (randomIndex_X, randomIndex_Y);
					azul.transform.position = new Vector2 (randomIndex_X, -randomIndex_Y);
					Instantiate (azul, this.transform);
					tablero [randomIndex_X, randomIndex_Y] = 3;
					break;
				}
			}
		}

	}

	public GameObject getFichaActiva(){
		return fichaActiva;
	}

	public GameObject getFlechaActiva(){
		return flechaActiva;
	}


    void creaTablero() {
		int y = 0; //Coordenadas de UNITY
		int cientoUno = tablero.GetLength(0) * tablero.GetLength(1);

		int randomIndex = Random.Range(0, cientoUno);

		for(int filas = 0; filas < tablero.GetLength(0); ++filas) {
			for(int columnas = 0; columnas < tablero.GetLength(1); ++columnas) {

				randomIndex = Random.Range(0, cientoUno);
				GameObject casilla;

				if (tablero[columnas, filas] == 3 || randomIndex < cientoUno - MaxNumCasillasEmbarradas - MaxNumCasillasBloqueadas){ //Casilla normal
					casilla = normal;
					casilla.GetComponent<Casilla> ().positionInMatrix.x = columnas;
					casilla.GetComponent<Casilla> ().positionInMatrix.y = filas;
					//Instantiate (casilla, new Vector2 (columnas, y), Quaternion.identity);
					casilla.transform.position = new Vector2 (columnas, y);
					Instantiate (casilla, this.transform);
					if (tablero [columnas, filas] != 3) {
						tablero [columnas, filas] = 0;
					}
				}
			

				else if (randomIndex >= cientoUno - MaxNumCasillasEmbarradas - MaxNumCasillasBloqueadas && randomIndex < cientoUno - MaxNumCasillasBloqueadas) { //Casilla embarrada
					if (numCasillasEmbarradas < 0) {
						casilla = normal;
						casilla.GetComponent<Casilla> ().positionInMatrix.x = columnas;
						casilla.GetComponent<Casilla> ().positionInMatrix.y = filas;
						//Instantiate (casilla, new Vector2 (columnas, y), Quaternion.identity);
						casilla.transform.position = new Vector2 (columnas, y);
						Instantiate (casilla, this.transform);
						tablero[columnas, filas] = 0;
					} 
					else {
						casilla = embarrada;
						casilla.GetComponent<Casilla> ().positionInMatrix.x = columnas;
						casilla.GetComponent<Casilla> ().positionInMatrix.y = filas;
						//Instantiate (casilla, new Vector2 (columnas, y), Quaternion.identity);
						casilla.transform.position = new Vector2 (columnas, y);
						Instantiate (casilla, this.transform);
						tablero[columnas, filas] = 1;
						numCasillasEmbarradas--;
					}
				}

				else { //Casilla bloqueada
					if (numCasillasBloqueadas < 0) {
						casilla = normal;
						casilla.GetComponent<Casilla> ().positionInMatrix.x = columnas;
						casilla.GetComponent<Casilla> ().positionInMatrix.y = filas;
						//Instantiate (casilla, new Vector2 (columnas, y), Quaternion.identity);
						casilla.transform.position = new Vector2 (columnas, y);
						Instantiate (casilla, this.transform);

						tablero[columnas, filas] = 0;
					} else {
						casilla = bloqueada;
						casilla.GetComponent<Casilla> ().positionInMatrix.x = columnas;
						casilla.GetComponent<Casilla> ().positionInMatrix.y = filas;
						//Instantiate (casilla, new Vector2 (columnas, y), Quaternion.identity);
						casilla.transform.position = new Vector2 (columnas, y);
						Instantiate (casilla, this.transform);

						tablero[columnas, filas] = 2;
						numCasillasBloqueadas--;
					}
				}
			}
			y = -filas -1 ;
		}

		Camera.main.orthographicSize = 10 * 0.55f;
	}


	public void setFichaActiva(GameObject FichaAct){
	//Si hay una ficha activa, la desactivamos y activamos la que nos viene
    //Si no, activamos directamente la que llega
		if (fichaActiva == null) {
			fichaActiva = FichaAct;
		} 
		else {

			//Se desactiva la que está activa
			fichaActiva.GetComponent<Ficha> ().activa = false;
			//fichaActiva.GetComponent<Ficha> ().cambiaEstrella (false);


			//Y se activa la que se ha clickado nueva
			fichaActiva = FichaAct;
			cambiaTexto ();

            if (fichaActiva.tag == "FichaRoja") flechaActiva = rojaAct;

            else if (fichaActiva.tag == "FichaVerde") flechaActiva = verdeAct;

            else if (fichaActiva.tag == "FichaAzul") flechaActiva = azulAct;


        }

	}
	//Cambia el texto que indica la ficha seleccionada
	void cambiaTexto(){
		switch (fichaActiva.tag) {

		case "FichaRoja":
			seleccion.color = new Color (1, 0, 0);
			seleccion.text = "Unicornio activo: Rojo";
			break;
		
		case "FichaVerde":
			seleccion.color = new Color (0, 1, 0);
			seleccion.text = "Unicornio activo: Verde";
			break;

		case "FichaAzul":
			seleccion.color = new Color (0, 0, 1);
			seleccion.text = "Unicornio activo: Azul";
			break;

		}
	}

    //Instancia las flechas pero sin darles una posición de la matriz
	void instanciaFlecha(GameObject casilla, string ficha){
		switch (ficha) {

		case "FichaRoja":
               
                    rojaAct = Instantiate(flechaRoja, casilla.transform);
                    flechaActiva = rojaAct;
               
                break;

		case "FichaVerde":

                    verdeAct = Instantiate(flechaVerde, casilla.transform);
                    flechaActiva = verdeAct;
                
                break;

		case "FichaAzul":

                    azulAct = Instantiate(flechaAzul, casilla.transform);
                    flechaActiva = azulAct;
              
                break;

		}

    }


    public void trazaCaminoAEstrella()
    {
        if (fichaActiva != null && flechaActiva != null) { 
            List<Vector2Int> caminoAEstrella = IAManager.GetComponent<IAManager>().CalculaCamino(tablero);

            Debug.Log(caminoAEstrella); //Debug del camino

            //Inicializo el estado antiguo
            int estadoAntiguo = fichaActiva.GetComponent<Ficha>().getTipoAntiguo();
            
            for (int i = 0; i < caminoAEstrella.Count; i++)
            {
                //Restituyo el valor antiguo
                Vector2Int aux = fichaActiva.GetComponent<Ficha>().getPosMatriz();
                tablero[aux.x, aux.y] = estadoAntiguo;
              
                estadoAntiguo = tablero[caminoAEstrella[i].x, caminoAEstrella[i].y];    //Guarda el estado de la siguiente casilla //¿-y?
                fichaActiva.GetComponent<Ficha>().mueveFicha(caminoAEstrella[i]);       // Mueve la casilla
                tablero[caminoAEstrella[i].x, caminoAEstrella[i].y] = 3;                //Pone el estado a 3
            }

            //Para que en la siguiente llamada a la misma ficha recuerde su valor.
            fichaActiva.GetComponent<Ficha>().setTipoAntiguo(estadoAntiguo);
        }
    }


}
