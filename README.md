# Unity RTS Test

Unity Job System/Burst 기반 대규모 RTS 유닛 이동 시스템을 만들기 전, GameObject + MonoBehaviour 기반의 기준 구현을 쌓는 프로젝트입니다.

## 현재 구현 상태

- `Assets/Scenes/TestScene.unity` 기반의 URP 3D 테스트 씬
- `Unit` 프리팹과 `TestManager` 기반 유닛 배치
- 화면 가장자리 마우스 이동과 휠 줌 카메라 조작
- 좌클릭 드래그 선택 박스 UI
- 화면 좌표 기준 유닛 일괄 선택
- 선택 유닛의 MaterialPropertyBlock 색상 표시
- 우클릭 지점 이동 명령
- 다중 선택 유닛에 대한 격자형 도착 위치(formation) 분배
- 런타임 시작 시 `TestManager`의 자식 유닛을 다시 수집해 선택 목록을 복원
- Inspector에서 유닛 생성/삭제를 실행하는 Editor 도구

## 실행 방법

1. Unity Hub에서 Unity `6000.3.10f1`로 프로젝트를 엽니다.
2. `Assets/Scenes/TestScene.unity`를 엽니다.
3. Play Mode를 시작합니다.
4. 좌클릭 드래그로 유닛을 선택하고, 우클릭으로 이동 지점을 지정합니다.

## 현재 구조

| 역할 | 구성 요소 |
| --- | --- |
| 유닛 이동 및 선택 시각화 | `UnitMovement` |
| 유닛 생성/목록 복원 | `TestManager` |
| 드래그 선택 및 우클릭 이동 명령 | `UnitSelectionManager` |
| 선택 박스 표시 | `SelectionBoxUI` |
| 카메라 조작 | `CameraMovement` |

## 구현 시 확인한 점

- Unity 씬에 유닛 GameObject가 남아 있어도 런타임 `List<GameObject>`는 Play Mode가 시작될 때 새로 초기화됩니다.
- 그래서 `TestManager.Awake()`에서 자식 유닛을 다시 수집해 선택 시스템이 사용할 목록을 복원합니다.
- 선택 박스는 Canvas의 화면 좌표를 사용하므로, `RectTransform` 앵커를 화면 전체 Stretch가 아닌 좌하단 기준으로 설정해야 드래그 영역이 정확히 표시됩니다.

## 다음 단계

1. MonoBehaviour 방식의 100 / 1,000 / 5,000 / 10,000 유닛 성능 측정
2. 개별 `Update()`를 중앙 `UnitManager` 루프로 통합
3. 배열 중심 데이터 구조와 `NativeArray`로 전환
4. `IJobParallelFor`, `JobHandle`, Burst 적용 전후 비교
5. Transform 반영, 선택 탐색, 경로 탐색을 별도 병목으로 측정

## Git 정책

- Unity 생성 폴더(`Library`, `Temp`, `Logs` 등)는 추적하지 않습니다.
- 로컬 기획 문서 `테스트 기획.docx`는 GitHub에 올리지 않습니다.
