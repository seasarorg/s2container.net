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
using System.Collections;
using log4net;
using Seasar.Framework.Message;

namespace Seasar.Framework.Log
{
    public sealed class Logger
    {
        private static readonly Hashtable _loggerTable = Hashtable.Synchronized(new Hashtable());
        private readonly ILog _log;
        private readonly Type _type;

        private Logger(Type type)
        {
            _log = LogManager.GetLogger(type);
            _type = type;
        }

        public static Logger GetLogger(Type type)
        {
            Logger logger = (Logger) _loggerTable[type];
            if (logger == null)
            {
                logger = new Logger(type);
                _loggerTable.Add(type, logger);
            }
            return logger;
        }

        public bool IsDebugEnabled
        {
            get { return _log.IsDebugEnabled; }
        }

        public void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
            {
                _log.Debug(message, exception);
            }
        }

        public void Debug(object message)
        {
            if (IsDebugEnabled)
            {
                _log.Debug(message);
            }
        }

        public bool IsInfoEnabled
        {
            get { return _log.IsInfoEnabled; }
        }

        public void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
            {
                _log.Info(message, exception);
            }
        }

        public void Info(object message)
        {
            if (IsInfoEnabled)
            {
                _log.Info(message);
            }
        }

        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        public void Log(Exception exception)
        {
            Error(exception.Message, exception);
        }

        public void Log(string messageCode, object[] args)
        {
            Log(messageCode, args, null);
        }

        public void Log(string messageCode, object[] args, Exception exception)
        {
            char messageType = messageCode.ToCharArray()[0];
            if (IsEnabledFor(messageType))
            {
                string message = MessageFormatter.GetSimpleMessage(messageCode, args, _type.Assembly);
                switch (messageType)
                {
                    case 'D':
                        _log.Debug(message, exception);
                        break;
                    case 'I':
                        _log.Info(message, exception);
                        break;
                    case 'W':
                        _log.Warn(message, exception);
                        break;
                    case 'E':
                        _log.Error(message, exception);
                        break;
                    case 'F':
                        _log.Fatal(message, exception);
                        break;
                    default:
                        throw new ArgumentException(messageType.ToString());
                }
            }
        }

        private bool IsEnabledFor(char messageType)
        {
            switch (messageType)
            {
                case 'D': return _log.IsDebugEnabled;
                case 'I': return _log.IsInfoEnabled;
                case 'W': return _log.IsWarnEnabled;
                case 'E': return _log.IsErrorEnabled;
                case 'F': return _log.IsFatalEnabled;
                default: throw new ArgumentException(new String(messageType, 1));
            }
        }
    }
}
