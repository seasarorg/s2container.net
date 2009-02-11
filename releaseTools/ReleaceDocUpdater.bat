rem ------------------------------------------------------------------
rem cd XXXの部分にはSeasar.NETのwwwフォルダがあるパスを指定して下さい。
rem ------------------------------------------------------------------
set path=%~dp0.

rem ------------------------------------------------------------------------------
rem ReleaceDocUpdater.exe Seasarのホームページ上のリリース情報の
rem 　　　　　　　　　　　バージョン番号を更新
rem USAGE:
rem    1:現在のバージョン番号
rem    2:新しいバージョン番号
rem    3～:更新対象となるファイルパス（複数指定する場合は空白区切りでパスを記述）
rem -----------------------------------------------------------------------------

rem s2conariner.net ja
cd C:\MyPrograms\Seasar\www\ja
ReleaceDocUpdater.exe 1.3.13 1.3.14 index.html seasarnet.html download.html

rem s2conariner.net en
cd C:\MyPrograms\Seasar\www\en
ReleaceDocUpdater.exe 1.3.13 1.3.14 index.html releases.html

rem s2dao.net
cd C:\MyPrograms\Seasar\s2dao_www
ReleaceDocUpdater.exe 1.3.13 1.3.14 ja\index.html en\index.html