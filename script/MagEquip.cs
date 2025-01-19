using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MagEquip : UdonSharpBehaviour
{
    [SerializeField] Transform MagagineBelt;
    bool IsEquiped = false;

    [Header("Equip Sound")]
    [SerializeField] private AudioSource equipSound; // 装填音

    public override void Interact()
    {
        IsEquiped = !IsEquiped;
        equipSound.Play(); // 装填音を再生
    }   
    
    void Update()
    {
           
        if(!IsEquiped) 
        { 
            return; 
        }
        if(!Utilities.IsValid(Networking.LocalPlayer))
        { 
            return; //エラー回避
        }

        //ベルトの位置と向きをプレイヤーのHipsに合わせる
        MagagineBelt.transform.position = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Hips);
        MagagineBelt.transform.rotation = Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.Hips);
       
    }
}

