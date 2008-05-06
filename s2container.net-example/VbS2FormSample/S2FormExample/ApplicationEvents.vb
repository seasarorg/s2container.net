''
'' Copyright 2005-2008 the Seasar Foundation and the Others.
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
Imports log4net.Config
Imports Microsoft.VisualBasic.ApplicationServices
Imports log4net
Imports System.Reflection
Imports log4net.Util

Namespace My
    ' 次のイベントは MyApplication に対して利用できます:
    ' 
    ' Startup: アプリケーションが開始されたとき、スタートアップ フォームが作成される前に発生します。
    ' Shutdown: アプリケーション フォームがすべて閉じられた後に発生します。このイベントは、通常の終了以外の方法でアプリケーションが終了されたときには発生しません。
    ' UnhandledException: ハンドルされていない例外がアプリケーションで発生したときに発生するイベントです。
    ' StartupNextInstance: 単一インスタンス アプリケーションが起動され、それが既にアクティブであるときに発生します。 
    ' NetworkAvailabilityChanged: ネットワーク接続が接続されたとき、または切断されたときに発生します。
    Partial Friend Class MyApplication
        Private Shared ReadOnly logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

        ''' <summary>
        ''' アプリケーション フォームがすべて閉じられた後に発生するイベント
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub MyApplication_Shutdown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shutdown
            logger.Info("終了")
        End Sub

        ''' <summary>
        ''' アプリケーションが開始されたとき、スタートアップ フォームが作成される前に発生するイベント
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>スプラッシュスクリーンを起動している</remarks>
        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As StartupEventArgs) Handles Me.Startup

            Try
                Dim info As FileInfo = _
                        New FileInfo(Format(SystemInfo.AssemblyShortName(Assembly.GetExecutingAssembly()), _
                                              "{0}.exe.config"))
                XmlConfigurator.Configure(LogManager.GetRepository(), info)
                logger.Info("起動")

            Catch ex As Exception
                MessageBox.Show(ex.Message, "起動中", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try

        End Sub
    End Class
End Namespace

