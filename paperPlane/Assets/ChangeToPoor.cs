using UnityEngine;
using System.Collections;
using BellaProject;

public class ChangeToPoor : MonoBehaviour
{
	public void OnButtonPress ()
	{
		GameObject.Find ("Global").GetComponent<global> ().skinname = "plane_poor";
		Application.LoadLevel ("BellaScene");
	}
}
