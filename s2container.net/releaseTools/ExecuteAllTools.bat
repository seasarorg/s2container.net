rem アセンブリファイルのバージョン番号更新
call AssemblyUpdater.bat

rem コンパイルとbuildフォルダへのコピー
cd ..\source
call Seasar-Build.bat