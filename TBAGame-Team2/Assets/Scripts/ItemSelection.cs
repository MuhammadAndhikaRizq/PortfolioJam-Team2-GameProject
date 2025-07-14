using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelection : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 offset;
    private Collider2D collider2d;

    public bool isDragging = false;

    void Awake()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        InputSystem();
        if (isDragging)
        {
            transform.position = MouseWorldPosition(Input.mousePosition) + offset;
        }
    }

    private void InputSystem()
    {
        if (Input.GetMouseButtonDown(0))
        {
       
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            isDragging = true;
            offset = transform.position - MouseWorldPosition(Input.mousePosition);
        }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            DropObject();
        }
    }

    protected virtual void DropObject()
    {
        isDragging = false;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.zero);

        // collider2d.enabled = true;
    }
    
    protected virtual Vector3 MouseWorldPosition(Vector3 inputPosition)
    {
        inputPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(inputPosition);
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}
