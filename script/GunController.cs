using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class GunController : UdonSharpBehaviour
{
    ParticleSystem particleSystem; //弾丸のパーティクル

    [Header("Gun status")]
    [SerializeField] private int maxAmmo = 10; //最大弾数
    [SerializeField] private int currentAmmo; //現在の弾数
    

    [Header("Gun UI")]
    [SerializeField] private Text ammoText; //現在の弾数表示用
    

    

    private void Start()   
    {
        particleSystem = GetComponent<ParticleSystem>(); //弾のパーティクルを取得
        UpdateAmmoUI(); //弾数表示を更新
    }

    public override void OnPickupUseDown()  //オブジェクトを持ってUseする
    {
        if (currentAmmo > 0) // 残弾がある場合
        {
            particleSystem.Play();  // 弾のパーティクルを発射
            PlayHaptics(); //コントローラを振動させる
            currentAmmo--; // 残弾を減らす
            UpdateAmmoUI(); // 弾数表示を更新
        }
        else
        {
            Debug.Log("弾切れ");
            // TODO: リロード処理を後で追加
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
        Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Left, duration, amplitude, frequency);
        Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Right, duration, amplitude, frequency);
    }

}
