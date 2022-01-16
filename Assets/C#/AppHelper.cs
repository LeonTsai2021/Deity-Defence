using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 2021/03/28
 * 新增 func foreceClose()
 */
public class AppHelper : MonoBehaviour
{
    AndroidJavaClass unityPlayerClass;
    AndroidJavaObject unityContext;
    AndroidJavaObject appHelper;

    // Start is called before the first frame update
    void Start()
    {
        unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
        unityContext = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        appHelper = new AndroidJavaObject("com.habaco.test.blemanager.helper.AppHelper", unityContext);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    public void Quit(){
        appHelper.Call("finish");

    }
}
