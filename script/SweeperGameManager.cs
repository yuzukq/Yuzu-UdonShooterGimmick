
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SweeperGameManager : UdonSharpBehaviour
{
    [Header("ゲーム設定")]
    [SerializeField] float playTime = 300f;
    [SerializeField] int sweeperCount = 0;
    [SerializeField] int sweeperCountMax = 120;
    
    [Header("スイーパープレハブ設定")]
    [SerializeField] GameObject Sweeper001;
    [SerializeField] GameObject Sweeper002;
    [SerializeField] GameObject Sweeper003;

    private int waveCount = 0;
    private float waveTime = 0f;
    
    
}
