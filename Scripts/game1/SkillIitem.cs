using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIitem : MonoBehaviour
{
    public float coldTime = 15;//技能冷卻時間
    private float timer = 0;
    [SerializeField] Image filledImage;
    private bool isStartTime;
    [SerializeField] Text cooldownText;
    [SerializeField] GameObject timeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isStartTime = true;        
        }

        if (isStartTime) //開始計時
        {
            timeText.SetActive(true);
            timer += Time.deltaTime;
            filledImage.fillAmount = (coldTime - timer) / coldTime;//控制技能冷卻圖片
            cooldownText.text = (coldTime - timer).ToString("0.0");
        }
        if(timer >= coldTime)//計時器>冷卻時間:停止冷卻
        {
            filledImage.fillAmount = 0;
            timer = 0;
            isStartTime = false;
            timeText.SetActive(false);
        }
    }
}
