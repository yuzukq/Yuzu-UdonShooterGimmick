using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class UpGradeUILever : UdonSharpBehaviour
{
    [SerializeField] private Transform handle;
    
    [SerializeField] private Transform upGradeUI; 

    [SerializeField] private float minX = 0f; //レールの開始位置
    [SerializeField] private float maxX = 0.5f; //レールの終了位置

    private void Update()
    {
        Vector3 handleLocalPosition = handle.localPosition;
        
    
        handleLocalPosition.x = Mathf.Clamp(handleLocalPosition.x, minX, maxX); //X軸の範囲をクランプ
        handleLocalPosition.y = 0f; //Y軸を固定
        handleLocalPosition.z = 0f; //Z軸を固定
        
        handle.localPosition = handleLocalPosition; //ポジションに反映
        
        handle.localRotation = Quaternion.identity; //回転軸を(0, 0, 0)に固定

        float scaleValue = Mathf.InverseLerp(minX, maxX, handleLocalPosition.x); //基準点からの移動量でUpGradeUIのスケールを変更
        upGradeUI.localScale = Vector3.one * scaleValue;
    }
}
