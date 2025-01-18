using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MagEquip : UdonSharpBehaviour
{
    [SerializeField] Transform MagagineBelt;
    bool IsEquiped = false;

    public override void Interact()
    {
        IsEquiped = !IsEquiped;
    }   
    
    void Update()
    {
        if(!Utilities.IsValid(Networking.LocalPlayer))
        { 
            return; //エラー回避
        }   
        if(IsEquiped == false) 
        { 
            return; 
        }
 
        MagagineBelt.position = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Chest);
        Vector3 XVector = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.RightShoulder) - Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LeftShoulder);
        Vector3 YVector = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Neck) - Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Hips);
        Vector3 ZVector = Vector3.Cross(XVector, YVector);
        MagagineBelt.transform.rotation = Quaternion.LookRotation(ZVector, YVector);
    }
}

