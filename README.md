# WPFKakaoTalk

WPF로 KakaoTalk PC버전 클론 프로젝트 입니다. <br/>
서버와 별도 통신 기능이 없는 순수 클라이언트 기능을 WPF로 구현한 프로젝트 입니다. <br/>
서버 통신 이외 가급적 실제 동작할 수 있도록 구현 목표를 잡았으며 이 프로젝트는 WPF를 배우는 초급, 초중급 수준의 대상으로 학습 목적으로 <br/>
제작하였습니다. <br/>
저 역시 모자란 부분이 많아 잘못된 부분이 있다면 같이 함께 학습하면서 고쳐봤으면 좋겠습니다.

이 프로젝트는 다음과 같은 부분을 학습하실 수 있습니다.<br/>
- C# 기본문법을 익힐 수 있습니다.<br/>
(c# 9.0 일부 Record,  init 속성, C# 8.0 일부 IAsyncEnumerable 비동기 스트림 간략 예제 포함)
- 기본적인 WPF xaml에 대해 익힐 수 있습니다.
- 기본적인 WPF MVVM 패턴에 대해 익힐 수 있습니다.
- WPF UC 제작 방식에 대해 익힐 수 있습니다.
- 기본 컨트롤에 스타일 적용을 하여 커스텀한 UI 제작에 대해 알아 볼 수 있습니다.
- 커스텀한 로컬 환경설정 처리 방식에 대해 알아 볼 수 있습니다.
- Command처리 및 다양한 바인딩 처리 방식에 대해 익힐 수 있습니다.
- 간단한 커스텀 컨트롤 샘플 예제가 있어 직접 활용할 수 있습니다.

개발 환경 정보
-

- IDE : VS 2022
- Language : C# (WPF)
- Framework : .Net6 / Windows Only

사용 라이브러리
-

- 로그 관련
  - LogHelper / 자체 제작
- MVVM 관련
  - Microsoft.Toolkit.Mvvm / ver : 7.1.2
  - Microsoft.Xaml.Behaviors
- DependencyInjection 관련
  - Microsoft.Extensions.DependencyInjection / ver : 6.0.0


솔루션 구조
-

Model / View / ViewModel 모두 물리적 분리 목표


View -> Common 의존 참조 (외부에서 ViewModel 주입)
ViewModel -> Common, Model, Service 의존 참조
Service -> View 의존 참조
Model 의존 참조 없음 (단독 모듈)

구현 기능
-

- 로그인 화면
- 로그인 잠금 화면
- 로그인 환경설정
- 메인 환경설정
- 친구 리스트 화면
- 채팅방 리스트 화면

앞으로 구현 기능
-

- 채팅방 화면
- 실제 채팅 기능 [서버와 통신 X]
- 채팅 목록 캡쳐 해서 내보내기 기능
- 채팅 화면에서 인피니티 스크롤 구현

캡쳐 화면
-

![image](https://user-images.githubusercontent.com/13028129/168229017-63e40d38-4b87-45bc-b040-fea457932bef.png)<br/>
**[로그인]**

![image](https://user-images.githubusercontent.com/13028129/168229218-d91e6407-26de-401a-a9f6-85e791790ad2.png)<br/>
**[잠금 화면]**

![image](https://user-images.githubusercontent.com/13028129/168229251-a6136f83-1388-40b7-bc8b-fbb3b5be3c78.png)<br/>
**[메인 환경설정 화면]**

![image](https://user-images.githubusercontent.com/13028129/168229303-30a339a1-49ee-4ef6-8dba-d1d532ad23fb.png)<br/>
**[메인 환경설정 > 프로필]**

![image](https://user-images.githubusercontent.com/13028129/168229352-954a75b4-0eff-474c-af10-b4c50658307c.png)<br/>
**[친구 리스트]**

![image](https://user-images.githubusercontent.com/13028129/168229381-1d8329de-3c4d-4b34-8d6d-8bd8a270695c.png)<br/>
**[채팅방 리스트]**
