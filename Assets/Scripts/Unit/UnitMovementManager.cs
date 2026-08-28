using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class UnitMovementManager : MonoBehaviour
{
    private List<UnitMovement> unitMovements = new();
    private Vector3[] unitsPos;
    private Vector3[] unitsTargetPos;
    private float[] unitsSpeed;

    private Transform[] unitsTransform;

    private int unitCount = 0;

    TestManager testManager;
    private void Awake()
    {
        testManager = GetComponent<TestManager>();
        unitMovements = GetComponentsInChildren<UnitMovement>().ToList();
    }
    void Start()
    {
        unitCount = unitMovements.Count;
        unitsPos = new Vector3[unitCount];
        unitsTargetPos = new Vector3[unitCount];
        unitsSpeed = new float[unitCount];
        unitsTransform = new Transform[unitCount];

        for(int i = 0; i < unitCount; i++)
        {
            UnitMovement unit = unitMovements[i];
            unit.Index = i;
            unitsPos[i] = unit.transform.position;
            unitsTargetPos[i] = unit.transform.position;
            unitsSpeed[i] = testManager.moveSpeed;
            unitsTransform[i] = unit.transform;
        }
    }
    public void SetTargetPos(int index, Vector3 pos)
    {
        unitsTargetPos[index] = pos;
    }
    public void SetSpeed(int index, float speed)
    {
        unitsSpeed[index] = speed;
    }
    void Update()
    {
        for (int i = 0; i < unitCount; i++)
        {
            unitsPos[i] = Vector3.MoveTowards(unitsPos[i], unitsTargetPos[i], unitsSpeed[i] * Time.deltaTime);
            unitsTransform[i].position = unitsPos[i];
        }
    }
}
