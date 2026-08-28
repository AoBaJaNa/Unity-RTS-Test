using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Mathematics;
using System.Linq;
using System;
using Unity.Collections;
using Unity.Jobs;

public sealed class UnitNativeData : IDisposable
{
    bool _dispose;
    public NativeArray<float3> pos;
    public NativeArray<float3> targetPos ;
    public NativeArray<float> speed;

    public UnitNativeData(int count)
    {
        pos = new NativeArray<float3>(count, Allocator.Persistent);
        targetPos = new NativeArray<float3>(count, Allocator.Persistent);
        speed = new NativeArray<float>(count, Allocator.Persistent);
    }
    public void Dispose()
    {
        if (_dispose)
            return;
        if(pos.IsCreated)pos.Dispose();
        if(targetPos.IsCreated)targetPos.Dispose();
        if(speed.IsCreated)speed.Dispose();

        _dispose = true;

        GC.SuppressFinalize(this);
    }
}
public class UnitMovementManager : MonoBehaviour
{
    private List<UnitMovement> unitMovements = new();
    private UnitNativeData unitNativeData;
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
        unitsTransform = new Transform[unitCount];
        unitNativeData = new UnitNativeData(unitCount);

        for(int i = 0; i < unitCount; i++)
        {
            UnitMovement unit = unitMovements[i];
            unit.Index = i;
            unitNativeData.pos[i] = unit.transform.position;
            unitNativeData.targetPos[i] = unit.transform.position;
            unitNativeData.speed[i] = testManager.moveSpeed;
            unitsTransform[i] = unit.transform;
        }
    }
    public void SetTargetPos(int index, Vector3 pos)
    {
        unitNativeData.targetPos[index] = pos;
    }
    public void SetSpeed(int index, float speed)
    {
        unitNativeData.speed[index] = speed;
    }
    void Update()
    {
        UnitMoveJob unitMoveJob = new UnitMoveJob(unitNativeData.pos, unitNativeData.targetPos, unitNativeData.speed, Time.deltaTime);

        JobHandle handle = unitMoveJob.Schedule(unitCount,64); //64°³·Î for¹® ¹­¾î¼­ ÅõÃ´

        handle.Complete();


        for (int i = 0; i < unitCount; i++)
        {
            unitsTransform[i].position = unitNativeData.pos[i];
        }
    }
    private void OnDestroy()
    {
        unitNativeData?.Dispose();
    }

    public float3 MoveToWards(float3 current, float3 target, float speed)
    {
        float3 dir = target - current;
        float dist = math.length(dir);
        float step =  speed * Time.deltaTime;

        if (dist <= 0.001f || dist <= step)
            return target;
        else
            return current + (dir / dist) * step;
        
    }
}
