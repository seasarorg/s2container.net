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

using System;
using System.Collections.Generic;
using Seasar.S2FormExample.Logics.Dao;
using Seasar.S2FormExample.Logics.Dto;
using Seasar.S2FormExample.Logics.Page;

namespace Seasar.S2FormExample.Logics.Service.Impl
{
    /// <summary>
    /// 社員リストサービス用実装クラス
    /// </summary>
    public class EmployeeListServiceImpl : BaseServiceImpl, IEmployeeListService
    {
        /// <summary>
        /// CSV用社員DAO
        /// </summary>
        protected IEmployeeCSVDao daoOfCsv;

        /// <summary>
        /// 社員DAO
        /// </summary>
        protected IEmployeeDao daoOfEmployee;

        /// <summary>
        /// 出力用DAO
        /// </summary>
        protected IOutputCSVDao daoOfOutput;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmployeeListServiceImpl()
        {
            ;
        }

        #region IEmployeeListService Members

        /// <summary>
        /// 社員一覧を取得する
        /// </summary>
        /// <returns>社員一覧</returns>
        public EmployeeListPage GetAll()
        {
            EmployeeListPage page = new EmployeeListPage();

            page.GenderId = "99";
            page.GenderName = "全員";
            page.List = daoOfEmployee.GetAll();

            return page;
        }


        /// <summary>
        /// 社員一覧を検索する
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>社員一覧</returns>
        public EmployeeListPage Find(EmployeeListPage condition)
        {
            EmployeeListPage page = new EmployeeListPage();
            page.GenderId = condition.GenderId;
            IList<GenderDto> genderList = this.GetGenderAll();
            foreach (GenderDto dto in genderList)
            {
                if (dto.Id == Convert.ToInt32(condition.GenderId))
                    page.GenderName = dto.Name;
            }
            if (condition.GenderId == "99")
                page.GenderName = "全員";

            IList<EmployeeDto> list;
            if (page.GenderId != "99")
                list = daoOfEmployee.FindByGender(Convert.ToInt32(condition.GenderId));
            else
                list = daoOfEmployee.GetAll();

            if (list != null)
                page.List = list;

            return page;
        }

        /// <summary>
        /// CSVで出力する
        /// </summary>
        /// <param name="path">出力先パス</param>
        /// <returns>出力件数</returns>
        public int OutputCSV(string path)
        {
            if (path == "")
                throw new ArgumentNullException("path");

            IList<EmployeeCsvDto> list = daoOfCsv.GetAll();

            if (list.Count == 0)
                return 0;

            return (daoOfOutput.OutputEmployeeList(path, list));
        }

        #endregion
    }
}