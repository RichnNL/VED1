using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_SpaceBar : MonoBehaviour {

    public GameObject Emitter;
    public GameObject Beam;
    public float force;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            GameObject temp_beam;
            temp_beam = Instantiate(Beam, Emitter.transform.position, Emitter.transform.rotation) as GameObject;

            Rigidbody rb;
            rb = temp_beam.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * force);
             
            Destroy(temp_beam, 5.0f);
        }
	}
}
