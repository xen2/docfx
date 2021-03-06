﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.MarkdownLite
{
    using System.Collections.Immutable;
    using System.Text.RegularExpressions;

    public class GfmStrongEmInlineRule : IMarkdownRule
    {
        public virtual string Name => "Inline.Gfm.StringEm";

        public virtual Regex StrongEm => Regexes.Inline.Gfm.StrongEm;

        public virtual IMarkdownToken TryMatch(IMarkdownParser parser, ref string source)
        {
            var match = StrongEm.Match(source);
            if (match.Length == 0)
            {
                return null;
            }
            if (match.Groups[1].Length > 0)
            {
                source = source.Substring(match.Groups[1].Length);
                return new MarkdownTextToken(this, parser.Context, match.Groups[1].Value, match.Groups[1].Value);
            }

            source = source.Substring(match.Length);

            return new MarkdownStrongInlineToken(
                this,
                parser.Context,
                GetContent(parser, match),
                match.Value);
        }

        private ImmutableArray<IMarkdownToken> GetContent(IMarkdownParser parser, Match match)
        {
            var emContent = new MarkdownEmInlineToken(
                this,
                parser.Context,
                parser.Tokenize(match.Groups[2].Value),
                "*" + match.Groups[1].Value + "*");

            if (match.Groups[2].Length > 0)
            {
                return parser.Tokenize(match.Groups[3].Value).Insert(0, emContent);
            }
            else
            {
                return ImmutableArray.Create<IMarkdownToken>(emContent);
            }
        }
    }
}
