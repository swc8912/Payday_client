using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;
using System;

public class OpenBox : MonoBehaviour {
    public GameManager gm;

    public void onOpenClick()
    {
        Debug.Log("onopenclick");
        // 상자가 없으면 없다고 메세지창
        if (GameManager.userData.heart <= 0)
        {
            gm.NoHeart();
            return;
        }

        // 랜덤 계산해서 아이템 정함
        // 선물상자 없어지고 해당 아이템 이미지 나옴
        // 대화상자 나와서 ~~를 획득하였다고 나옴
        GetItems gi = new GetItems();
        long val = gi.getRandom();
        Debug.Log("val: " + val);

        GiftItem rightItem = new GiftItem();
        int boxid = Convert.ToInt32(GameManager.userData.currentBoxId) - 1;
        BoxData bd = (BoxData)(GameManager.GiftList[boxid][0]);
        for (int i = 0; i < bd.itemList.Count; i++)
        {
            GiftItem item = (GiftItem)bd.itemList[i];
            if (val >= item.rangeStart && val <= item.rangeEnd)
            {
                Debug.Log(item.text + " 당첨!");
                rightItem = item;
                break;
            }
        }

        gm.SetGiftResult(rightItem);
    }
}
