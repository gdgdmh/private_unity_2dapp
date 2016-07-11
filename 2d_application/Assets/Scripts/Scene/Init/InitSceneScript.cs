using UnityEngine;
using System.Collections;

public class InitSceneScript : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        // 共有データの初期化
        // 重いデータがあるかもしれないのでスレッドを使用したかったけど、
        // シングルトンに対してスレッドを使用できなかったので現状は使わない
        //ShareData.Instance.Initialize();
        // 次のシーンへ
        UnityEngine.SceneManagement.SceneManager.LoadScene("debug_menu");
    }

    void OnDestroy() {
    }

}
