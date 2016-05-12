using UnityEngine;
using System.Collections;

public class DroneFollowCamRaycaster : MonoBehaviour {


    [SerializeField]
    private float m_rayLength = 500f;
    [SerializeField]
    private LayerMask m_ExclusionLayers;    // Layers to exclude from the raycast.
    [SerializeField]
    private Camera m_Camera;

    private RaycastHit hitInfo;
    private Ray detectionRay;

    void Update()
    {
        //if user clicks anywhere on screen
        if (Input.GetMouseButtonDown(0))
        {
            detectionRay = m_Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(detectionRay, out hitInfo, m_rayLength, ~m_ExclusionLayers))
            {
                Debug.Log("hit " + hitInfo.collider.name);
            }
        }

    }
}
