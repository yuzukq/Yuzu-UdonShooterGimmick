using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ResetButton : UdonSharpBehaviour
{
    private GridShotManager gameManage = default; // ゲーム管理のクラス

    private void Start()
    {
        gameManage = transform.parent.GetComponent<GridShotManager>(); // ゲーム管理のクラスを取得
    }

    public override void Interact() // ボタンを押すと
    {
        gameManage.ResetGame(); // ゲームをリセット関数呼び出し
    }
}
