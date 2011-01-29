using System.Windows.Forms;
using Nullables;
using Seasar.Windows.Attr;

namespace Seasar.WindowsBlank.Forms
{
    /// <summary>
    /// 画面遷移定義FormDispatcherインターフェイス
    /// </summary>
    /// <remarks>
    /// モーダレスで表示するフォームは、diconファイルでinstance=prototypeにする
    /// <newpara>
    /// フォームに値を渡すときは表示メソッドに引数を用意し、同名のプロパティを対象のフォームに用意する。
    /// </newpara>
    /// <newpara>
    /// メソッド(または対象フォーム)と名前空間を変更し、使いまわしてもよい。
    /// </newpara>
    /// </remarks>
    public interface IFormDispatcher
    {
        
    }
}