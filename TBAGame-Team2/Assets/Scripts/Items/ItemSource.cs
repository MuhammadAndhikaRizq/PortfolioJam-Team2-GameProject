using UnityEngine;

public class ItemSource : MonoBehaviour
{
    public GameObject objectPrefab;

    void OnMouseDown()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        GameObject clone = Instantiate(objectPrefab, mouseWorld, Quaternion.identity);
    }
}