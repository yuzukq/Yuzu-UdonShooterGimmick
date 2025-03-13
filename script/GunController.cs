using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class GunController : UdonSharpBehaviour
{
    ParticleSystem bulletParticle; //弾丸のパーティクル

    [Header("銃のステータス")]
    [SerializeField] private int maxAmmo = 10; //最大弾数
    [SerializeField] private int currentAmmo; //現在の弾数
    [SerializeField] private int bulletAttackPower = 10; //攻撃力
    [SerializeField] private Animator GunAnimator;

    [Header("銃呼び出し用")]
    [SerializeField] private VRC_Pickup pickupGun;
    

    

    [Header("銃UI")]
    [SerializeField] private Text ammoText; //現在の弾数表示用
    //[SerializeField] private Image UpGradeGauge;

    [Header("銃音")]
    [SerializeField] private AudioSource gunSoundSource;
    [SerializeField] private AudioClip fireSoundClip;
    [SerializeField] private AudioClip emptyFireSoundClip;
    [SerializeField] private AudioClip reloadSoundClip;
    //[SerializeField] private AudioSource upgradeSound; // アップグレード音
    
    public bool isPickup = false; 
    bool inVR;
    

    private void Start()   
    {
        bulletParticle = GetComponent<ParticleSystem>(); //弾のパーティクルを取得
        var mainModule = bulletParticle.main; 
        mainModule.startSpeed = 50f;
        UpdateAmmoUI(); //弾数表示を更新

        if(Networking.LocalPlayer.IsUserInVR() )
        {
            inVR = true;
        }
    }

    private void Update()
    {
        if(inVR) {return;}
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //現在の頭の位置と向きを取得 -> 呼び出し関数に渡す
            Vector3 HeadPos = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            Quaternion HeadRot = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
            SummonWeapon(HeadPos, HeadRot);//1を押したら銃を呼び出す
        }
    }

    public void SummonWeapon(Vector3 HeadPos, Quaternion HeadRot)
    {
        //現在の頭の位置プラス1m前方に銃を移動させる
        pickupGun.transform.position = HeadPos + HeadRot * Vector3.forward; //forward: Vector3(0, 0, 1)と同義
        pickupGun.transform.rotation = HeadRot;
    }
    
    public override void OnPickup()
    {
        isPickup = true;
    }

    public override void OnDrop()
    {
        isPickup = false;
    }

    public override void InputGrab(bool value, UdonInputEventArgs args) //コントローラを掴む
    {
        TrySummonWeapon(value, args);
    }

    public void TrySummonWeapon(bool value, UdonInputEventArgs args) //オブジェクトを呼び出せるか判定する
    {
        if(!inVR) {return;}

        Vector3 HandPos;
        if(args.handType == HandType.LEFT)
        {
            HandPos = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).position;
        }
        else
        {   
            HandPos = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position;
        }

        //HMDの位置と向きを取得する
        Vector3 HeadPos = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        Quaternion HeadRot = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;

        //Grabした手の位置が、頭の後方にあるかチェックする
        //HandVector.zがマイナスなら頭の後ろ
        Vector3 HandVector = Quaternion.Inverse(HeadRot) * (HandPos - HeadPos);
        if (HandVector.z < 0) { SummonWeapon(HeadPos, HeadRot); }  
    }

    public override void OnPickupUseDown()  //オブジェクトを持ってUseする
    {
        if (currentAmmo > 0) // 残弾がある場合
        {
            bulletParticle.Play();  // 弾のパーティクルを発射
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
        if(_targetHitBox)
        {
            _targetHitBox.Hit();
        }

        SweeperController _sweeperController = other.GetComponent<SweeperController>();
        if(_sweeperController)
        {
            _sweeperController.Damaged(bulletAttackPower);
        }
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
        var mainModule = bulletParticle.main;
        var currentSpeed = mainModule.startSpeed.constant;
        mainModule.startSpeed = currentSpeed + 50f;
    }

}
