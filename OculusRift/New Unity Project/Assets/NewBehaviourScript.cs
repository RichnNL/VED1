using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    public GameObject player;
    public float height = 2;
    public float distance = 4;
    private Vector3 offset;
    private Transform trans;
    public float Distance = 20.0f;
    void Start()
    {
        offset = transform.position - player.transform.position;
        trans = transform;

        Vector3 euler = player.transform.eulerAngles;
        euler[0] = player.transform.eulerAngles[2];
        euler[2] = player.transform.eulerAngles[0];

        float angle = Vector3.Angle(transform.up, player.transform.right);
        print(angle);
        euler[1] = angle;
        trans.eulerAngles = euler;


    }

    // Update is called once per frame
    void LateUpdate()
    {
        trans.position = new Vector3(player.transform.position.x - distance, player.transform.position.y + height, player.transform.position.z);
        //transform.LookAt(player.transform);

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Distance;
        Vector3 v = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 temp = v;
        v.x = temp.z;
        v.z = temp.x;
        this.transform.LookAt(v);
        Debug.Log(v);
        Debug.DrawLine(transform.position, v);

    }
}
