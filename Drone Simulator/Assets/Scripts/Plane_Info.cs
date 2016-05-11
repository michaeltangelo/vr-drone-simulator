using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Vehicles.Aeroplane;

public class Plane_Info : MonoBehaviour {

    Text instruction;
	// Use this for initialization
	void Start () {
        instruction = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        GameObject AircraftPropeller = GameObject.Find("AircraftPropeller");
        AeroplaneController script = AircraftPropeller.GetComponent <AeroplaneController> ();
        string throttle = (script.Throttle * 100).ToString("0");
        instruction.text = "Throttle: " + throttle;
	}
}
