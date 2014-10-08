using UnityEngine;
using System.Collections;
using BellaProject;

public class ChangeToBest : MonoBehaviour
{
	public void OnButtonPress ()
	{
		GameObject.Find ("Global").GetComponent<global> ().skinname = "plane_best";
		Application.LoadLevel ("BellaScene");
	}
}
