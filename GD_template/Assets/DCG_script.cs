using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.IO;
using System;

public class DCG_script : MonoBehaviour
{
    private bool initialized = false;
    public GameObject man;
    public float Z = 2;
    public float Y = 1;

    private new Camera [] camera = null;
    public float speed = 1.0f;

    protected Transform offset = null;

    protected float zNear = 1.0f / 16.0f;
    protected float zFar = 65536.0f;	
	public const int port = 25000;

    void Start()
    {
        if (!DCG.init("D:/daemon_client.xml"))
        {
            Debug.LogError("Error initializing DCG.");
            return;
        }

        Debug.Log("DCG initialized.");

        Application.runInBackground = true;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        Screen.SetResolution(DCG.getWidth(), DCG.getHeight(), false);
        DCG.SetPosition(0, 0);
        Cursor.visible = false;
#else
        Screen.SetResolution(DCG.getWidth(), DCG.getHeight(), !DCG.isWindowed());

        Cursor.visible = false;
#endif
        

        Debug.Log("Resolution set.");

        {
            var go = GameObject.Find("GDC_Offset");

            go.AddComponent<NetworkView>();
           
            offset = go.GetComponent<Transform>();

            if (offset == null)
            {
                Debug.LogError("GDC_Offset.transform not present.");
                return;
            }
        }

        camera = new Camera[DCG.getScreens()];

        for (int i = 0; i < DCG.getScreens(); i++)
        {

            Camera c = null;

            /*
            
            if (i == 0)
            {
                c = FindObjectOfType<Camera>();

                if(c == null)
                {
                    Debug.LogError("You have to put a camera in the scene.");
                    return;
                }
            }
            */
                        
            //if(c == null)
            //{
                var go = new GameObject();

            go.name = "Camera" + i;

                //go.AddComponent<Transform>();

                c = go.AddComponent<Camera>();

                //Instantiate(go);
                
            //}

           

          

            DCG.Screen s = DCG.getScreen(i, 1, 2);

 
            {
                float x = (float)s.viewport.x;
                float y = (float)s.viewport.y;
                float width = (float)s.viewport.width;
                float height = (float)s.viewport.height;
                c.pixelRect = new Rect(x, y, width, height);
            }

            c.enabled = true;

            //Camera.SetupCurrent(c);

            camera[i] = c;

            //Instantiate(go);
           
        }

        

        Debug.Log(DCG.getScreens() + " camera(s) instantiated.");

        if (DCG.isServer())
		{
            Debug.Log("Starting server.");

			var nce = Network.InitializeServer (32,port,false);

            if(nce != NetworkConnectionError.NoError)
            {
                Debug.LogError(nce);
                return;
            }

			StartServer();            
        }
        else
        {
            var nce = Network.Connect(DCG.getServerString(), port);

            if (nce != NetworkConnectionError.NoError)
            {
                Debug.LogError(nce);
                return;
            }
        }

        initialized = true;
    }
	
	public virtual void StartServer()
	{
       		
	}

    void OnApplicationQuit()
    {
        DCG.deinit();
    }

    //Client functions called by Unity
    void OnConnectedToServer()
	{
		Debug.Log("This client connected to the server.");	
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
        Debug.Log("This client disconnected from the server.");
	}

	void OnFailedToConnect(NetworkConnectionError error)
	{
        Debug.Log("Failed to connect to server: " + error);
	}
	
	void OnServerInitialized()
	{
		Debug.Log("The server has been initialized.");
	}
	
		
	void OnNetworkInstantiate (NetworkMessageInfo info)
	{
		Debug.Log("A network object has been instantiated: " + info.sender);
	}		
	
	public virtual void UpdateServer()
	{
        

        if (Input.GetKey("up"))
            man.transform.position += (new Vector3(0, 0, Time.deltaTime * speed));
        

        if (Input.GetKey("down"))
            man.transform.position +=  (new Vector3(0, 0, -Time.deltaTime * speed));

        if (Input.GetKey("left"))
            man.transform.position +=  (new Vector3(-Time.deltaTime * speed, 0, 0));

        if (Input.GetKey("right"))
            man.transform.position += (new Vector3(Time.deltaTime * speed, 0, 0));       
    }

	

	// Update is called once per frame
	void Update ()
	{
        transform.position = man.transform.position + new Vector3(0, Y, -Z);
        transform.eulerAngles = man.transform.eulerAngles;
        if(!initialized)
        {
            Debug.LogError("Not initialized.");
            return;            
        }

		DCG.update ();

        if (DCG.isServer())
            UpdateServer();
        
        Matrix4x4 invOffset = offset.worldToLocalMatrix;

        for(int i = 0; i < DCG.getScreens(); i++)
        {
            DCG.Screen s = DCG.getScreen(i, zNear, zFar);
            Camera c = camera[i];
            c.worldToCameraMatrix = s.modelview * DCG.lr() * invOffset;
            c.projectionMatrix = s.projection;
        }
        
	}
}
