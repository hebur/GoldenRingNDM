using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButton : MonoBehaviour
{
    [SerializeField] private List<Player> listPlayer;
    private int _curTurn = -1;
    private int _otherTurn = -1;
    public int CurTurn { 
        get => _curTurn; 
        set 
        { 
            _curTurn = value;
            _otherTurn = (value + listPlayer.Count - 1) % listPlayer.Count;
        } 
    }
    public void OnClickPlayerButton(int rsh)
    {
        if(rsh != _curTurn && rsh != _otherTurn)
        {
            for(int i = 0; i < listPlayer.Count; i++)
            {
                if (i == _curTurn)
                    continue;
                else
                {
                    listPlayer[i].CollectCard();
                }
            }
            StartCoroutine(RepositionTargetPlayer(rsh));
            _otherTurn = rsh;
        }
    }

    private IEnumerator RepositionTargetPlayer(int rsh)
    {
        yield return new WaitForSeconds(0.8f);

        listPlayer[rsh].RePosition_OtherField();
    }

}
