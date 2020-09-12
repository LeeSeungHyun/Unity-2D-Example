using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 점수, 스테이지 관리

    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    public Image[] UIHealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestartButton; 

    // Start is called before the first frame update
    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }
    public void NextStage()
    {
        // Change Stage
        if(stageIndex < Stages.Length - 1) {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);
        } else { // Game Clear
            Time.timeScale = 0;

            Debug.Log("게임 클리어");

            Text btnText = UIRestartButton.GetComponentInChildren<Text>();
            btnText.text = "Game Clear!";
            UIRestartButton.SetActive(true);
        }

        // Calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if(health > 1) {
            health--;
            UIHealth[health].color = new Color(1, 0, 0, 0.4f);
        } else {
            // Player Die Effect
            player.OnDie();
            // Result UI
            Debug.Log("죽었습니다.");
            // Retry Button UI
            UIRestartButton.SetActive(true);
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            // Player Reposition
            if(health > 1) {
               PlayerReposition();
            }
         
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
