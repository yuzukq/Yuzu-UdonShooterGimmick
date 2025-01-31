/*メモ
[同期について]
タイミング同期(関数同期) :クライアント全員その関数を実行する．
SendCustomNetworkEvent(インスタンス全員/ オーナー, 　"実行する関数名")
引数を持った関数を実行することはできない点に注意. 実行時点でいないクライアントはもちろんその関数を実行しないのでレイトジョインに対応できない．


データ同期(変数同期) : オブジェクトのオーナーを基準に各プレーヤーが持つローカル変数を同期する
[UdonSynced] int Syncvariable おまじない(プロパティ)

RequestSerialization();同期送信
OnDeserialization();同期受信
*/
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections.Generic;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class GridShotManager : UdonSharpBehaviour
{
    //--------------------------------
    [Header("ゲーム設定")]
    [SerializeField] float playTime = 10f;
    [SerializeField] int scorePerHit = 100;
    [SerializeField] int pointRate = 100;

    [Header("ターゲットオブジェクト")]
    public GameObject gridShotTarget;

    [Header("ターゲット生成場所")]
    public Transform[] spawnPlaces;

    [Header("ゲームUI設定")]
    [SerializeField] Text ScoreText;
    [SerializeField] Text TimeText;
    [SerializeField] Text BestScoreText;

    [Header("オーディオ設定")]
    [SerializeField] AudioSource GameSESource;
    [SerializeField] AudioClip HUDSEClip;
    [SerializeField] AudioClip TimerSEClip;
    
    [Header("HUD設定")]
    [SerializeField] Transform ClearHUD;
    [SerializeField] Text ClearScoreText;
    [SerializeField] Text GetPointText;
    

    [Header("ゲームオブジェクト")]
    [SerializeField] GridHit gridHit;
    [SerializeField] ShopManager shopManager;
    
    //--------------------------------
    
    [UdonSynced] int SyncedBestScore = 0;
    [UdonSynced] string SyncedBestPlayerName = "";
    private float timer;
    private bool isGameActive = false;
    private bool isHUDActive = false;
    private int score = 0;
    private int erlierScore = 0;

    //--------------------------------

    private void Start()
    {  
        timer = playTime;
        ScoreText.text = $"Score: {score.ToString()}"; //canvasに表示
        TimeText.text = $"Time: {timer.ToString()}";
        BestScoreText.text = "None";
        ClearHUD.localScale = Vector3.zero;//HUDをスケール0で非表示

        //以下デバッグ用
        //EndGame();
    }

    private void Update()
    {
        //まれにインスタンスにプレイヤーが入って来た時にプレイヤーに関するデータが壊れていることがあるのでエラー回避
        if(!Utilities.IsValid(Networking.LocalPlayer)){return;}

        //HUDの位置を常にプレイヤーの頭の位置に合わせる
        Vector3 position = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        Quaternion rotation = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
        ClearHUD.SetPositionAndRotation(position, rotation);

        if (isGameActive == true)
        {
            timer -= Time.deltaTime;
            TimeText.text = $"Time: {timer.ToString()}";
            
            if(timer == 3.0f || timer == 2.0f || timer == 1.0f)
            {
                GameSESource.PlayOneShot(TimerSEClip);
            }
            if (timer <= 0)
            {
                EndGame();
            }
        }

    }

    public void AddScore()
    {
        score += scorePerHit;
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
        erlierScore = score; //プレイ結果を保持
        if(erlierScore > SyncedBestScore) //プレイ結果がベストスコアより高かったら
        {
            UpdateBestScore(); //ベストスコア更新
        }

        //スコアに応じてショップマネージャーにポイント付与
        shopManager.upGradePoints += erlierScore / pointRate;
        shopManager.UpdateCurrentPoints();
    
        //初期化
        isGameActive = false;
        gridHit.Interval();
        score = 0;
        timer = playTime;
        //UI更新
        TimeText.text = $"Time: {timer.ToString()}";
        ScoreText.text = $"Score: {score.ToString()}";
        
        
        
        //----------------HUD表示-------------------------------
        ClearScoreText.text = $"Score: {erlierScore.ToString()}";
        GetPointText.text = $"Point: {shopManager.upGradePoints.ToString()}pt";
        isHUDActive = true;
        GameSESource.PlayOneShot(HUDSEClip);
        ClearHUD.localScale = Vector3.one; // HUDを表示

        // HUDの位置をプレイヤーの頭の位置に合わせる
        if (Utilities.IsValid(Networking.LocalPlayer))
        {
            Vector3 position = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            Quaternion rotation = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
            ClearHUD.SetPositionAndRotation(position, rotation);
        }
        
        SendCustomEventDelayedSeconds("HideHUD", 5.0f); // 5秒後にHUDを非表示にする
        //-----------------------------------------------
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

    public void UpdateBestScore()   //ベストスコアを更新する
    {
        //スコアと名前をデータ同期
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        SyncedBestScore = score;
        SyncedBestPlayerName = Networking.LocalPlayer.displayName;
        Sync();
    }

    public void Sync()
    {
        RequestSerialization();

        if (Networking.GetOwner(this.gameObject) == Networking.LocalPlayer)
        {
            OnDeserialization();
        }
    }

    public override void OnDeserialization()
    {
        BestScoreText.text = $"{SyncedBestPlayerName}: {SyncedBestScore.ToString()}";
    }

  
}

