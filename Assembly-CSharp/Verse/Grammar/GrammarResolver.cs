using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse.Grammar
{
	// Token: 0x020007AB RID: 1963
	public static class GrammarResolver
	{
		// Token: 0x06003F47 RID: 16199 RVA: 0x00194C84 File Offset: 0x00192E84
		public static string Resolve(string rootKeyword, GrammarRequest request, string debugLabel = null, bool forceLog = false, string untranslatedRootKeyword = null, List<string> extraTags = null, List<string> outTags = null, bool capitalizeFirstSentence = true)
		{
			if (LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage)
			{
				return GrammarResolver.ResolveUnsafe(rootKeyword, request, debugLabel, forceLog, false, extraTags, outTags, capitalizeFirstSentence);
			}
			bool flag;
			string text;
			Exception ex;
			try
			{
				text = GrammarResolver.ResolveUnsafe(rootKeyword, request, out flag, debugLabel, forceLog, false, extraTags, outTags, capitalizeFirstSentence);
				ex = null;
			}
			catch (Exception ex2)
			{
				flag = false;
				text = "";
				ex = ex2;
			}
			if (flag)
			{
				return text;
			}
			string text2 = "Failed to resolve text. Trying again with English.";
			if (ex != null)
			{
				string str = text2;
				string str2 = " Exception: ";
				Exception ex3 = ex;
				text2 = str + str2 + ((ex3 != null) ? ex3.ToString() : null);
			}
			Log.ErrorOnce(text2, text.GetHashCode());
			if (outTags != null)
			{
				outTags.Clear();
			}
			return GrammarResolver.ResolveUnsafe(untranslatedRootKeyword ?? rootKeyword, request, out flag, debugLabel, forceLog, true, extraTags, outTags, capitalizeFirstSentence);
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x00194D38 File Offset: 0x00192F38
		public static string ResolveUnsafe(string rootKeyword, GrammarRequest request, string debugLabel = null, bool forceLog = false, bool useUntranslatedRules = false, List<string> extraTags = null, List<string> outTags = null, bool capitalizeFirstSentence = true)
		{
			bool flag;
			return GrammarResolver.ResolveUnsafe(rootKeyword, request, out flag, debugLabel, forceLog, useUntranslatedRules, extraTags, outTags, capitalizeFirstSentence);
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x00194D58 File Offset: 0x00192F58
		public static string ResolveUnsafe(string rootKeyword, GrammarRequest request, out bool success, string debugLabel = null, bool forceLog = false, bool useUntranslatedRules = false, List<string> extraTags = null, List<string> outTags = null, bool capitalizeFirstSentence = true)
		{
			bool flag = forceLog || DebugViewSettings.logGrammarResolution;
			GrammarResolver.rules.Clear();
			GrammarResolver.rulePool.Clear();
			if (flag)
			{
				GrammarResolver.logSbTrace = new StringBuilder();
				GrammarResolver.logSbMid = new StringBuilder();
				GrammarResolver.logSbRules = new StringBuilder();
			}
			List<Rule> rulesAllowNull = request.RulesAllowNull;
			if (rulesAllowNull != null)
			{
				if (flag)
				{
					GrammarResolver.logSbRules.AppendLine("CUSTOM RULES");
				}
				for (int i = 0; i < rulesAllowNull.Count; i++)
				{
					GrammarResolver.AddRule(rulesAllowNull[i]);
					if (flag)
					{
						GrammarResolver.logSbRules.AppendLine("■" + rulesAllowNull[i].ToString());
					}
				}
				if (flag)
				{
					GrammarResolver.logSbRules.AppendLine();
				}
			}
			List<RulePackDef> includesAllowNull = request.IncludesAllowNull;
			if (includesAllowNull != null)
			{
				HashSet<RulePackDef> hashSet = new HashSet<RulePackDef>();
				List<RulePackDef> list = new List<RulePackDef>(includesAllowNull);
				if (flag)
				{
					GrammarResolver.logSbMid.AppendLine("INCLUDES");
				}
				while (list.Count > 0)
				{
					RulePackDef rulePackDef = list[list.Count - 1];
					list.RemoveLast<RulePackDef>();
					if (!hashSet.Contains(rulePackDef))
					{
						if (flag)
						{
							GrammarResolver.logSbMid.AppendLine(string.Format("{0}", rulePackDef.defName));
						}
						hashSet.Add(rulePackDef);
						List<Rule> list2 = useUntranslatedRules ? rulePackDef.UntranslatedRulesImmediate : rulePackDef.RulesImmediate;
						if (list2 != null)
						{
							foreach (Rule rule in list2)
							{
								GrammarResolver.AddRule(rule);
							}
						}
						if (!rulePackDef.include.NullOrEmpty<RulePackDef>())
						{
							list.AddRange(rulePackDef.include);
						}
					}
				}
			}
			List<RulePack> includesBareAllowNull = request.IncludesBareAllowNull;
			if (includesBareAllowNull != null)
			{
				if (flag)
				{
					GrammarResolver.logSbMid.AppendLine();
					GrammarResolver.logSbMid.AppendLine("BARE INCLUDES");
				}
				for (int j = 0; j < includesBareAllowNull.Count; j++)
				{
					List<Rule> list3 = useUntranslatedRules ? includesBareAllowNull[j].UntranslatedRules : includesBareAllowNull[j].Rules;
					for (int k = 0; k < list3.Count; k++)
					{
						GrammarResolver.AddRule(list3[k]);
						if (flag)
						{
							GrammarResolver.logSbMid.AppendLine("  " + list3[k].ToString());
						}
					}
				}
			}
			if (flag && !extraTags.NullOrEmpty<string>())
			{
				GrammarResolver.logSbMid.AppendLine();
				GrammarResolver.logSbMid.AppendLine("EXTRA TAGS");
				for (int l = 0; l < extraTags.Count; l++)
				{
					GrammarResolver.logSbMid.AppendLine("  " + extraTags[l]);
				}
			}
			List<Rule> list4 = useUntranslatedRules ? RulePackDefOf.GlobalUtility.UntranslatedRulesPlusIncludes : RulePackDefOf.GlobalUtility.RulesPlusIncludes;
			for (int m = 0; m < list4.Count; m++)
			{
				GrammarResolver.AddRule(list4[m]);
			}
			GrammarResolver.loopCount = 0;
			Dictionary<string, string> constantsAllowNull = request.ConstantsAllowNull;
			if (flag && constantsAllowNull != null)
			{
				GrammarResolver.logSbMid.AppendLine("CONSTANTS");
				foreach (KeyValuePair<string, string> keyValuePair in constantsAllowNull)
				{
					GrammarResolver.logSbMid.AppendLine(keyValuePair.Key.PadRight(38) + " " + keyValuePair.Value);
				}
			}
			if (flag)
			{
				GrammarResolver.logSbTrace.Append("GRAMMAR RESOLUTION TRACE");
			}
			string text = "err";
			bool flag2 = false;
			List<string> list5 = new List<string>();
			if (GrammarResolver.TryResolveRecursive(new GrammarResolver.RuleEntry(new Rule_String("", "[" + rootKeyword + "]")), 0, constantsAllowNull, out text, flag, extraTags, list5, request.customizer))
			{
				if (outTags != null)
				{
					outTags.Clear();
					outTags.AddRange(list5);
				}
			}
			else
			{
				flag2 = true;
				if (request.Rules.NullOrEmpty<Rule>())
				{
					text = "ERR";
				}
				else
				{
					text = "ERR: " + request.Rules[0].Generate();
				}
				if (flag)
				{
					GrammarResolver.logSbTrace.Insert(0, "Grammar unresolvable. Root '" + rootKeyword + "'\n\n");
				}
				else
				{
					GrammarResolver.ResolveUnsafe(rootKeyword, request, debugLabel, true, useUntranslatedRules, extraTags, null, true);
				}
			}
			text = GrammarResolver.ResolveAllFunctions(text, constantsAllowNull);
			text = GenText.CapitalizeSentences(Find.ActiveLanguageWorker.PostProcessed(text), capitalizeFirstSentence);
			text = GrammarResolver.Spaces.Replace(text, (Match match) => match.Groups[1].Value);
			text = text.Trim();
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(GrammarResolver.logSbTrace.ToString().TrimEndNewlines());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append(GrammarResolver.logSbMid.ToString().TrimEndNewlines());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append(GrammarResolver.logSbRules.ToString().TrimEndNewlines());
				if (flag2)
				{
					if (DebugViewSettings.logGrammarResolution)
					{
						Log.Error(stringBuilder.ToString().Trim() + "\n");
					}
					else
					{
						Log.ErrorOnce(stringBuilder.ToString().Trim() + "\n", stringBuilder.ToString().Trim().GetHashCode());
					}
				}
				else
				{
					Log.Message(stringBuilder.ToString().Trim() + "\n");
				}
				GrammarResolver.logSbTrace = null;
				GrammarResolver.logSbMid = null;
				GrammarResolver.logSbRules = null;
			}
			success = !flag2;
			return text;
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x0019531C File Offset: 0x0019351C
		public static string ResolveAllFunctions(string input, Dictionary<string, string> constants)
		{
			if (input.Contains("%%"))
			{
				return input;
			}
			string text = input;
			MatchCollection matchCollection = GrammarResolver.FunctionCall.Matches(text);
			for (int i = matchCollection.Count - 1; i >= 0; i--)
			{
				Match match = matchCollection[i];
				string value = match.Groups[1].Value;
				string args = match.Groups[2].Value.Trim();
				if (value == "lookup" || value == "replace")
				{
					string value2 = GrammarResolverSimple.ResolveFunction(value, args, match.Value);
					text = text.Remove(match.Index, match.Length);
					text = text.Insert(match.Index, value2);
				}
				else
				{
					Log.Warning("Unknown grammar function name: " + value + ". Supported functions: lookup, replace.");
				}
			}
			MatchCollection matchCollection2 = GrammarResolver.GenderCall.Matches(text);
			for (int j = matchCollection2.Count - 1; j >= 0; j--)
			{
				Match match2 = matchCollection2[j];
				string value3 = match2.Groups[1].Value;
				string args2 = match2.Groups[2].Value.Trim();
				string text2;
				if (constants.TryGetValue(value3 + "_gender", out text2))
				{
					Gender gender;
					if (Enum.TryParse<Gender>(text2, out gender))
					{
						string value4 = GrammarResolverSimple.ResolveGenderSymbol(gender, false, args2, match2.Value);
						text = text.Remove(match2.Index, match2.Length);
						text = text.Insert(match2.Index, value4);
					}
					else
					{
						Log.Warning("Unknown gender: " + text2 + ".");
					}
				}
				else
				{
					Log.Warning("Cannot find rules for pawn symbol " + value3 + ".");
				}
			}
			return text;
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x001954EC File Offset: 0x001936EC
		private static void AddRule(Rule rule)
		{
			List<GrammarResolver.RuleEntry> list = null;
			if (!GrammarResolver.rules.TryGetValue(rule.keyword, out list))
			{
				list = GrammarResolver.rulePool.Get();
				list.Clear();
				GrammarResolver.rules[rule.keyword] = list;
			}
			list.Add(new GrammarResolver.RuleEntry(rule));
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x00195540 File Offset: 0x00193740
		private static bool TryResolveRecursive(GrammarResolver.RuleEntry entry, int depth, Dictionary<string, string> constants, out string output, bool log, List<string> extraTags, List<string> resolvedTags, GrammarRequest.ICustomizer customizer)
		{
			string text = "";
			for (int i = 0; i < depth; i++)
			{
				text += "  ";
			}
			if (log && depth > 0)
			{
				GrammarResolver.logSbTrace.AppendLine();
				GrammarResolver.logSbTrace.Append(depth.ToStringCached().PadRight(3));
				GrammarResolver.logSbTrace.Append(text + ((entry != null) ? entry.ToString() : null));
			}
			text += "     ";
			GrammarResolver.loopCount++;
			if (GrammarResolver.loopCount > 1000)
			{
				Log.Error("Hit loops limit resolving grammar.");
				output = "HIT_LOOPS_LIMIT";
				if (log)
				{
					GrammarResolver.logSbTrace.Append("\n" + text + "UNRESOLVABLE: Hit loops limit");
				}
				return false;
			}
			if (depth > 50)
			{
				Log.Error("Grammar recurred too deep while resolving keyword (>" + 50.ToString() + " deep)");
				output = "DEPTH_LIMIT_REACHED";
				if (log)
				{
					GrammarResolver.logSbTrace.Append("\n" + text + "UNRESOLVABLE: Depth limit reached");
				}
				return false;
			}
			string text2 = entry.rule.Generate();
			bool flag = false;
			int num = -1;
			for (int j = 0; j < text2.Length; j++)
			{
				char c = text2[j];
				if (c == '[')
				{
					num = j;
				}
				if (c == ']')
				{
					if (num == -1)
					{
						Log.Error("Could not resolve rule because of mismatched brackets: " + text2);
						output = "MISMATCHED_BRACKETS";
						if (log)
						{
							GrammarResolver.logSbTrace.Append("\n" + text + "UNRESOLVABLE: Mismatched brackets");
						}
						flag = true;
					}
					else
					{
						string text3 = text2.Substring(num + 1, j - num - 1);
						GrammarResolver.RuleEntry ruleEntry;
						List<string> list;
						string str;
						for (;;)
						{
							ruleEntry = GrammarResolver.RandomPossiblyResolvableEntry(text3, constants, extraTags, resolvedTags, customizer);
							if (ruleEntry == null)
							{
								break;
							}
							ruleEntry.uses++;
							list = resolvedTags.ToList<string>();
							if (GrammarResolver.TryResolveRecursive(ruleEntry, depth + 1, constants, out str, log, extraTags, list, customizer))
							{
								goto Block_15;
							}
							ruleEntry.MarkKnownUnresolvable();
						}
						entry.MarkKnownUnresolvable();
						output = "CANNOT_RESOLVE_SUBSYMBOL:" + text3;
						if (log)
						{
							GrammarResolver.logSbTrace.Append("\n" + text + text3 + " → UNRESOLVABLE");
						}
						flag = true;
						goto IL_294;
						Block_15:
						text2 = text2.Substring(0, num) + str + text2.Substring(j + 1);
						j = num;
						resolvedTags.Clear();
						resolvedTags.AddRange(list);
						if (!ruleEntry.rule.tag.NullOrEmpty() && !resolvedTags.Contains(ruleEntry.rule.tag))
						{
							resolvedTags.Add(ruleEntry.rule.tag);
						}
						if (customizer != null)
						{
							customizer.Notify_RuleUsed(ruleEntry.rule);
						}
					}
				}
				IL_294:;
			}
			output = text2;
			return !flag;
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x001957FC File Offset: 0x001939FC
		private static GrammarResolver.RuleEntry RandomPossiblyResolvableEntry(string keyword, Dictionary<string, string> constants, List<string> extraTags, List<string> resolvedTags, GrammarRequest.ICustomizer customizer)
		{
			List<GrammarResolver.RuleEntry> list = GrammarResolver.rules.TryGetValue(keyword, null);
			if (list == null)
			{
				return null;
			}
			float maxPriority = float.MinValue;
			int maxBucket = 0;
			for (int i = 0; i < list.Count; i++)
			{
				GrammarResolver.RuleEntry ruleEntry = list[i];
				if (GrammarResolver.ValidateRule(constants, extraTags, resolvedTags, ruleEntry, customizer) && ruleEntry.SelectionWeight != 0f)
				{
					maxPriority = Mathf.Max(maxPriority, ruleEntry.Priority);
					if (customizer != null)
					{
						maxBucket = Mathf.Max(maxBucket, customizer.PriorityBucket(ruleEntry.rule));
					}
				}
			}
			return list.RandomElementByWeightWithFallback(delegate(GrammarResolver.RuleEntry rule)
			{
				if (GrammarResolver.ValidateRule(constants, extraTags, resolvedTags, rule, customizer) && rule.Priority == maxPriority)
				{
					GrammarRequest.ICustomizer customizer2 = customizer;
					if (((customizer2 != null) ? customizer2.PriorityBucket(rule.rule) : 0) == maxBucket)
					{
						return rule.SelectionWeight;
					}
				}
				return 0f;
			}, null);
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x001958ED File Offset: 0x00193AED
		private static bool ValidateRule(Dictionary<string, string> constants, List<string> extraTags, List<string> resolvedTags, GrammarResolver.RuleEntry rule, GrammarRequest.ICustomizer customizer)
		{
			return !rule.knownUnresolvable && rule.ValidateConstantConstraints(constants) && rule.ValidateRequiredTag(extraTags, resolvedTags) && rule.ValidateTimesUsed() && (customizer == null || customizer.ValidateRule(rule.rule));
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x00195927 File Offset: 0x00193B27
		public static bool ContainsSpecialChars(string str)
		{
			return str.IndexOfAny(GrammarResolver.SpecialChars) >= 0;
		}

		// Token: 0x040029DA RID: 10714
		private static SimpleLinearPool<List<GrammarResolver.RuleEntry>> rulePool = new SimpleLinearPool<List<GrammarResolver.RuleEntry>>();

		// Token: 0x040029DB RID: 10715
		private static Dictionary<string, List<GrammarResolver.RuleEntry>> rules = new Dictionary<string, List<GrammarResolver.RuleEntry>>();

		// Token: 0x040029DC RID: 10716
		private static int loopCount;

		// Token: 0x040029DD RID: 10717
		private static StringBuilder logSbTrace;

		// Token: 0x040029DE RID: 10718
		private static StringBuilder logSbMid;

		// Token: 0x040029DF RID: 10719
		private static StringBuilder logSbRules;

		// Token: 0x040029E0 RID: 10720
		private const int DepthLimit = 50;

		// Token: 0x040029E1 RID: 10721
		private const int LoopsLimit = 1000;

		// Token: 0x040029E2 RID: 10722
		private static Regex Spaces = new Regex(" +([,.])");

		// Token: 0x040029E3 RID: 10723
		private static Regex FunctionCall = new Regex("{\\s*(\\w+)\\s*\\:\\s*([^}]* ?)\\s*}");

		// Token: 0x040029E4 RID: 10724
		private static Regex GenderCall = new Regex("{\\s*(\\w+)_gender\\s*\\?\\s*([^}]* ?)\\s*}");

		// Token: 0x040029E5 RID: 10725
		public const char SymbolStartChar = '[';

		// Token: 0x040029E6 RID: 10726
		public const char SymbolEndChar = ']';

		// Token: 0x040029E7 RID: 10727
		private static readonly char[] SpecialChars = new char[]
		{
			'[',
			']',
			'{',
			'}'
		};

		// Token: 0x02002AB3 RID: 10931
		private class RuleEntry
		{
			// Token: 0x17002BED RID: 11245
			// (get) Token: 0x06010740 RID: 67392 RVA: 0x005C84A7 File Offset: 0x005C66A7
			public float SelectionWeight
			{
				get
				{
					return this.rule.BaseSelectionWeight * 100000f / (float)((this.uses + 1) * 1000);
				}
			}

			// Token: 0x17002BEE RID: 11246
			// (get) Token: 0x06010741 RID: 67393 RVA: 0x005C84CA File Offset: 0x005C66CA
			public float Priority
			{
				get
				{
					return this.rule.Priority;
				}
			}

			// Token: 0x06010742 RID: 67394 RVA: 0x005C84D7 File Offset: 0x005C66D7
			public RuleEntry(Rule rule)
			{
				this.rule = rule;
				this.knownUnresolvable = false;
			}

			// Token: 0x06010743 RID: 67395 RVA: 0x005C84ED File Offset: 0x005C66ED
			public void MarkKnownUnresolvable()
			{
				this.knownUnresolvable = true;
			}

			// Token: 0x06010744 RID: 67396 RVA: 0x005C84F6 File Offset: 0x005C66F6
			public bool ValidateConstantConstraints(Dictionary<string, string> constraints)
			{
				if (!this.constantConstraintsChecked)
				{
					this.constantConstraintsValid = true;
					if (this.rule.constantConstraints != null)
					{
						this.constantConstraintsValid = this.rule.ValidateConstraints(constraints);
					}
					this.constantConstraintsChecked = true;
				}
				return this.constantConstraintsValid;
			}

			// Token: 0x06010745 RID: 67397 RVA: 0x005C8533 File Offset: 0x005C6733
			public bool ValidateRequiredTag(List<string> extraTags, List<string> resolvedTags)
			{
				return this.rule.requiredTag.NullOrEmpty() || (extraTags != null && extraTags.Contains(this.rule.requiredTag)) || resolvedTags.Contains(this.rule.requiredTag);
			}

			// Token: 0x06010746 RID: 67398 RVA: 0x005C8572 File Offset: 0x005C6772
			public bool ValidateTimesUsed()
			{
				return this.rule.usesLimit == null || this.uses < this.rule.usesLimit.Value;
			}

			// Token: 0x06010747 RID: 67399 RVA: 0x005C85A0 File Offset: 0x005C67A0
			public override string ToString()
			{
				return this.rule.ToString();
			}

			// Token: 0x0400AEFB RID: 44795
			public Rule rule;

			// Token: 0x0400AEFC RID: 44796
			public bool knownUnresolvable;

			// Token: 0x0400AEFD RID: 44797
			public bool constantConstraintsChecked;

			// Token: 0x0400AEFE RID: 44798
			public bool constantConstraintsValid;

			// Token: 0x0400AEFF RID: 44799
			public int uses;
		}
	}
}
