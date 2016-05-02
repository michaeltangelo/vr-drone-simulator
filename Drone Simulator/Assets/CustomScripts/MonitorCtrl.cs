using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class MonitorCtrl : MonoBehaviour {

    [SerializeField] private VRInteractiveItem mInteractivItem;
    [SerializeField] private Renderer monitorRenderer;
    [SerializeField] private float m_rayLength = 500f;
    [SerializeField] private LayerMask rayCastMask;
    [SerializeField] private float zoomDetectTime = 2f; //zooms in/out after user hovers over/away from screen for duration
    [SerializeField] private Vector3 camZoomPos;


    private float zoomTimer = 0;
    private Vector3 monitorPos;
    bool zoomed = false;
    bool exitingZoom = false;

    void Awake()
    {
        mInteractivItem.OnOver += HandleOver;
        monitorPos = monitorRenderer.transform.position;
    }


    private void HandleOver()
    {
        Debug.Log("Is over flight screen");
    }

    // Use this for initialization
    void Start()
    {

    }

    public void RayCastDroneSelection(Vector3 hitPos)
    {
        //Vector3 center = monitorRenderer.bounds.center;
        //Vector3 extents = monitorRenderer.bounds.extents;
        //Vector3 remappedVPPos = Vector3.zero;
        //remappedVPPos.x = hitPos.x.Remap(center.x - extents.x, center.x + extents.x, -1, 1);
        //remappedVPPos.y = hitPos.y.Remap(center.y - extents.y, center.y + extents.y, -1, 1);
        //Debug.Log("Remapped viewport pos: " + remappedVPPos);
        //Ray ray = CameraManager.Instance.DroneFollowCam.ViewportPointToRay(remappedVPPos);

        if (!zoomed) return; //can only select while camera is zoomed
        Camera targetCam = CameraManager.Instance.DroneFollowCam;
        Ray ray = targetCam.ScreenPointToRay(new Vector2(hitPos.x * targetCam.pixelWidth, hitPos.y * targetCam.pixelHeight));
        //Debug.Log("hit position: " + hitPos + ", screen pixel dimensions:" + targetCam.pixelWidth + "," + targetCam.pixelHeight);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, m_rayLength, rayCastMask))
        {
            Debug.Log("hit " + hitInfo.collider.name);
            DroneComponentInfo info = hitInfo.collider.GetComponent<DroneComponentInfo>();
            if (info != null) DroneComponentSelector.Instance.SetCurrSelection(info.componentName);
        }
    }

    void Update()
    {
        if(zoomed && exitingZoom)
        {
            if(zoomTimer >= zoomDetectTime)
            {
                zoomed = false;
                exitingZoom = false;
                zoomTimer = 0;
                //zoom out
                CameraManager.Instance.ControlRoomCam.transform.position = Vector3.zero;
                Debug.Log("Zooming out");
            }
            zoomTimer += Time.deltaTime;
        }
    }

    void OnMouseEnter()
    {
        zoomTimer = 0;
        exitingZoom = false;
    }

    void OnMouseOver()
    {
        if (zoomed) return;

        if(zoomTimer >= zoomDetectTime)
        {
            zoomed = true;
            //zoom in the camera
            CameraManager.Instance.ControlRoomCam.transform.position = camZoomPos;
            Debug.Log("Zooming in");
        }
        zoomTimer += Time.deltaTime;
    }

    void OnMouseExit()
    {
        zoomTimer = 0;
        exitingZoom = true;
    }

    //void OnMouseDown()
    //{
    //    Debug.Log("mouse position + " + Input.mousePosition);
    //}
}
