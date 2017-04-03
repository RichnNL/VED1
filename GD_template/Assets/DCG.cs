using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

public class DCG	
{
    public class Viewport
    {
        public int x;
        public int y;
        public int width;
        public int height;
    }

    public class Screen
    {
        public int eyeTransform;
        public int screenTransform;
        public int eye;
        public Matrix4x4 transform;
        public Viewport viewport;
        public Matrix4x4 eyeInverse;
        public Matrix4x4 orientation;
        public Matrix4x4 modelview;
        public Matrix4x4 projection;
    }

    public static Matrix4x4 lr()
    {
        Matrix4x4 result = Matrix4x4.identity;
        result.m22 = -1;
        return result;
    }

    public static bool init(string filename)
    {
        return dcg_init(filename) != 0;
    }

    public static void deinit()
    {
        dcg_deinit();
    }

    public static bool initialized()
    {
        return dcg_initialized() != 0;
    }

    public static void update()
	{
		dcg_update();
	}
    
	public static double getTime()
	{
		return dcg_getTime();
	}
	
	public static bool postMessage(byte[] message)
	{
		return ((dcg_postMessage(message, message.Length) == 0) ? false : true);
	}

    uint getDigital(int index)
    {
        return dcg_getDigital(index);
    }

    float getAnalog(int index)
    {
        return dcg_getAnalog(index);
    }

    Matrix4x4 getTransform(int index)
    {
        var t = intPtrToFloatArray(dcg_getTransform(index),7);
        var v = arrayToVector3(t);
        var qa = new float[4];

        for (int i = 0; i < 4; i++)
            qa[i] = t[i + 3];

        var q = arrayToQuaternion(qa);

        var result = new Matrix4x4();

        result.SetTRS(v, q, Vector3.one);

        return result;
    }


	public static byte[] takeCustomMessage()
	{	byte[] buffer = new byte[65536];
		int size = 0; //must be set, value is irrelevant
		
		if(dcg_takeCustomMessage(buffer, ref size) != 0)
		{	byte[] result = new byte[size];
		
			for(int i = 0; i < size; i++)
				result[i] = buffer[i];
				
			return result;
		}
		
		return null;
	}//returns null if no message present, byte array otherwise

	public static int getScreens()
    {
        return dcg_getScreens();
    }

    public static Screen getScreen(int index, float near, float far)
    {
        dcg_getScreen(index);

        Viewport v = new Viewport();

        v.x = dcg_screen_getViewportX();
        v.y = dcg_screen_getViewportY();
        v.width = dcg_screen_getViewportWidth();
        v.height = dcg_screen_getViewportHeight();

        Screen result = new Screen();

        result.eye = dcg_screen_getEye();
        result.screenTransform = dcg_screen_getScreenTransform();
        result.eyeTransform = dcg_screen_getEyeTransform();
        result.eyeInverse = arrayToMatrix(intPtrToFloatArray(dcg_screen_getEyeInverseMatrix(),16));
        result.modelview = arrayToMatrix(intPtrToFloatArray(dcg_screen_getModelviewMatrix(),16));
        result.projection = arrayToMatrix(intPtrToFloatArray(dcg_screen_getProjectionMatrix(near, far),16));
        result.viewport = v;

        return result;
    }

    public static bool usesHMD(int device)
    {
        return dcg_usesHMD(device) != 0;
    }

    public static int getWidth()
    {
        return dcg_getWidth();
    }

    public static int getHeight()
    {
        return dcg_getHeight();
    }

    public static bool isServer()
    {
        return dcg_isServer() != 0;
    }

    public static bool isWindowed()
    {
        return dcg_isWindowed() != 0;
    }

    public static UInt64 getTimeMilliseconds()
    {
        return dcg_getTimeMilliseconds();
    }

    public static string getServerString()
    {
        return Marshal.PtrToStringAnsi(dcg_getServerString());
    }

    [DllImport ("daemon_sdk_generic_64.dll")]
	private static extern int dcg_init([MarshalAs(UnmanagedType.LPStr)] string filename );

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern void dcg_deinit();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_initialized();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_update();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern uint dcg_getDigital(int index);

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern float dcg_getAnalog(int index);

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern IntPtr dcg_getTransform(int index);

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_getScreens();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern void dcg_getScreen(int index);

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_screen_getEye();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_screen_getEyeTransform();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_screen_getScreenTransform();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern IntPtr dcg_screen_getSize();

    [DllImport ("daemon_sdk_generic_64.dll")]
     private static extern IntPtr dcg_screen_getRotation();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern IntPtr dcg_screen_getTranslation();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_screen_getViewportX();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_screen_getViewportY();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_screen_getViewportWidth();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_screen_getViewportHeight();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern IntPtr dcg_screen_getEyeInverseMatrix();

    [DllImport ("daemon_sdk_generic_64.dll")]
    
    private static extern IntPtr dcg_screen_getOrientationMatrix();

    [DllImport ("daemon_sdk_generic_64.dll")]
    
    private static extern IntPtr dcg_screen_getModelviewMatrix();

    [DllImport ("daemon_sdk_generic_64.dll")]
    
    private static extern IntPtr dcg_screen_getProjectionMatrix(double n, double f);

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_usesHMD(int device);

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_postMessage([MarshalAs(UnmanagedType.LPArray)] byte[] message, int size);

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_takeCustomMessage([MarshalAs(UnmanagedType.LPArray)] byte[] buffer, ref int size);

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_getWidth();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_getHeight();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_isServer();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern int dcg_isWindowed();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern UInt64 dcg_getTimeMilliseconds();

    [DllImport ("daemon_sdk_generic_64.dll")]
    private static extern double dcg_getTime();

    [DllImport ("daemon_sdk_generic_64.dll")]    
    private static extern IntPtr dcg_getServerString();

    private static float[] intPtrToFloatArray(IntPtr ip, int size)
    {
        float[] result = new float[size];
        Marshal.Copy(ip, result, 0, size);
        return result;
    }

    private static Vector3 arrayToVector3(float []v3)
	{
        var result = new Vector3();		
		result.x = v3[0];
		result.y = v3[1];
		result.z = v3[2];			
		return result;
	}

	private static Vector2 arrayToVector2(float [] v2)
	{
        var result = new Vector2();
		result.x = v2[0];
		result.y = v2[1];
		return result;
	}

    private static Quaternion arrayToQuaternion(float[] v4)
    {
        var result = new Quaternion();
        result.x = v4[0];
        result.y = v4[1];
        result.z = v4[2];
        result.w = v4[3];
        return result;
    }

    private static Matrix4x4 arrayToMatrix(float [] m44)
	{
        Matrix4x4 result = new Matrix4x4();
	
		result.SetColumn(0,new Vector4(m44[0],m44[1],m44[2],m44[3]));
		result.SetColumn(1,new Vector4(m44[4],m44[5],m44[6],m44[7]));
		result.SetColumn(2,new Vector4(m44[8],m44[9],m44[10],m44[11]));
		result.SetColumn(3,new Vector4(m44[12],m44[13],m44[14],m44[15]));
		
		return result;
	}

    //borrowed from http://answers.unity3d.com/questions/13523/is-there-a-way-to-set-the-position-of-a-standalone.html

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);

    public static void SetPosition(int x, int y, int resX = 0, int resY = 0)
    {
        SetWindowPos(FindWindow(null, "My Unity Window Title"), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
    }
#endif
}