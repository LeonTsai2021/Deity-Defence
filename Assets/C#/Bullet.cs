using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("多少時間後自行毀滅")]
    public float DeleteTime;
    [Header("移動速度")]
    public float Speed;

    [Header("打到怪物要扣多少血")]
    public float HurtHP;
    [Header("打到Boss要扣多少血")]
    public float BossHurtHP;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject指的是有該腳本的物件
        //Destroy刪除物件(要刪除的物件,幾秒以後刪除)
        Destroy(gameObject, DeleteTime);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate物件朝哪個軸向位移(0,0,0);
        //Vector3.right=(1,0,0),Vector3.left=(-1,0,0),Vector3.up=(0,1,0)...etl. 
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider hit)
    {
        //如果普功物件打到怪物
        if (hit.GetComponent<Collider>().tag == "NPC")
        {
            //普攻打到怪物，呼叫怪物身上的Monster腳本並執行DiscountHP
            hit.GetComponent<Monster>().DiscountHP(HurtHP);
            //刪除普功物件
            Destroy(gameObject);
        }
        //如果普功物件打到Boss
        if(hit.GetComponent<Collider>().tag == "Boss")
        {
            //普功打到怪物，呼叫怪物身上的Monster腳本並執行DiscountHP
            hit.GetComponent<Monster>().DiscountHP(BossHurtHP);
            //刪除普功物件
            Destroy(gameObject);
        }
    }
}
