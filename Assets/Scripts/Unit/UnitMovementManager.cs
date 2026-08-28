using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class UnitMovementManager : MonoBehaviour
{
    private List<UnitMovement> unitMovements = new();
    void Start()
    {
        unitMovements = GetComponentsInChildren<UnitMovement>().ToList();
    }

    void Update()
    {
        
        for(int i = 0; i< unitMovements.Count; i++){

            UnitMovement unitMovement = unitMovements[i];
            unitMovement.transform.position = Vector3.MoveTowards(
                unitMovement.transform.position, 
                unitMovement.targetPos, 
                unitMovement.moveSpeed * Time.deltaTime);
        }

    }
}
