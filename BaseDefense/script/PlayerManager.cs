using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PlayerManager : UdonSharpBehaviour
{
    [Header("プレイヤー初期設定")]
    [SerializeField] Image playerHpBar;

    [Header("プレイヤーステータス")]
    [SerializeField] int maxHp = 100;
    int currentHp;

    [Header("HUD設定")]
    public Transform GameHUDPosition;
    public Transform ResultHUD;
    public Text ResultScoreText;
    public Text GetPointText;
    
    [Header("リスポーン位置")]
    [SerializeField] Transform spawnPoint; // スポーンポイントを指定


    //--------------------------------
    private bool isHUDActive = false;
    //--------------------------------

    void Start()
    {
        playerHpBar.fillAmount = 1;
        currentHp = maxHp;

        ResultHUD.localScale = Vector3.zero;
    }

    private void Update()
    {
        if(!Utilities.IsValid(Networking.LocalPlayer)){return;}

        //HUDの入れ物をプレイヤーの頭の位置に合わせる
        Vector3 position = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        Quaternion rotation = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
        GameHUDPosition.SetPositionAndRotation(position, rotation);
    }

    public void Damaged(int damage)
    {
        currentHp -= damage;
        playerHpBar.fillAmount = (float)currentHp / maxHp;

        if(currentHp <= 0)
        {
            currentHp = maxHp;
            playerHpBar.fillAmount = 1;
            
            if(Utilities.IsValid(Networking.LocalPlayer))
            {
                Networking.LocalPlayer.TeleportTo(spawnPoint.position, spawnPoint.rotation); // スポーンポイントにテレポート
            }
        }
    }

    public void ShowGridShotResultHUD()
    {
        isHUDActive = true;
        ResultHUD.localScale = Vector3.one;

        SendCustomEventDelayedSeconds("HideGridShotResultHUD", 5.0f);   
    }

    public void HideGridShotResultHUD()
    {
        isHUDActive = false;
        ResultHUD.localScale = Vector3.zero;
    }
}
