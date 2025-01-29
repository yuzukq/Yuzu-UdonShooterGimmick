using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Magazine : UdonSharpBehaviour
{
    [Header("Magazine")]
    [SerializeField] GameObject magazine;
    [SerializeField] float destroyTime = 10.0f;
    [SerializeField] float spawnSideOffset = -0.2f;

    

    
    public override void Interact()
    {
        if (magazine == null) { return; } //エラー回避 magazineがnullであればreturn

        GameObject newObject = Object.Instantiate(magazine);
        
        
        // 生成位置の設定
        newObject.transform.position = this.transform.position + new Vector3(spawnSideOffset, 0, 0); // x軸方向に-0.2fずらす
        
        // 親オブジェクトにスケールをかけているためスケールを調整
        Vector3 parentScale = this.transform.lossyScale;
        newObject.transform.localScale = new Vector3(
            magazine.transform.localScale.x / parentScale.x,
            magazine.transform.localScale.y / parentScale.y,
            magazine.transform.localScale.z / parentScale.z
        );

        InstantiateObject instantiateObject = newObject.GetComponent<InstantiateObject>();
        if (instantiateObject != null) { instantiateObject.SetAsCloned(); }//エラー回避 InstantiateObjectクラスのSetAsCloned()メソッドを呼び出す
        

        Destroy(newObject, destroyTime);
    }
    
}
