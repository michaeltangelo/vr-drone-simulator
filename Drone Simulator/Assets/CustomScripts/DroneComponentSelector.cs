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
    [SerializeField] private Camera m_Camera;
    public List<DroneComponentData> droneComponentData;
    [SerializeField] private Color selectionColor;

    DroneComponentData prevSelection;
    private DroneComponentData mCurSelection;
    public DroneComponentData CurrentSelection
    {
        get { return mCurSelection;  }
        set
        {
            if(prevSelection != null)
            {
                //remove markers from previous selection
            }

            mCurSelection = value;

            //add markers to current selection

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
        if (m_Camera == null) m_Camera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        //if user clicks anywhere on screen
        if (Input.GetMouseButtonDown(0))
        {
            detectionRay = m_Camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(detectionRay, out hitInfo, m_rayLength, ~m_ExclusionLayers))
            {
                //Debug.Log("hit " + hitInfo.collider.name);
                MonitorCtrl monitorSelector = hitInfo.collider.GetComponent<MonitorCtrl>();
                if(monitorSelector != null)
                {
                    Debug.Log("hit monitor");
                    monitorSelector.RayCastDroneSelection(hitInfo.textureCoord);
                }
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
        if (data != null) CurrentSelection = data;
        else Debug.Log("selection not found");
    }
}

[System.Serializable]
public class DroneComponentData
{
    public string nameID;
    public GameObject[] componentParts;
    public GameObject[] controllerParts;
}
