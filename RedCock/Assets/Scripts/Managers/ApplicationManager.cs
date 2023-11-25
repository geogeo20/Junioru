using RedCock.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationManager : SingletonBehaviour<ApplicationManager>, IManager
{
    public string Name => "Application Manager";

    public Action OnInitialized { get; set; }

    [SerializeField] private List<GameObject> managers;

	private Transform managerHolder;


	private void Start()
    {
        Application.targetFrameRate = 165;
        Init();
    }

    public Task Init()
    {
        _ = InitManagers();
		OnInitialized?.Invoke();
		OnInitialized = null;
		return null;
    }

	/// <summary>
	/// Create all managers
	/// </summary>
	/// <param name="onDone"></param>
	public async Task InitManagers()
	{
		foreach (var manager in managers)
		{
			if (manager == null)
			{
				continue;
			}
			await InitManager(manager);
		}
	}

	/// <summary>
	/// Instantiate a new manager
	/// </summary>
	/// <param name="manager"></param>
	public async Task InitManager(GameObject manager)
	{
		Task initTask = manager.GetComponent<IManager>().Init();

		if (initTask != null)
        {
			await initTask;
        }
		Debug.LogFormat("Manager initialized {0}", manager.name);
	}
}