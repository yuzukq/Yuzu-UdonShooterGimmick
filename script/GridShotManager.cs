using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections.Generic;

public class GridShotManager : UdonSharpBehaviour
{
    [Header("Set Target object")]
    public GameObject gridShotTarget;

    [Header("Target spawn places")]
    public Transform[] spawnPlaces;

    [Header("UI Elements")]
    [SerializeField] Text ScoreText;
    [SerializeField] Text TimeText;

    private int score = 0;
    private float timer = 30f;
    private bool isGameActive = false;


    private void Start()
    {  
        ScoreText.text = score.ToString();
        TimeText.text = timer.ToString();
    }

    private void Update()
    {
        if (isGameActive)
        {
            timer -= Time.deltaTime;
            //Debug.Log("時間経過");
            TimeText.text = timer.ToString();

            if (timer <= 0)
            {
                EndGame();
            }
        }
    }

    public void AddScore()
    {
        score++;
        Debug.Log($"現在のスコア: {score}");
        ScoreText.text = score.ToString();
    }

    public void StartGame()
    {
        isGameActive = true;
        SpawnTarget(); // 初期的を生成
    }


    public void SpawnTarget()
    {
        // ランダムな場所を選択
        int randomIndex = Random.Range(0, spawnPlaces.Length);
        Transform spawnPoint = spawnPlaces[randomIndex];

        // 的を生成
        Instantiate(gridShotTarget, spawnPoint.position, Quaternion.identity);
    }

    private void EndGame()
    {
        isGameActive = false;
        Debug.Log("終了");
        // TODO: ゲーム終了処理
    }

  
}
