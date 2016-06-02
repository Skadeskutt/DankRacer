using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PathNode))]
public class PathNodeEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        PathNode pn = (PathNode) target;

        //Add a new node and automatically connect it to the selected node
        if(GUILayout.Button("Create next node")) {
            if(pn.nextNode.Count == 0 || pn.nextNode[0] == null) {
                GameObject newNode = (GameObject) PrefabUtility.InstantiatePrefab((GameObject) Resources.Load("Prefabs/Path/PathNode"));
                newNode.transform.SetParent(pn.transform.parent);
                newNode.transform.position = pn.transform.position;
                newNode.transform.localPosition += (Vector3.right * 10f);
                newNode.transform.rotation = pn.transform.rotation;
                PathNode newPN = newNode.GetComponent<PathNode>();
                newPN.nodeColor = pn.nodeColor;
                pn.nextNode.Add(newPN);
                Selection.activeGameObject = newNode;
            } else {
                Debug.LogError("Node already has a next node.");
            }
        }

        //Add another path on the selected node
        if(GUILayout.Button("Add new path")) {
            Color c = pn.getRandomColor();
            GameObject newNode = (GameObject) PrefabUtility.InstantiatePrefab((GameObject) Resources.Load("Prefabs/Path/PathNode"));
            newNode.transform.SetParent(pn.transform);
            newNode.transform.position = pn.transform.position;
            newNode.transform.localPosition += (Vector3.right * 10f);
            newNode.transform.rotation = pn.transform.rotation;
            PathNode newPN = newNode.GetComponent<PathNode>();
            newPN.nodeColor = c;
            pn.nextNode.Add(newPN);
            Selection.activeGameObject = newNode;
        }
        EditorUtility.SetDirty(target);
    }
}
