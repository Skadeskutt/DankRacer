using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class EnemyAI : MonoBehaviour {
    public float speed = 80f;
    public GameObject detectors;
    public List<RayCastAI> rayCast = new List<RayCastAI>();

    private float verticalSens = 3f;

	void Start() {
        if(detectors == null)
            detectors = transform.FindChild("Detectors").gameObject;
	}
	
	void FixedUpdate() {
        listDetectors();


        if(Application.isPlaying) {


            float step = (speed) * Time.deltaTime;
            transform.Translate(Vector3.forward * step);


            foreach(RayCastAI ai in rayCast) {
                RaycastHit hit;
                if(Physics.Raycast(ai.GetBasePos(), ai.GetDirection(), out hit)) {
                    if(hit.distance <= ai.GetDirection().magnitude)
                        Debug.Log("HIT dist: " + hit.distance);
                }
            }
        }
    }

    private void listDetectors() {
        rayCast.Clear();
        foreach(Transform go in detectors.transform) {
            rayCast.Add(go.GetComponent<RayCastAI>());
        }
    }

}
