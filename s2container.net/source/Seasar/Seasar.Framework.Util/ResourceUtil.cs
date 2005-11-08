#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using System.IO;
using System.Reflection;
using System.Resources;

namespace Seasar.Framework.Util
{
	/// <summary>
	/// ResourceUtil �̊T�v�̐����ł��B
	/// </summary>
	public sealed class ResourceUtil
	{
		private ResourceUtil()
		{
		}

		public static string GetExtension(string path)
		{
			int extPos = path.LastIndexOf(".");
			if(extPos >= 0)
			{
				return path.Substring(extPos + 1);
			}
			else
			{
				return null;
			}
		}

		public static string GetResourcePath(string path, string extension)
		{
			if(extension == null) return path;
			if(path.EndsWith(extension)) return path;
			extension = "." + extension;
			return path.Replace(Path.DirectorySeparatorChar,'.') + extension;
		}

		public static StreamReader GetResourceAsStreamReader(string path)
		{
			return GetResourceAsStreamReader(path,Assembly.GetEntryAssembly());
		}

		public static StreamReader GetResourceAsStreamReader(string path, string extension)
		{
			return GetResourceAsStreamReader(GetResourcePath(path, extension), Assembly.GetEntryAssembly());
		}

		public static StreamReader GetResourceAsStreamReader(string path, string extension, Assembly assembly)
		{
			return GetResourceAsStreamReader(GetResourcePath(path, extension), assembly);
		}

		public static StreamReader GetResourceAsStreamReader(string path, Assembly assembly)
		{
			return new StreamReader(GetResourceAsStream(path, assembly));
		}

		public static Stream GetResourceAsStream(string path, Assembly assembly)
		{
			if(assembly == null) throw new ResourceNotFoundRuntimeException(path);
			
			Stream stream = GetResourceNoException(path, assembly);
			if(stream != null)
			{
				return stream;
			}
			else
			{
				throw new ResourceNotFoundRuntimeException(path);
			}
		}

		public static Stream GetResourceNoException(string path, Assembly asm)
		{
			if(asm == null) return null;
			Stream stream = asm.GetManifestResourceStream(path);
			return stream;
		}

		public static bool IsExist(string path, Assembly asm)
		{
			return GetResourceNoException(path, asm) != null;
		}
	}
}