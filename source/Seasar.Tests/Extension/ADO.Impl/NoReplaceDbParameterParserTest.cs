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

using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Text.RegularExpressions;
using MbUnit.Framework;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [TestFixture]
    public class NoReplaceDbParameterParserTest : S2TestCase
    {
        private readonly IDbParameterParser _parser = new NoReplaceDbParameterParser();

        [Test]
        public void ParseAtMark()
        {
            string sql = "INSERT INTO emp ( empno , ename , job ) VALUES ( @empno , 'M@NAGER' , @job )";
            Assert.AreEqual(
                "INSERT INTO emp ( empno , ename , job ) VALUES ( @empno , 'M@NAGER' , @job )",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp ( empno , ename , job ) VALUES ( @empno , 'M@NAGER' , @job )",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseAtMark2()
        {
            string sql = "INSERT INTO emp(empno,ename,job)VALUES(@empno,'m@nager',@job)";
            Assert.AreEqual(
                "INSERT INTO emp(empno,ename,job)VALUES(@empno,'m@nager',@job)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp(empno,ename,job)VALUES(@empno,'m@nager',@job)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseAtMark3()
        {
            string sql = "INSERT INTO emp (empno, ename, job) VALUES (@$empno, @#manager, @dto.job)";
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (@$empno, @#manager, @dto.job)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (@$empno, @#manager, @dto.job)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseAtMark4()
        {
            string sql = "INSERT INTO emp (empno, ename, job) VALUES (@$è]ã∆àıî‘çÜ, @Ç©ÇÒÇËÇµÇ·, @ÉVÉáÉNÉMÉáÉE)";
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (@$è]ã∆àıî‘çÜ, @Ç©ÇÒÇËÇµÇ·, @ÉVÉáÉNÉMÉáÉE)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (@$è]ã∆àıî‘çÜ, @Ç©ÇÒÇËÇµÇ·, @ÉVÉáÉNÉMÉáÉE)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseAtMark5()
        {
            string sql = "INSERT INTO emp (empno, ename, job) VALUES (@1empno, @m2anager, @job3)";
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (@1empno, @m2anager, @job3)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (@1empno, @m2anager, @job3)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseAtMark6()
        {
            string sql = "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (@EMP_NO, @MANAGER, @_JOB)";
            Assert.AreEqual(
                "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (@EMP_NO, @MANAGER, @_JOB)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (@EMP_NO, @MANAGER, @_JOB)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseAtMark7()
        {
            string sql = "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (@$#, @@USERID, @A.B)";
            Assert.AreEqual(
                "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (@$#, @@USERID, @A.B)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (@$#, @@USERID, @A.B)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseColon()
        {
            string sql = "INSERT INTO emp ( empno , ename , job ) VALUES ( :empno , 'M:NAGER' , :job )";
            Assert.AreEqual(
                "INSERT INTO emp ( empno , ename , job ) VALUES ( :empno , 'M:NAGER' , :job )",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp ( empno , ename , job ) VALUES ( :empno , 'M:NAGER' , :job )",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseColon2()
        {
            string sql = "INSERT INTO emp(empno,ename,job)VALUES(:empno,'m:nager',:job)";
            Assert.AreEqual(
                "INSERT INTO emp(empno,ename,job)VALUES(:empno,'m:nager',:job)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp(empno,ename,job)VALUES(:empno,'m:nager',:job)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseColon3()
        {
            string sql = "INSERT INTO emp (empno, ename, job) VALUES (:$empno, :#manager, :dto.job)";
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (:$empno, :#manager, :dto.job)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (:$empno, :#manager, :dto.job)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseColon4()
        {
            string sql = "INSERT INTO emp (empno, ename, job) VALUES (:$è]ã∆àıî‘çÜ, :Ç©ÇÒÇËÇµÇ·, :ÉVÉáÉNÉMÉáÉE)";
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (:$è]ã∆àıî‘çÜ, :Ç©ÇÒÇËÇµÇ·, :ÉVÉáÉNÉMÉáÉE)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (:$è]ã∆àıî‘çÜ, :Ç©ÇÒÇËÇµÇ·, :ÉVÉáÉNÉMÉáÉE)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseColon5()
        {
            string sql = "INSERT INTO emp (empno, ename, job) VALUES (:1empno, :m2anager, :job3)";
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (:1empno, :m2anager, :job3)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (:1empno, :m2anager, :job3)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseColon6()
        {
            string sql = "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (:EMP_NO, :MANAGER, :_JOB)";
            Assert.AreEqual(
                "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (:EMP_NO, :MANAGER, :_JOB)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (:EMP_NO, :MANAGER, :_JOB)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseColon7()
        {
            string sql = "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (:$#, ::USERID, :A.B)";
            Assert.AreEqual(
                "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (:$#, ::USERID, :A.B)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO EMP (EMPNO, ENAME, JOB) VALUES (:$#, ::USERID, :A.B)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseQuestion()
        {
            string sql = "INSERT INTO emp ( empno , ename , job ) VALUES ( :empno , 'M:NAGER' , :job )";
            Assert.AreEqual(
                "INSERT INTO emp ( empno , ename , job ) VALUES ( :empno , 'M:NAGER' , :job )",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp ( empno , ename , job ) VALUES ( :empno , 'M:NAGER' , :job )",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(0, ret.Count);
        }

        [Test]
        public void ParseQuestion2()
        {
            string sql = "INSERT INTO emp(empno,ename,job)VALUES(?,'m?nager',?)";
            Assert.AreEqual(
                "INSERT INTO emp(empno,ename,job)VALUES(?,'m?nager',?)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp(empno,ename,job)VALUES(?,'m?nager',?)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(2, ret.Count);
            Assert.AreEqual("?", ret[0].Value);
            Assert.AreEqual("?", ret[1].Value);
        }

        [Test]
        public void ParseQuestion3()
        {
            string sql = "INSERT INTO emp (empno, ename, job) VALUES (?, ?, ?)";
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (?, ?, ?)",
                _parser.ChangeSignSql(new SqlCommand(), sql)
                );
            Assert.AreEqual(
                "INSERT INTO emp (empno, ename, job) VALUES (?, ?, ?)",
                _parser.ChangeSignSql(new OracleCommand(), sql)
                );
            MatchCollection ret = _parser.Parse(sql);
            Assert.AreEqual(3, ret.Count);
        }
    }
}
