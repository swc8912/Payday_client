using UnityEngine;
using System.Collections;

public class GiftItem {
    /*
     *  "description" : "4만원이다.",
     *      "index" : 10001,
     *      "rangeStart" : 1,
     *      "rangeEnd" : 4000,
     *      "text" : "4만원",
     *      "type" : 1,
     *      "value" : 4
     */
    public string description { get; set; }
    public int index { get; set; }
    public int rangeStart { get; set; }
    public int rangeEnd { get; set; }
    public string text { get; set; }
    public int type { get; set; } // 돈인가 아닌가
    public int value { get; set; } // 돈이면 그 값
}
