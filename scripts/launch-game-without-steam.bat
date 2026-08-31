@echo off
REM Launch RetroArch content WITHOUT Steam so Steam Input cannot remap the pad.
REM Edit the three paths below for your machine.

chcp 65001 >nul
title Launch without Steam

set "XOUTPUT=%~dp0..\tools-local\XOutput\XOutput.exe"
set "RETROARCH=C:\Path\To\RetroArch"
set "CORE=%RETROARCH%\cores\pcsx2_libretro.dll"
set "CONTENT=C:\Path\To\Game.iso"

echo.
echo  1) Gamepad Mode LED = RED (analog)
echo  2) XOutput Start
echo  3) Launching RetroArch outside Steam
echo.

if exist "%XOUTPUT%" (
  tasklist /FI "IMAGENAME eq XOutput.exe" | find /I "XOutput.exe" >nul
  if errorlevel 1 (
    start "" "%XOUTPUT%"
    timeout /t 2 /nobreak >nul
  )
) else (
  echo  [i] XOutput path not set — start it manually.
)

if not exist "%RETROARCH%\retroarch.exe" (
  echo  [!] Set RETROARCH path in this bat file.
  pause
  exit /b 1
)
if not exist "%CONTENT%" (
  echo  [!] Set CONTENT path in this bat file.
  pause
  exit /b 1
)

cd /d "%RETROARCH%"
start "" "retroarch.exe" -L "%CORE%" "%CONTENT%"
