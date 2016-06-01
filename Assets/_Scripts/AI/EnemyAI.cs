using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class EnemyAI : MonoBehaviour {
    public float speed = 0f;
    public float speedTarget = 100f;
    public GameObject detectors;
    public PathNode nextNode;
    public List<RayCastAI> rayCast = new List<RayCastAI>();

    private float verticalSens = 3f;
    private int nodeCounter = 0;
    private Vector3 targetPos;
    private Vector3 lastNodePos;

    void Start() {
        if(detectors == null)
            detectors = transform.FindChild("Detectors").gameObject;
        if(nextNode == null)
            nextNode = GameObject.Find("Path").transform.GetChild(0).GetComponent<PathNode>();
        nodeCounter = Random.Range(0, 10000);
        lastNodePos = transform.position;
        targetPos = nextNode.transform.position;

    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if(targetPos != Vector3.zero) {
            Gizmos.DrawLine(transform.position, targetPos);
            Gizmos.DrawSphere(targetPos, 1f);
        } else {
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * 10f));
            Gizmos.DrawSphere(transform.position + (Vector3.right * 10f), 1f);
        }  
    }

    void FixedUpdate() {
        listDetectors();

        if(transform.position.y <= -10f) {
            transform.position = lastNodePos;
            speed = 0f;
        }

        if(Application.isPlaying) {

            speed = updatedSpeed(speed);

            if(Vector3.Distance(transform.position, targetPos) <= 20f) {
                lastNodePos = nextNode.transform.position;
                nextNode = nextNode.getNextNode(gameObject.name + "" + nodeCounter);
                targetPos = nextNode.getRandomPoint(gameObject.name + "" + nodeCounter, transform.position);
                speed -= 10f;
                nodeCounter++;
            }

            float step = (speed) * Time.deltaTime;
            Vector3 tp = new Vector3(targetPos.x, transform.position.y, targetPos.z);

            //transform.position = Vector3.MoveTowards(transform.position, tp, step);
            transform.Translate(Vector3.forward * (speed * Time.deltaTime), Space.Self);

            GameObject lookLocation = new GameObject("Looking at");
            lookLocation.transform.SetParent(transform);
            lookLocation.transform.position = tp;
            //transform.LookAt(lookLocation.transform);

            Vector3 relativePos = tp - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * (speed / 10f) / 2.5f);


            Destroy(lookLocation);


            foreach(RayCastAI ai in rayCast) {
                RaycastHit hit;
                if(Physics.Raycast(ai.GetBasePos(), ai.GetDirection(), out hit)) {
                    /*if(hit.distance <= ai.GetDirection().magnitude)
                        Debug.Log("HIT dist: " + hit.distance);*/
                }
            }
        }
    }

    private float updatedSpeed(float last) {
        float newSpeed = last += (last / 100f);
        if(newSpeed == 0f)
            newSpeed += Random.Range(1f, 10f);
        return Mathf.Clamp(newSpeed, 0f, speedTarget);
    }

    private void listDetectors() {
        rayCast.Clear();
        foreach(Transform go in detectors.transform) {
            rayCast.Add(go.GetComponent<RayCastAI>());
        }
    }

}
