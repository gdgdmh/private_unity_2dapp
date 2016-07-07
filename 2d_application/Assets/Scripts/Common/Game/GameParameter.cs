using UnityEngine;
using System.Collections;

/**
 * ゲームとパラメータのやり取りをするクラス
 */
public class GameParameter {

    public GameParameter() {
        stage_data_.Initialize();
        is_clear_ = false;
    }

    public void Initialize() {
        stage_data_.Initialize();
        is_clear_ = false;
    }

    // ステージ関連のデータ
    private StageData stage_data_ = new StageData();
    // ステージをクリアしたか
    private bool is_clear_ { set; get; }
}
