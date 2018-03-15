using System.Collections;
using System.Collections.Generic;
using UnityEngine; //Para usar :MonoBehaviour y GameObject del Game Manager

/*
 * LA BIBLIA: buildnewgames.com/astar/
 * 
*/

//Enumerado de direcciones, que definen el movimiento que ha hecho el hueco desde el nodo padre
//para llegar a la matriz del nodo I-ésimo
public enum Movimiento {Arriba, Abajo, Izquierda, Derecha, No};


public class Nodo
{
	//Recibe un padre y recibe una posicion en el tablero. Y ya supongo que tambien el movimiento y tal
	public Nodo (int [,] tablrAux, Movimiento operador, Nodo n,  int c) 
	{
		tablero = tablrAux;
		coste = c;
		movRealizado = operador;
		padre = n;
	}

	public int coste;
	public int[,] tablero;
	public Movimiento movRealizado;
	public Nodo padre;
	//
	//Necesito un int f que sea, usando manhattan, el coste desde el PRINCIPIO a este nodo. Creo que sería nuestro "c"

	//Nececito un int g que sea, usando manhattan, el coste desde este NODO al FINAL. Que sería calcularlo cuando creamos nodos

	//Luego podríamos tener un int que sea la suma de esa mierda o algo por el estilo.
}


public class IAManager : MonoBehaviour {

	public GameObject GM; //GameManager
	GameObject fichaActiva;
	GameObject flechaActiva;

	Vector2 posIni, posFin;
	public Stack<Movimiento> movsSolucion;	//Stack de movimientos

	int[,] tablero;
	/*
	 * CASILLA VACIA = 0;
	 * CASILLA EMBARRADA = 1;
	 * CASILLA BLOQUEADA = 2;
     * CASILLA CON FICHA = 3;
	*/


	void preparacionIA () {
			
		movsSolucion = new Stack<Movimiento>();
		fichaActiva = GM.GetComponent<GameManager>().getFichaActiva();
		posIni = fichaActiva.GetComponent<Ficha>().getPosMatriz();  //Tomamos la posicion en la matriz de la ficha

		flechaActiva = GM.GetComponent<GameManager> ().getFlechaActiva ();
		posFin = flechaActiva.GetComponent<Flecha>().getPosMatriz(); //Tomamos la posición del final

		tablero = new int[10, 10];

		
	}

	//La distancia (Heuristica)
	//Quizá haya que hacer una distinción para aumentar el coste en uno si la casilla es embarrada?
	float distanciaManhattan(Vector2 Ahora, Vector2 Fin){
       
        return Mathf.Abs(Ahora.x - Fin.x) + Mathf.Abs(Ahora.y - Fin.y);
        
	}

	//Sirve para encontrar casillas adyacentes que no están bloqueadas alrededor :3
	//Es básicamente el modelo transición
	int[,] encuentraVecinos(int[,] tablero, Movimiento nuevoMov){

		int aux; //por si los swaps y tal


		int[,] nuevoTablero = new int[10, 10]; 
		igualaTablero(nuevoTablero, tablero);


		//Lo desplazamos en funcion del movimiento dado
		switch (nuevoMov)
		{
		case Movimiento.Derecha:
			//aux = nuevoTablero[filaEmpty, colEmpty + 1];
			//Comprobar si col + 1 > 10 -> fuera
			//Si no, comprobar si se puede pasar ahí. 
			//ENTONCES quizas pues no sé, hacer el swap y mover la ficha no tengo ni puta idea la verdad
			break;

		case Movimiento.Izquierda:
			//aux = nuevoTablero[filaEmpty, colEmpty - 1];

			break;

		case Movimiento.Arriba:
			//aux = nuevoTablero[filaEmpty - 1, colEmpty];

			break;

		case Movimiento.Abajo:
			//aux = nuevoTablero[filaEmpty + 1, colEmpty];

			break;
		}


		return nuevoTablero;

	}

	//Indica si la ficha puede desplazarse a la casilla de cordenadas (x,y)
	bool puedesMoverte(int x, int y)
	{
		//TRUE si puede moverse
		return tablero [x, y] != 2 /*bloqueda*/ && tablero [x, y] != 3 /*Ficha*/; 
	
	}


