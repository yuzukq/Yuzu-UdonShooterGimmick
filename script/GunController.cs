using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class GunController : UdonSharpBehaviour
{
    ParticleSystem particleSystem; //弾丸のパーティクル

    [Header("Gun status")]
    [SerializeField] private int maxAmmo = 10; //最大弾数
    [SerializeField] private int currentAmmo; //現在の弾数
    [SerializeField] private Animator GunAnimator;

    

    [Header("Gun UI")]
    [SerializeField] private Text ammoText; //現在の弾数表示用
    //[SerializeField] private Image UpGradeGauge;

    [Header("Gun Sound")]
    [SerializeField] private AudioSource gunSoundSource;
    [SerializeField] private AudioClip fireSoundClip;
    [SerializeField] private AudioClip emptyFireSoundClip;
    [SerializeField] private AudioClip reloadSoundClip;
    //[SerializeField] private AudioSource upgradeSound; // アップグレード音
    
    public bool isPickup = false; 
    //private int upgradeCount = 0;
    
    private int fireFrameCounter = 0; // フレームカウンター
    
    
    

    private void Start()   
    {
        particleSystem = GetComponent<ParticleSystem>(); //弾のパーティクルを取得
        var mainModule = particleSystem.main; 
        mainModule.startSpeed = 50f;
        UpdateAmmoUI(); //弾数表示を更新
    }
    
    public override void OnPickup()
    {
        isPickup = true;
    }

    public override void OnDrop()
    {
        isPickup = false;
    }

    public override void OnPickupUseDown()  //オブジェクトを持ってUseする
    {
        if (currentAmmo > 0) // 残弾がある場合
        {
            particleSystem.Play();  // 弾のパーティクルを発射
            GunAnimator.SetTrigger("Fire");
            PlayHaptics(); //コントローラを振動させる
            currentAmmo--; // 残弾を減らす
            UpdateAmmoUI(); // 弾数表示を更新
            gunSoundSource.PlayOneShot(fireSoundClip); // 発砲音を再生
            
        }
        else
        {
            gunSoundSource.PlayOneShot(emptyFireSoundClip); // 弾切れ時の音を再生
        }
    }

    

    private void UpdateAmmoUI()
    {
        ammoText.text = $"{currentAmmo}/{maxAmmo}";
    }

    public void OnParticleCollision(GameObject other)   //被弾判定が発生した
    {
        if (!Utilities.IsValid(other)) { return; }

        //GridHitクラス                            GridHitというスクリプト(コンポーネント)
        GridHit _targetHitBox = other.GetComponent<GridHit>();    //被弾相手にターゲット用のスクリプトがあれば、被弾エフェクトを出す
        if (_targetHitBox) { _targetHitBox.Hit(); }
    }

    public void PlayHaptics()   //両手のコントローラを振動させる
    {
        float duration = 1.0f;
        float amplitude = 1.0f;
        float frequency = 1.0f;
        //Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Left, duration, amplitude, frequency);
        Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Right, duration, amplitude, frequency);
    }

    public void Reload()
    {
        PlayHaptics();
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        gunSoundSource.PlayOneShot(reloadSoundClip); // リロード音を再生
    }

    public void IncreasedBulletVelocity()
    {
        //upgradeCount++;
        //float gauge = (float)upgradeCount / 5.0f;
        //UpGradeGauge.fillAmount = gauge;
        //if(upgradeCount > 5){return;}
        //upgradeSound.Play(); // アップグレード音を再生
        var mainModule = particleSystem.main;
        var currentSpeed = mainModule.startSpeed.constant;
        mainModule.startSpeed = currentSpeed + 50f;
    }

}
