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
using System.Collections;
using log4net;
using Seasar.Framework.Message;

namespace Seasar.Framework.Log
{
	/// <summary>
	/// Logger ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public sealed class Logger
	{
		private static Hashtable  loggerTable_ = Hashtable.Synchronized(new Hashtable());
		private ILog log_;
        private Type type_;

		private Logger(Type type)
		{
			log_ = LogManager.GetLogger(type);
            type_ = type;
		}

		public static Logger GetLogger(Type type)
		{
			Logger logger = (Logger) loggerTable_[type];
			if(logger == null)
			{
				logger = new Logger(type);
				loggerTable_.Add(type,logger);
			}
			return logger;
		}


		public bool IsDebugEnabled
		{
			get { return log_.IsDebugEnabled; }
		}


		public void Debug(object message,Exception exception)
		{
			if(this.IsDebugEnabled)
			{
				log_.Debug(message,exception);
			}
		}


		public void Debug(object message)
		{
			if(this.IsDebugEnabled)
			{
				log_.Debug(message);
			}
		}

		public bool IsInfoEnabled
		{
			get { return log_.IsInfoEnabled; }
		}


		public void Info(object message,Exception exception)
		{
			if(this.IsInfoEnabled)
			{
				log_.Info(message,exception);
			}
		}


		public void Info(object message)
		{
			if(this.IsInfoEnabled)
			{
				log_.Info(message);
			}
		}


		public void Warn(object message,Exception exception)
		{
			log_.Warn(message,exception);
		}


		public void Warn(object message)
		{
			log_.Warn(message);
		}


		public void Error(object message,Exception exception)
		{
			log_.Error(message,exception);
		}


		public void Error(object message)
		{
			log_.Error(message);
		}


		public void Fatal(object message,Exception exception)
		{
			log_.Fatal(message,exception);
		}
	

		public void Fatal(object message)
		{
			log_.Fatal(message);
		}


		public void Log(Exception exception)
		{
			this.Error(exception.Message,exception);
		}


		public void Log(string messageCode,object[] args)
		{
			this.Log(messageCode,args,null);
		}

		public void Log(string messageCode,object[] args,Exception exception)
		{
			char messageType = messageCode.ToCharArray()[0];
			if(this.IsEnabledFor(messageType))
			{
                
				string message = MessageFormatter.GetSimpleMessage(messageCode,args, type_.Assembly);
				switch(messageType)
				{
					case 'D':
						log_.Debug(message,exception);
						break;
					case 'I':
						log_.Info(message,exception);
						break;
					case 'W':
						log_.Warn(message,exception);
						break;
					case 'E':
						log_.Error(message,exception);
						break;
					case 'F':
						log_.Fatal(message,exception);
						break;
					default:
						throw new ArgumentException(messageType.ToString());
				}
			}
		}


		private bool IsEnabledFor(char messageType)
		{
			switch(messageType)
			{
				case 'D': return log_.IsDebugEnabled;
				case 'I': return log_.IsInfoEnabled;
				case 'W': return log_.IsWarnEnabled;
				case 'E': return log_.IsErrorEnabled;
				case 'F': return log_.IsFatalEnabled;
				default: throw new ArgumentException(new String(messageType,1));
			}
		}
	}
}
