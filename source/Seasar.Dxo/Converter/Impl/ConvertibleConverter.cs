#region Copyright

/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Seasar.Dxo.Converter.Impl
{
    /// <summary>
    /// �I�u�W�F�N�g��IConvertible�ɕϊ����邽�߂̃R���o�[�^�N���X
    /// </summary>
    public class ConvertibleConverter : AbstractPropertyConverter
    {
        /// <summary>
        /// �I�u�W�F�N�g�̃v���p�e�B��C�ӂ̌^�ɕϊ����܂�
        /// (���ۃ��\�b�h�͔h���N���X�ŕK���I�[�o���C�h����܂�)
        /// </summary>
        /// <param name="source">�ϊ����̃I�u�W�F�N�g</param>
        /// <param name="dest">�ϊ���̃I�u�W�F�N�g</param>
        /// <param name="expectType">�ϊ���̃I�u�W�F�N�g�Ɋ��҂���Ă���^</param>
        /// <returns>bool �ϊ������������ꍇ�ɂ�true</returns>
        protected override bool DoConvert(object source, ref object dest, Type expectType)
        {
            Debug.Assert(typeof (IConvertible).IsAssignableFrom(expectType)
                         , String.Format(DxoMessages.EDXO1004, "expectType", "IConvertible"));
//            Debug.Assert(typeof(IConvertible).IsAssignableFrom(expectType)
//                         , "expectType��IConvertible�̂͂�");

            if (source is IConvertible)
            {
                dest = System.Convert.ChangeType(source, expectType);
                return true;
            }
            else if (source.GetType().IsEnum)
            {
                if (Enum.IsDefined(source.GetType(), source))
                {
                    dest = source;
                    return true;
                }
            }
            else
            {
                //�^�R���o�[�^���擾
                TypeConverter converter = TypeDescriptor.GetConverter(expectType);

                if (converter != null)
                {
                    if (converter.CanConvertFrom(source.GetType()))
                    {
                        dest = converter.ConvertFrom(source);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}