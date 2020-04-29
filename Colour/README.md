# PROJECT : Colour

색을 이용한 게임을 제작합니다.  
기본 베이스는 슈팅게임을 바탕으로 색에 맞추어 컨트롤합니다.

PV 영상입니다.  
<https://youtu.be/vQSucYXattw>

## **주의 사항**

- **`Sound Folder`에 유로 에셋이 포함되있습니다. 저작권에 위반될 시 삭제하겠습니다.**
- **이미지 리소스는 `골드메탈`님의 슈팅게임 강의영상을 기반으로 제작했습니다.**

## **프로젝트 디자인**

- Object Pool 디자인
- Manager scripts 구성
- Dotween을 활용 (오브젝트 움직임 관리)
- .txt를 읽어 스테이지 구성
- player 색상 변경을 Sprite Shader으로 구성

## **기본 구상**

- Player는 특정키로 색을 변경할 수 있다.
- 장애물은 `3`개로 구성 (우선은)되어있고 각 색깔의 변화에 따라서 조작해야된다.
- 색은 5종류 (빨, 파, 초, 노, 분)이고 각 색의 특성이 있다.
  - MoveSpeed : 기체 속도
  - Shooting Time : 총알 발사 수
  - End Time : 총알의 속도
  - Power : 총알 데미지
  - **빨간색 총알**
    - 빨간색 종류의 색 충동을 무시하고 총알 속도,발사간격이 빠르다. 데미지가 낮음
      - 기체 속도 : 보통 (3)
      - 총알 발사 수 : 빠름 (0.05)
      - 총알 속도 : 빠름 (0.5)
      - 데미지 : 낮음 (5)
      - 크기 : 보통 (1,1,1)
  - **초록색 총알**
    - 빨간색 종류의 색 충동을 무시하고 총알 발사 대미지가 높다. 기체 속도가 낮음
      - 기체 속도 : 낮음 (0.7)
      - 총알 발사 수 : 보통 (0.1)
      - 총알 속도 : 보통 (2)
      - 데미지 : 보통 (10)
      - 크기 : 높음 (2,2,1)
  - **파란색 총알**
    - 빨간색 종류의 색 충동을 무시하고 기체 속도가 높다. 총알 속도가 낮음
      - 기체 속도 : 빠름 (7)
      - 총알 발사 수 : 낮음 (0.2)
      - 총알 속도 : 보통 (1)
      - 데미지 : 보통 (10)
      - 크기 : 보통 (1,1,1)
  - **노란색 총알**
    - 모든 종류의 색 충돌을 무시한다. 모든 스텟이 높다. (스킬)
      - 기체 속도 : 높음 (7)
      - 총알 발사 수 : 높음 (0.05)
      - 총알 속도 : 빠름 (0.5)
      - 데미지 : 높음 (20)
      - 크기 : 높음 (2,2,1)

## **Using Asset, package**

- DOTWEEN (트윈 에셋) // 무료
- Interface and Item Sounds (사운드 에셋) // 유료
- FREE Casual Game SFX Pack (사운드 에셋) // 무료
- Loop & Music Free (사운드 에셋) // 무료
- GoldMetal Resource (이미지) // 무료

## **화면 비율**

540 : 900

## **생성 규칙**

**`시간,타입,생성지점`**

- BOSS
  - 0 point에서만 나올것
- RedL(빨간색 큰 기체)
  - 1,2,3 point에서만 나올것
  - 체력이 많다.
  - 2개의 총알을 발사
- RedM (빨간색 세모 기체)
  - 4,5 point에서만 나올것
  - player 위치를 타켓으로 하지말고 특정 위치로만 이동
  - 보통의 스팩을 가진다.
  - 지나간 위치에 고정된 패턴으로 총알 생성
- RedS (빨간색 작은 기체)
  - 6,7 point에서만 나올것
  - player 위치를 타켓으로 한다.
  - 체력이 낮음 (약 1,2발에 파괴)
  - 총알 발사는 없지만 player기체에 다가오는 시간이 빠름
