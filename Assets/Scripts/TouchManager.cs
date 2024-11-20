using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{

    private GameObject selectedBuilding;

    private void Update()
    {
        HandleBuildingDrag();
    }

    private void HandleBuildingDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Building"))
                {
                    selectedBuilding = hit.transform.gameObject;
                    Debug.Log(selectedBuilding.name);
                }
            }
        }
    }
}
