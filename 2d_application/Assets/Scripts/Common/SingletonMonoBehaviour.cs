using UnityEngine;
using System.Collections;

// MonoBehaviourのシングルインスタンス
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

    // instance
    private static volatile T instance_;
    // 同期オブジェクト
    private static object sync_object_ = new object();
    // アプリケーションが終了しているかどうか
    private static bool is_quit_ = false;

    // インスタンスの生成を不可能にする
    protected SingletonMonoBehaviour() { }

    void OnApplicationQuit() {
        is_quit_ = true;
    }

    void OnDestroy() {
        instance_ = null;
    }

    public static T Instance {
        get {
            // アプリケーション終了時にはオブジェクト生成をしない
            if (is_quit_ == true) {
                return null;
            }

            // インスタンスがない場合に探す
            if (instance_ == null) {
                instance_ = FindObjectOfType<T>() as T;
                // 見つかった
                if (FindObjectsOfType<T>().Length > 1) {
                    return instance_;
                }
                // なかったので新しくオブジェクトを生成
                if (instance_ == null) {
                    if (sync_object_ != null) {
                        // 同時にインスタンス生成をしないようにロックする
                        lock (sync_object_) {
                            GameObject singleton = new GameObject();
                            // 名前を付ける
                            singleton.name = typeof(T).ToString() + "(singleton)";
                            instance_ = singleton.AddComponent<T>();
                            // シーン変更時に破棄させない
                            DontDestroyOnLoad(singleton);
                        }
                    }
                }
            }
            return instance_;
        }

        private set {
            instance_ = value;
        }
    }



}
