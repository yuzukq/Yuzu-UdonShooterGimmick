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
    [SerializeField] Text BestScoreText;
    private int score = 0;
    private int bestScore = 0;
    private float timer = 10f;
    private bool isGameActive = false;


    private void Start()
    {  
        ScoreText.text = $"Score: {score.ToString()}"; //canvasに表示
        TimeText.text = $"Time: {timer.ToString()}";
        BestScoreText.text = $"BestScore: {bestScore.ToString()}";
    }

    private void Update()
    {
        if (isGameActive == true)
        {
            timer -= Time.deltaTime;
            TimeText.text = $"Time: {timer.ToString()}";

            if (timer <= 0)
            {
                if (score > bestScore)
                {
                    bestScore = score;
                    BestScoreText.text = $"BestScore: {bestScore.ToString()}";
                }   
                EndGame();
            }
        }
    }

    public void AddScore()
    {
        score++;
        Debug.Log($"現在のスコア: {score}");
        ScoreText.text = $"Score: {score.ToString()}";
    }

    public void StartGame()
    {
        if(isGameActive == false)   //もしゲームが開始されていなかったら．
        {
            isGameActive = true;
            Debug.Log("ゲーム開始");
        }
    }

    public void SwapTarget()
    {
        // ランダムな場所を選択
        int randomIndex = Random.Range(0, spawnPlaces.Length);
        // ターゲットを移動
        gridShotTarget.transform.position = spawnPlaces[randomIndex].position;
    }   

    private void EndGame()
    {
        isGameActive = false;
        timer = 10f;
        score = 0;
        Debug.Log("終了");
        TimeText.text = $"Time: {timer.ToString()}";
        ScoreText.text = $"Score: {score.ToString()}";
        // TODO: ゲーム終了処理
    }

    public void ResetGame()
    {
        isGameActive = false;
        timer = 10f;
        score = 0;
        TimeText.text = $"Time: {timer.ToString()}";
        ScoreText.text = $"Score: {score.ToString()}";
    }

  
}
