using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor (typeof(gridManager))]
public class MapEditor : Editor {
 public override void OnInspectorGUI(){
	 base.OnInspectorGUI ();

	 gridManager grid = target as gridManager;

	 grid.InitializeGrid();
 }
}
