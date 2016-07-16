using UnityEngine;
using System.Collections;

public class UserData {
    public string uid { get; set; }
    public string email { get; set; }
    public long money { get; set; }
    public int heart { get; set; } // 하트 개수
    public int charge { get; set; } // 남은 충전 시간
    // 상자 데이터, 뽑은 아이템들
}
