using UnityEngine;
using System.Collections;

public class SceneChange2 : MonoBehaviour
{
		[SerializeField]
		private int
				sceneIndexToLoad = 0;
		public void OnButtonPress ()
		{
				Application.LoadLevel (sceneIndexToLoad);
		} 
}
