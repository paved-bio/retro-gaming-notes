@echo off
chcp 65001 >nul
cd /d "%~dp0"
title PadTest build + run

if not exist PadTest.exe (
  echo Compiling PadTest.cs ...
  "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /out:PadTest.exe PadTest.cs
  if errorlevel 1 (
    echo Build failed. Need .NET Framework 4.x csc.
    pause
    exit /b 1
  )
)

PadTest.exe
echo.
pause
