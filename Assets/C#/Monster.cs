using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("移動速度")]
    public float Speed;
    //程式中移動速度
    public float ScriptSpeed;

    [Header("怪物倒退多少距離值")]
    public float BackPos;
    [Header("怪物總血量")]
    public float TotalHP;
    float ScriptHP;

    void Start()
    {
        //預設程式中移動速度為屬性面板中調整的速度值
        ScriptSpeed = Speed;

        //預設程式中血量為屬性面板中調整的總血量
        ScriptHP = TotalHP;
    }

    void Update()
    {
        //怪物朝Z軸前進
        transform.Translate(Vector3.forward * ScriptSpeed * Time.deltaTime);
    }
    //穿透型碰撞
    //OnTriggerEnter兩個物件撞在一起後腳本內的程式只會執行一次
    //OnTriggerExit兩個物件撞在一起且離開後腳本內的程式只會執行一次
    //OnTriggerStay兩個物件持續撞在一起，腳本內的程式持續執行一次

    //怪物碰到mazu_wall的物件
    void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().name == "mazu_wall") {
            //動作切換到攻擊
            GetComponent<Animator>().SetBool("Att", true);
            //移動速度為0
            ScriptSpeed = 0;
        }
    }
    //怪物離開mazu_wall的物件
     void OnTriggerExit(Collider hit)
    {
        if (hit.GetComponent<Collider>().name == "mazu_wall")
        {
            //動作切換到攻擊
            GetComponent<Animator>().SetBool("Att", false);
            //預設程式中移動速度為屬性面板中調整的速度值
            ScriptSpeed = Speed;
        }
    }

    //怪物被攻擊扣血與倒退
    public void DiscountHP(float hurtHP) {
        //扣除血量 ScriptHP=ScriptHP-hurtHP;
        ScriptHP -= hurtHP;
        //如果血量<=0
        if (ScriptHP <= 0)
        {
            //觸發怪物死亡動畫
            GetComponent<Animator>().SetTrigger("Die");
            //怪物身上的碰撞器關閉
            GetComponent<Collider>().enabled = false;
            //移動速度為0
            ScriptSpeed = 0;
            if(gameObject.tag == "NPC")
            {
                //增加怪物死亡數量
                GameObject.Find("GM").GetComponent<GM>().MonsterDieNum++;
                //加分
                GameObject.Find("GM").GetComponent<GM>().Score();
            }
            else
            {
                //Boss加分
                GameObject.Find("GM").GetComponent<GM>().BossScore();
                //遊戲結束
                GameObject.Find("GM").GetComponent<GM>().GameOver(true);
            }
        }
        //如果還有血
        else {
            //怪物倒退
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - BackPos);
        }
    }
    //當怪物的動畫做到揮下，呼叫扣除玩家的血量
    public void DiscountPlayerHP() {
        if(gameObject.tag == "NPC")
        {
            GameObject.Find("GM").GetComponent<GM>().HurtPlayer();
        }
        else
        {
            GameObject.Find("GM").GetComponent<GM>().BossHurtPlayer();
        }
    }
}
