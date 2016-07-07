using UnityEngine;
using System.Collections;

/**
 * ゲームのステージに関する定義
 */
public class StageDefine : MonoBehaviour {
    // ステージ
    public enum Stage {
        kTutorial,
        k1_1,
        k1_2,
        k1_3,
        k1_4,
        k2_1,
        kMax    // 最終ステージクリア後にこの値になる
    };

    public static Stage GetTutorialStage() {
        return Stage.kTutorial;
    }

    /**
     * 指定ステージが最終ステージかどうかを返す
     * (kMaxは最終ステージの意味ではなく最終ステージクリア後の値)
     */
    public static bool IsLastStage(Stage stage) {
        if (stage == (Stage.kMax - 1)) {
            return true;
        }
        return false;
    }

    public static bool IsAllStageClear(Stage stage) {
        if (stage == Stage.kMax) {
            return true;
        }
        return false;
    }

    /**
     * 次のステージを取得
     * kMaxのときはそのまま返す
     */
    public static Stage GetNextStage(Stage stage) {
        if (stage == Stage.kMax) {
            return stage;
        }
        return stage + 1;
    }

}
