using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 
/// </summary>

public class IaManager : MonoBehaviour {


	public int MaxThreads
	{
		get { return _maxThreads; }
		private set
		{
			_maxThreads = value; //Igual al valor dado. Realmente el set es privado??
		}
	}

	public IActionQueue UpdateQueue { get; private set; } // executes at next update
	public IActionQueue ThreadPooledQueue { get; private set; } //Cola de trabajo, que se ejecuta cuando alguna hebra este disponible

	private static ThreadSafeQueue _debugQueue = null; //Cola de tipo "Safe"
	private int _maxThreads; //Relativo a la definicion de su "clase int" dada por MaxThreads. Creo que esto es por hacerse el chulo. (Realmente es para proteger variables)

	void Awake()
	{
		_debugQueue = _debugQueue ?? new ThreadSafeQueue();
		MaxThreads = SystemInfo.processorCount > 1 ? SystemInfo.processorCount * 2 : 1;
		UpdateQueue = UpdateQueue ?? new ThreadSafeQueue();
		ThreadPooledQueue = ThreadPooledQueue ?? new ThreadPoolQueue();
	}

	//INTERFAZ: Esta interfaz implementa las acciones de meter y sacar elementos en una cola de Acciones.
	//La idea de esto es que la cola por sí misma puede no saber cómo trabajar con Acciones, y porque queremos darle un if al enqueue (lock)
	public interface IActionQueue
	{
		void Enqueue(Action action);
		Action Dequeue(); //Devuelve la acción del Front
	}



	void Update()
	{
		Action action; 
		while ((action = _debugQueue.Dequeue()) != null) //Cuando queden acciones 
		{
			action();
		}
		while ((action = UpdateQueue.Dequeue()) != null)
		{
			action();
		}
	}

	//ESTO ES LA COLA COMO TAL
	private class ThreadSafeQueue : IActionQueue
	{
		private System.Object _lock; 				//Lock del sistema?
		private Queue<Action> _queuedActions;		//Cola de acciones, interna

		public ThreadSafeQueue()					//Constructora: Creo una cola de acciones y un lock del sistema
		{
			_queuedActions = new Queue<Action>();
			_lock = new System.Object();
		}

		public Action Dequeue()						//Dequeue de acciones: Si está el lock activo, y hay acciones entonces las devuelvo
													//Si no, devuelvo true.
		{
			lock (_lock)
			{
				if (_queuedActions.Count > 0)
				{
					return _queuedActions.Dequeue();
				}
				return null;
			}
		}

		public void Enqueue(Action action)			//Pone una acción en cola, si el lock del sistema está activo
		{
			lock (_lock)
			{
				_queuedActions.Enqueue(action);
			}
		}
	}

	//COSAS QUE NO SÉ QUE COJONES SON//

	//Esto se usa para acciones asíncronas.
	static void RunAsync(Action action)
	{
		ThreadPool.QueueUserWorkItem((object state) => //Y este flechote?
			{
				try
				{
					action();
				}
				catch (Exception e)
				{
					_debugQueue.Enqueue(delegate ()
						{
							Debug.LogError("Thread Pool Exception");
							Debug.LogException(e);
						});
				}
			});
	}
	//
	private class ThreadPoolQueue : IActionQueue
	{
		public void Enqueue(Action action)
		{
			RunAsync(action);
		}

		public Action Dequeue()
		{
			return null;
		}
	}
}

