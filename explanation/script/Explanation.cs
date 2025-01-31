using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Explanation : UdonSharpBehaviour
{
    public AudioSource explanationAudio; // 説明音声のAudioSource
    public GameObject nextExplanationObject; // 次の説明用オブジェクト

    private bool isPlayed = false;

    void Start()
    {
        // 初期設定
        if (nextExplanationObject != null)
        {
            nextExplanationObject.SetActive(false); // 次のオブジェクトを非表示に
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (!player.isLocal) { return; } // プレイヤーがコライダーに入ったか確認
        if (isPlayed) { return; }
        isPlayed = true;
        if (explanationAudio != null)
        {
            explanationAudio.Play(); // 説明音声を再生
            SendCustomEventDelayedSeconds(nameof(ActivateNextObject), explanationAudio.clip.length); // 音声終了後に次のオブジェクトを表示
        }
    }

    public void ActivateNextObject()
    {
        //if (nextExplanationObject != null)
        //{
            nextExplanationObject.SetActive(true); // 次のオブジェクトを表示
        //}
        Destroy(gameObject); // 現在のオブジェクトを削除
    }
}
