using UnityEngine;
using System.Collections;
using BellaProject;

public class ChangeToGood : MonoBehaviour
{
	public void OnButtonPress ()
	{
		GameObject.Find ("Global").GetComponent<global> ().skinname = "plane_good";
		Application.LoadLevel ("BellaScene");
	}
}
