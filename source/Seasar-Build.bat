@ECHO OFF
SETLOCAL

echo @SET PATH=%windir%\Microsoft.NET\Framework\v3.5;%windir%\Microsoft.NET\Framework\v3.0;%windir%\Microsoft.NET\Framework\v2.0.50727;Microsoft.NET\Framework\v4.0.30319;%PATH%
@SET PATH=%windir%\Microsoft.NET\Framework\v4.0.30319;%windir%\Microsoft.NET\Framework\v3.5;%windir%\Microsoft.NET\Framework\v3.0;%windir%\Microsoft.NET\Framework\v2.0.50727;Microsoft.NET\Framework\v4.0.30319;%PATH%

ECHO Seasar.NETのビルドを開始します。
REM TODO : エラー処理
msbuild Seasar.sln /p:Configuration=Release /t:Clean;Rebuild
msbuild Seasar-Build.xml /target:CopyBuildFiles

ENDLOCAL
