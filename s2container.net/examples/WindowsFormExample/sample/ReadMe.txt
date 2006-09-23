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

※ソースコードは.NET2.0向けと.NET1.1向けが共存している部分があります。
コンパイルオプションで切り替わっています。
