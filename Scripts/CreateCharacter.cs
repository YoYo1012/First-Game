using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateCharacter : MonoBehaviour
{
    public Button creatWarrior;
    public Button creatHunter;
    public Button startGame;
    public InputField inputName;
    public GameObject load;
    public Text loadText,playerName;
    public Image loadImage;
    

    public GameObject Warrior;
    public GameObject Hunter;
    int player;
    // Start is called before the first frame update
    void Start()
    {
        creatWarrior = GetComponent<Button>();
        creatHunter = GetComponent<Button>();
        startGame = GetComponent<Button>();
        

    }
    private void Update()
    {
    }

    public void HunterClick() {//點擊顯示Hunter
        player = 1;
        Warrior.SetActive(false);
        Hunter.SetActive(true);        
    }

    public void WarriorClick() {//點擊顯示Warrior
        player = 2;
        Hunter.SetActive(false);
        Warrior.SetActive(true);
    }

    public void StartClick()
    {
        load.SetActive(true);//顯示讀取畫面
        playerName.text = "玩家" + inputName.text + "已登入";
        if (player == 1 )
        {
            StartCoroutine(DisplayLoadingScreen("Hunter"));//進入獵人場景
            Hunter.SetActive(false);
        }
        else 
        {
            StartCoroutine(DisplayLoadingScreen("Warrior"));//進入戰士場景
            Warrior.SetActive(false);
        }
            
    }

    IEnumerator DisplayLoadingScreen(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);//異步操作
        while (!async.isDone)
        {

            loadText.text = "讀取中..." + (async.progress * 100).ToString() + "%";//讀取值
            loadImage.fillAmount = async.progress;//進度條
            yield return null;
        }
    }
}
