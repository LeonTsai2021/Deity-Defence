using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("BGM 預製物物件")]
    public GameObject BGM;
    // Start is called before the first frame update
    void Start()
    {
        //偵測場景上是否有BGM物件，如果沒有BGM物件，產生BGM物件
        if (GameObject.FindGameObjectsWithTag("BGM").Length == 0) {
            //動態生成BGM物件
            Instantiate(BGM);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartButton() {
        // Application.LoadLevel切換場景("下一個場景的名稱");
        Application.LoadLevel("Game");
    }
    public void QuitButton()
    {
        // Application.Quit();遊戲關閉
        Application.Quit();
    }
}