- BlueL
  - 8,9 point에서 생성
  - 중간 보스 역할
  - 좌우를 루프하면서 탄을 발사
- BlueS
  - 10 point에서 생성
  - 일정 구역을 반복하는 동작
  - 중간 보스 역할

## **만들면서**

- **transfrom.Translate 와 Postion의 차이점**
  - `Translate`는 게임오브젝트를 `이동` 시키기 위함입니다. 옵션으로 월드,로컬을 지정할수 있습니다.
  - `Postion`은 게임오브젝트의 `절대좌표`를 나타냅니다. 순간적 이동을 위해 사용됩니다.

- **URP 설정 관해서**
  - 어쩌면 당연한 거지만 프로젝트를 만들때 `URP를 초기에 설정`을 해주어야된다. 중간에 URP를 설정하게 되면 설정전에 오브젝트, 이미지등 설정관련이 적용되지 않아서 파이프라인의 영향을 받지 않는다

- **Material 변경**
  - MainTex의 변경은 되지않는다...(아마도) 그래서 쉐이더 그래프를 만들어 Material을 적용시켰다.
  - public Material playerMaterial; // player 머테리얼을 가져오고
  - playerMaterial.SetTexture ("_SubTex" ,sprites[num]); // SetTexture("변수명", Texture)으로 변경

- **PNG 와 JPG**
  - PNG : 투명한 배경으로 저장, 파일 크기가 커짐
  - JPG : 투명한 배경은 하얀색 바탕으로 저장되고, 파일 크기가 작음

- **싱글톤 Instance?**
  - SoundManager를 싱글톤으로 구성하고 사용할 때 마다 Instance쓰기 불편
  - soundManager를 전역변수로 설정하고
  - public SoundManager soundManager = SoundManager.Instance 로 설정을 해도
  - 코드상에서는 오류가 생김... 나중에 분석 요망

- **스크립트, 오브젝트 비활성화**
  - 비활성화 한다고 내부에 있는 값들이 사라지는건 아님.
  - 생성주기를 이용해 로직을 구현할 때 스크립트의 내부 값을 초기화 해주는걸 고려해야함.
  - start, Awake 등이 포함. 활성화 될 때 최초 1번 실행이 되었다면 재실행 되지 않음

- **Rigidbody type**
  - **dynamic** : 딜폴트 타입으로 다른 body type과 모두 충돌하고 중력과 힘의 영향을 받습니다.
    - 위치나 회전값을 동적(코드)으로 변경하는 오브젝트에 사용하기엔 비효율 적입니다.
  - **Kinematic** : 중력과 힘의 영향을 받지 않아  동적(코드)으로 제어하는 오브젝트에 적합한 타입입니다.
    - dynamic 타입을 갖는 오브젝트와 충돌하고 그외 나머지 kinematic 과 static 타입과는 통과합니다.
  - **static** : 움직이지 않는 오브젝트에 적합한 타입입니다.

- **생명 주기**
  - 알고는 있었지만 Awake() Start()의 실행 시간은 다르다.
  - Awake에서 비활성화된 오브젝트가 있을 경우 Start가 실행되지 않아 오류가 발생했다.

- **Animation delay**
  - Exis Time을 체크했는대도 얘기치 못한 딜레이가 발생할수 있다.
  - 아마 코루틴으로 애니메이션을 제어하는 부분에서 발생하는 딜레이인거 같다.
  - 이럴땐 Exis Time을 해제하고 타임을 0으로 만들면 정상적으로 작동한다.

- **Get Set 프로퍼티**
  - Get : 값을 가져올때
  - Set : 값을 새로 쓸 때

```c#
      [GameManager Script]

      private float skillPoint; // 스킬 포인트
      public float SkillPoint // 스킬 포인트 프로퍼티
      {
        get
        {
            return skillPoint;
        }

        set
        {
            Debug.Log("value입니다. : " + value);
            skillPoint = value;
            UpdateSkill();
        }
      }

      [Enemy Script]

      GameManager.Instance.SkillPoint += enemyPoint; // 기체의 스킬 포인트 획득

      `value`는 `GameManager.Instance.SkillPoint += enemyPoint` 이 로직이 끝난 값을 받는다.
```

