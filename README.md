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

🛠️ 개발 환경 정보
-

- IDE : VS 2022
- Language : C# (WPF)

📁 What's included?
-

| Name| Framework | Build Status |
| --- | --- | --- | 
| **WPFKakaoTalk**<br />main application project | ![NET6](https://img.shields.io/badge/.NET-6.0-red)<br/>(Windows Only) | None


📕 library to use
-

| Name | Version |
| --- | --- |
| **LogHelper**<br/>Log | ![NETFramework45](https://img.shields.io/badge/.NET%20Framework-4.5-orange)<br/>self-production
| **Microsoft.Toolkit.Mvvm**<br/>MVVM | 7.1.2 |
| **Microsoft.Xaml.Behaviors**<br/>MVVM |  |
| **Microsoft.Extensions.DependencyInjection**<br />DependencyInjection | 6.0.0 |


***

> KakaoTalk?
>> Messenger serviced by Kakao, a representative Korean company

<br/>
This is a KakaoTalk PC version clone project with WPF<br/>
This is a WPF project that implements pure client functions without a separate communication function with the server.<br/>
We set the goal of implementation so that it can actually operate as much as possible other than server communication.
Made.
<br/><br/>
through this project
You can learn basic WPF syntax, MVVM architecture pattern.

Environment
-

- IDE : VS 2022
- Language : C# (WPF)

***


💡 솔루션 구조
-

Model / View / ViewModel 모두 물리적 분리 목표


View -> Common 의존 참조 (외부에서 ViewModel 주입)<br/>
ViewModel -> Common, Model, Service 의존 참조<br/>
Service -> View 의존 참조 (popup window IoC 관리) <br/>
Model 의존 참조 없음 (단독 모듈)

✅ 구현 기능
-

- [x] 로그인 화면
- [x] 로그인 잠금 화면
- [x] 로그인 환경설정 [팝업]
- [x] 메인 환경설정 [팝업]
- [x] 프로필 정보 보기 화면 [팝업]
- [x] 친구 리스트 화면
- [x] 친구 검색 및 추가
- [x] 채팅방 리스트 화면
- [x] 채팅 화면
- [x] 실제 채팅 기능 [서버와 통신 X]
- [x] 채팅 화면에서 Infinite scroll 구현
- [x] 채팅 대화 말풍선 마우스 드래그로 선택 구현 [Adorner(**마우스 드래그 사각형 영역 표시**), ItemsControlSelector(**ListBox Item 매뉴얼하게 선택**) 구현 / SelectedItems Behavior 구현 ]

☑️ 앞으로 구현 기능
-

- [ ] 채팅 목록 캡쳐 해서 내보내기 기능
- [ ] 채팅방 드래그 앤 드롭 으로 프로그램 외부에 플로팅 띄우기


📷 Screenshots
-

#### `로그인`
![image](https://user-images.githubusercontent.com/13028129/168229017-63e40d38-4b87-45bc-b040-fea457932bef.png)


#### `잠금 화면`
![image](https://user-images.githubusercontent.com/13028129/168453264-f05b781f-b9ce-49da-a4ea-0f9fca887631.png)


#### `메인 환경설정`
![image](https://user-images.githubusercontent.com/13028129/168229251-a6136f83-1388-40b7-bc8b-fbb3b5be3c78.png)


#### `메인 환경설정 > 프로필`
![image](https://user-images.githubusercontent.com/13028129/168229303-30a339a1-49ee-4ef6-8dba-d1d532ad23fb.png)


#### `친구 리스트`
![image](https://user-images.githubusercontent.com/13028129/168229352-954a75b4-0eff-474c-af10-b4c50658307c.png)


#### `친구 검색`
![wpfKakaoTalk_FriendFilter](https://user-images.githubusercontent.com/13028129/169466846-1fc9317e-90a0-44aa-bf2e-88e138ca7015.gif)


#### `친구 검색 및 친구 등록`
![image](https://user-images.githubusercontent.com/13028129/168712827-ad47f974-ba86-46f8-ac76-db5f1bab5621.png)


#### `프로필 보기 및 친구 이름 변경`
![image](https://user-images.githubusercontent.com/13028129/168711985-78ebf7d5-cd69-404d-a5e8-54fa3933665f.png)


#### `채팅방 리스트`
![image](https://user-images.githubusercontent.com/13028129/168229381-1d8329de-3c4d-4b34-8d6d-8bd8a270695c.png)<br/>


#### `채팅1`
![wpfKakaoTalk_Chat](https://user-images.githubusercontent.com/13028129/169466176-c6977249-462d-4057-a35a-16a1e5fe8654.gif)


#### `채팅2`
![image](https://user-images.githubusercontent.com/13028129/169466073-f4e1e605-eaab-4c80-9c8c-6a59e772aa05.png)
