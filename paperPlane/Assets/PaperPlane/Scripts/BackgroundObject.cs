using UnityEngine;
using System.Collections;

public class BackgroundObject : MonoBehaviour {

	private int speed;
	private float ySpeed = 0.4f;
	private float offset;

	public GameObject airplane;
	// Use this for initialization
	void Start () {
		speed = Random.Range (2, 5);
		offset = Random.Range (0, 2 * Mathf.PI);
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localPosition.x < -620f) {
			transform.localPosition = new Vector3(660f, transform.localPosition.y);
		}
		transform.Translate ((float)-speed * Time.deltaTime, Mathf.Sin (Time.time + offset) * ySpeed * Time.deltaTime, 0f);
	}
}
