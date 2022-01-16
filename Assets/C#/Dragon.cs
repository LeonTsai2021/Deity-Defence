using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [Header("����Ǫ��n���h�֦�")]
    public float HurtHP;
    [Header("����Boss�n���h�֦�")]
    public float BossHurtHP;
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().name == "mazu_floor")
        {
            StartCoroutine(WaitDestroy(1f));
        }
        if (hit.GetComponent<Collider>().tag == "NPC")
        {
            //���𥴨�Ǫ��A�I�s�Ǫ����W��Monster�}���ð���DiscountHP
            hit.GetComponent<Monster>().DiscountHP(HurtHP);
            StartCoroutine(WaitDestroy(1f));
        }
        //�p�G���\���󥴨�Boss
        if (hit.GetComponent<Collider>().tag == "Boss")
        {
            //���\����Ǫ��A�I�s�Ǫ����W��Monster�}���ð���DiscountHP
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
