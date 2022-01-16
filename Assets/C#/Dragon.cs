using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [Header("打到怪物要扣多少血")]
    public float HurtHP;
    [Header("打到Boss要扣多少血")]
    public float BossHurtHP;
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().name == "mazu_floor")
        {
            StartCoroutine(WaitDestroy(1f));
        }
        if (hit.GetComponent<Collider>().tag == "NPC")
        {
            //普攻打到怪物，呼叫怪物身上的Monster腳本並執行DiscountHP
            hit.GetComponent<Monster>().DiscountHP(HurtHP);
            StartCoroutine(WaitDestroy(1f));
        }
        //如果普功物件打到Boss
        if (hit.GetComponent<Collider>().tag == "Boss")
        {
            //普功打到怪物，呼叫怪物身上的Monster腳本並執行DiscountHP
            hit.GetComponent<Monster>().DiscountHP(BossHurtHP);
            StartCoroutine(WaitDestroy(1f));
        }
    }

    IEnumerator WaitDestroy(float DeletTime)
    {
        yield return new WaitForSeconds(DeletTime);
        Destroy(transform.parent.gameObject);

    }
}
