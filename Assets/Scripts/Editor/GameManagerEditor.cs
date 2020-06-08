using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		GameManager gameManager = target as GameManager;
		
		if (Application.isPlaying)
		{
			if (GUILayout.Button("Main Collapse"))
			{
				gameManager.OnMainCollapse();
			}

			if (GUILayout.Button("Overflow"))
			{
				gameManager.OnOverflow();
			}
		}
		DrawDefaultInspector();
	}
}
