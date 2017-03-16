using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHand : MonoBehaviour {

    public GameObject player;
    public float height;
    public float distance; 
    private Vector3 offset;
    private Transform trans;
	void Start () {
        offset = transform.position - player.transform.position;
        trans = transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        trans.position = new Vector3(player.transform.position.x, player.transform.position.y + height, player.transform.position.z - distance);
        transform.LookAt(player.transform);
    }
}
