# MiniRTS
RTS 게임 구조에 대한 이해를 높이고, 클라이언트 개발 역량을 키우기 위해 미니 RTS 프로젝트를 진행하였습니다. 실제 서비스 수준의 완성도를 지향하기보다는, 핵심 시스템(유닛 제어 / 자원 수집 / FSM / 매니저 분리) 설계와 게임 아키텍처 구조화에 중점을 두었습니다.

![스크린샷 2025-07-02 171353](https://github.com/user-attachments/assets/b995e157-3c3e-4d3f-bcf2-b52fcba283ce)

## Version
Unity: 2022.3.20f1

<details>
<summary>📁 폴더 구조 보기</summary>

```plaintext
AnimationControllers
├── Mouse
└── UI
    ├── GameMap
    └── Start
Animations
├── Mouse
└── UI
    ├── GameMap
    └── Start
Art
├── Font
├── Map
│   └── map_tile
│       ├── tiles_01
│       └── tiles_02
├── Mouse
└── UI
    ├── GameMap
    └── Start
Data
├── Rank
└── UnitBase
Prefab
├── Mouse
└── Unit
Scenes
ScriptableObject
Scripts
├── Core
│   └── PathFinding
├── Enemy
├── FX
├── System
├── UI
│   ├── GameMap
│   └── Start
├── Unit
└── Utilities

