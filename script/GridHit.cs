
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GridHit : UdonSharpBehaviour
{
    ParticleSystem particleSystem;
    Animator animator;
    AudioSource audioSource;

    private GridShotManager gameManage = default;
    
    private void Start()    //初期化
    {
        //trasform.parent: 親オブジェクト
        gameManage = transform.parent.GetComponent<GridShotManager>();

        particleSystem = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        //以下デバッグ用
        //gameManage.AddScore();
        //gameManage.SpawnTarget();
    }
    
    public void Hit()   //被弾エフェクトと効果音を出す
    {
        GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("Hit");
        audioSource.PlayOneShot(audioSource.clip);

        gameManage.AddScore();
        gameManage.SpawnTarget();

    }
    
}
