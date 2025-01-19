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
    [SerializeField] Text ClearScoreText;
    [SerializeField] Text TimeText;
    [SerializeField] Text BestScoreText;
    [SerializeField] Transform ClearHUD;

    [Header("Game Settings")]
    [SerializeField] float playTime = 10f;

    private int score = 0;
    private int erlierScore = 0;
    private int bestScore = 0;
    private float timer;
    private bool isGameActive = false;
    private bool isHUDActive = false;

    private void Start()
    {  
        timer = playTime;
        ScoreText.text = $"Score: {score.ToString()}"; //canvasに表示
        TimeText.text = $"Time: {timer.ToString()}";
        BestScoreText.text = $"BestScore: {bestScore.ToString()}";
        ClearHUD.localScale = Vector3.zero;//HUDをスケール0で非表示

        //以下デバッグ用
        //EndGame();
    }

    private void Update()
    {

        if(!Utilities.IsValid(Networking.LocalPlayer)){return;}

        //HUDの位置を常にプレイヤーの頭の位置に合わせる
        Vector3 position = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        Quaternion rotation = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
        ClearHUD.SetPositionAndRotation(position, rotation);

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
        ScoreText.text = $"Score: {score.ToString()}";
    }

    public void StartGame()
    {
        if(isGameActive == false)   //もしゲームが開始されていなかったら．
        {
            isGameActive = true;
        }
    }

    public void SwapTarget()
    {
        // 現在の位置を取得
        Vector3 currentPosition = gridShotTarget.transform.position;
        int randomIndex;

        // 現在の位置と異なるランダムな場所を選択
        do{
            randomIndex = Random.Range(0, spawnPlaces.Length);
        } while (spawnPlaces[randomIndex].position == currentPosition);

        // ターゲットを移動
        gridShotTarget.transform.position = spawnPlaces[randomIndex].position;
    }   

    private void EndGame()
    {
        isGameActive = false;
        timer = playTime;
        erlierScore = score;
        score = 0;
        TimeText.text = $"Time: {timer.ToString()}";
        ScoreText.text = $"Score: {score.ToString()}";
        ClearScoreText.text = $"Score: {erlierScore.ToString()}";
        
        isHUDActive = true;
        ClearHUD.localScale = Vector3.one; // HUDを表示

        // HUDの位置をプレイヤーの頭の位置に合わせる
        if (Utilities.IsValid(Networking.LocalPlayer))
        {
            Vector3 position = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            Quaternion rotation = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
            ClearHUD.SetPositionAndRotation(position, rotation);
        }
        
        SendCustomEventDelayedSeconds("HideHUD", 5.0f); // 5秒後にHUDを非表示にする
    }

    public void HideHUD()
    {
        isHUDActive = false;
        ClearHUD.localScale = Vector3.zero; // HUDを非表示
    }

    public void ResetGame()
    {
        isGameActive = false;
        timer = playTime;
        score = 0;
        TimeText.text = $"Time: {timer.ToString()}";
        ScoreText.text = $"Score: {score.ToString()}";
    }

  
}
