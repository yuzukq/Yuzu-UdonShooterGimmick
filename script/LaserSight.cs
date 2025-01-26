using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LaserSight : UdonSharpBehaviour
{
    [SerializeField] Transform RaycastTransform; // レーザーの発射位置と方向を決定するTransform
    [SerializeField] float range; // レーザーの最大射程距離
    [SerializeField] LineRenderer lineRenderer; // レーザーを描画するためのLineRendererコンポーネント
    private bool isLaserActive = false; // レーザーがアクティブかどうかを示すフラグ

    // オブジェクトがUSEされたときに呼び出されるメソッド
    public override void Interact()
    {
        ToggleLaser(); // レーザーのオン/オフを切り替える
    }

    // レーザーのオン/オフを切り替えるメソッド
    private void ToggleLaser()
    {
        isLaserActive = !isLaserActive; // フラグを反転
        lineRenderer.enabled = isLaserActive; // LineRendererの有効/無効を切り替え
    }

    // 毎フレーム呼び出されるメソッド
    private void Update()
    {
        if (isLaserActive) // レーザーがアクティブな場合
        {
            DrawLaser(); // レーザーを描画する
        }
    }

    // レーザーを描画するメソッド
    private void DrawLaser()
    {
        RaycastHit hit; // Raycastの結果を格納する変数
        // レーザーの終点を計算（初期値は最大射程距離）
        Vector3 endPosition = RaycastTransform.position + RaycastTransform.forward * range;

        // Raycastを実行し、何かに当たった場合はその点を終点とする
        if (Physics.Raycast(RaycastTransform.position, RaycastTransform.forward, out hit, range))
        {
            endPosition = hit.point; // 当たった点を終点に設定
        }

        // LineRendererの始点と終点を設定
        lineRenderer.SetPosition(0, RaycastTransform.position); // 始点
        lineRenderer.SetPosition(1, endPosition); // 終点
    }
}
