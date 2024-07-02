# Some1
슈팅MMORPG 게임입니다.

[구글 플레이](https://play.google.com/store/apps/details?id=com.someft.some1a)에서 체험해 보실 수 있습니다.

## 슈팅
https://github.com/kimk-s/some1/assets/71550078/2243ef17-c0e4-4ebf-aaa4-5489bf9618e0

## MMO
https://github.com/kimk-s/some1/assets/71550078/74c60a1d-c242-41aa-baf3-6ec8d2c6dd55

### 부하테스트 

#### 부하테스트 조건
- 클라이언트 개수 : 200개
- 클라이언트 행동 : 모든 클라이언트가 근접한 곳에서 활발한 이동과 공격(총알 5발 발사)
- 서버 스펙 : 2vCPU, 8GiB Memory (AWS EC2 m7g.large 인스턴스)
- 서버 게임로직의 목표 프레임 레이트 : 30

#### 부하테스트 결과
- 부하테스트 진행 시간 : 약 15분
- 서버 게임로직의 평균 프레임 레이트 : 23

> **NOTE**: 프레임 레이트는 높을수록 좋습니다.

#### 서버 측 프레임 시간 결과

| 시간 (약 20초 간격) | 평균 프레임 시간 (ms) | 최대 프레임 시간 (ms) | 목표 프레임 시간 (ms) |
| --- | --- | --- | --- |
| End | . | . | . |
| 08:13:13 ~ 08:13:39 | 43 | 89 | 33 |
| 08:12:47 ~ 08:13:13 | 42 | 89 | 33 |
| 08:12:21 ~ 08:12:47 | 43 | 104 | 33 |
| 08:11:55 ~ 08:12:21 | 43 | 79 | 33 |
| 08:11:29 ~ 08:11:55 | 43 | 94 | 33 |
| ... | ... | ... | ... |
| 08:01:55 ~ 08:02:19 | 38 | 147 | 33 |
| 08:01:31 ~ 08:01:55 | 38 | 1,006 | 33 |
| 08:01:08 ~ 08:01:31 | 36 | 192 | 33 |
| 08:00:45 ~ 08:01:08 | 37 | 100 | 33 |
| 08:00:22 ~ 08:00:45 | 34 | 244 | 33 |
| Start | . | . | . |

> **NOTE**: 프레임 시간은 낮을수록 좋습니다.

#### 서버 측 CPU 사용량 결과
![Screenshot 2024-06-29 172932](https://github.com/kimk-s/some1/assets/71550078/c088c728-2808-46f0-8c0d-2317e671db12)

#### 서버 측 Network In 사용량 결과
![Screenshot 2024-06-29 172949](https://github.com/kimk-s/some1/assets/71550078/55ae69ab-b867-4d32-991e-a38f78cbd493)

#### 서버 측 Network Out 사용량 결과
![Screenshot 2024-06-29 173000](https://github.com/kimk-s/some1/assets/71550078/5a1e1aab-eba6-4245-91ed-f6c09008c787)

## 애플리케이션 구성도
<img width="700" src="https://github.com/kimk-s/some1/assets/71550078/44a6959f-06ce-4de4-bde2-28163ce77ff1" />

### 서버 애플리케이션
- [Wait](https://github.com/kimk-s/some1/tree/main/src/Some1.Wait.Back.MagicServer) : 웹과 같은 요청/응답을 처리하는 아웃게임 서버 애플리케이션입니다.
- [Play](https://github.com/kimk-s/some1/tree/main/src/Some1.Play.Server.Tcp) : 실시간 멀티플레이를 처리하는 인게임 서버 애플리케이션입니다.

### 클라이언트 애플리케이션
- [Unity](https://github.com/kimk-s/some1/tree/main/src/Some1.User.Unity) : 게임 이용자가 사용하는 GUI 애플리케이션입니다.
- [CLI](https://github.com/kimk-s/some1/tree/main/src/Some1.User.CLI) : 게임 개발자가 부하테스트 등에 사용하는 명령줄 인터페이스 애플리케이션입니다.

## Play(인게임) 모듈 구성도
<img width="700" src="https://github.com/kimk-s/some1/assets/71550078/8a258aea-a476-4970-8b2d-92f5790139d8" />

### 서버 모듈
- [Core](https://github.com/kimk-s/some1/tree/main/src/Some1.Play.Core) : 비즈니스 로직 모듈입니다.
- [Data](https://github.com/kimk-s/some1/tree/main/src/Some1.Play.Data.Abstractions) : 데이터베이스 모듈입니다.

### 공용 모듈
- [Sync](https://github.com/kimk-s/some1/tree/main/src/Some1.Sync) : 객체의 상태 변화를 추적하고, 서버와 클라이언트 사이에 동기화하는 모듈입니다.
- [Net](https://github.com/kimk-s/some1/tree/main/src/Some1.Net) : 공용 네트워크 모듈입니다.

### 클라이언트 모듈
- [Front](https://github.com/kimk-s/some1/tree/main/src/Some1.Play.Front) : 비즈니스 로직 모듈입니다.
- [Client](https://github.com/kimk-s/some1/tree/main/src/Some1.Play.Client.Abstractions) : 네트워크 모듈입니다.

## Play(인게임) 서버 애플리케이션의 게임로직 루프
<img width="700" src="https://github.com/kimk-s/some1/assets/71550078/4dcc269f-8b62-4254-8c03-7df704c784d3" />

- [GameLogic](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/PlayCore.cs#L150) : 게임로직 최상위 메소드입니다.
- [Update Time](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/Internal/Time.cs#L58) : [시간](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/Internal/Time.cs)을 업데이트합니다.
- [Enter/Load Player](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/Internal/PlayerGroup.cs#L116) : 네트워크 연결이 시작된 플레이어를 입장시키고, 데이터베이스에서 데이터를 로드합니다.
- [Update Objects](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/Internal/LeaderGroup.cs#L24) : 플레이어의 요청을 수신합니다. [공간](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/Internal/Space.cs)에 있는 캐릭터, 총알 등의 [객체](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/Internal/Object.cs)를 업데이트합니다.
- [Update Space](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/Internal/LeaderGroup.cs#L29) : [공간](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/Internal/Space.cs)에 있는 이동한 [객체](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/Internal/Object.cs)의 위치 인덱싱을 업데이트합니다.
- [Leave/Save Player](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/Internal/PlayerGroup.cs#L122) : 네트워크 연결이 끝난 플레이어를 퇴장시키고, 데이터베이스에 데이터를 세이브합니다.
- [Send/Sync](https://github.com/kimk-s/some1/blob/main/src/Some1.Play.Core/Internal/LeaderGroup.cs#L34) : 플레이어에게 요청의 응답과 동기화 정보 등을 송신합니다.





