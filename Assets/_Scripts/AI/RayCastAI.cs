using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RayCastAI : MonoBehaviour {
    public Transform target;

	void Start() {
	    target = this.transform.FindChild("Target");
    }
	
	public Vector3 GetTargetPos() {
        return target.position;
    }

    public Vector3 GetBasePos() {
        return this.transform.position;
    }

    public Vector3 GetDirection() {
        return GetTargetPos() - GetBasePos();
    }
}
