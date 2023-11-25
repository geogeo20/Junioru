using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour
{
    [SerializeField] private GameObject screenHolder;

    public int ID { get; protected set; }

    protected virtual void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        UIManager.Instance.AddScreen(this);
    }

    public void ToggleScreen(bool toggle)
    {
        screenHolder.SetActive(toggle);
    }
}