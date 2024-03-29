using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

namespace LevelMaze
{
    public sealed class Bonuses : MonoBehaviour
    {
        [SerializeField] List<BadBonus> badBonuses;
        [SerializeField] List<GoodBonus> goodBonuses;

        XMLSaver bonusSaver = new XMLSaver();

        void Start()
        {
            //��� ������� ��������� ������ ������ ���� ����� ����
            try
            {
                GoodBonus.onGoodBonusTook += OnGoodBonusTook;
                BadBonus.onBadBonusTook += OnBadBonusTook;

                if (GoodBonus.onGoodBonusTook == null || BadBonus.onBadBonusTook == null) throw new MyNullException("��� ������ �� ����� ���� null");

                OnGoodBonusTook();
                OnBadBonusTook();

                foreach (var goodBonus in goodBonuses)
                {
                    bonusSaver.Save(goodBonus.gameObject.transform.position, Path.Combine(Application.streamingAssetsPath, $"{goodBonus.name}.txt"));
                }
                foreach (var badBonus in badBonuses)
                {
                    bonusSaver.Save(badBonus.gameObject.transform.position, Path.Combine(Application.streamingAssetsPath, $"{badBonus.name}.txt"));
                }

                foreach (var goodBonus in goodBonuses)
                {
                    Log($"{goodBonus.name} " + bonusSaver.Load(Path.Combine(Application.streamingAssetsPath, $"{goodBonus.name}.txt")));
                }
                foreach (var badBonus in badBonuses)
                {
                    Log($"{badBonus.name} " + bonusSaver.Load(Path.Combine(Application.streamingAssetsPath, $"{badBonus.name}.txt")));
                }
            } catch (MyNullException ex) { Debug.LogException(ex); }
        }

        void ResetBonuses(bool isBadBonus)
        {
            if (isBadBonus)
            {
                foreach (var bonus in badBonuses)
                {
                    bonus.gameObject.SetActive(false);
                }
            } else
            {
                foreach (var bonus in goodBonuses)
                {
                    bonus.gameObject.SetActive(false);
                }
            }
        }

        void OnBadBonusTook()
        {
            ResetBonuses(true);
            badBonuses[Random.Range(0, badBonuses.Count-1)].gameObject.SetActive(true);
            //�������� ���������� �������� �������
        }
        void OnGoodBonusTook()
        {
            ResetBonuses(false);
            goodBonuses[Random.Range(0, goodBonuses.Count-1)].gameObject.SetActive(true);
            //�������� ���������� �������� �������
        }
        void OnDestroy()
        {
            GoodBonus.onGoodBonusTook -= OnGoodBonusTook;
            BadBonus.onBadBonusTook -= OnBadBonusTook;
        }
    }
}