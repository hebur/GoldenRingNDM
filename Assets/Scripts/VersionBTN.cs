using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionBTN : MonoBehaviour
{
    [SerializeField] private List<Image> boxes;
    [SerializeField] private Sprite box, boxcheck;

    /// <summary>
    /// ��ư�� ���� ������ �ٲ�
    /// </summary>
    public void BTN_version(int version)
    {
        MainMenuController.instance.Version = version;
        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].sprite = box;
        }
        boxes[version].sprite = boxcheck;
    }
}
