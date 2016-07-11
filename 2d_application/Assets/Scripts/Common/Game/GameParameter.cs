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

    /**
     * ゲーム起動時に行う初期化
     */
    public void Initialize() {
        stage_data_.Initialize();
        is_clear_ = false;
        is_finish_ = false;
    }

    /**
     * コンティニュー用、再度ゲームを開始するための設定
     */
    public void Restart() {
        is_clear_ = false;
        is_finish_ = false;
    }

    /**
     * 次のステージへ移行する
     * trueなら次のステージへ進行
     * falseならゲームクリア
     */
    public bool SetNextStage() {
        is_clear_ = false;
        is_finish_ = false;
        return stage_data_.SetNextStage();
    }

    /**
     * ステージをクリアしたときの処理
     */
    public void SetSuccess() {
        // クリア
        is_clear_ = true;
        // 結果が出たのでtrue
        is_finish_ = true;
    }

    /**
     * ステージを失敗したときの処理
     */
    public void SetFailure() {
        // クリアしてない
        is_clear_ = false;
        // 結果が出たのでtrue
        is_finish_ = true;
    }


    // ステージ関連のデータ
    public StageData stage_data_ = new StageData();
    // ステージをクリアしたか
    public bool is_clear_ { set; get; }
    // 勝利または敗北条件を満たしたか
    public bool is_finish_ { set; get; }
}
