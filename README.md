# MarkingSystemV2

만텍 각인 시스템 v2 — PLC 없이 wizMES API만 사용하는 단순 조회/저장용 WPF 앱.

## 신규 개발자 1회 설정

저장소 clone 후 **한 번만** 실행하세요. 자동 SemVer 태깅이 동작하려면 필수입니다.

```bash
git clone https://github.com/fomalhaut17/MarkingSystemV2.git
cd MarkingSystemV2
git config core.hooksPath .githooks
git config push.followTags true
```

## 빌드 및 실행

```bash
# 개발 빌드
dotnet build MarkingSystemV2/MarkingSystemV2.csproj -c Debug

# 릴리스용 단일 exe
dotnet publish MarkingSystemV2/MarkingSystemV2.csproj -c Release
```

> `dotnet build`는 incremental 빌드라 dll에 박힌 버전이 stale할 수 있음. **버전을 정확히 박으려면 `--no-incremental` 또는 `publish` 사용**.

## 버전 관리 (SemVer)

git tag(`vX.Y.Z`) 자체가 버전의 single source of truth. **MinVer**가 빌드 시 태그를 읽어 `AssemblyInformationalVersion`에 자동 주입합니다. csproj엔 버전 값을 적지 않습니다.

### 매 커밋마다 patch 자동 +1

`.githooks/post-commit`이 직전 vX.Y.Z 태그를 찾아 patch를 1 올린 annotated 태그를 자동 생성합니다.

```
v0.1.2 → git commit ... → [hook] 자동 태그 생성: v0.1.3
```

`git push`로 master를 올리면 새 태그도 함께 따라갑니다 (`push.followTags=true` 필요).

### 자동 태그 skip (특수 케이스)

태그를 만들고 싶지 않은 임시 커밋이라면 둘 중 하나:

```bash
# 환경변수
NO_BUMP=1 git commit -m "wip: ..."

# 또는 커밋 메시지 마지막에 trailer
git commit -m "$(cat <<'EOF'
wip: 작업 중 임시 커밋

Skip-Bump: true
EOF
)"
```

### minor / major 올리기

자동 hook은 patch만 올립니다. minor/major 릴리스는 수동으로:

```bash
git tag -a v0.2.0 -m "Release v0.2.0"
git push --follow-tags
```

## UI에서 현재 버전 확인

메인 화면 하단 상태바 우측에 `v0.1.x` 형태로 표시됩니다. 테스터/사용자가 알려주면 개발팀이 git tag 목록과 비교해 차이를 확인합니다.
