using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private GameObject popupHolder;
    
    protected void TogglePopup(object data)
    {
        if (data is not bool) return;

        popupHolder.SetActive((bool)data);
    }
}