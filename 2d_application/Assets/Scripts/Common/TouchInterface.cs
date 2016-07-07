using UnityEngine;
using System.Collections;

public interface TouchInterface {
    // ジェスチャーの処理順番番号
    int Order { get; }

    // 指定ジェスチャーを処理する必要があるか
    bool IsGestureProcess(TouchInfo info);
    // おした時に呼び出される
    void OnTouchDown(TouchInfo info);
    // 持ち上げた時に呼び出される
    void OnTouchUp(TouchInfo info);
    // ドラッグ時に呼び出される
    void OnGestureDrag(TouchInfo info);
    // フリック時に呼び出される
    void OnGestureFlick(TouchInfo info);
}

