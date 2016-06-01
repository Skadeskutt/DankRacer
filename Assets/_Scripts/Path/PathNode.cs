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

        Vector3 f = transform.InverseTransformPoint(forward);
        Vector3 p = transform.InverseTransformPoint(transform.position);
        Vector3 b = transform.InverseTransformPoint(back);

        float howCloseToPrefered = rand.Next(0, 100);
        float z;

        //Try to cut the corner
        if(howCloseToPrefered != -1f) {
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
        } else {
            if(f.z > b.z) {
                z = randomCloseToPrefered(rand, b.z, f.z);
            } else {
                z = randomCloseToPrefered(rand, f.z, b.z);
            }
        }
        
        Vector3 result = new Vector3(0f, 0f, z);

        return transform.TransformPoint(result);

    }

    public float randomCloseToPrefered(System.Random rand, float min, float max) {
        float part = (max - min) / 5;
        int terning = rand.Next(1, 6);
        float percent = rand.Next(0, 10000) / 100f;
        float newMax;
        float newMin;

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

    public PathNode getNextNode(int seed) {
        return getNextNode(seed.ToString());
    }

    public PathNode getNextNode(string seed) {
        System.Random rand = new System.Random(seed.GetHashCode());

        return nextNode[rand.Next(0, nextNode.Count)];
    }
}
