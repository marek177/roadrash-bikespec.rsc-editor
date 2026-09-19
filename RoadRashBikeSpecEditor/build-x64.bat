@echo off
setlocal
cd /d "%~dp0"
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:Platform=x64
echo.
echo Output: bin\Release\net8.0-windows\win-x64\publish\
pause
