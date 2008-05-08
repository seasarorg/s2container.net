rem ----------------------------------------------------
rem Seasar.NETのwwwフォルダがあるパスを指定して下さい。
rem ----------------------------------------------------
set path=%~dp0.

rem s2conariner.net ja
rem cd D:\work\OSS\Seasar\seasar_net_www\ja
rem ReleaceDocUpdater.exe 1.3.8 1.3.9 index.html seasarnet.html download.html

rem s2conariner.net en
cd D:\work\OSS\Seasar\seasar_net_www\en
ReleaceDocUpdater.exe 1.3.8 1.3.9 index.html releases.html

rem s2dao.net
rem cd D:\work\OSS\Seasar\s2dao_net_www
rem ReleaceDocUpdater.exe 1.3.8 1.3.9 ja\index.html en\index.html