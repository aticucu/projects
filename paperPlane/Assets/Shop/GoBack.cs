using UnityEngine;
using System.Collections;

public class GoBack : MonoBehaviour {

	public void OnButtonPress ()
	{
		Application.LoadLevel ("BellaStart");
	}
}
