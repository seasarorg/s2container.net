#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using System.Text.RegularExpressions;
using Seasar.Framework.Util;

namespace Seasar.Framework.Container.AutoRegister
{
    /// <summary>
    /// �����o�^�̑ΏہA��ΏۂƂȂ�N���X���̃p�^�[����ێ����܂��B
    /// </summary>
    public class ClassPattern
    {
        private Regex[] _shortClassNamePatterns;

        /// <summary>
        /// �f�t�H���g�̃R���X�g���N�^�ł��B
        /// </summary>
        public ClassPattern()
        {
        }

        /// <summary>
        /// ���O��Ԗ��ƃN���X���̃p�^�[����󂯎��R���X�g���N�^�ł��B
        /// </summary>
        /// <param name="namespaceName">���O��Ԗ�</param>
        /// <param name="shortClassNames">�N���X���̃p�^�[��</param>
        public ClassPattern(string namespaceName, string shortClassNames)
        {
            NamespaceName = namespaceName;
            ShortClassNames = shortClassNames;
        }

        /// <summary>
        /// ���O��Ԗ���擾�E�ݒ肵�܂��B
        /// </summary>
        public string NamespaceName { set; get; }

        /// <summary>
        /// �i���O��Ԃ�܂܂Ȃ��j�N���X���̃p�^�[����ݒ肵�܂��B
        /// </summary>
        /// <remarks>
        /// �����̃p�^�[����ݒ肷��ꍇ�A','�ŋ�؂�܂��B
        /// </remarks>
        public string ShortClassNames
        {
            set
            {
                var classNames = value.Split(',');
                _shortClassNamePatterns = new Regex[classNames.Length];

                for (var i = 0; i < classNames.Length; ++i)
                {
                    var className = classNames[i].Trim();
                    _shortClassNamePatterns[i] = new Regex(className, RegexOptions.Compiled);
                }
            }
        }

        /// <summary>
        /// �i���O��Ԃ�܂܂Ȃ��j�N���X�����p�^�[���Ɉ�v���Ă��邩�ǂ�����Ԃ��܂��B
        /// </summary>
        /// <param name="shortClassName">�N���X��</param>
        /// <returns>��v���Ă���ꍇ��true, ��v���Ă��Ȃ��ꍇ��false</returns>
        public bool IsAppliedShortClassName(string shortClassName)
        {
            if (_shortClassNamePatterns == null)
            {
                return true;
            }

            for (var i = 0; i < _shortClassNamePatterns.Length; ++i)
            {
                if (_shortClassNamePatterns[i].IsMatch(shortClassName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// ���O��Ԗ����p�^�[���Ɉ�v���Ă��邩�ǂ�����Ԃ��܂��B
        /// </summary>
        /// <param name="namespaceName">���O��Ԗ�</param>
        /// <returns>��v���Ă���ꍇ��true, ��v���Ă��Ȃ��ꍇ��false</returns>
        public bool IsAppliedNamespaceName(string namespaceName)
        {
            if (!StringUtil.IsEmpty(namespaceName)
                && !StringUtil.IsEmpty(this.NamespaceName))
            {
                return AppendDelimiter(namespaceName).StartsWith(
                    AppendDelimiter(this.NamespaceName));
            }

            if (StringUtil.IsEmpty(namespaceName)
                && StringUtil.IsEmpty(this.NamespaceName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// �f���~�^��ǉ����܂��B
        /// </summary>
        /// <param name="name">���O��Ԗ�</param>
        /// <returns>���O��Ԗ��Ɍ��Ƀf���~�^('.')��ǉ��������</returns>
        protected static string AppendDelimiter(string name)
        {
            return name.EndsWith(".") ? name : name + ".";
        }
    }
}
