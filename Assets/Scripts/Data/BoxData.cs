using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxData {
    // 이름
    // id
    // 강화 비용
    // 강화 확률 시작
    // 강화 확률 끝
    // 아이템 리스트
    // 강화 성공시 다음 박스 id
    public string name { get; set; }
    public string boxId { get; set; }
    public int cost { get; set; }
    public int rangeStart { get; set; }
    public int rangeEnd { get; set; }
    public List<GiftItem> itemList { get; set; }
    public string nextId { get; set; }
}
