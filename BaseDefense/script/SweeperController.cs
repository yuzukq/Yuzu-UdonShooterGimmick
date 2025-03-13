using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.AI;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SweeperController : UdonSharpBehaviour
{
    [Header("エージェント設定")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Image sweeperHpBar;
    [SerializeField] PlayerManager playerManager;
    

    [Header("リーチ\nAgentのストップ距離よりも短い値を設定すると\n攻撃モーションが発動しないので注意")]
    [SerializeField] float attackRange = 2.0f;

    [Header("攻撃力")]
    [SerializeField] int sweeperAttackPower = 10;

    [Header("HP")]
    [SerializeField] int maxHp = 100;
    int currentHp;

    private void Start()
    {
        currentHp = maxHp;
        sweeperHpBar.fillAmount = (float)currentHp / maxHp;
    }
    
    private void Update()
    {
        if(!Utilities.IsValid(Networking.LocalPlayer)) { return; }//エラー回避


        float distance = Vector3.Distance(transform.position, Networking.LocalPlayer.GetPosition());
        if(distance > attackRange)
        {
            agent.SetDestination(Networking.LocalPlayer.GetPosition());
            animator.SetBool("IsMoving", agent.velocity.magnitude > 0.1f);
            animator.SetBool("IsAttacking", false);
        }
        

        if(distance <= attackRange) 
        {
            agent.ResetPath();
            animator.SetBool("IsAttacking", true);//攻撃モーション発動(ループアニメーション)

            //30フレームごとに攻撃を行う
            if(Time.frameCount % 100 == 0)
            {
                Attack();
            }
        }

    }

    public void Attack()
    {
        playerManager.Damaged(sweeperAttackPower);
    }
        
    public void Damaged(int damage)
    {
        currentHp -= damage;
        sweeperHpBar.fillAmount = (float)currentHp / maxHp;
        
        if(currentHp <= 0)
        {
            currentHp = 0;
            
            agent.speed = 0; // エージェントのSpeedを0にして停止
            animator.SetTrigger("Dead");

            //SendCustomEventDelayedSecondsで呼び出す関数はパブリックである必要があるため注意!
            SendCustomEventDelayedSeconds(nameof(DestroyThisObject), 2.5f);
            
        }
    }

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}