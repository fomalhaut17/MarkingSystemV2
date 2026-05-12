@echo off
setlocal

set ENV=%1
if "%ENV%"=="" (
    echo Usage: build-release.bat [local^|prod]
    echo.
    echo   local  - Mock API server target ^(localhost:47300^)
    echo   prod   - wizMES production API target
    exit /b 1
)

if not "%ENV%"=="local" if not "%ENV%"=="prod" (
    echo Error: ENV must be 'local' or 'prod'
    exit /b 1
)

set OUT=dist\%ENV%

git describe --tags --abbrev=0 > %TEMP%\msv2_gittag.txt 2>nul
set /p TAG=<%TEMP%\msv2_gittag.txt
del /q %TEMP%\msv2_gittag.txt
if "%TAG%"=="" set TAG=unknown
powershell -NoProfile -Command "Set-Content -Path '%TEMP%\msv2_buildtime.txt' -Value (Get-Date -Format 'yyyyMMdd-HHmm') -Encoding ASCII -NoNewline"
set /p BUILDTIME=<%TEMP%\msv2_buildtime.txt
del /q %TEMP%\msv2_buildtime.txt

echo.
echo [1/4] Publishing MarkingSystemV2 (%ENV%)...
dotnet publish MarkingSystemV2\MarkingSystemV2.csproj -c Release -o %OUT% --nologo
if errorlevel 1 (
    echo Publish failed.
    exit /b 1
)

echo [2/4] Setting AppMode to %ENV%...
powershell -NoProfile -Command "Set-Content -Path '%OUT%\appsettings.json' -Value '{\"AppMode\":\"%ENV%\"}' -Encoding UTF8"

echo [3/4] Cleaning unused files (other appsettings, pdb)...
for %%e in (local prod) do (
    if not "%%e"=="%ENV%" (
        if exist %OUT%\appsettings.%%e.json del /f /q %OUT%\appsettings.%%e.json
    )
)
if exist %OUT%\MarkingSystemV2.pdb del /f /q %OUT%\MarkingSystemV2.pdb

echo [4/4] Creating ZIP archive...
set ZIP=dist\MarkingSystemV2-%ENV%-%TAG%-%BUILDTIME%.zip
if exist %ZIP% del /f /q %ZIP%
powershell -NoProfile -Command "Compress-Archive -Path '%OUT%\*' -DestinationPath '%ZIP%'"
if errorlevel 1 (
    echo ZIP failed.
    exit /b 1
)

echo.
echo Done.
echo   Tag:           %TAG%
echo   Build time:    %BUILDTIME%
echo   Output folder: %OUT%\
echo   ZIP archive:   %ZIP%
echo.

endlocal
