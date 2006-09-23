※このソリューションは次のような構造をしています。

1. ソリューションの構造
　a. S2WindowsExample.Formsプロジェクト
　　　・スタートアッププロジェクト
　　　・WindowsFormおよび画面関連クラスを配置
　　　・プレゼンテーション層に相当する

　b. S2WindowsExample.Logicsプロジェクト
　　　・ロジック関連クラスを配置
　　　・Serviceフォルダはサービス層に相当する
　　　・DAOフォルダはドメイン層に相当する
　　　・DTOフォルダはData Transfer Objectを配置する

　c. S2WindowsExample.Testsプロジェクト
　　　・Test関連クラスを配置

※利用するときの変更点は次のとおりです。
・名前空間の変更(既存クラス、各プロジェクトの規定名前空間）
・Example.diconファイル内でIFormDisptcherの名前空間を変更する
・Ex.diconのDB設定を変更する
・各App.config内のこのソリューションの名前空間を変更する
・各App.config内のProviderを変更する
・各プロジェクトのビルドイベント内のフォルダを指定している
ところを、変更する。