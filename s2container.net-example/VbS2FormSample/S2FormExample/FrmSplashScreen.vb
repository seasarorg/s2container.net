''
'' Copyright 2005-2007 the Seasar Foundation and the Others.
''
'' Licensed under the Apache License, Version 2.0 (the "License");
'' you may not use this file except in compliance with the License.
'' You may obtain a copy of the License at
''
''     http://www.apache.org/licenses/LICENSE-2.0
''
'' Unless required by applicable law or agreed to in writing, software
'' distributed under the License is distributed on an "AS IS" BASIS,
'' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
'' either express or implied. See the License for the specific language
'' governing permissions and limitations under the License.
''
Imports System.IO

Public NotInheritable Class FrmSplashScreen
    'TODO: このフォームは、プロジェクト デザイナ ([プロジェクト] メニューの下の [プロパティ]) の [アプリケーション] タブを使用して、
    '  アプリケーションのスプラッシュ スクリーンとして簡単に設定することができます


    Private Sub FrmSplashScreen_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'アプリケーションのアセンブリ情報に従って、ランタイムにダイアログ テキストを設定します。  

        'TODO: [プロジェクト] メニューの下にある [プロジェクト プロパティ] ダイアログの [アプリケーション] ペインで、アプリケーションのアセンブリ情報を 
        '  カスタマイズします

        'アプリケーション タイトル

        'デザイン時に書式設定文字列としてバージョン管理で設定されたテキストを使用して、バージョン情報の書式を
        '  設定します。これにより効率的なローカリゼーションが可能になります。
        '  ビルドおよび リビジョン情報は、次のコードを使用したり、バージョン管理のデザイン時のテキストを 
        '  "Version {0}.{1:00}.{2}.{3}" のように変更したりすることによって含めることができます。
        '  詳細については、ヘルプの String.Format() を参照してください。
        '
        '    Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)

        Version.Text = _
            String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)

        '著作権情報
        Copyright.Text = My.Application.Info.Copyright
    End Sub
End Class
