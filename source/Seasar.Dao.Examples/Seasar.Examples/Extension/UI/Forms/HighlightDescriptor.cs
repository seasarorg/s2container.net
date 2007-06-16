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
using System.Drawing;

namespace Seasar.Extension.UI.Forms
{
    public class HighlightDescriptor
    {
        public HighlightDescriptor(string token, Color color, Font font, DescriptorType descriptorType, DescriptorRecognition dr)
        {
            if (descriptorType == DescriptorType.ToCloseToken)
            {
                throw new ArgumentException("You may not choose ToCloseToken DescriptorType without specifing an end token.");
            }
            Color = color;
            Font = font;
            Token = token;
            DescriptorType = descriptorType;
            DescriptorRecognition = dr;
            CloseToken = null;
        }
        public HighlightDescriptor(string token, string closeToken, Color color, Font font, DescriptorType descriptorType, DescriptorRecognition dr)
        {
            Color = color;
            Font = font;
            Token = token;
            DescriptorType = descriptorType;
            CloseToken = closeToken;
            DescriptorRecognition = dr;
        }
        public readonly Color Color;
        public readonly Font Font;
        public readonly string Token;
        public readonly string CloseToken;
        public readonly DescriptorType DescriptorType;
        public readonly DescriptorRecognition DescriptorRecognition; 
    }

    
    public enum DescriptorType
    {
        /// <summary>
        /// Causes the highlighting of a single word
        /// </summary>
        Word,
        /// <summary>
        /// Causes the entire line from this point on the be highlighted, regardless of other tokens
        /// </summary>
        ToEOL,
        /// <summary>
        /// Highlights all text until the end token;
        /// </summary>
        ToCloseToken
    }

    public enum DescriptorRecognition
    {
        /// <summary>
        /// Only if the whole token is equal to the word
        /// </summary>
        WholeWord,
        /// <summary>
        /// If the word starts with the token
        /// </summary>
        StartsWith,
        /// <summary>
        /// If the word contains the Token
        /// </summary>
        Contains
    }
}
