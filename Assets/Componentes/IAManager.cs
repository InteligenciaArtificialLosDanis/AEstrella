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
	public Nodo (Movimiento operador, Nodo n,  int c, Vector2 posFicha) 
	{
		coste = c;
		movRealizado = operador;
		padre = n;

		//
		x = posFicha.x;
		y = posFicha.y;

		valor = x + (y * 10); //¿Nani es esto?

	}

	public int coste;
	public int[,] tablero;
	public Movimiento movRealizado;
	public Nodo padre;

	int valor; //Point.x + (Point.y*10);
	int x, y;

	int f;	   // el coste desde el PRINCIPIO al nodo final. Que sería calcularlo cuando creamos nodos
	int g;    // el coste desde el PRINCIPIO a este nodo. 

	//Luego podríamos tener un int que sea la suma de esa mierda o algo por el estilo.

	public void setf(int newf){
		f = newf;
	}

	public void setg(int newg){
		g = newg;
	}

	public int getF(){
		return f;
	}

	public int getG(){
		return g;
	}


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
	//x e y son las posiciones de la ficha en el caso n-ésimo
	bool [] encuentraVecinos(int x, int y){

		// Direcciones // ¿?
		int norte = y - 1;
		int sur = y + 1;
		int este = x + 1;
		int oeste = x - 1;

		// Comprobaciones //
		bool movimientoNorte = norte > -1 && puedesMoverte(x, norte);
		bool movimientoSur = sur < 10 && puedesMoverte (x, sur);
		bool movimientoEste = este < 10 && puedesMoverte (este, y);
		bool movimientoOeste = oeste > -1 && puedesMoverte (oeste, y);

		bool[] resultado = new bool[4](false);

		if (movimientoNorte)
			resultado [0] = true;

		if (movimientoSur)
			resultado [1] = true;

		if (movimientoEste)
			resultado [2] = true;

		if (movimientoOeste)
			resultado [3] = true;

		return resultado;
		
	}

	//Indica si la ficha puede desplazarse a la casilla de cordenadas (x,y)
	bool puedesMoverte(int x, int y)
	{
		//TRUE si puede moverse
		return tablero [x, y] != 2 /*bloqueda*/ && tablero [x, y] != 3 /*Ficha*/; 
	
	}

	void CalculaCamino(int[,] tableroOrigen){

		preparacionIA ();
		igualaTablero (tablero, tableroOrigen); //Clonamos de forma profunda el tablero

		//Creamos nodos del inicio y del final
		Nodo origen = new Nodo(Movimiento.No,null,0,posIni);
		//Nodo final = new Nodo (Movimiento.No, null, 10000, posFin); //¿Quitar el coste del nodo? ¿Movimiento del final?

		//Lista de nodos abiertos
		List <Nodo> nodosAbiertos = new List<Nodo>();
		nodosAbiertos.Add(origen); 
		//Lista de nodos cerrados (No vas a volver a tocar)
		List <Nodo> nodosCerrados = new List<Nodo>();

		//Nodo cercano
		Nodo nodoVecino;
		//Nodo actual (el que consideramos en esta iteracion)
		Nodo nodoActual;

		//Nodo Camino: Referecia a un nodo que empieza un camino en cuestión
		Nodo nodoCamino;

		//Variables que usaremos en los cálculos
		int max, min, j;
		bool fin = false;

		//BUCLE PRINCIPAL
		while (nodosAbiertos.Count > 0 && !fin) {
			max = 10;
			min = -1;

			for (int i = 0; i < nodosAbiertos.Count; i++) {
				if(nodosAbiertos[i]< max)
				{
					max = nodosAbiertos[i].getF();
					min = i;
				}
			}
		
		}

	}

	//AQUI VA EL PATHFINDING :3
	public void BFS(int[,] tableroOrigen)
	{
		
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
