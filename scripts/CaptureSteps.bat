@echo off
chcp 65001 >nul
cd /d "%~dp0"
title CaptureSteps build + run

if not exist CaptureSteps.exe (
  echo Compiling CaptureSteps.cs ...
  "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /out:CaptureSteps.exe CaptureSteps.cs
  if errorlevel 1 (
    echo Build failed. Need .NET Framework 4.x csc.
    pause
    exit /b 1
  )
)

CaptureSteps.exe
if errorlevel 1 pause
