using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BellaProject;
using Bindings;

namespace Bindings

{
	[RequireComponent(typeof(Text))]
public class PointAdd : MonoBehaviour {

	public GameObject airplaneGameObject;
		private AirplaneController script;

		Text label;
		float _scoreAdd, timer;
	// Use this for initialization
	void Start () {
			
			label = GetComponent<Text> ();

			script = airplaneGameObject.GetComponent<AirplaneController> ();
			_scoreAdd = script.scoreAdd;
	}
	
	// Update is called once per frame
	void Update () {
			timer += Time.deltaTime;
			if (script.status) {
								_scoreAdd = script.scoreAdd;
								label.text = "$" + ((int)_scoreAdd).ToString ();
								script.status = false;
						}
			if (timer >= 2f) {
								label.text = "";
				timer = 0f;
						}

			transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + 2f, 0f);

	
	}
}
}
