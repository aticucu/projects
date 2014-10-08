using UnityEngine;
using System.Collections;
using BellaProject;

public class ChangeGameMode2 : MonoBehaviour

{
		[SerializeField]
		private GameMode.GameModes
				gameModeOnPress = GameMode.GameModes.Normal;
		public void OnButtonPress ()
		{
				GameMode.gameMode = gameModeOnPress;
		}
}
