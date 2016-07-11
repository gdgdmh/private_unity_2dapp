using UnityEngine;
using System.Collections;

/**
 * シーン間共有データ(シングルトン)
 */
public class ShareData : SingletonMonoBehaviour<ShareData> {

    void Awake() {
        if (this != Instance) {
            // 既に存在しているなら破棄
            Destroy(this);
        } else {
            // シーン遷移破棄させない
            DontDestroyOnLoad(this.gameObject);
        }
        Initialize();
    }

    void OnDestroy() {
        // 破棄しておく
        game_parameter_ = null;
    }

    void Initialize() {
        game_parameter_ = new GameParameter();
        game_parameter_.Initialize();

    }
    
    // ゲーム間でやり取りするデータ
    public GameParameter game_parameter_ { set; get; }
}
