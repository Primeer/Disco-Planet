using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoosterManager))]
public class BoosterManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		BoosterManager boosterManager = target as BoosterManager;
		
		if (Application.isPlaying)
		{
			if (GUILayout.Button("Ready"))
			{
				boosterManager.SetBoostersState(BoosterController.State.Ready);
			}

			if (GUILayout.Button("Refresh"))
			{
				boosterManager.RefreshBoosters();
			}
		}

		DrawDefaultInspector();
	}
}
