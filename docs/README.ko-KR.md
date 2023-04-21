# TeamMatcher

![](https://img.shields.io/tokei/lines/github/Sharp0802/TeamMatcher)
![](https://img.shields.io/github/license/sharp0802/TeamMatcher)
![](https://img.shields.io/github/release-date-pre/sharp0802/TeamMatcher)
![](https://img.shields.io/github/languages/top/sharp0802/TeamMatcher)
![](https://img.shields.io/github/languages/count/sharp0802/TeamMatcher)

언어: [English](README.md), [한국어](README.ko-KR.md)

C#으로 작성된, 휴리스틱한 팀 매칭 프로그램.

# 개요

`teammatcher` - 데이터를 특정한 수의 팀으로 섞고, 안정화 한다.

# 문법

```shell
$ teammatcher [option]...
```

# 설명

팀 데이터를 생성하기 위해 다음과 같은 셸 명령어를 사용할 수 있다.

```shell
$ teammatcher --csv-file <your-csv-data> --team-count <team-count>
```

이때, 입력 CSV는 다음과 같은 꼴이어야 한다:

```csv
name, kind,num,val
name1,여자, 01, 06
name2,남자, 02, 07
name3,여자, 03, 08
name4,남자, 04, 09
```
(유니코드를 지원한다.)

종류 값은 항상 성별만을 나타낼 필요는 없다:

```csv
name, kind,  num,val
name1,kind-0,01, 06
name2,kind-1,02, 07
name3,kind-0,03, 08
name4,kind-2,04, 09
```

이 CSV헤더에서, 각각의 헤더는 다음을 표현한다:

- `name`: 사람이 읽을 수 있는 이름.
- `kind`: 열의 종류. 섞는 작업은 이 종류 값에 따라 분리되어 진행된다. 따라서, 섞는 작업 이후에, 특정한 종류의 요소들은 각각의 팀에서 대략 균등하게 분포되어 있을 것이다.
- `num`: 열의 식별자.
- `val`: 열의 값. 섞는 작업은 이 값을 기준으로 정렬될 것이다. 섞는 작업 이후에, 각각의 팀의 모든 이 값의 합은 대략 균등하게 분포되어 있을 것이다..

CSV헤더의 순서는 작업에 영향을 주지 않는다.

# 옵션

- `--csv-file`: CSV 데이터 파일.
- `--team-count`: 팀의 수.

# 경고

`teammatcher`는 휴리스틱한 알고리즘을 사용한다.
따라서, `teammatcher`의 결과는 최적근사값으로부터 멀어질 수 있다.

`teammatcher`가 휴리스틱한 알고리즘을 사용하므로,
팀의 수가 증가하면, 오차 또한 커질 것이다.
이러한 오차는 팀의 수에 비례하여 증가한다.

## 컴파일 (셸 환경)

### 요구사항

- dotnet SDK 8.0.0 또는 상위 버전

### 컴파일

```shell
cd <project-dir>/src
dotnet publish -c Release -o publish
```

위의 명령이 성공했다면, 다음의 명령으로 `teammatcher`를 실행시킬 수 있다:

```shell
cd publish
teammatcher [option]...
```