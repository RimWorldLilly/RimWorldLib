using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x02000216 RID: 534
	public class LanguageWorker_Czech : LanguageWorker
	{
		// Token: 0x06000F8D RID: 3981 RVA: 0x000576D8 File Offset: 0x000558D8
		public override string ResolveFunction(string functionName, List<string> args, string fullStringForReference)
		{
			try
			{
				if (functionName == "lookup" && (args.Count == 3 || args.Count == 2))
				{
					if (DebugSettings.logTranslationLookupErrors)
					{
						LanguageWorker_Czech._log.Clear();
						LanguageWorker_Czech._log.AppendLine("ResolveLookup - (" + string.Join(", ", args) + ")");
					}
					return this.DoLookup(args, fullStringForReference);
				}
			}
			finally
			{
				if (DebugSettings.logTranslationLookupErrors && LanguageWorker_Czech._emitLog)
				{
					Log.Message(LanguageWorker_Czech._log.ToString());
					LanguageWorker_Czech._emitLog = false;
					LanguageWorker_Czech._log.Clear();
				}
			}
			if (functionName == "replace" && args.Count > 1)
			{
				return this.DoReplace(args);
			}
			return base.ResolveFunction(functionName, args, fullStringForReference);
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x000577B0 File Offset: 0x000559B0
		private string DoLookup(List<string> args, string fullStringForReference)
		{
			if (DebugSettings.logTranslationLookupErrors)
			{
				LanguageWorker_Czech._emitLog = true;
			}
			string text = args[0];
			string text2 = args[1];
			if (Path.DirectorySeparatorChar != '\\')
			{
				text2 = text2.Replace('\\', Path.DirectorySeparatorChar);
				args[1] = text2;
			}
			int num;
			int index = (args.Count == 3) ? (int.TryParse(args[2], out num) ? num : -1) : 1;
			LanguageWorker_Czech.LookupCacheKey lookupCacheKey = new LanguageWorker_Czech.LookupCacheKey(text, text2, index);
			string text3;
			if (this.TryLookup(args, lookupCacheKey, out text3))
			{
				return text3;
			}
			int num2;
			if (LanguageWorker_Czech.TryFindIndexOfAny(text, LanguageWorker_Czech._fastLookupChars, 0, out num2, true))
			{
				if (this.TryLookup(args, text, 0, num2, out text3))
				{
					LanguageWorker_Czech.SaveToLookupCache(lookupCacheKey, text3);
					return text3;
				}
			}
			else
			{
				num2 = 0;
			}
			if (this.TryLookupRecursive(args, text, 0, (num2 == 0) ? text.Length : num2, out text3))
			{
				LanguageWorker_Czech.SaveToLookupCache(lookupCacheKey, text3);
				return text3;
			}
			args[0] = text;
			text3 = base.ResolveLookup(args, fullStringForReference);
			LanguageWorker_Czech.SaveToLookupCache(lookupCacheKey, text3);
			return text3;
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x000578A8 File Offset: 0x00055AA8
		private bool TryLookup(List<string> args, LanguageWorker_Czech.LookupCacheKey lookupCacheKey, out string lookupResult)
		{
			if (LanguageWorker_Czech._lookupCache.TryGetValue(lookupCacheKey, out lookupResult))
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					StringBuilder log = LanguageWorker_Czech._log;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 3);
					defaultInterpolatedStringHandler.AppendLiteral(" - Cache hit for ");
					defaultInterpolatedStringHandler.AppendFormatted(lookupCacheKey.Path);
					defaultInterpolatedStringHandler.AppendLiteral(" at index ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(lookupCacheKey.Index);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(lookupResult);
					log.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				LanguageWorker_Czech._emitLog = false;
				return true;
			}
			if (this.TryLookup(args, lookupCacheKey.OriginalInput, 0, lookupCacheKey.OriginalInput.Length, out lookupResult))
			{
				LanguageWorker_Czech.SaveToLookupCache(lookupCacheKey, lookupResult);
				return true;
			}
			return false;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00057960 File Offset: 0x00055B60
		private bool TryLookupRecursive(List<string> args, string originalInput, int fromIndex, int count, out string output)
		{
			if (fromIndex + count >= originalInput.Length)
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					LanguageWorker_Czech._emitLog = true;
					StringBuilder log = LanguageWorker_Czech._log;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 3);
					defaultInterpolatedStringHandler.AppendLiteral("TryLookupRecursive - Invalid range ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(fromIndex + count);
					defaultInterpolatedStringHandler.AppendLiteral(" >= ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(originalInput.Length);
					defaultInterpolatedStringHandler.AppendLiteral(" (length of '");
					defaultInterpolatedStringHandler.AppendFormatted(originalInput);
					defaultInterpolatedStringHandler.AppendLiteral("'), returning false.");
					log.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				output = null;
				return false;
			}
			int value = 0;
			while (value++ <= 50)
			{
				int num = fromIndex + count - 1;
				if (!LanguageWorker_Czech.TryFindWhiteSpaceBackward(originalInput, ref num, fromIndex, true))
				{
					output = null;
					return false;
				}
				LanguageWorker_Czech.TryFindWhiteSpaceBackward(originalInput, ref num, fromIndex, false);
				count = num - fromIndex;
				if (this.TryLookup(args, originalInput, fromIndex, count, out output))
				{
					return true;
				}
			}
			if (DebugSettings.logTranslationLookupErrors)
			{
				LanguageWorker_Czech._emitLog = true;
				StringBuilder log2 = LanguageWorker_Czech._log;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(61, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("TryLookupRecursive - Too many iterations (");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(value);
				defaultInterpolatedStringHandler2.AppendLiteral("), returning false.");
				log2.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			output = null;
			return false;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00057A90 File Offset: 0x00055C90
		private bool TryLookup(List<string> args, string originalInput, int fromIndex, int count, out string output)
		{
			if (DebugSettings.logTranslationLookupErrors)
			{
				StringBuilder log = LanguageWorker_Czech._log;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
				defaultInterpolatedStringHandler.AppendLiteral("TryLookup: from:");
				defaultInterpolatedStringHandler.AppendFormatted<int>(fromIndex);
				defaultInterpolatedStringHandler.AppendLiteral(", count:");
				defaultInterpolatedStringHandler.AppendFormatted<int>(count);
				log.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			output = "";
			if (count <= 0)
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					LanguageWorker_Czech._log.AppendLine(" - Invalid count, returning false.");
				}
				return false;
			}
			if (fromIndex != 0 || count != originalInput.Length)
			{
				args[0] = originalInput.Substring(fromIndex, count);
			}
			string text = base.ResolveLookup(args, originalInput);
			if (text == "" || text == args[0])
			{
				if (DebugSettings.logTranslationLookupErrors)
				{
					LanguageWorker_Czech._log.AppendLine(" - FAILED - args[0]: '" + args[0] + "'");
				}
				return false;
			}
			bool flag = fromIndex > 0;
			int num = fromIndex + count;
			bool flag2 = num < originalInput.Length;
			output = (flag ? (flag2 ? (originalInput.Substring(0, fromIndex) + text + originalInput.Substring(num)) : (originalInput.Substring(0, fromIndex) + text)) : (flag2 ? (text + originalInput.Substring(num)) : text));
			if (DebugSettings.logTranslationLookupErrors)
			{
				StringBuilder log2 = LanguageWorker_Czech._log;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(86, 6);
				defaultInterpolatedStringHandler2.AppendLiteral(" - args[0]: '");
				defaultInterpolatedStringHandler2.AppendFormatted(args[0]);
				defaultInterpolatedStringHandler2.AppendLiteral("' - return: ");
				defaultInterpolatedStringHandler2.AppendFormatted(output);
				defaultInterpolatedStringHandler2.AppendLiteral(" (lookupResult: ");
				defaultInterpolatedStringHandler2.AppendFormatted(text);
				defaultInterpolatedStringHandler2.AppendLiteral(", hasPrefix: ");
				defaultInterpolatedStringHandler2.AppendFormatted<bool>(flag);
				defaultInterpolatedStringHandler2.AppendLiteral(", hasSuffix: ");
				defaultInterpolatedStringHandler2.AppendFormatted<bool>(flag2);
				defaultInterpolatedStringHandler2.AppendLiteral(", endLookupIndex: ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler2.AppendLiteral(")");
				log2.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			return output != "";
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00057C91 File Offset: 0x00055E91
		private static void SaveToLookupCache(LanguageWorker_Czech.LookupCacheKey lookupCacheKey, string lookupResult)
		{
			LanguageWorker_Czech.SaveToCache<LanguageWorker_Czech.LookupCacheKey, string>(lookupCacheKey, lookupResult, LanguageWorker_Czech._lookupCache, LanguageWorker_Czech._lookupKeys, ref LanguageWorker_Czech._lookupKeyDeleteIndex, 107);
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00057CAC File Offset: 0x00055EAC
		private string DoReplace(List<string> args)
		{
			if (args.Count == 0)
			{
				return null;
			}
			string text = args[0];
			if (args.Count == 1)
			{
				return text;
			}
			for (int i = 1; i < args.Count; i++)
			{
				string input = args[i];
				Match match = LanguageWorker_Czech._replacePatternArgRegex.Match(input);
				if (!match.Success)
				{
					return null;
				}
				string value = match.Groups["old"].Value;
				string value2 = match.Groups["new"].Value;
				if (text.Contains(value))
				{
					return text.Replace(value, value2);
				}
				Regex replaceRegex = this.GetReplaceRegex(value);
				if (replaceRegex != null)
				{
					string text2 = replaceRegex.Replace(text, value2);
					if (!(text2 == text))
					{
						return text2;
					}
				}
			}
			return text;
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00057D78 File Offset: 0x00055F78
		private Regex GetReplaceRegex(string regexPattern)
		{
			Regex regex;
			if (LanguageWorker_Czech._replaceRegexCache.TryGetValue(regexPattern, out regex))
			{
				return regex;
			}
			int num;
			if (!LanguageWorker_Czech.TryFindIndexOfAny(regexPattern, LanguageWorker_Czech._firstPossibleRegexChars, 0, out num, false))
			{
				LanguageWorker_Czech.SaveToReplaceCache(regexPattern, null);
				return null;
			}
			Regex result;
			try
			{
				regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
				LanguageWorker_Czech.SaveToReplaceCache(regexPattern, regex);
				result = regex;
			}
			catch
			{
				LanguageWorker_Czech.SaveToReplaceCache(regexPattern, null);
				result = null;
			}
			return result;
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00057DE4 File Offset: 0x00055FE4
		private static void SaveToReplaceCache(string replacePattern, Regex regex)
		{
			LanguageWorker_Czech.SaveToCache<string, Regex>(replacePattern, regex, LanguageWorker_Czech._replaceRegexCache, LanguageWorker_Czech._replaceRegexKeys, ref LanguageWorker_Czech._replaceRegexKeyDeleteIndex, 107);
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00057DFE File Offset: 0x00055FFE
		private static bool TryFindIndexOfAny(string input, char[] chars, int fromIndex, out int index, bool trim = false)
		{
			index = input.IndexOfAny(chars, fromIndex);
			if (index == -1)
			{
				return false;
			}
			if (trim)
			{
				LanguageWorker_Czech.TryFindWhiteSpaceBackward(input, ref index, fromIndex, false);
			}
			return true;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x00057E20 File Offset: 0x00056020
		private static bool TryFindWhiteSpaceBackward(string input, ref int index, int stopIndex, bool lookForWhitespace)
		{
			while (index > 0)
			{
				if (index <= stopIndex)
				{
					return false;
				}
				if (char.IsWhiteSpace(input[index - 1]) == lookForWhitespace)
				{
					return true;
				}
				index--;
			}
			return false;
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00057E4C File Offset: 0x0005604C
		private static void SaveToCache<TKey, TValue>(TKey key, TValue value, Dictionary<TKey, TValue> cache, TKey[] keys, ref int deleteIndex, int cacheSize)
		{
			if (cache.Count == cacheSize)
			{
				TKey key2 = keys[deleteIndex];
				cache.Remove(key2);
				keys[deleteIndex] = key;
				deleteIndex = (deleteIndex + 1) % cacheSize;
			}
			else
			{
				keys[cache.Count] = key;
			}
			cache[key] = value;
		}

		// Token: 0x04000ECF RID: 3791
		private const int ReplaceRegexCacheSize = 107;

		// Token: 0x04000ED0 RID: 3792
		private const int LookupCacheSize = 107;

		// Token: 0x04000ED1 RID: 3793
		private static readonly StringBuilder _log = new StringBuilder();

		// Token: 0x04000ED2 RID: 3794
		private static bool _emitLog;

		// Token: 0x04000ED3 RID: 3795
		private static readonly Dictionary<LanguageWorker_Czech.LookupCacheKey, string> _lookupCache = new Dictionary<LanguageWorker_Czech.LookupCacheKey, string>(107);

		// Token: 0x04000ED4 RID: 3796
		private static readonly LanguageWorker_Czech.LookupCacheKey[] _lookupKeys = new LanguageWorker_Czech.LookupCacheKey[107];

		// Token: 0x04000ED5 RID: 3797
		private static int _lookupKeyDeleteIndex;

		// Token: 0x04000ED6 RID: 3798
		private static readonly char[] _fastLookupChars = new char[]
		{
			'(',
			'['
		};

		// Token: 0x04000ED7 RID: 3799
		private static readonly Dictionary<string, Regex> _replaceRegexCache = new Dictionary<string, Regex>(107);

		// Token: 0x04000ED8 RID: 3800
		private static readonly string[] _replaceRegexKeys = new string[107];

		// Token: 0x04000ED9 RID: 3801
		private static int _replaceRegexKeyDeleteIndex;

		// Token: 0x04000EDA RID: 3802
		private static readonly char[] _firstPossibleRegexChars = new char[]
		{
			'(',
			'[',
			'{',
			'*',
			'+',
			'?',
			'.',
			'^',
			'$',
			'|'
		};

		// Token: 0x04000EDB RID: 3803
		private static readonly Regex _replacePatternArgRegex = new Regex("(?<old>[^\"]*?)\"-\"(?<new>[^\"]*?)\"", RegexOptions.Compiled);

		// Token: 0x0200259F RID: 9631
		private readonly struct LookupCacheKey : IEquatable<LanguageWorker_Czech.LookupCacheKey>
		{
			// Token: 0x17002A6B RID: 10859
			// (get) Token: 0x0600F039 RID: 61497 RVA: 0x00587AAF File Offset: 0x00585CAF
			public string OriginalInput { get; }

			// Token: 0x17002A6C RID: 10860
			// (get) Token: 0x0600F03A RID: 61498 RVA: 0x00587AB7 File Offset: 0x00585CB7
			public string Path { get; }

			// Token: 0x17002A6D RID: 10861
			// (get) Token: 0x0600F03B RID: 61499 RVA: 0x00587ABF File Offset: 0x00585CBF
			public int Index { get; }

			// Token: 0x0600F03C RID: 61500 RVA: 0x00587AC7 File Offset: 0x00585CC7
			public LookupCacheKey(string originalInput, string path, int index)
			{
				this.OriginalInput = originalInput;
				this.Path = path;
				this.Index = index;
			}

			// Token: 0x0600F03D RID: 61501 RVA: 0x00587ADE File Offset: 0x00585CDE
			private bool Equals(LanguageWorker_Czech.LookupCacheKey other)
			{
				return this.Index == other.Index && string.Equals(this.Path, other.Path, StringComparison.Ordinal) && string.Equals(this.OriginalInput, other.OriginalInput, StringComparison.Ordinal);
			}

			// Token: 0x0600F03E RID: 61502 RVA: 0x00587B1C File Offset: 0x00585D1C
			public override bool Equals(object obj)
			{
				if (obj is LanguageWorker_Czech.LookupCacheKey)
				{
					LanguageWorker_Czech.LookupCacheKey other = (LanguageWorker_Czech.LookupCacheKey)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x0600F03F RID: 61503 RVA: 0x00587B41 File Offset: 0x00585D41
			public override int GetHashCode()
			{
				int num = this.Index * 397;
				string path = this.Path;
				int num2 = (num ^ ((path != null) ? path.GetHashCode() : 0)) * 397;
				string originalInput = this.OriginalInput;
				return num2 ^ ((originalInput != null) ? originalInput.GetHashCode() : 0);
			}

			// Token: 0x0600F040 RID: 61504 RVA: 0x00587B7B File Offset: 0x00585D7B
			bool IEquatable<LanguageWorker_Czech.LookupCacheKey>.Equals(LanguageWorker_Czech.LookupCacheKey other)
			{
				return this.Equals(other);
			}
		}
	}
}
