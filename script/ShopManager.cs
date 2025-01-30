
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ShopManager : UdonSharpBehaviour
{
    //--------------------------------
    [Header("アップグレードUI操作レール")]
    [SerializeField] private Transform handle;
    [SerializeField] private float minX = 0f; //レールの開始位置
    [SerializeField] private float maxX = 0.5f; //レールの終了位置

    [Header("アップグレードUIのスケール調整")]
    [SerializeField] private Transform scaleUI;
    [SerializeField] private float minScale = 0f; //スケールの最小値
    [SerializeField] private float maxScale = 1f; //スケールの最大値

    [Header("UI中身")]
    [SerializeField] private Image VerocityUpGradeGauge;

    [Header("サウンド設定")]
    [SerializeField] private AudioSource upgradeSoundSource;
    [SerializeField] private AudioClip upgradeSoundClip;

    [Header("gunController")]
    [SerializeField] private GunController gunController;
    [SerializeField] private Animator lazerAnimator;

    //--------------------------------
    private int VerocityUpgradeCount = 0;
    private bool isLazerUpGrade = false;

    //--------------------------------

    private void Update()
    {
        Vector3 handleLocalPosition = handle.localPosition;
        
    
        handleLocalPosition.x = Mathf.Clamp(handleLocalPosition.x, minX, maxX); //X軸の範囲をクランプ
        handleLocalPosition.y = 0f; //Y軸を固定
        handleLocalPosition.z = 0f; //Z軸を固定
        
        handle.localPosition = handleLocalPosition; //ポジションに反映
        
        handle.localRotation = Quaternion.identity; //回転軸を(0, 0, 0)に固定

        float scaleValue = Mathf.InverseLerp(minX, maxX, handleLocalPosition.x); //基準点からの移動量でUpGradeUIのスケールを変更
        scaleUI.localScale = Vector3.one * scaleValue;
    }

    public void VerocityUpGrade()
    {
        if(VerocityUpgradeCount > 5){return;}
        VerocityUpgradeCount++;
        VerocityUpGradeGauge.fillAmount = (float)VerocityUpgradeCount / 5.0f;

        upgradeSoundSource.PlayOneShot(upgradeSoundClip);
        gunController.IncreasedBulletVelocity();
    }

    public void LazerUpGrade()
    {
        if(isLazerUpGrade){return;}
        isLazerUpGrade = true;
        lazerAnimator.SetTrigger("UpGrade2");
        upgradeSoundSource.PlayOneShot(upgradeSoundClip);
    }
    
}

