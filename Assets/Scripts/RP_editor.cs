using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if (UNITY_EDITOR)
[CustomEditor(typeof(RP_helper))]
public class RP_editor : Editor{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		RP_helper RP = (RP_helper)target;
		if (GUILayout.Button("HDRP"))
		{
			RP.hdrp();
		}

		if (GUILayout.Button("URP"))
		{
			RP.urp();
		}
	}
}
#endif
