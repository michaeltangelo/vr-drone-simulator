using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public Camera ControlRoomCam;
    public Camera DroneFollowCam;


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

    void Awake()
    {
        mInstance = this;
        if (ControlRoomCam == null) ControlRoomCam = Camera.main;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
