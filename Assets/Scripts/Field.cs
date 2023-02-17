using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    [SerializeField] private List<Player> listPlayer;
    [SerializeField] private PlayerInfoPanel curPlayerPanel;
    [SerializeField] private PlayerInfoPanel otherPlayerPanel;
    [SerializeField] private List<Image> curSlots;
    [SerializeField] private List<Image> otherSlots;
    [SerializeField] private Sprite voidSlotImage;
    [SerializeField] private Sprite SlotImage;

    private int _curTurn = -1;
    private int _otherTurn = -1;

    public void UpdateTurn(int cur)
    {
        _curTurn = cur;
        _otherTurn = (cur + listPlayer.Count - 1) % listPlayer.Count;

        int bef = _otherTurn;

        //필드에 카드 재배치
        listPlayer[cur].RePosition_PlayerCard();
        listPlayer[bef].RePosition_OtherField();

        //필드의 정보 수정
        curPlayerPanel.DrawInfo(true, listPlayer[cur].Resource, listPlayer[cur].Score);
        otherPlayerPanel.DrawInfo(false, listPlayer[bef].Resource, listPlayer[bef].Score);

        //필드의 슬롯 수정
        for(int i = 0; i < 4; i++)
        {
            curSlots[i].sprite = voidSlotImage;
            otherSlots[i].sprite = voidSlotImage;
        }
        for(int i = 0; i < listPlayer[cur].SlotUsed; i++)
            curSlots[i].sprite = SlotImage;
        for(int i = 0; i < listPlayer[bef].SlotUsed; i++)
            otherSlots[i].sprite = SlotImage;
    }

    /// <summary>
    /// 오른쪽 플레이어 버튼을 클릭했을 때.
    /// 위쪽 필드에 있는 카드, 정보, 슬롯을 변경.
    /// </summary>
    public void OnClickPlayerButton(int rsh)
    {
        if (rsh != _curTurn && rsh != _otherTurn)
        {
            for (int i = 0; i < listPlayer.Count; i++)
            {
                if (i == _curTurn)
                    continue;
                else
                {
                    listPlayer[i].CollectCard();
                }
            }
            
            StartCoroutine(RepositionTargetPlayer(rsh));
            otherPlayerPanel.DrawInfo(false, listPlayer[rsh].Resource, listPlayer[rsh].Score);
            for (int i = 0; i < 4; i++)
                otherSlots[i].sprite = voidSlotImage;
            for (int i = 0; i < listPlayer[rsh].SlotUsed; i++)
                otherSlots[i].sprite = SlotImage;

            _otherTurn = rsh;
        }
    }

    /// <summary>
    /// 선택한 플레이어의 카드를 위쪽 필드에 배치
    /// </summary>
    private IEnumerator RepositionTargetPlayer(int rsh)
    {
        yield return new WaitForSeconds(0.8f);

        listPlayer[rsh].RePosition_OtherField();
    }

    /// <summary>
    /// 슬롯이 부족할 때 CS에서 호출. Field의 FlashRed를 호출.
    /// </summary>
    public void FlashRed()
    {
        StartCoroutine(RunFlashRed());
    }

    private IEnumerator RunFlashRed()
    {
        for (int i = 0; i < 4; i++)
        {
            curSlots[i].sprite = voidSlotImage;
            curSlots[i].color = Color.red;
        }
        yield return new WaitForSeconds(1.0f);
        for (int j = 0; j < 4; j++)
            curSlots[j].sprite = voidSlotImage;
        for (int j = 0; j < listPlayer[_curTurn].SlotUsed; j++)
            curSlots[j].sprite = SlotImage;
        for (int j = listPlayer[_curTurn].SlotUsed; j < 4; j++)
            curSlots[j].color = Color.white;
    }
}
