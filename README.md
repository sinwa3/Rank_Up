# Rank_Up

부트캠프 승급 과제

- 다이아 퀘스트: 아이템 획득 & 인벤토리
- 마스터 퀘스트: 상점 시스템

상점이 인벤토리를 전제로 해서 두 과제를 한 프로젝트에서 구현했다.

## 실행

- Unity 2022.3.62f3
- 씬: `Assets/Scenes/Rank_Up.unity`

## 조작

| 키 | 동작 |
|---|---|
| WASD / 방향키 | 이동 |
| Space | 점프 |
| I | 인벤토리 열기/닫기 |
| E | 상점 열기/닫기 (상인 근처에서만 열림) |
| 마우스 클릭 | 아이템 선택, 사용, 구매 |

## 폴더

- `Assets/Scripts/Runtime/Rank_Up/` 과제 스크립트
  - 아이템: `ItemData`, `HealItemData`
  - 인벤토리: `Inventory`, `InventorySlot`, `InventoryUI`, `InventorySlotUI`, `WorldItem`
  - 플레이어 HP: `PlayerStatus`, `PlayerHpUI`
  - 재화/상점: `Wallet`, `GoldUI`, `ShopZone`, `ShopUI`, `ShopSlotUI`
- `Assets/SO/` 아이템 데이터 (회복약 소 100G, 대 250G)
- `Assets/Scripts/Runtime/Player/` 플레이어 이동
