using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class MonitorCtrl : MonoBehaviour {

	enum PointerState{Idle, Entering, Exiting}
	enum ZoomState{ZoomedIn, Zooming, ZoomedOut}

    [SerializeField] private VRInteractiveItem mInteractivItem;
    [SerializeField] private Renderer monitorRenderer;
    [SerializeField] private float m_rayLength = 500f;
    [SerializeField] private LayerMask rayCastMask;
    [SerializeField] private float zoomDetectTime = 2f; //zooms in/out after user hovers over/away from screen for duration
//    [SerializeField] private Vector3 camZoomPos;


    private float zoomTimer = 0;
//    private Vector3 monitorPos;
//    bool zoomed = false;
//    bool exitingZoom = false;
	private PointerState curState = PointerState.Idle;
	private ZoomState curZoomState = ZoomState.ZoomedOut;

    void Awake()
    {
        mInteractivItem.OnOver += HandleOver;
//        monitorPos = monitorRenderer.transform.position;
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

		if(curZoomState != ZoomState.ZoomedIn) return;

        Camera targetCam = CameraManager.Instance.DroneFollowCam;
        Ray ray = targetCam.ScreenPointToRay(new Vector2(hitPos.x * targetCam.pixelWidth, hitPos.y * targetCam.pixelHeight));
        Debug.Log("hit position: " + hitPos + ", screen pixel dimensions:" + targetCam.pixelWidth + "," + targetCam.pixelHeight);
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
		if(curState != PointerState.Idle)
        {
            if(zoomTimer >= zoomDetectTime)
			{
				if(curState == PointerState.Entering) curZoomState = ZoomState.ZoomedIn;
				else curZoomState = ZoomState.ZoomedOut;
				curState = PointerState.Idle;
                zoomTimer = 0;
                
				if(curZoomState == ZoomState.ZoomedIn){
//					CameraManager.Instance.ControlRoomCam.transform.position = camZoomPos;
					CameraManager.Instance.ControlRoomRootTransform.position = transform.TransformPoint(0,0,-CameraManager.Instance.MonitorZoomZOffset);
					if(DroneComponentSelector.Instance.currMonitor != null) DroneComponentSelector.Instance.currMonitor.curZoomState = ZoomState.ZoomedOut;
					DroneComponentSelector.Instance.currMonitor = this;
					Debug.Log("Zoomed in");
				}
				else{
					//zoom out
//					CameraManager.Instance.ControlRoomCam.transform.position = DroneComponentSelector.Instance.CameraStartPos;
					CameraManager.Instance.ControlRoomRootTransform.position = DroneComponentSelector.Instance.CameraStartPos;
					DroneComponentSelector.Instance.currMonitor = null;
					Debug.Log("Zoomed out");
				}
            }
            zoomTimer += Time.deltaTime;
        }
    }

    public void OnHoverIn()
    {
		Debug.Log("pointer entering");
		if(curZoomState == ZoomState.ZoomedOut){
			//check the zoom state of the current monitor
//			if(DroneComponentSelector.Instance.currMonitor != null){
//				DroneComponentSelector.Instance.currMonitor
//			}
			curZoomState = ZoomState.Zooming;
			zoomTimer = 0;
			curState = PointerState.Entering;
			Debug.Log("zooming in");
		}
		else{
			curZoomState = ZoomState.ZoomedIn;
			curState = PointerState.Idle;
		}
    }

	public void DetectDroneComponent(){
		DroneComponentSelector.Instance.DetectDroneComponent();
	}

//	    void OnMouseOver()
//    {
//        if (zoomed) return;
//
//        if(zoomTimer >= zoomDetectTime)
//        {
//            zoomed = true;
//            //zoom in the camera
//            CameraManager.Instance.ControlRoomCam.transform.position = camZoomPos;
//            Debug.Log("Zooming in");
//        }
//        zoomTimer += Time.deltaTime;
//    }
//
    public void OnHoverOut()
    {
		Debug.Log("pointer exiting");
		if(curZoomState == ZoomState.ZoomedIn){
			curZoomState = ZoomState.Zooming;
			zoomTimer = 0;
			curState = PointerState.Exiting;
			Debug.Log("Zooming out");
		}
		else{
			curZoomState = ZoomState.ZoomedOut;
			curState = PointerState.Idle;
		}
    }

    //void OnMouseDown()
    //{
    //    Debug.Log("mouse position + " + Input.mousePosition);
    //}
}
