using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Singleton class that manages selection of individual drone components and highlight controls for the
/// correspondent component in the cockpit
/// </summary>
public class DroneComponentSelector : MonoBehaviour {

    [SerializeField] private float m_rayLength = 500f;
    [SerializeField] private LayerMask m_ExclusionLayers;    // Layers to exclude from the raycast.
	[SerializeField] private Transform m_CameraTransform;
    private Camera m_Camera;
    public List<DroneComponentData> droneComponentData;
    [SerializeField] private Color selectionColor;
	public Vector3 CameraStartPos;

	[HideInInspector] public MonitorCtrl currMonitor = null;

//    DroneComponentData prevSelection;
    private DroneComponentData mCurSelection;
    public DroneComponentData CurrentSelection
    {
        get { return mCurSelection;  }
        set
        {
			Material mat;
			Color color;

			if(mCurSelection != null)
            {
                //remove markers from previous selection
				for(int i=0;i<mCurSelection.componentParts.Length;i++){
					mat = mCurSelection.componentParts[i].material;
					color = mat.GetColor("_RimColor");
					color.a = 0;
					mat.SetColor("_RimColor", color);
				}

				for(int i=0;i<mCurSelection.controllerParts.Length;i++){
					mat = mCurSelection.controllerParts[i].material;
					color = mat.GetColor("_OutlineColor");
					color.a = 0;
					mat.SetColor("_OutlineColor", color);
				}
            }

            mCurSelection = value;

            //add markers to current selection
			for(int i=0;i<mCurSelection.componentParts.Length;i++){
				mat = mCurSelection.componentParts[i].material;
				color = mat.GetColor("_RimColor");
				color.a = 1;
				mat.SetColor("_RimColor", color);
			}

			for(int i=0;i<mCurSelection.controllerParts.Length;i++){
				mat = mCurSelection.controllerParts[i].material;
				color = mat.GetColor("_OutlineColor");
				color.a = 1;
				mat.SetColor("_OutlineColor", color);
			}


            if(OnSelectionChanged != null)OnSelectionChanged();
        }
    }

    public delegate void SelectionChangeDelegate();
    public event SelectionChangeDelegate OnSelectionChanged;

    private RaycastHit hitInfo;
    private Ray detectionRay;

    private static DroneComponentSelector mInstance;
    public static DroneComponentSelector Instance
    {
        get
        {
            if (mInstance != null) return mInstance;
            else
            {
                GameObject target = GameObject.Find("Managers");
                if (target)
                {
                    mInstance = target.GetComponent<DroneComponentSelector>();
                    return mInstance;
                }
                else return null;
            }
        }
    }

    void Awake()
    {
        mInstance = this;
		if (m_CameraTransform == null){
			m_Camera = Camera.main;
			m_CameraTransform = Camera.main.transform;
		}
		CameraStartPos = m_CameraTransform.position;
    }
	
	// Update is called once per frame
//	void Update () {
        //if user clicks anywhere on screen
//        if (Input.GetMouseButtonDown(0))
//        {
//			DetectDroneComponent();
//        }

//	}

	public void DetectDroneComponent(){
//		detectionRay = m_Camera.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(m_CameraTransform.position,m_CameraTransform.forward, out hitInfo, m_rayLength, ~m_ExclusionLayers))
		{
			//Debug.Log("hit " + hitInfo.collider.name);
			MonitorCtrl monitorSelector = hitInfo.collider.GetComponent<MonitorCtrl>();
			if(monitorSelector != null)
			{
//				Debug.Log("hit monitor, tex coord: " + hitInfo.textureCoord + ", tex coord 2:" + hitInfo.textureCoord2);
				monitorSelector.RayCastDroneSelection(hitInfo.textureCoord); //monitor needs to have mesh collider
//				monitorSelector.RayCastDroneSelection(hitInfo.point);
			}
		}	
	}

    DroneComponentData FindComponentData(string componentName)
    {
        for(int i = 0; i < droneComponentData.Count; i++)
        {
            if (droneComponentData[i].nameID == componentName) return droneComponentData[i];
        }
        return null;
    }

    public void SetCurrSelection(string componentName)
    {
        DroneComponentData data = FindComponentData(componentName);
		if (data != null){
			CurrentSelection = data;
			Debug.Log("selection found: " + data.nameID);
		}
        else Debug.Log("selection not found");
    }
}

[System.Serializable]
public class DroneComponentData
{
    public string nameID;
	public Renderer[] componentParts;
	public Renderer[] controllerParts;
}
