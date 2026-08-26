using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
public class TestManager : MonoBehaviour
{
    [Header("Spanw Setting")]
    public GameObject unitPrefab;
    public int spawnCount = 100;
    public float moveSpeed = 5f;
    public Transform spawnPoint;
    public float minimumRadius = 5f;
    public List<GameObject> SpawnPosList { get; private set; } = new();

    private void Awake()
    {
        SpawnPosList = GetComponentsInChildren<UnitMovement>(true)
        .Select(unit => unit.gameObject)
        .ToList();
    }
    public void SpawnUnit()
    {

        if (unitPrefab == null)
        {
            Debug.Log("unitPrefabs is Null!");
            return;
        }


        for (int i =0; i<spawnCount; i++)
        {
            Vector2 random = Random.insideUnitCircle * minimumRadius;
            Vector3 unitPos = new Vector3(random.x, 1, random.y);

            if (SpawnPosList.Count > 0)
            {
                bool able = true;
                float currentSearchRadius = minimumRadius;
                    foreach (GameObject obj in SpawnPosList)
                    {
                        if (Vector3.Distance(unitPos, obj.transform.position) < minimumRadius)
                        {
                            able = false;
                            break;
                        }
                    }
                while (!able)
                {
                    able = true;

                    foreach (GameObject obj in SpawnPosList)
                    {
                        if (Vector3.Distance(unitPos, obj.transform.position) < minimumRadius)
                        {
                            able = false;
                            break;
                        }
                    }
                    if(!able)
                    {
                        currentSearchRadius += minimumRadius;
                        random = Random.insideUnitCircle * currentSearchRadius;
                        unitPos = new Vector3(random.x, 1, random.y);
                    }
                }

            }

            GameObject instance = Instantiate(unitPrefab, unitPos, Quaternion.identity);
            instance.transform.SetParent(this.transform);
            SpawnPosList.Add(instance);

        }
    }

    public void ClearUnits()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Undo.DestroyObjectImmediate(transform.GetChild(i).gameObject);
        }

        SpawnPosList.Clear();
        Debug.Log("[TestManager] 생성된 모든 유닛이 삭제되었습니다.");
    }
}
