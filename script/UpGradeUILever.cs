
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UpGradeUILever : UdonSharpBehaviour
{
    [SerializeField] private Transform handle;
    
    [SerializeField] private Transform upGradeUI; 

    [SerializeField] private float minX = 0f; //レールの開始位置
    [SerializeField] private float maxX = 0.5f; //レールの終了位置

    private void Update()
    {
        // 持ち手のローカル位置を取得
        Vector3 handleLocalPosition = handle.localPosition;
        handle.localPosition = handleLocalPosition;
        handleLocalPosition.x = Mathf.Clamp(handleLocalPosition.x, minX, maxX);

        
        handleLocalPosition.y = 0f; //軸固定
        handleLocalPosition.z = 0f; 
        handle.localRotation = Quaternion.identity; //回転軸を(0, 0, 0)に固定

        //基準点からの移動量でUpGradeUIのスケールを変更
        float scaleValue = Mathf.InverseLerp(minX, maxX, handleLocalPosition.x);
        upGradeUI.localScale = Vector3.one * scaleValue;
    }
}
