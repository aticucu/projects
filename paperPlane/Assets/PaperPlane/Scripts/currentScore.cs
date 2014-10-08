using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BellaProject;
using Bindings;

namespace Bindings
	
{
	[RequireComponent(typeof(Text))]
	public class currentScore : MonoBehaviour {

		public GameObject airplaneGameObject;
		private AirplaneController script;

		Text label;
		float _score, addScore, height, width;

//		ScoreKeeping scoreKeeping;
		// Use this for initialization
		void Start () {

			script = airplaneGameObject.GetComponent<AirplaneController> ();
			label = GetComponent<Text> ();
			_score = script.score;

			Camera cam = Camera.main;
			this.height = 2f * cam.orthographicSize;
			this.width = height * cam.aspect;
			
		}
		
		// Update is called once per frame
		void Update () {
			checkUp ();
			label.text = "$" + ((int)_score).ToString ();

			transform.position = new Vector3(Camera.main.transform.position.x + (width / 2f) *0.9f, Camera.main.transform.position.y + (height / 2f) * .9f, 0f);
			
			
		}
		
		void checkUp() {
			_score = script.score;
		}
	}
}
