using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Vehicles.Aeroplane;

public class Plane_Info : MonoBehaviour {

    TextMesh text;

	// Use this for initialization
	void Start () {
        text = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        //GameObject AircraftPropeller = GameObject.Find("AircraftPropeller");
        //AeroplaneController script = AircraftPropeller.GetComponent <AeroplaneController> ();
        //string throttle = (script.Throttle * 100).ToString("0");
        //text.text = "Throttle: " + throttle;

        GameObject AircraftPropeller = GameObject.Find("AircraftPropeller");
        AeroplaneUserControl4Axis script = AircraftPropeller.GetComponent<AeroplaneUserControl4Axis>();
        bool pilotEnabled = script.getPilotMode();
        string opmode;
        if (pilotEnabled) opmode = "Pilot";
        else opmode = "Sensor";
        text.text = opmode + "Operating Mode Activated"; 
	}
}
