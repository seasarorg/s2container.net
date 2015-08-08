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

using System.Data;

namespace Seasar.Extension.ADO.Impl
{
    public class ObjectDataReaderHandler : IDataReaderHandler
    {
        #region IDataReaderHandler �����o

        public object Handle(IDataReader dataReader)
        {
            // �y�Œ��return null;�Ƃ��Ă��闝�R�z
            // BasicSelectHandler#Execute()�ɂ�����
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            // if (_dataReaderHandler is ObjectDataReaderHandler) {
            //     return CommandFactory.ExecuteScalar(DataSource, cmd);
            // }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            // �Ƃ���������s���Ă��邽�߂ł���B
            // ObjectDataReaderHandler�Ƃ����N���X���̂͒P�Ȃ�}�[�J�[�N���X�ł���A
            // ���̎������̂ɓ��ɈӖ�������Ă��Ȃ��B
            // 
            return null;
        }

        #endregion
    }
}
