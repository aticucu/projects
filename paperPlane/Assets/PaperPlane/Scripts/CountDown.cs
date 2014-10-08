using UnityEngine;
using System.Collections;
using BellaProject;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
	private AirplaneController airplaneController;
	public float TimeUntilTurnOffDisplay;
	Text DisplayScreen;
	
	void InitializeGameObjects ()
	{
		airplaneController = GameObject.Find ("Plane").GetComponent<AirplaneController> ();
	}
	
	// Use this for initialization
	void Start ()
	{
		InitializeGameObjects ();
		DisplayScreen = GetComponent<Text> ();
		TimeUntilTurnOffDisplay = 5f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		TimeUntilTurnOffDisplay -= Time.deltaTime;
		int InttimeLeftUntilStart = (int)airplaneController.timeLeftUntilStart;
		
		if (TimeUntilTurnOffDisplay < 0) {
			DisplayScreen.text = "";
		} else if (airplaneController.timeLeftUntilStart >= 0) {
			DisplayScreen.text = "Starting in: " + InttimeLeftUntilStart.ToString ("G");
		} else {
			DisplayScreen.text = "Start!";
		}

		if (this.airplaneController.IsLanded()) {
			DisplayScreen.text = "We have landed Safely! Going to shop in: " + (int) airplaneController.timeLeftUntilToShop;
		}


	}
	
	
}
