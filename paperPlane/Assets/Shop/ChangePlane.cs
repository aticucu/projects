using UnityEngine;
using System.Collections;
//using BellaProject;

public class ChangePlaneButton : MonoBehaviour
{
	public void OnButtonPress ()
	{
		Debug.Log ("Plane_poor");
		GameObject.Find ("Global").GetComponent<global> ().skinname = "plane_poor";
	}
}
