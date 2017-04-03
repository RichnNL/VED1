using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_SpaceBar : MonoBehaviour {

    public GameObject Emitter;
    public GameObject Beam;
    public float Speed = 10f;
    public float force = 2000f;
    public float Distance = 20f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float rotation = 0.0f;

   
       // transform.rotation = Quaternion.LookRotation(v);
        if (Input.GetKeyDown("space"))
        {
            GameObject temp_beam;
            temp_beam = Instantiate(Beam, Emitter.transform.position, Emitter.transform.rotation) as GameObject;
            temp_beam.transform.Rotate(90, 90, 0);
            Rigidbody rb;
            rb = temp_beam.GetComponent<Rigidbody>();
            rb.AddForce(transform.right * force);

            Destroy(temp_beam, 5.0f);
           ;
        }
        if (Input.GetKey("up"))
        {
            GameObject child = GameObject.Find("HandDirection");
            child.transform.position = new Vector3(0.3f, 0, 0);

            transform.Translate(child.transform.position);
        }
        else if (Input.GetKey("down"))
        {
            GameObject child = GameObject.Find("HandDirection");
            child.transform.position = new Vector3(-0.3f, 0, 0) ;
            
            transform.Translate(child.transform.position);
        }
        else if (Input.GetKey("right"))
        {
            GameObject child = GameObject.Find("HandDirection");
            child.transform.position = new Vector3(0, 0, -0.3f);

            transform.Translate(child.transform.position);
        }
        else if (Input.GetKey("left"))
        {
            GameObject child = GameObject.Find("HandDirection");
            child.transform.position = new Vector3(0, 0, 0.3f);

            transform.Translate(child.transform.position);
        }
       
            

        }

    }

