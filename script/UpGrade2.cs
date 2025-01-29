
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]

public class UpGrade2 : UdonSharpBehaviour
{

    [SerializeField] Animator lazerAnimator;


    public override void Interact()
    {
        lazerAnimator.SetTrigger("UpGrade2");
    }
    
}
