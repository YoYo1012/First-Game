using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour//Tab鍵開遊戲選單
{
    public Button quitButton, backButton;
    public GameObject gameMenu;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    bool mouse = false;
    void Update()
    {
        if (mouse)
        {
            Cursor.visible = true;         //控制滑鼠隱藏
            Cursor.lockState = CursorLockMode.None;//控制滑鼠鎖定
            gameMenu.SetActive(true);//顯示選單
        }
        else
        {
            Cursor.visible = false;          
            Cursor.lockState = CursorLockMode.Locked;
            gameMenu.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            mouse = !mouse;
        }
    }

    public void quitClick()
    {
        Application.Quit();
    }

    public void backClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
