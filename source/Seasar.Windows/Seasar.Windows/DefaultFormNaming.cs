using System;
using Seasar.Framework.Container.AutoRegister;

namespace Seasar.Windows
{
    /// <summary>
    /// 起動WindowsForm指定用クラス
    /// </summary>
    public class DefaultFormNaming : IAutoNaming
    {
        private string _mainFormName;
        private string _label = "MainForm";

        /// <summary>
        /// 起動WindowsForm
        /// </summary>
        public string MainFormName
        {
            get { return _mainFormName; }
            set { _mainFormName = value; }
        }

        /// <summary>
        /// WindowsForm指定ラベル
        /// </summary>
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        #region IAutoNaming Members

        /// <summary>
        /// コンポーネント名を定義します。
        /// </summary>
        /// <param name="type">コンポーネント名を定義したいType</param>
        /// <returns>コンポーネント名</returns>
        public string DefineName(Type type)
        {
            string name = type.Name;
            if (name == _mainFormName)
            {
                if (!String.IsNullOrEmpty(_label))
                    return _label;
                else
                    return name;
            }
            else
            {
                return name;
            }
        }

        #endregion
    }
}