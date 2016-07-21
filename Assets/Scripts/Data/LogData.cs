using UnityEngine;
using System.Collections;

public class LogData {
    // 앱 접속 시간: 1, 게임 시작 시간: 2, 아이템 얻음: 3, 앱 종료 시간: 4
    public int logCmd { get; set; }
    public string email { get; set; }
    public int getItemIdx { get; set; }
    public string date { get; set; }

}
