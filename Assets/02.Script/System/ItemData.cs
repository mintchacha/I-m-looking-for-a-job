using System.Collections.Generic;
using UnityEngine;

// 아이템의 정보를 나열 할 컴포넌트 여기서 프리팹을 리스트에 넣고 Dictionary로 정보 가져올거임
public class ItemData : MonoBehaviour
{
    [Header("게임에 배치할 아이템 설정")]
    [SerializeField] List<GameObject> itemList = new List<GameObject>();
    Dictionary<int, GameObject> itemDictionaries = new Dictionary<int, GameObject>();

    private void Awake()
    {
        if (itemList.Count == 0)
        {
            Debug.Log($"[ItemData] itemList가 비어있습니다.");
            return;
        }

        itemDictionaries.Clear();

    }
}
