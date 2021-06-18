using UnityEngine;
using UnityEditor;
using Thuleanx.Optimization;

[CustomEditor(typeof(BubblePool))]
public class BubblePoolEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EditorGUI.BeginDisabledGroup(true);
		EditorGUILayout.LabelField(((BubblePool) target).bubblePool.Count.ToString(), "Number");
		EditorGUI.EndDisabledGroup();
	}
}