- **++ 과 + 1**
  - 간혹 ++과 +1에 로직차이를 보임.

- **DOtween의 Kill, Pause**
  - Start에서 초기화를 해줘야지 원하는 타이밍에 정지가 가능
  - Awake에서는 DoTween 전역 매니저가 생성되지 않았기 때문에 반드시 Start에서 호출!
  - `이 점은 직접 실험해 봐야할 필요성`
  - 모든 트윈을 관리하는 스크립트를 생성하여 매니저로 관리하는게 좋을듯 (오브젝트 풀 처럼)
  - 간단한 움직임도 트윈으로 관리하는게 종료시점을 파악하는데 큰 도움이 된다.

- **코루틴 While 무한 루프**
  - A 코루틴의 경우 반복된 thread 생성으로 메모리상 더 좋지않음
  - B 코루틴의 경우 A보단 좋다.

```c#
  public IEnumerator A()
  {
    yield retrun newWaitForSeconds(1f);
    StartCoroutine("A");
  }

  public IEnumerator B()
  {
    while(반복조건)
    {
         yield return newWaitForSeconds(1f);
    }
  }
```

- **Trigger 조건**
  - 두 GameObject가 모두 Collider를 가지고 있어야 함.
  - 두 GameObject 중 하나는 Rigidbody를 가지고 있어야 함.
  - 두 GameObject 중 하나만 움직인다면 움직이는 GameObject가 Rigidbody를 가지고 있어야 함.

- **오브젝트 링크**
  - 인스펙터뷰 상에서 링크하는 방법은 간단하나 자동으로 연결되게 하는게 좋음
  - Prefab의 경우 오브젝트를 링크하는 방법이 스크립트로 한정되 있음
  - 이 경우 Find을 사용해야하는데 비용이 많이듬

- **Prefab의 초기화 순서?**
  - public int A라는 전역변수가 있다.
  - 인스펙터 뷰에선 2라고 설정했는데
  - Awack에서 1로 초기화하면 1로 설정된다.
  - 인스펙터 뷰의 값으로 초기화가 되는걸로 알고있었는데 테스트가 필요하다.

