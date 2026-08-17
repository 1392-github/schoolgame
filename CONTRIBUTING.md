# 기여 규칙
## 코딩 스타일
 * Allman 코딩 스타일을 사용해야 합니다. ({, }를 별도 줄에 배치)
 * 아래에서 별도로 명시되지 않은 한 protected는 private에 포함합니다.
 * internal, protected internal, private protected 접근 제어자는 사용할 수 없습니다.
 * public 함수 이름 및 클래스 이름은 PascalCase를, private 함수 이름은 camelCase를 사용하세요.
 * 변수 이름은 public, private 무관하게 camelCase를 사용하세요.
 * private 변수 및 함수의 이름은 동명의 public 변수 및 함수와의 구분을 위하여 _로 시작할 수 있습니다. public 변수 및 함수의 이름은 _로 시작할 수 없습니다.
 * 공백 4칸 들여쓰기를 사용합니다.
 * sealed는 사용하지 않습니다.
 * 본 프로젝트에서 자체적으로 사용하는 코드에서는 namespace를 사용하지 않습니다.
 * var 키워드는 사용하지 않습니다. (익명 객체를 저장하기 위한 변수는 예외입니다.)
 * 자동 구현 속성으로 구현할 수 있는 형태의 속성은 속성 대신 필드로 선언해야 합니다.
  * "자동 구현 속성으로 구현할 수 있는 형태의 속성"은 속성의 읽기 동작이 클래스 내부의 특정 필드의 값을 아무런 가공이나 추가 처리 없이 그대로 반환하기만 하고, 쓰기 동작이 같은 필드에 값을 아무런 가공이나 추가 처리 없이 그대로 쓰기만 하는 속성을 의미합니다.
 * 배열을 한 줄로 선언할 때 {와 첫 번째 원소 사이, 마지막 원소와 } 사이에는 공백 없이 붙여 쓰고, 마지막 원소 뒤에는 ,를 넣지 않습니다.
 * private 키워드는 생략합니다.
### 잘못된 코딩 스타일의 예시
```
namespace DoNotUseNamespace // 네임스페이스 선언하면 안 됨
{
    public sealed class test { // PascalCase 아님, Allman 스타일 아님, sealed 붙음
        internal int dontUseInternal; // internal 안 됨
        private int test; // private는 생략해야 함
        public readonly int[] gradeCutPercentile = { 96, 89, 77, 60, 40, 23, 11, 4, }; // 마지막 원소 뒤에 , 붙음, {} 앞뒤에 공백 있음
        public int PascalCase; // camelCase 아님
		public int tab; // 공백 대신 탭 씀
      public int twoSpaces; // 공백 4칸이 아니라 2칸임
        var score = 0; // var 안 됨
        public int _mustNotUnderbar; // public 변수 앞에 _ 안됨
        public int money { get; set; } // 프로퍼티 말고 필드로 선언해야 함
    }
}
```
## 난독화 금지
 * 난독화(다른 사람이 코드를 보는 것을 방해하는 목적으로 가독성을 의도적으로 낮추는 행위)는 허용되지 않습니다.
## 실명 사용
이 프로젝트에 PR(Pull Request)을 보낼 때는 반드시 기여자의 실명(성 포함)의 전체를 게시글 본문에 게시해야 합니다.
 * GitHub 프로필에만 게시하는 것은 나중에 프로필을 삭제 또는 변경하여 실명을 비공개할 가능성으로 인하여 인정되지 않습니다.
 * 실명은 성을 포함한 실명의 전체를 의미합니다. 다음은 올바른 표기로 간주되지 않습니다. ("조운혁"이 올바른 표기)
  * 1392 (실명이 아니라 닉네임임)
  * 운혁 (성 빠짐)
  * 조O혁 (글자 가림)
  * ㅈㅇㅎ (초성만 나옴)
  * 조 모 군 (성만 나옴)
 * 실명과 닉네임을 병기(같이 표기)하는 것은 허용됩니다.
  * 닉네임이 통상적인 실명의 형태와 비슷하여 어느 것이 실명인지 모호한 경우(예: 조운혁(홍길동))는 어느 것이 실명인지 또는 닉네임인지 명시해야 합니다.
 * PR 없이 Issue만 보내는 경우는 해당되지 않습니다. 
## 에셋 저작권 제한
 * 에셋은 본인이 제작했거나, 저작권이 인정되지 않거나(만료 저작물, 창작성을 충족하지 않는 저작물(플레이스홀더, 단색 이미지 등), AI를 이용한 이미지 등), 불특정 다수가 이용 및 재배포할 수 있는 라이선스(CC BY, CC0 등)를 가져야 합니다.
  * Unity Asset Store의 에셋의 경우 [Asset Store EULA](https://unity.com/kr/legal/as-terms)의 2.2.1.1 (d)로 인하여 재배포할 수 없으므로 Unity Asset Store의 에셋은 본 프로젝트에 사용할 수 없습니다.
 * CCL의 NC, ND, SA 및 이와 유사한 조건이 있는 라이선스(공공누리 2, 3, 4유형, GPL, GFDL 등)는 금지됩니다.
 * 본인이 직접 AI를 이용하여 만든 이미지의 경우 AI의 프롬프트(공유 링크 등)를 README.md에 기재해야 합니다.