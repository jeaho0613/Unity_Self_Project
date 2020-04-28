# 수정 사항

프로젝트 제작하면서 생각나는 `수정 사항`을 정리 합니다.

## BOSS

- Boss 스크립트에는 `FindObjectOfType`을 사용했습니다.  
  - Awack에서 초기화시에만 사용하지만 찾는 비용이 높습니다.  
  - 사용하는 로직은 보스 공격 패턴에서 플레이어의 방향으로 탄을 발사하는 곳에 사용했습니다.
  - 계속적인 참조가 아니라 사용할 때 참조를 하고 패턴 종료 시 참조를 해제해 줘야 할꺼같습니다.

- Boss 스크립트의 BossSkill_  로직들
  - 너무 중구 난방입니다.
  - 스크립트의 재활용성, 수정에 어려움점등 수정이 필요합니다.

- Invoke() 의 델리게이트화
  - Invoke("string", delay); string으로 함수를 넘겨줘서 클레스명이 변경됐을 때 하나하나 수정해야됩니다.
  - 델리게이트로 바꿔주면 편할꺼 같습니다.

- Sound Play 로직
  - SoundManager에서 함수를 하나 만들어서 Play를 관리 하는게 용이했을 꺼 같다.
  - Playoneshot은 다중으로 음악 사운드를 출력해주는 이점을 활용했어야 했다.
  
   ```C
  PlaySound(Clip playClip)
  {
    AudioSound.PlayOnShot(playClip)
  }
  ```
