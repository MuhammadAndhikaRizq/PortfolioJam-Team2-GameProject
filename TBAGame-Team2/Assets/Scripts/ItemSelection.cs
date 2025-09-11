using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelection : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 offset;

    public bool isDragging = false;

    void Awake()
    {
        startPosition = transform.position;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log("Collision with " + other.gameObject.name); 
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
        if (DialogueManager.isDialogueActive) return; // Disable drag saat Fungus aktif

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
    }
    
    protected virtual Vector3 MouseWorldPosition(Vector3 inputPosition)
    {
        inputPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(inputPosition);
    }

    public void ResetPosition()
    {
        isDragging = false;
        transform.position = startPosition;
    }
}
