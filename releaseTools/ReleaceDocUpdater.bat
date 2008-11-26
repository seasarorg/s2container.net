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
cd D:\work\OSS\Seasar\seasar_net_www\ja
ReleaceDocUpdater.exe 1.3.12 1.3.13 index.html seasarnet.html download.html

rem s2conariner.net en
cd D:\work\OSS\Seasar\seasar_net_www\en
ReleaceDocUpdater.exe 1.3.12 1.3.13 index.html releases.html

rem s2dao.net
cd D:\work\OSS\Seasar\s2dao_net_www
ReleaceDocUpdater.exe 1.3.12 1.3.13 ja\index.html en\index.html