	//AQUI VA EL PATHFINDING :3
	public void BFS(int[,] tableroOrigen)
	{
		preparacionIA ();
		igualaTablero (tablero, tableroOrigen); //Clonamos de forma profunda el tablero

		//tableroIni = convierteMatrizGOaInt();


		//Creamos el nodo inicial
		Nodo raiz = new Nodo(tableroOrigen, Movimiento.No, null, 0);

		//if (raiz.tablero == solucion) Debug.Log("De puta madre, has llegado a la solucion");
	
		Queue<Nodo> frontera = new Queue<Nodo>();
		frontera.Enqueue(raiz);
		Debug.Log(frontera.Count + " ANtes del bucle");

	
		List<int[,]> explorado = new List<int[,]>(); //Lista de tableros explorados


		bool fin = false;
		while (!fin && frontera.Count != 0)
			{

				Nodo front = frontera.Dequeue();
				explorado.Add(front.tablero);

				int filaEmpty, colEmpty;
				filaEmpty = colEmpty = 0;
				//buscaEmpty(front.tablero, ref filaEmpty, ref colEmpty); //Buscamos el empty en la Matriz

				Debug.Log("Empty: " + filaEmpty + " " + colEmpty);

				//Para cada una de las direcciones posibles->
				for (int i = 0; i < 4; i++)
				{
					Movimiento nuevoMov = Movimiento.No;
					switch (i)
					{
					case 0: //ARRIBA
						nuevoMov = Movimiento.Arriba;
						break;

					case 1: //ABAJO
						nuevoMov = Movimiento.Abajo;
						break;

					case 2://Izquierda
						nuevoMov = Movimiento.Izquierda;
						break;

					case 3: //Derecha
						nuevoMov = Movimiento.Derecha;
						break;


					}

					//if (movimientoLegalIA(nuevoMov, filaEmpty, colEmpty))
					//{

						int[,] tableroHijo = new int[3, 3];
						//igualaTablero(tableroHijo, modeloTransicion(front.tablero, nuevoMov, filaEmpty, colEmpty));
						//Nodo hijo = new Nodo(tableroHijo, nuevoMov, front, front.coste + 1);

						//if (!frontera.Contains(hijo) && !containsEnLista(explorado, hijo.tablero))
						//{

							/*
							if (comparaTableros(hijo.tablero, solucion))
							{

								//Llama al método de solución
								DevuelveSolucion(hijo);
								fin = true;
							}

							else
							{
								frontera.Enqueue(hijo);

							}
							*/
						//}
					}

				}
			}
		

		bool containsEnLista(List<int[,]> lista, int[,] tablero)
		{
			int i = 0;
			bool contiene = false;
			while(!contiene && i < lista.Count)
			{
				if (comparaTableros(lista[i], tablero))
					contiene = true;
				i++;
			}

			return contiene;
		}
		
	//SOLAMENTE necesito comparar la posicion de la ficha con la posición del goal
		public bool comparaTableros(int [,] tablero, int[,] tableroFuente)
		{

			bool iguales = true;
			int i = 0;
			int j;
			while (iguales && i < 3)
			{
				j = 0;

				while (iguales && j < 3)
				{

					if (tablero[i, j] != tableroFuente[i, j])
						iguales = false;

					j++;
				}

				i++;
			}

			return iguales;
		}
		
	//Método auxiliar que permite hacer copias profundas
	void igualaTablero( int[,] tableroDestino, int[,] tableroFuente)
		{

			for(int i = 0; i < 3; i++)
			{
				for(int j = 0; j < 3; j++)
				{
					tableroDestino[i, j] = tableroFuente[i, j];
				}
			}

		}

		//Método recursivo que carga en una pila (LIFO) los movimientos realizados hasta la solucion
		void DevuelveSolucion(Nodo nodoSol){
			if(nodoSol.padre != null){
				movsSolucion.Push(nodoSol.movRealizado);
				Debug.Log(nodoSol.movRealizado);
				DevuelveSolucion (nodoSol.padre);
			}
		}


		
}
