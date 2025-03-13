﻿
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
    [SerializeField] private float maxX = 0.001028f; //レールの終了位置

    [Header("アップグレードUIのスケール調整")]
    [SerializeField] private Transform scaleUI;
    [SerializeField] private float minScale = 0f; //スケールの最小値
    [SerializeField] private float maxScale = 1f; //スケールの最大値

    [Header("UI中身")]
    [SerializeField] private Image VerocityUpGradeGauge;
    [SerializeField] private Image LazerUpGradeGauge;
    [SerializeField] private Text upGradePointsText;
    [SerializeField] private Text UserNameText;
    [SerializeField] private Text BulletPriceText;
    [SerializeField] private Text LazerPriceText;

    [Header("サウンド設定")]
    [SerializeField] private AudioSource upgradeSoundSource;
    [SerializeField] private AudioClip upgradeSoundClip;
    [SerializeField] private AudioClip cantUpgradeSoundClip;
    [SerializeField] private AudioClip shopOpenSoundClip;
 
    [Header("gunController")]
    [SerializeField] private GunController gunController;
    [SerializeField] private Animator lazerAnimator;

    [Header("価格設定")]
    [SerializeField] private int verocityUpgradePrice = 5;
    [SerializeField] private int lazerUpgradePrice = 10;

    //--------------------------------
    private int verocityUpgradeCount = 0;
    public int upGradePoints = 0;
    private bool isLazerUpGrade = false;

    //--------------------------------

    private void Start()
    {
        UpdateCurrentPoints();
        UserNameText.text = $"ID: {Networking.LocalPlayer.displayName}";
        BulletPriceText.text = $"Use: {verocityUpgradePrice.ToString()}pt";
        LazerPriceText.text = $"Use: {lazerUpgradePrice.ToString()}pt";
    }

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
        if(verocityUpgradeCount > 5)//5回までしかアップグレードできない
        {
            upgradeSoundSource.PlayOneShot(cantUpgradeSoundClip);
            return;
        } 
        if(upGradePoints < verocityUpgradePrice)//ポイントが足りない
        {
            upgradeSoundSource.PlayOneShot(cantUpgradeSoundClip);
            return;
        } 

        upGradePoints -= verocityUpgradePrice; //ポイント減少
        UpdateCurrentPoints();
        verocityUpgradeCount++;
        VerocityUpGradeGauge.fillAmount = (float)verocityUpgradeCount / 5.0f;//ゲージに反映

        upgradeSoundSource.PlayOneShot(upgradeSoundClip);
        gunController.IncreasedBulletVelocity();
    }

    public void LazerUpGrade()
    {
        if(isLazerUpGrade)//すでにアップグレード済み
        {
            upgradeSoundSource.PlayOneShot(cantUpgradeSoundClip);
            return;
        } 
        if(upGradePoints < lazerUpgradePrice)//ポイントが足りない
        {
            upgradeSoundSource.PlayOneShot(cantUpgradeSoundClip);
            return;
        } 

        upGradePoints -= lazerUpgradePrice; //ポイント減少
        UpdateCurrentPoints();
        isLazerUpGrade = true;
        lazerAnimator.SetTrigger("UpGrade2");
        upgradeSoundSource.PlayOneShot(upgradeSoundClip);
        LazerUpGradeGauge.fillAmount = 1.0f;
    }

    public void UpdateCurrentPoints()
    {
        upGradePointsText.text = $"Point: {upGradePoints.ToString()}pt";
    }
}