@echo off
setlocal
cd /d "%~dp0"
call build-x86.bat
call build-x64.bat
