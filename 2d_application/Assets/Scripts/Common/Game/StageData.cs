using UnityEngine;
using System.Collections;

public class StageData {

    public StageData() {
        stage_ = StageDefine.GetTutorialStage();
    }

    public void Initialize() {
        stage_ = StageDefine.GetTutorialStage();
    }

    /**
     * 次のステージをセットする
     * bool 次のステージへ移行できるならtrue
     */
    public bool SetNextStage() {
        if (StageDefine.IsAllStageClear(stage_) == true) {
            // 全ステージクリアしている
            return false;
        }
        // 次のステージへ移行
        stage_ = StageDefine.GetNextStage(stage_);
        return true;
    }

    // ステージ
    private StageDefine.Stage stage_ { set; get; }
}
