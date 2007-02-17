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
using System.Reflection;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Util
{
	public sealed class ClassUtil
	{
		private ClassUtil()
		{
		}

		public static ConstructorInfo GetConstructorInfo(Type type,Type[] argTypes)
		{
			Type[] types;
			if(argTypes == null)
			{
				types = Type.EmptyTypes;
			}
			else
			{
				types = argTypes;
			}
			ConstructorInfo constructor = type.GetConstructor(types);
			if(constructor == null)
				throw new NoSuchConstructorRuntimeException(type,argTypes);
			return constructor;
		}

		public static Type ForName(string className,Assembly[] assemblys)
		{
			Type type = Type.GetType(className);
			if(type != null) return type;
			foreach(Assembly assembly in assemblys)
			{
				type = assembly.GetType(className);
				if(type != null) return type;
			}
			return null;
		}

		public static object NewInstance(Type type)
		{
			return Activator.CreateInstance(type);
		}

		public static object NewInstance(string className,string assemblyName)
		{
			Assembly[] asms = new Assembly[1] {Assembly.LoadFrom(assemblyName) };
            return NewInstance(ForName(className,asms));
		}
	}
}
