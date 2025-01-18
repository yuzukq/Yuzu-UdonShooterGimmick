
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GunController : UdonSharpBehaviour
{
    ParticleSystem particleSystem;

    private void Start()    //初期化
    {
        particleSystem = GetComponent<ParticleSystem>(); //弾のパーティクルを取得
    }

    public override void OnPickupUseDown()  //オブジェクトを持ってUseする
    {
        particleSystem.Play();  //弾のパーティクルを発射
    }

    public void OnParticleCollision(GameObject other)   //被弾判定が発生した
    {
        if (!Utilities.IsValid(other)) { return; }

        //GridHitクラス                            GridHitというスクリプト(コンポーネント)
        GridHit _targetHitBox = other.GetComponent<GridHit>();    //被弾相手にターゲット用のスクリプトがあれば、被弾エフェクトを出す
        if (_targetHitBox) { _targetHitBox.Hit(); }
    }

}
