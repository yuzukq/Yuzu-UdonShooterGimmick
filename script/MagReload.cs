
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MagReload : UdonSharpBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (!Utilities.IsValid(other)) { return; }

        GunController gunController = other.GetComponent<GunController>();
        
        if (gunController)
        {
            gunController.Reload();
        }

        Destroy(this.gameObject);
    }
}
