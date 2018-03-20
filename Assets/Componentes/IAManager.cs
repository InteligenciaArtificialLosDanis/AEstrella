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
	public Nodo (Nodo p, int c, Vector2Int posFicha) 
	{
		coste = c;
        
		//
        padre = p;
		x = posFicha.x;
		y = posFicha.y;

		valor = x + (y * 10);

	}

	public int coste;
	public int[,] tablero;
	public Movimiento movRealizado;
	public Nodo padre;

	public int valor; //Point.x + (Point.y*10);
	public int x, y;

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

	Vector2Int posIni, posFin;
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
	List<Nodo>encuentraVecinos(Nodo actual){

        int coste = 0;

		// Direcciones //
		int norte = actual.y - 1;
		int sur = actual.x + 1;
		int este = actual.x + 1;
		int oeste = actual.x - 1;

        //Movimientos//
        Vector2Int n = new Vector2Int(actual.x, norte); //norte
        Vector2Int s = new Vector2Int(actual.x, sur); //sur
        Vector2Int e = new Vector2Int(este, actual.y); //este
        Vector2Int o = new Vector2Int(oeste, actual.y); //oeste

		// Comprobaciones //
		bool movimientoNorte = norte > -1 && puedesMoverte(actual.x, norte);
		bool movimientoSur = sur < 10 && puedesMoverte (actual.x, sur);
		bool movimientoEste = este < 10 && puedesMoverte (este, actual.y);
		bool movimientoOeste = oeste > -1 && puedesMoverte (oeste, actual.y);

		List<Nodo> resultado = new List<Nodo>();

		if (movimientoNorte){

            if (tablero[actual.x, norte] == 1) coste = 2;
            else coste = 1;

			resultado [0] = new Nodo(actual, coste, n);
        }

        if (movimientoSur)
        {

            if (tablero[actual.x, sur] == 1) coste = 2;
            else coste = 1;

            resultado[0] = new Nodo(actual, coste, s);
        }

        if (movimientoEste)
        {

            if (tablero[este, actual.y] == 1) coste = 2;
            else coste = 1;

            resultado[0] = new Nodo(actual, coste, e);
        }

        if (movimientoOeste)
        {

            if (tablero[oeste, actual.y] == 1) coste = 2;
            else coste = 1;

            resultado[0] = new Nodo(actual, coste, o);
        }

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
		Nodo origen = new Nodo(null, 0, posIni);
		Nodo final = new Nodo (null, 0, posFin);

		//Lista de nodos abiertos
		List <Nodo> nodosAbiertos = new List<Nodo>();
		nodosAbiertos.Add(origen); 
		//Lista de nodos cerrados (No vas a volver a tocar)
		List <Nodo> nodosCerrados = new List<Nodo>();

        //Lista de nodos vecinos
        List<Nodo> nodosVecinos = new List<Nodo>();


        //Lista de posiciones elegidos para seguir el camino más corto
        List<Vector2> resultado = new List<Vector2>();

		//Nodo cercano
		Nodo nodoVecino;

		//Nodo actual (el que consideramos en esta iteracion)
		Nodo nodoActual;

		//Nodo Camino: Referecia a un nodo que empieza un camino en cuestión
		Nodo nodoCamino;

        //Posición de cada nodoCamino a la hora de pasarlo al resultado
        Vector2 posNodo;

		//Variables que usaremos en los cálculos
		int max, min, j;
		bool fin = false;

		//BUCLE PRINCIPAL
		while (nodosAbiertos.Count > 0 && !fin) {
			max = 10;
			min = -1;

			for (int i = 0; i < nodosAbiertos.Count; i++) {
				if(nodosAbiertos[i].getF()< max)
				{
					max = nodosAbiertos[i].getF();
					min = i;
				}
			}

            nodoActual = nodosAbiertos[0];
            nodosAbiertos.Remove(nodosAbiertos[0]);

            //¿Es la posición de destino?
            if (nodoActual.valor == final.valor)
            {
                nodosCerrados.Add(nodoActual);

                nodoCamino = nodosCerrados[nodosCerrados.Count -1];

                do
                {
                    //Se añade al resultado cada posición por la que pasa el nodoCamino
                    posNodo = new Vector2(nodoCamino.x, nodoCamino.y);
                    resultado.Add(posNodo);

                    //nodoCamino se iguala a su padre para ir desandando el camino hacia atrás y guardarlo en el resultado
                    nodoCamino = nodoCamino.padre;
                    

                }

                while (nodoCamino != null);

                //Limpiamos las listas
                nodosAbiertos.Clear();
                nodosCerrados.Clear();

                //Damos la vuelta al resultado para que se realice desde el origen al final (se guardó desandando desde el origen al final)
                resultado.Reverse();

            }

            else //No ha llegado al destino
            {
                //Buscamos por qué posiciones vecinas se puede pasar (No bloqueadas o con ficha)
                nodosVecinos = encuentraVecinos(nodoActual);
                j = nodosVecinos.Count;

                //Comprobamos cada vecino
                for (int i = 0; i < j; i++)
                {
                    nodoVecino = nodosVecinos[i];
                    posNodo = new Vector2Int(nodoVecino.x, nodoVecino.y);
                    //nodoCamino = new Nodo(nodoActual, nodoVecino.coste, posNodo);

                    //Falta el if
                }

                nodosCerrados.Add(nodoActual);

            }
		
		}

            //return resultado;

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
