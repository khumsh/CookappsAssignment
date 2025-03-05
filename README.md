# 프로그램 실행 방법

## 데이터 값 코드 수정 없이 바꾸기

Assets/Resources/Data/ExcelData 폴더 내에 있는 엑셀 데이터 내 값을 바꾼 후, 
![Untitled](https://github.com/user-attachments/assets/38648404-f4a2-4234-9737-40fcbf696e13)

엑셀 창을 닫고 Tools - ParseExcel을 누르거나 F4 버튼을 누르면 Excel Data가 Json Data로 파싱됩니다.
![Untitled 1](https://github.com/user-attachments/assets/4543f49b-b0e6-4009-b098-cc08903b65b5)

그 후 게임을 켜면 데이터 값이 바뀐 상태로 시작합니다.

# 주요 구현 내용

## 자동 전투

### 상태 머신 (EntityStateMachine)

캐릭터(Hero, Monster)는 각각 상태머신을 이용해 각 상태 별 동작을 구현하였습니다.

각 캐릭터는 Idle, Move, Atk, Dead 상태가 있으며, 각 상태 별 동작은 다음과 같습니다.

- Idle State
    - 타겟이 유효하다면
        - 공격 범위 밖이면 Move 상태로 전이
        - 공격 가능한 상태라면 Atk 상태로 전이
        - 그렇지 않다면 Move 상태로 전이
    - 타겟이 유효하지 않다면
        - 타겟 서치
            - 타겟이 유효하다면 Move 상태로 전이
    
    **[Monster]**
    
    - 몬스터는 타겟이 유효하지 않다면 일정 확률로 순찰을 합니다.
    
- Move State
    - 타겟이 유효하다면
        - 공격 가능한 상태라면 Atk 상태로 전이
        - 그렇지 않다면 타겟에게 이동
    - 타겟이 유효하지 않다면
        - Idle 상태로 전이
    
    **[Monster]**
    
    - 몬스터는 타겟이 유효하지 않다면 랜덤한 순찰 위치로 이동 후 Idle 상태로 전이합니다.
- Atk State
    - 공격 애니메이션 재생
        - 공격 애니메이션 이벤트를 통해 스킬 상태를 Use 상태로 전이
    - 타겟이 유효하지 않다면
        - Idle 상태로 전이
    - 공격 애니메이션이 끝났으면
        - Idle 상태로 전이
- Dead State
    - 부활 시간이 지났고 게임이 안끝났으면
        - Idle 상태로 전이

그리고 각 캐릭터는 공격할 때 스킬 시스템을 사용합니다.

각 스킬은 SkillSystem 코드에서 관리하고 있고, 스킬마다 상태 머신을 갖고 있습니다.

스킬마다 각각 Ready, Use, Cooldown 상태를 갖습니다.

Hero는 Default Skill, Special Skill을 갖고 있으며,

Monster는 Default Skill만 갖습니다.

스킬의 상태에 대한 대략적인 설명은 다음과 같습니다.

- Ready State
    - 스킬 사용 가능한 상태
    - 스킬을 사용하면 Use 상태로 전이
- Use State
    - 스킬을 사용할 때의 상태
        - 스킬 적용
        - 나중에 스킬을 더 다양하게 늘리고 싶다면 이 부분을 스킬에 따라 다르게 적용되도록 수정해 주면 됩니다.
        - 현재는 간단하게 모든 스킬을 구현할 수 있도록 만들어 놓았습니다.
    - 스킬을 사용한 후 바로 Cooldown 상태로 전이
- Cooldown State
    - 스킬을 사용한 후 쿨타임 상태
        - 쿨타임 시간이 지나면 Ready 상태로 전이

## UI
![Untitled 2](https://github.com/user-attachments/assets/861dcd14-7c08-4fd2-9fb4-2a4aa9204fe4)
![Untitled 3](https://github.com/user-attachments/assets/d8a852eb-4428-44bc-ba81-c2c75ee76b11)
![Untitled 4](https://github.com/user-attachments/assets/aca70e21-a7a6-44bb-a539-04a1aa5ea2d1)

- 카메라 이동
    - 아래에 있는 캐릭터 아이콘을 누르면 해당 캐릭터를 카메라가 추적해 이동합니다.
- 캐릭터 정보
    - 캐릭터의 레벨, 공격력, 최대 체력, 체력바, 경험치바를 표시합니다.
    - 캐릭터 위에 캐릭터를 따라다니는 체력바를 캔버스의 WorldSpace 렌더 모드를 이용하여 구현하였습니다.
    - 추가로 체력바가 서서히 줄어들도록 구현하였습니다.
- 스테이지 정보
    - 상단에 현재 몇 스테이지인지 표시하도록 하였습니다.
    - 현재 가지고 있는 골드를 표시하도록 하였습니다.
    - 골드의 수량이 바뀔 때마다 두트윈을 이용해 간단하게 골드 아이콘에 애니메이션을 넣었습니다.
- Floating Text 추가
    - 데미지 폰트를 추가하였습니다.
    - 스페셜 스킬을 사용할 때, 레벨업 할 때 Floating Text를 사용하였습니다.
- 캐릭터 사망시 남은 부활시간 표시
    - 캐릭터가 죽으면 다음 부활 시간을 표시합니다.
    ![Untitled 5](https://github.com/user-attachments/assets/984e2eb3-1994-4c91-ac64-ce5bc13b2fd5)

- 스탯 버프 팝업
    - 골드를 이용해 스탯을 버프할 수 있습니다.
    - 공격력, 최대 체력을 버프할 수 있습니다.
    

## 이펙트 추가

스페셜 스킬 사용할 때, 레벨 업 할 때, 부활할 때, 회복될 때 간단한 이펙트를 넣었습니다.
![Untitled 6](https://github.com/user-attachments/assets/8f774f9f-f4ef-4da8-bb6b-cec21b12420a)
![Untitled 7](https://github.com/user-attachments/assets/e16a98e7-288d-405b-ab5c-ef072f9f1459)
![Untitled 8](https://github.com/user-attachments/assets/dfa08e5d-4bac-4840-9d08-2d4fba8eda18)
![Untitled 9](https://github.com/user-attachments/assets/dc9da723-e9a6-4eec-880f-cfaadd8ee050)

## 스테이지

### 스테이지 성공 실패 조건

- 성공
    - 보스 몬스터를 처치하면 성공.
- 실패
    - 모든 캐릭터가 사망하면 실패.
    - 캐릭터가 사망할 때 다른 캐릭터가 전부 죽었는지 확인.

### 추가 스테이지 및 보스 몬스터 추가
![Untitled 10](https://github.com/user-attachments/assets/dc263f98-e594-47bf-b4ae-f84c2ecf4944)

- 각 스테이지마다 5마리씩 잡으면 보스 몬스터가 등장하도록 하였습니다.
- 보스 몬스터가 등장하면 보스 몬스터 체력바 UI가 등장하고, 보스 몬스터가 처치되었을 때 사라집니다.
- 보스 몬스터를 처치하면 스테이지 내의 모든 몬스터가 사망하며, Hero는 승리 애니메이션을 재생합니다.
- 그 후 다음 스테이지로 이동하며 스테이지마다 강화된 몬스터가 등장합니다.
