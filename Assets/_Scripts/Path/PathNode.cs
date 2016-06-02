using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PathNode : MonoBehaviour {
    public List<PathNode> nextNode = new List<PathNode>();
    public Color nodeColor = Color.blue;

    private Vector3 forward;
    private Vector3 back;
    private List<Color> childColors = new List<Color>();

    void Start() {
        childColors.Add(Color.green);
        childColors.Add(Color.magenta);
        childColors.Add(Color.red);
        childColors.Add(Color.yellow);
        childColors.Add(Color.white);
        childColors.Add(Color.grey);
        childColors.Add(Color.blue);
        if(nextNode.Count <= 0) {
            Debug.LogError("Node needs one or more nodes to point to.", gameObject);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 5f);

        if(back != null && forward != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, forward);
            Gizmos.DrawLine(transform.position, back);
        }

        foreach(PathNode pn in nextNode) {
            Gizmos.color = pn.nodeColor;
            Gizmos.DrawLine(transform.position, pn.transform.position);
        }
    }

    void Update() {
        RaycastHit fw, bk;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out fw)) {
            forward = fw.point;
        }
        if(Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.forward), out bk)) {
            back = bk.point;
        }
    }

    public Color getRandomColor() {
        Color result = childColors[Random.Range(0, childColors.Count)];
        childColors.Remove(result);
        return result;
    }

    public Vector3 getRandomPoint(int seed, Vector3 playerPos) {
        return getRandomPoint(seed.ToString(), playerPos);
    }

    public Vector3 getRandomPoint(string seed, Vector3 playerPos) {
        System.Random rand = new System.Random(seed.GetHashCode());

        //Get local position of raycast hit and node
        Vector3 f = transform.InverseTransformPoint(forward);
        Vector3 p = transform.InverseTransformPoint(transform.position); //This is probably always 0,0,0. Whatever.
        Vector3 b = transform.InverseTransformPoint(back);

        float z;

        //Try to cut the corner
        if(Vector3.Distance(playerPos, forward) < Vector3.Distance(playerPos, back)) {
            if(f.z > p.z) {
                z = randomCloseToPrefered(rand, p.z, f.z);
            } else {
                z = randomCloseToPrefered(rand, f.z, p.z);
                z -= f.z;
            }
        } else {
            if(b.z > p.z) {
                z = randomCloseToPrefered(rand, p.z, b.z);
            } else {
                z = randomCloseToPrefered(rand, b.z, p.z);
                z -= b.z;
            }
        }
        //We only need the z coord
        Vector3 result = new Vector3(0f, 0f, z);
        return transform.TransformPoint(result);
    }

    public float randomCloseToPrefered(System.Random rand, float min, float max) {
        //Divide the node side in 5 equal parts
        float part = (max - min) / 5;
        float percent = rand.Next(0, 10000) / 100f;
        float newMax;
        float newMin;

        //Prefer a point closer to the node center
        if(percent > 25f) {
            newMin = min;
            newMax = min + part;
        } else if(percent > 12.5f) {
            newMin = min + (part * 1);
            newMax = min + (part * 2);
        } else if(percent > 6.5f) {
            newMin = min + (part * 2);
            newMax = min + (part * 3);
        } else if(percent > 3.125f) {
            newMin = min + (part * 3);
            newMax = min + (part * 4);
        } else {
            newMin = min + (part * 4);
            newMax = min + (part * 5);
        }

        return (rand.Next(Mathf.RoundToInt(newMin * 100f), Mathf.RoundToInt(newMax * 100f)) / 100f);
    }

    public Vector3 getLocalPosition(Transform parent) {
        return parent.InverseTransformPoint(transform.position);
    }

    public PathNode getNextNode(int seed, Transform parent = null) {
        return getNextNode(seed.ToString(), parent);
    }

    public PathNode getNextNode(string seed, Transform parent = null) {
        System.Random rand = new System.Random(seed.GetHashCode());

        //If the node has 2 or more nodes, pick one
        if(nextNode.Count >= 2) {
            float percentage = rand.Next(0, 10000) / 100f;

            //25% of the time it just randomly picks any of the nodes
            if(percentage <= 25f)
                return nextNode[rand.Next(0, nextNode.Count)];
            
            //Calculate distance to the next node from the current node center
            Vector3 testPos = this.transform.position;

            //if a parent is provided, we want a 25% chance of using what would probably be the EnemyAI's position instead of the node center
            if(percentage < 50f && parent != null)
                testPos = parent.position;
            
            //Find the closest node to whatever we decided above
            float closestDist = float.MaxValue;
            int index = 0;
            for(int i = 0; i < nextNode.Count; i++) { 
                float currDist = Vector3.Distance(testPos, nextNode[i].transform.position);
                //Distance was shorter than whatever we've tested before, use this one instead
                if(currDist < closestDist) {
                    closestDist = currDist;
                    index = i;
                }
            }

            //Picked the perfect node for us
            return nextNode[index];
        }

        //Had only one node anyway
        return nextNode[0];
    }
}
