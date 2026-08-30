# Unity RTS 대규모 유닛 이동 최적화

Unity에서 **최대 50,000개의 RTS 유닛을 GameObject 기반으로 이동**시키며, 구조별 성능 차이와 실제 병목을 단계적으로 확인한 테스트 프로젝트입니다.

단순히 Job System과 Burst를 적용하는 데서 끝내지 않고, **가설 → 구현 → 측정 → 병목 재분석 → 구조 변경** 순서로 실험했습니다.

> 상세한 실험 과정과 Profiler 캡처는 [Notion 포트폴리오](https://app.notion.com/p/3cc74e134c79806d98acd3a648a2e234)에서 확인할 수 있습니다.

## 실험 흐름

1. 유닛별 `MonoBehaviour.Update()` 이동
2. Manager가 유닛 이동 로직을 일괄 호출
3. 위치·목표·속도를 배열로 분리해 데이터 중심으로 계산
4. `NativeArray + IJobParallelFor` 적용
5. `BurstCompile` 적용 및 순수 계산 구간 분리 측정
6. `TransformAccessArray + IJobParallelForTransform`으로 Transform 갱신 병렬화

중간 결과가 항상 좋아지지는 않았습니다. 특히 `IJobParallelFor`만 적용했을 때는 50K에서 오히려 전체 Frame Time이 증가했고, Profiler로 다시 확인한 뒤 Main Thread의 대량 `Transform.position` 갱신을 다음 병목으로 판단했습니다.

## 평균 Frame Time

동일 Scene에서 이동 명령 후 안정된 **100프레임 구간의 평균 Frame Time**을 비교했습니다.  
CPU: **AMD Ryzen 9 7950X**

| 구조 | 10K | 30K | 50K |
| --- | ---: | ---: | ---: |
| MonoBehaviour | 23.00ms | 67.45ms | 131.68ms |
| 데이터 배열 기반 Manager | 19.74ms | 53.61ms | 92.21ms |
| IJobParallelFor | 18.93ms | 51.28ms | 96.72ms |
| Burst + IJobParallelForTransform | **17.21ms** | **42.30ms** | **77.47ms** |

**50K 기준 131.68ms → 77.47ms, 약 41% 감소**했습니다.

## 확인한 점

- `Update()` 호출을 Manager로 모으는 것만으로는 큰 차이가 없었습니다.
- 객체에 흩어진 이동 데이터를 배열로 분리했을 때 첫 번째 큰 개선이 나타났습니다.
- Job/Burst는 이동 계산 자체를 크게 줄였지만, 전체 Frame Time 개선은 제한적이었습니다.
- 계산 비용을 줄인 뒤에는 Main Thread의 대량 Transform 갱신이 주요 병목으로 남았습니다.
- `IJobParallelForTransform`으로 이동 계산과 Transform 반영을 함께 처리하면서 최종 Frame Time이 크게 감소했습니다.

## 핵심 코드

| 파일 | 역할 |
| --- | --- |
| [`UnitMovementManager.cs`](Assets/Scripts/Unit/UnitMovementManager.cs) | 이동 데이터 관리, NativeArray 수명 관리, Job 스케줄링 |
| [`UnitMoveJob.cs`](Assets/Scripts/Unit/UnitMoveJob.cs) | `IJobParallelFor` 기반 이동 계산 |
| [`UnitMoveTransformJob.cs`](Assets/Scripts/Unit/UnitMoveTransformJob.cs) | Burst + `IJobParallelForTransform` 기반 이동 및 Transform 반영 |
| [`UnitSelectionManager.cs`](Assets/Scripts/Unit/UnitSelectionManager.cs) | 드래그 선택 및 이동 명령 |

## 테스트 기능

- 대량 유닛 배치
- 좌클릭 드래그 선택
- 우클릭 이동 명령
- 선택 유닛 격자형 도착 위치 분배
- MaterialPropertyBlock 기반 선택 표시
- RTS 카메라 이동 및 줌
- ProfilerMarker 기반 구간별 비용 측정

## 실행 방법

1. Unity Hub에서 Unity `6000.3.10f1`로 프로젝트를 엽니다.
2. `Assets/Scenes/TestScene.unity`를 엽니다.
3. Play Mode를 시작합니다.
4. 좌클릭 드래그로 유닛을 선택하고 우클릭으로 이동 명령을 내립니다.

## 사용 기술

`Unity 6` · `C#` · `Job System` · `Burst Compiler` · `NativeArray` · `Unity.Mathematics` · `TransformAccessArray` · `Unity Profiler`
