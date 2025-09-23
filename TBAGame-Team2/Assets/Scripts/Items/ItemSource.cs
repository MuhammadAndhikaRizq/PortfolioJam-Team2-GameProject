using System.Collections;
using UnityEngine;

public class ItemSource : MonoBehaviour
{
    public GameObject objectPrefab;
    private float doubleClickTime = 0.3f;
    private float lastClickTime;

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < doubleClickTime)
            {
                return;
            }

            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            GameObject clone = Instantiate(objectPrefab, mouseWorld, Quaternion.identity);
            
            lastClickTime = Time.time;

        }
        
    }
    

    // IEnumerator WatitingClone()
    // {
    //     yield return new WaitForSeconds(1f);
    // }
}