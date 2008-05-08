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

using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
#if !NET_1_1
    [Bean(typeof(GenericNullableEntity))]
    public interface IGenericNullableEntityAutoDao
    {
        [Query("entityNo=/*entityNo*/")]
        GenericNullableEntity GetGenericNullableEntityByEntityNo(int entityNo);

        void Insert(GenericNullableEntity entity);

        void Update(GenericNullableEntity entity);

        [PersistentProps("EntityNo")]
        void UpdateWithPersistentProps(GenericNullableEntity entity);

        [NoPersistentProps("Ddate")]
        void UpdateWithNoPersistentProps(GenericNullableEntity entity);

        void Delete(GenericNullableEntity entity);
    }
#endif
}
