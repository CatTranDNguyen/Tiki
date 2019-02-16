using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenu;
    public GameObject winnerPanel1;
    public GameObject winnerPanel2;
    public GameObject drawPanel;
    public Text timeText;

    public Text LiveText1;
    public Text LiveText2;
    public Text DieText1;
    public Text DieText2;
    public Text CountDownText1;
    public Text CountDownText2;
    public Text CountDownTextStart;

    public Rigidbody plane;

    public Rigidbody knight1;
    public Rigidbody knight2;
    public AudioSource tickTock;

    private KnightController knightController1;
    private KnightController knightController2;
    public float timeLeft = 180;
    float startCount = 3.9f;
    public bool start = false;
    // Use this for initialization
    void Start()
    {
        CountDownTextStart.enabled = true;
        CountDownTextStart.text = startCount.ToString("0");
        SetUp();
    }
    float tmpCount = 1f;
    // Update is called once per frame

    void SetUp()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        pauseMenu.active = false;
        winnerPanel1.active = false;
        winnerPanel2.active = false;

        Rigidbody knigh1Rigid = Instantiate(knight1, new Vector3(-6, 0, -6), Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody;
        Rigidbody planeRigid = Instantiate(plane, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 180, 0))) as Rigidbody;
        Rigidbody knigh2Rigid = Instantiate(knight2, new Vector3(6, 0, 6), Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody;

        knightController1 = knigh1Rigid.GetComponent<KnightController>();
        knightController2 = knigh2Rigid.GetComponent<KnightController>();

        knightController1.DieText = DieText1;
        knightController2.DieText = DieText2;

        knightController1.CoundownText = CountDownText1;
        knightController2.CoundownText = CountDownText2;

        knightController1.liveText = LiveText1;
        knightController2.liveText = LiveText2;

        knightController1.plane = knightController2.plane = planeRigid.gameObject;
    }
    void countDownStartGame()
    {
        startCount -= Time.deltaTime;
        if (Math.Abs((int)startCount + 0.5f - startCount) <= 0.01f)
            tickTock.Play();
        if (startCount <= 0)
        {
            startCount = 0;
            start = true;
            CountDownTextStart.enabled = false;
            GetComponent<AudioSource>().Play();
            knightController1.YouCanStart();
            knightController2.YouCanStart();

        }
        CountDownTextStart.text = startCount.ToString("0");
    }

    void drawResult()
    {
        tmpCount -= Time.deltaTime;
        if (tmpCount <= 0)
        {
            GameIsPaused = true;
            Time.timeScale = 0;
        }
        drawPanel.active = true;
    }
    void winner2Result()
    {
        tmpCount -= Time.deltaTime;
        if (tmpCount <= 0)
        {
            GameIsPaused = true;
            Time.timeScale = 0;
        }
        winnerPanel2.active = true;
    }
    void winner1Result()
    {
        tmpCount -= Time.deltaTime;
        if (tmpCount <= 0)
        {
            GameIsPaused = true;
            Time.timeScale = 0;
        }
        winnerPanel1.active = true;
    }
    void Update()
    {
        // count down luc bat dau game
        if (!start)
        {
            countDownStartGame();
            return;
        }
        // nhan Esc de pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else Pause();
        }
        // xet ket qua
        if (knightController1.isDead()&&knightController2.isDead())
        {
            drawResult();

        }
        if (knightController1.isDead())
        {
            winner2Result();
        }
        if (knightController2.isDead())
        {
            winner1Result();
        }
        //Tinh thoi gian con lai cua game
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            PrintResult();
        }
        updateTimeText();
    }

    private void PrintResult()
    {
        int res = knightController1.CalRes(knightController2);
        if (res == 0)
        {
            GameIsPaused = true;
            Time.timeScale = 0;
            drawPanel.active = true;
        }
        else if (res == 1)
        {
            GameIsPaused = true;
            Time.timeScale = 0;
            winnerPanel1.active = true;
        }
        else
        {
            GameIsPaused = true;
            Time.timeScale = 0;
            winnerPanel2.active = true;
        }
    }

    private void updateTimeText()
    {
        int m = (int)timeLeft / 60;
        int s = (int)timeLeft % 60;
        timeText.text = m.ToString() + ":" + s.ToString();
    }

    public void Pause()
    {
        GameIsPaused = true;
        Time.timeScale = 0;
        pauseMenu.active = true;
    }

    public void Resume()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        pauseMenu.active = false;
    }

    public void Quit()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void Restart()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
