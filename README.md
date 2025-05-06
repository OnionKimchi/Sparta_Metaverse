# 메타버스 프로젝트

## 날짜별 트러블 슈팅 TIL링크
https://github.com/OnionKimchi/Sparta_git/blob/OnionKimchi-TIL/250502

https://github.com/OnionKimchi/Sparta_git/blob/OnionKimchi-TIL/250503

https://github.com/OnionKimchi/Sparta_git/blob/OnionKimchi-TIL/250504

https://github.com/OnionKimchi/Sparta_git/blob/OnionKimchi-TIL/250505

https://github.com/OnionKimchi/Sparta_git/blob/OnionKimchi-TIL/250506

## 구현한 것
- 강의에서 제공된 비행기 게임 만든걸 기반으로 메인 씬을 탑다운 뷰로 확장함
- 비행기 게임은 강의에서 제공된 기본적인 기능을 유지했습니다. 에셋도 강의에서 제공된 것과 동일하게 사용
- 이동은 방향키 공격은 Z 탭키는 퍼즈
- 엔피시 근처에 가면 대사가 뜨도록 상호작용을 만듬
- 맵 이동 에리어에 가면 맵 이동을 위한 팝업과 버튼 뜨도록 구현
- 비록 디펜스 게임을 완성하진 못했지만 플레이어 오브젝트의 cs 컴포넌트 인스펙터 값을 사용해 무기 길이, 무기 공속, 무기 범위 값을 조정할 수 있는 것을 확인
- 디펜스 게임은 입문 강의 3번째 게임의 아이디어를 기반으로 만듬
 
 ## 구현하지 못한것
- 로그라이크 디펜스 게임을 구상했으나 손이 느리고 작업에 막히는 부분이 많아서 미완성함
- 본래 킬 카운트를 통해 플레이어를 강화하고 스폰 카운트를 통해 적을 강화할 생각이었으나 손이 느려 생각대로 구현하지 못함

## 배운 점
- 텍스트 RPG를 만들 땐 스태틱 만으로 구현이 가능했던 기능들을 유니티에선 싱글턴 인스턴스를 사용해서 구현해야 한단걸 느낌. 찾아보니 MonoBehaviour를 상속받기 때문이라고 하는거 같은데 자세한 원리는 아직 이해 못함
- 유니티 만의 기능들에 대한 기본적인 이해가 필요함. 텍스트 알피지를 만들땐 모든 메서드가 어느 메서드와 연결되었는지 스크립트만 봐도 추적이 쉬웠는데 유니티는 네임스페이스나 유니티 인스펙터에서 오브젝트 할당을 하거나 메서드를 발동 때문에 보이지가 않을 때가 많음
