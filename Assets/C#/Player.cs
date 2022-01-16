using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //射線打擊到的所有物件
    RaycastHit[] hits;
    //找尋射線陣列中的物件
    RaycastHit hit;
    Vector3 lookPos;
    Vector3 targetPos;
    [Header("玩家的動畫")]
    public Animator PlayerAni;
    [Header("普功法術物件")]
    public GameObject FireObj;
    [Header("普功物件要產生的位置")]
    public GameObject CreatePos;

    [Header("判斷是否有點到大絕招的按鈕")]
    public bool isTouchMagic;
    [Header("大絕招物件")]
    public GameObject MagicObj;
    //暫存動態生成出來的大絕招
    GameObject MagicPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Input.GetMouseButtonDown(0)當按下滑鼠左鍵，if條件內只會觸發一次
        //0左鍵 1右鍵 2中鍵，0=手指點擊螢幕
        //Input.GetMouseButton(0)當按下滑鼠左鍵，if條件內會持續觸發
        if (Input.GetMouseButton(0)) {
            //Ray射線
            // Camera.main抓場景上標籤為Main Camera的攝影機
            //ScreenPointToRay將滑鼠點擊的2維座標轉換成3維座標 並與攝影機連成一線產生射線
            //Input.mousePosition 滑鼠在Game遊戲視窗的座標位置
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray, 100);
            //透過for迴圈找尋射線陣列中是否有地板物件
            for (int i = 0; i < hits.Length; i++) {
                hit = hits[i];
                Debug.DrawLine(Camera.main.transform.position,hit.point, Color.red);
                //如果射線打到地板
                if (hit.collider.name == "mazu_floor") {
                    if (!isTouchMagic)
                    {
                        //紀錄射線打到地板的座標值
                        targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        //使用內插法讓玩家進行轉動，如果沒有使用內插法，玩家不會慢慢從A點轉動到B點
                        lookPos = Vector3.Lerp(lookPos, targetPos,Time.deltaTime*10);
                        //玩家注視滑鼠點擊點
                        transform.LookAt(lookPos);
                        //玩家撥放普攻的動畫
                        PlayerAni.SetBool("Att", true);
                    }
                    else
                    {
                        //如果場景上沒有任何物件標籤為Magic
                        if (GameObject.FindGameObjectsWithTag("Magic").Length <= 0)
                        {
                            //動態生成一個Magic物件
                            MagicPrefab = Instantiate(MagicObj, hit.point, transform.rotation);
                        }
                        //修改暫存出來的Magic物件角度值
                        MagicPrefab.transform.eulerAngles = new Vector3(90f, 90f, 0f);
                        //暫存出來的Magic物件位置=滑鼠點到地板的位置
                        MagicPrefab.transform.position = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);
                    }
                }
            }
        }
        //放開滑鼠左鍵或手指
        if (Input.GetMouseButtonUp(0)) {
            if (!isTouchMagic)
            {
                //停止撥放玩家攻擊動畫
                PlayerAni.SetBool("Att", false);
            }
            else
            {
                //大絕招的龍掉下
                GameObject.FindGameObjectWithTag("Magic").GetComponentInChildren<Rigidbody>().useGravity = true;
                //重新計算可以產生大魔法的時間
                GameObject.Find("GM").GetComponent<GM>().ScriptMagicTimer = 0;
                //不能再施放大絕招
                isTouchMagic = false;
            }
        }
    }

    public void AttAni() {
        // Debug.Log("產生一個普攻物件");
        //動態生成(產生的物件,產生的座標位置,角度)
        Instantiate(FireObj, CreatePos.transform.position, CreatePos.transform.rotation);
    }
    public void TouchMagicBtn()
    {
        //偵測Magic bar是否填滿，填滿才可觸發大絕招
        if (GameObject.Find("GM").GetComponent<GM>().MagicBar.fillAmount == 1)
            isTouchMagic = true;
    }
}
