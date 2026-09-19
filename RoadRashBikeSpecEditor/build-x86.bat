@echo off
setlocal
cd /d "%~dp0"
dotnet publish -c Release -r win-x86 --self-contained true -p:PublishSingleFile=true -p:Platform=x86
echo.
echo Output: bin\Release\net8.0-windows\win-x86\publish\
pause
