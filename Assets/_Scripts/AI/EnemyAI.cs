using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class EnemyAI : MonoBehaviour {
    public float speed = 0f;
    public float speedTarget = 80f;
    public GameObject detectors;
    public PathNode nextNode;
    public List<RayCastAI> rayCast = new List<RayCastAI>();

    private float verticalSens = 3f;

	void Start() {
        if(detectors == null)
            detectors = transform.FindChild("Detectors").gameObject;
	}
	
	void FixedUpdate() {
        listDetectors();


        if(Application.isPlaying) {

            speed = updatedSpeed(speed);

            if(Vector3.Distance(transform.position, nextNode.transform.position) <= 10f) {
                nextNode = nextNode.getNextNode();
                speed -= 10f;
            }

            float step = (speed) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextNode.transform.position, step);

            transform.LookAt(nextNode.transform);


            foreach(RayCastAI ai in rayCast) {
                RaycastHit hit;
                if(Physics.Raycast(ai.GetBasePos(), ai.GetDirection(), out hit)) {
                    if(hit.distance <= ai.GetDirection().magnitude)
                        Debug.Log("HIT dist: " + hit.distance);
                }
            }
        }
    }

    private float updatedSpeed(float last) {
        float newSpeed = last += (last / 100f);
        if(newSpeed == 0f)
            newSpeed = 1f;
        return Mathf.Clamp(newSpeed, 0f, speedTarget);
    }

    private void listDetectors() {
        rayCast.Clear();
        foreach(Transform go in detectors.transform) {
            rayCast.Add(go.GetComponent<RayCastAI>());
        }
    }

}
