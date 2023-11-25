using UnityEngine;

namespace RedCock.Utils
{
	public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
	{
		public static T Instance { get; protected set; }

		public static bool InstanceExists
		{
			get { return Instance != null; }
		}

		protected virtual void Awake()
		{
			if (InstanceExists)
			{
				if (Application.isPlaying)
				{
					Destroy(gameObject);
				}
				else
				{
					DestroyImmediate(gameObject);
				}
			}
			else
			{
				Instance = (T)this;
			}
			DontDestroyOnLoad(this);
		}

		protected virtual void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}
	}
}