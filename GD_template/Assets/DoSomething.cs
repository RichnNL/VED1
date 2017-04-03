using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoSomething : DCG_script {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void StartServer()
    {
        base.StartServer();
    }
    public override void UpdateServer()
    {
        const float speed = 1.0f;

        offset.Translate(new Vector3(0, 0, Time.deltaTime * speed));
    }
}
