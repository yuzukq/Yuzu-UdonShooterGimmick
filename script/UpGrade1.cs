
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UpGrade1 : UdonSharpBehaviour
{
    [SerializeField] private GunController gunController;


    public override void Interact()
    {
        gunController.IncreasedBulletVelocity();
    }
}
