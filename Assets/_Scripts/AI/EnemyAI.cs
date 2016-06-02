using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class EnemyAI : MonoBehaviour {
    public float speed = 0f;
    public float speedTarget = 100f;
    public PathNode nextNode;

    private float verticalSens = 3f;
    private int nodeCounter = 0;
    private int randomSeed = 0;
    private Vector3 targetPos;
    private Vector3 lastNodePos;
    private AudioSource audioSource;

    void Start() {
        if(nextNode == null)
            nextNode = GameObject.Find("Path").transform.GetChild(0).GetComponent<PathNode>();
        randomSeed = Random.Range(0, 10000);
        lastNodePos = transform.position;
        targetPos = nextNode.transform.position;
        audioSource = GetComponent<AudioSource>();
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

        if(transform.position.y <= -10f) {
            transform.position = lastNodePos;
            speed = 0f;
        }

        if(Application.isPlaying) {

            //Honk randomly
            if(Random.Range(0, 1000) == 900 && !audioSource.isPlaying)
                audioSource.Play();

            speed = updatedSpeed(speed);

            //We're close to the node, get the next one.
            if(Vector3.Distance(transform.position, targetPos) <= 20f) {
                lastNodePos = nextNode.transform.position;
                nextNode = nextNode.getNextNode(gameObject.name + "" + (nodeCounter + randomSeed), transform);
                targetPos = nextNode.getRandomPoint(gameObject.name + "" + (nodeCounter + randomSeed), transform.position);
                speed -= 10f;
                nodeCounter++;
            }

            //Fly forward
            float step = (speed) * Time.deltaTime;
            Vector3 tp = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            transform.Translate(Vector3.forward * (speed * Time.deltaTime), Space.Self);

            //Look towards the next node
            GameObject lookLocation = new GameObject("Looking at");
            lookLocation.transform.SetParent(transform);
            lookLocation.transform.position = tp;
            Vector3 relativePos = tp - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * (speed / 10f) / 2.5f);
            Destroy(lookLocation);
        }
    }

    private float updatedSpeed(float last) {
        float newSpeed = last += (last / 100f);
        if(newSpeed == 0f)
            newSpeed += Random.Range(10f, 30f);
        return Mathf.Clamp(newSpeed, 0f, speedTarget);
    }
}
