﻿using UnityEngine;
using System.Collections;

public class LogData {
    // 앱 접속 시간: 1, 게임 시작 시간: 2, 게임 플레이 시간: 3, 앱 종료 시간: 4
    public int logCmd { get; set; }
    public string email { get; set; }
    public string date { get; set; }
}
