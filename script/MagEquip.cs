using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
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
           
        if(!IsEquiped) //マガジンポーチが装備されていないならリターン
        { 
            return; 
        }
        if(!Utilities.IsValid(Networking.LocalPlayer))
        { 
            return; //エラー回避
        }

        DeskTopReload();

        //ベルトの位置と向きをプレイヤーのHipsに合わせる
        MagagineBelt.transform.position = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Hips);
        MagagineBelt.transform.rotation = Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.Hips);
       
    }



    [SerializeField] private GunController gunController;

    private void DeskTopReload()
    {
        if(!gunController.isPickup){return;} //銃を持っていないならリターン

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(gunController != null)
            {
                gunController.Reload();
            }
        }
    }
}

