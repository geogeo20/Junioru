using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public RectTransform objectToMove;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(Input.mousePosition);
        mousePos.z = 0;
        objectToMove.position = mousePos;
    }
}