- **나숫셈 주의**
  - 2 / 3 = 0
  - 2f / 3f = 0.6666'''
  - 소수점을 가지고싶다면 뒤에 `f`를 붙여줘야함.

- **Collider2D 중복 충돌**
  - 겹쳐져 있는 collider에 충돌 시 여러번 동작이 일어남.
  - 이를 위한 bool 변수, 예외처리에 관해서 생각을 해야될 꺼 같다.

## **Time Line**

- `20-04-11`  
  - 기본 플레이어 이동
  - 게임 플레이 구상
  
- `20-04-12`
  - 플레이어 이동 제한
  - 버튼별 플레이어 색깔 변경
  - 플레이어 색깔별 총알 변경
  - 각 총알별 스텟 구성 필요

- `20-04-13`
  - 키 입력에 따른 플레이어 색 변경
  - 움직임 애니메이션 적용
  - 색 RGBY 순으로 변경
  - 최초 게임 시작시 Red를 기본값으로 초기화

- `20-04-14`
  - **`오브젝트 풀링`**
    - 총알 구현 완료
  - 총알 색깔 별 밸런스 조정

- `20-04-15`
  - Ranbom Bar 생성

- `20-04-16`
  - 적 비행물 설정
  - text.file로 적 기체 생성 조작
  - EnemySpanwer 설정

- `20-04-17`
  - 적 기체 생성 코루틴 완성하기
  - 화면 밖 (3,4,5,6)생성 기체 설정
  - 적 기체 생성
  - 기체 생성 시 총알 생성
  - 적 기체 타격시 애니메이션 설정
  - 총알 설정
    - playerBullet : PBullet tag로 관리
    - EnemyBullet : EBullet tag로 관리
  - 적 기체 shooting delay에 따른 총알 발사 구현

- `20-04-19`
  - 적 기체 움직임 설정
    - player의 위치가 아닌 특정 위치로 변경
    - 총알발사와 거리를 두기 위함
  - 기체, 플레이어 사망효과
  - **`생성 규칙 설정`**
  - 플레이어 사망, 리스폰중일 때 동작 변환
  - **`Physics 2D : Layer Collsion Matrix 설정`**
    - 각 오브젝트별 Layer 설정
    - 충돌별 Matrix설정

- `20-04-20`
  - 적 기체와 적 총알의 피격 판정 설정 완료
  - 적 기체, 플레이어의 파괴 효과 완료
  - **`애니메이션 StateBehaviour 제거`**
    - FX 스크립트에서 플레이 체크하는 방식으로 변경
  - Sound Manager 변경
    - 배열로 관리
    - 각 오브젝트 단위로 변경

- `20-04-21`
  - 필살기 구현 로직 완료
  - UI 설정 1/2 완료
    - 프로퍼티로 구성
    - skill gauge UI 완료
    - life UI 완료

- `20-04-22`
  - **`BOSS 구현`**
    - 페이지 1 구현
    - 페이지 2 전환 완료
      - 부드럽게 이어지지 않음 수정요망 (완료)

- `20-04-23`
  - **`BOSS 구현`**
    - 다음 페이지 연결하는 부분 요망 (완료)
    - 페이지에 맞는 Boss 움직임 구현 (완료)
    - 페이지 2까지 구현하기 요망 (완료)
    - 페이지 2 2번째 시작시 bossSkill시 바로 색변경되는 부분 수정 요망 (완료)
    - 공백 10초간 BAR 스폰 or 총알 스폰 요망 (완료)

- `20-04-24`
  - **`BOSS 구현`**
    - 페이지 3까지 구현하기 요망 (완료)
  - BossSkill 사용 함수로 변경하여 사용 (완료)
  - BossSkill_2 수정 (속도)
  - **`Boss Page2에서 원래 위치로 돌아오는 부근에서 사망시 트윈 오류가있음`** (수정)
  - BulletSpawn Skill 한번에 8개 생성 -> 순차적 생성으로 변경 (완료)
  - Laser Skill 가로,세로 경고선 표시 후 포탄 생성 (완료)

- `20-04-25`
  - stage 0.txt 밸런스, 생성 기체 만들기 (완료)
  - 이펙트 (완성)
  - 사운드 (미완성)
    - 보스 사운드 부실

- `20-04-26`
  - `UI 완성하기`
    - Boss Hp Bar 완료
    - Player Win or Lose UI 작업 요망 (완료)
    - Restart 버튼 (완료)
    - 화면은 트윈을 이용한 Fade in out 으로 작업

- `20-04-27`
  - Game Win Sound 출력
  - 마지막 Boss 물리쳤을 때 애니메이션 동작 (완성)
  - BOSS 처치 시 Baground Sound 제거 (완료)
  
- `20-04-28`
  - intro (완성)
  - 화면은 트윈을 이용한 Fade in out 으로 작업 (완성)
  - 스크롤링 Background (완성)
  - 처음 게임 Start But (완성)

- `20-04-29`
  - 처음 게임 Start 화면 배경 스크롤 이미지 사라짐 오류 (완료)
    - 씬 로드할 때 DonLoadObject 제거..
  - 중간 점검 후 오류 처리 `Bug` 부분.
  - **`중간점검`**
    - 보스 HP 조정 (완료)
    - 시간이 된다면 부 Boss전 (다른 색의 적 기체를 이용) (완료)
    - 다른 패턴의 적 기체 (Blue Enemy 완료)
    - 보스 Sound 추가 (완료)
    - 스폰 패턴 추가 (Blue Enemy 완료)
    - ObjectPool 갯수 조정 (완료)
    - Intro Sound 추가 (완료)

- `20-04-30`
  - 최종 평가
  - 스크립트 정리

- `Bug`
  - BOSS Razer 스킬 중 보스에게 타격이 들어가는 버그 (수정)
  