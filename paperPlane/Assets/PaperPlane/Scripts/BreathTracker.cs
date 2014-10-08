using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BellaProject;
using Bindings;

namespace Bindings{

public class BreathTracker : MonoBehaviour {
	
	public AirplaneController airplaneController;
	
	[SerializeField]
	private List<string>
		bindings = new List<string> ();
	
	private List<LabelProperty> properties = new List<LabelProperty> ();

		private int breathCount;
	
	System.Object[] values;

	public LabelProperty breathCountProperty;
	public LabelProperty breathsProperty;

	public LabelProperty GetProperty (int index)
	{
		if (index >= 0 && index < properties.Count) {
			return properties [index];
		}
		return null;
	}
	
	// Use this for initialization
		void Start () {
			breathCount = 0;
			breathCountProperty = new LabelProperty (bindings[0]);
			breathsProperty = new LabelProperty (bindings[1]);
			breathCountProperty.AddListener (OnValueChange);
			properties.Add (breathCountProperty);
			properties.Add (breathsProperty);
			OnValueChange (null);
		}
	
		
		void OnValueChange (System.Object value)
		{
			//add highscore add logic
			this.breathCount = (int) breathCountProperty.value;
			if (!airplaneController.status)
								airplaneController.status = true;
			if (this.breathCount == (int) breathsProperty.value) {

				airplaneController.StartLanding();
				airplaneController.CreateLandingPlatform();
			
			}
		}

	
	}
}
