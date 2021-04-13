using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetDamage : MonoBehaviour
{
    GameObject damageFont;
    Hashtable moveSetting;
    Vector3[] damagePath = new Vector3[3];
    CanvasGroup CGfont;
    [SerializeField] Text dDisplay;//傷害Text

    WeaponAttack attack;
    Arrow fire;//繼承武器和弓箭來讀取傷害
    


    // Use this for initialization
    void Start()
    {
        damageFont = transform.Find("DamDis").gameObject;//尋找物件

        CGfont = damageFont.GetComponent<CanvasGroup>();
        fire = gameObject.AddComponent<Arrow>();
        attack = gameObject.AddComponent<WeaponAttack>();

        dDisplay.text = "" + attack.Atk(); //傷害值傳給Text顯示
        dDisplay.text = "" + fire.Atk();

        DamageMoveSetting();//移動傷害顯示

        iTween.MoveTo(this.gameObject, moveSetting);//iTween移動

        StartCoroutine("fadeFont");
    }
    IEnumerator fadeFont()
    {
        while (true)
        {
            //每次延遲0.1秒
            yield return new WaitForSeconds(0.2f);
            //減少透明度15%
            CGfont.alpha -= 0.15f;
            //將大小改變成80%
            transform.localScale = new Vector3(transform.localScale.x * 0.8f, transform.localScale.y * 0.8f, transform.localScale.z * 0.8f);
        }
    }
    void DamageMoveSetting()
    {
        
        moveSetting = new Hashtable();
        //移動時間設為兩秒
        moveSetting.Add("time", 2.0f);
        //將移動方式設置成逐漸減速
        moveSetting.Add("easetype", iTween.EaseType.easeOutQuart);

        //設置三個移動path node為，此部分可依照角色特性微調
        damagePath[0] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        damagePath[1] = new Vector3(transform.position.x + 2, transform.position.y + 1, transform.position.z + 0.25f);//往上移動
        damagePath[2] = new Vector3(transform.position.x + 2, transform.position.y - 3, transform.position.z + 0.5f);//往下掉

        moveSetting.Add("path", damagePath);

        //在path結束時移除此字體物件
        moveSetting.Add("oncomplete", "destoryDamageFont");
    }
    void destoryDamageFont()
    {
        Destroy(this.gameObject);
    }
}