#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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

using Seasar.Extension.ADO;

namespace Seasar.Dao
{
    public interface IBeanMetaData : IDtoMetaData
    {
        string TableName { get; }
        IPropertyType VersionNoPropertyType { get; }
        string VersionNoPropertyName { get; }
        string VersionNoBindingName { get; }
        bool HasVersionNoPropertyType { get; }
        IPropertyType TimestampPropertyType { get; }
        string TimestampPropertyName { get; }
        string TimestampBindingName { get; }
        bool HasTimestampPropertyType { get; }
        string ConvertFullColumnName(string alias);
        IPropertyType GetPropertyTypeByAliasName(string aliasName);
        IPropertyType GetPropertyTypeByColumnName(string columnName);
        bool HasPropertyTypeByColumnName(string columnName);
        bool HasPropertyTypeByAliasName(string aliasName);
        int RelationPropertyTypeSize { get; }
        IRelationPropertyType GetRelationPropertyType(int index);
        IRelationPropertyType GetRelationPropertyType(string propertyName);
        int PrimaryKeySize { get; }
        string GetPrimaryKey(int index);
        IIdentifierGenerator IdentifierGenerator { get; }
        string AutoSelectList { get; }
        bool IsRelation { get; }
    }
}
