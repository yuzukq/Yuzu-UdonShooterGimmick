using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GridHit : UdonSharpBehaviour
{
    ParticleSystem particleSystem;
    Animator animator;
    AudioSource audioSource;

    private GridShotManager gameManage = default; // ゲーム管理のクラス
    
    private void Start()    
    {
        //trasform.parent: 親オブジェクト
        gameManage = transform.parent.GetComponent<GridShotManager>(); // ゲーム管理のクラスを取得

        particleSystem = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        //以下デバッグ用
        //gameManage.AddScore();
        //gameManage.SpawnTarget();
        //Hit();
    }
    
    public void Hit()   //被弾エフェクトと効果音を出す
    {
        GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("Hit");
        audioSource.PlayOneShot(audioSource.clip);
    
        gameManage.StartGame(); // ゲームを開始
        
        gameManage.AddScore(); // スコアを加算
        gameManage.SwapTarget(); // ターゲットを移動

        
    }
    
}
