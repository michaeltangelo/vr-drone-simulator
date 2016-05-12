using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public Transform ControlRoomRootTransform;
    public Camera ControlRoomCam;
    public Camera DroneFollowCam;

	public float MonitorZoomZOffset = 10;

    private static CameraManager mInstance;
    public static CameraManager Instance
    {
        get
        {
            if (mInstance != null) return mInstance;
            else
            {
                GameObject target = GameObject.Find("Managers");
                if (target)
                {
                    mInstance = target.GetComponent<CameraManager>();
                    return mInstance;
                }
                else return null;
            }
        }
    }

	enum GazeState{Idle, Entering, Exiting}
	enum ZoomState{ZoomedIn, Zooming, ZoomedOut}

	private GazeState curState = GazeState.Idle;
	private ZoomState curZoomState = ZoomState.ZoomedOut;

	[SerializeField] private float zoomDetectTime = 2f; //zooms in/out after user hovers over/away from screen for duration

    void Awake()
    {
        mInstance = this;
        if (ControlRoomCam == null) ControlRoomCam = Camera.main;
    }

//    // Use this for initialization
//    void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}
