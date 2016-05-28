using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode : MonoBehaviour {
    public List<PathNode> nextNode = new List<PathNode>();

    void Start() {
        if(nextNode.Count <= 0) {
            Debug.LogError("Node needs one or more nodes to point to.", gameObject);
        }
    }
    
	void Update () {
	
	}

    public PathNode getNextNode() {
        return nextNode[Random.Range(0, nextNode.Count)];
    }
}
