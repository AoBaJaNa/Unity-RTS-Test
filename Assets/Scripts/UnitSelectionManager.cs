using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionManager : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private SelectionBoxUI selectionBoxUI;
    private TestManager testManager;

    private void Awake()
    {
        mainCamera = Camera.main;
        testManager = GetComponent<TestManager>();
    }
    private void Update()
    {
        if (Mouse.current == null) return;

        // 드래그 마치는 순간 일괄 선택 판별
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            SelectUnitsInBox(selectionBoxUI.GetScreenRect());
        }
    }

    private void SelectUnitsInBox(Rect dragRect)
    {
        // TestManager에서 스폰된 유닛 리스트를 가져와 순회
        
        foreach (var unit in testManager.SpawnPosList)
        {
            if (unit == null) continue;

            // 3D 월드 좌표 -> 2D 화면 좌표 변환
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);

            // 카메라 전방에 있고, 드래그 영역 박스 안에 포함되어 있는가?
            if (screenPos.z > 0 && dragRect.Contains(screenPos))
            {
                unit.GetComponent<UnitMovement>().Selected();
            }
            else
            {
                unit.GetComponent<UnitMovement>().DeSelected();
            }
        }
        
    }
}