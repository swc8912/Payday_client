using UnityEngine;
using System.Collections;

public class UserData {
    //public string uid { get; set; }
    public string did { get; set; } // 디바이스 id
    public string email { get; set; }
    public int money { get; set; } // 가진 돈 단위 만원
    public string rank { get; set; } // 직급 인턴 사원 대리 과장 차장 부장 이사 상무 전무 사장 회장
    public int heart { get; set; } // 하트 개수
    public int charge { get; set; } // 남은 충전 시간 초
    public string currentBoxId { get; set; } // 상자 데이터
    public ArrayList pickItems { get; set; } // 뽑은 아이템들
    public bool getPush { get; set; } // 푸시 수신 여부

}
