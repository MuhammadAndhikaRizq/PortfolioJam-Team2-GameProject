using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHoverScale : MonoBehaviour
{
    private Vector3 originalScale;
    private Vector3 targetScale;

    public float hoverlScaleFactor = 5f;
    public float hoverSpeed = 5;

    private bool isHovered = false;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void OnMouseEnter()
    {
        isHovered = true;
        targetScale = originalScale * hoverlScaleFactor;    
    }

    void OnMouseExit()
    {
        isHovered = false;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * hoverSpeed);
    }
}
