using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld.QuestGen;

namespace Verse.Grammar
{
	// Token: 0x020007AF RID: 1967
	public class Rule_String : Rule
	{
		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06003F6A RID: 16234 RVA: 0x001962F8 File Offset: 0x001944F8
		public override float BaseSelectionWeight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06003F6B RID: 16235 RVA: 0x00196300 File Offset: 0x00194500
		public override float Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06003F6C RID: 16236 RVA: 0x00196308 File Offset: 0x00194508
		public bool OutputNull
		{
			get
			{
				return this.output == null;
			}
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x00196313 File Offset: 0x00194513
		public override Rule DeepCopy()
		{
			Rule_String rule_String = (Rule_String)base.DeepCopy();
			rule_String.output = this.output;
			rule_String.weight = this.weight;
			rule_String.priority = this.priority;
			return rule_String;
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x00196344 File Offset: 0x00194544
		public Rule_String()
		{
		}

		// Token: 0x06003F6F RID: 16239 RVA: 0x00196357 File Offset: 0x00194557
		public Rule_String(string keyword, string output)
		{
			this.keyword = keyword;
			this.output = output;
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x00196378 File Offset: 0x00194578
		public Rule_String(string rawString)
		{
			Match match = Rule_String.Pattern.Match(rawString);
			if (!match.Success)
			{
				Log.Error("Bad string pass when reading rule " + rawString);
				return;
			}
			this.keyword = match.Groups["keyword"].Value;
			this.output = match.Groups["output"].Value;
			for (int i = 0; i < match.Groups["paramname"].Captures.Count; i++)
			{
				string value = match.Groups["paramname"].Captures[i].Value;
				string value2 = match.Groups["paramoperator"].Captures[i].Value;
				string value3 = match.Groups["paramvalue"].Captures[i].Value;
				if (value == "p")
				{
					if (value2 != "=")
					{
						Log.Error("Attempt to compare p instead of assigning in rule " + rawString);
					}
					this.weight = float.Parse(value3);
				}
				else if (value == "priority")
				{
					if (value2 != "=")
					{
						Log.Error("Attempt to compare priority instead of assigning in rule " + rawString);
					}
					this.priority = float.Parse(value3);
				}
				else if (value == "tag")
				{
					if (value2 != "=")
					{
						Log.Error("Attempt to compare tag instead of assigning in rule " + rawString);
					}
					this.tag = value3;
				}
				else if (value == "requiredTag")
				{
					if (value2 != "=")
					{
						Log.Error("Attempt to compare requiredTag instead of assigning in rule " + rawString);
					}
					this.requiredTag = value3;
				}
				else if (value == "uses")
				{
					if (value2 != "=")
					{
						Log.Error("Attempt to compare uses instead of assigning in rule " + rawString);
					}
					this.usesLimit = new int?(int.Parse(value3));
				}
				else if (value == "debug")
				{
					Log.Error("Rule " + rawString + " contains debug flag; fix before commit");
				}
				else if (Rule_String.ValidOperators.Contains(value2))
				{
					base.AddConstantConstraint(value, value3, value2);
				}
				else
				{
					Log.Error("Unknown parameter " + value + " in rule " + rawString);
				}
			}
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x001965F7 File Offset: 0x001947F7
		public override string Generate()
		{
			return this.output;
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x001965FF File Offset: 0x001947FF
		public override string ToString()
		{
			return (this.keyword ?? "null_keyword") + " → " + ((this.output != null) ? this.output.Replace("\n", "\\n") : "null_output");
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x00196640 File Offset: 0x00194840
		public void AppendPrefixToAllKeywords(string prefix)
		{
			Rule_String.tmpPrefix = prefix;
			if (this.output == null)
			{
				Log.Error("Rule_String output was null.");
				this.output = "";
			}
			this.output = Regex.Replace(this.output, "\\[(.*?)\\]", new MatchEvaluator(Rule_String.RegexMatchEvaluatorAppendPrefix));
			if (this.constantConstraints != null)
			{
				for (int i = 0; i < this.constantConstraints.Count; i++)
				{
					Rule.ConstantConstraint constantConstraint = default(Rule.ConstantConstraint);
					constantConstraint.key = this.constantConstraints[i].key;
					if (!prefix.NullOrEmpty())
					{
						constantConstraint.key = prefix + "/" + constantConstraint.key;
					}
					constantConstraint.key = QuestGenUtility.NormalizeVarPath(constantConstraint.key);
					constantConstraint.value = this.constantConstraints[i].value;
					constantConstraint.type = this.constantConstraints[i].type;
					this.constantConstraints[i] = constantConstraint;
				}
			}
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x00196748 File Offset: 0x00194948
		private static string RegexMatchEvaluatorAppendPrefix(Match match)
		{
			string text = match.Groups[1].Value;
			if (!Rule_String.tmpPrefix.NullOrEmpty())
			{
				text = Rule_String.tmpPrefix + "/" + text;
			}
			text = QuestGenUtility.NormalizeVarPath(text);
			return "[" + text + "]";
		}

		// Token: 0x040029F6 RID: 10742
		[MustTranslate]
		private string output;

		// Token: 0x040029F7 RID: 10743
		private float weight = 1f;

		// Token: 0x040029F8 RID: 10744
		private float priority;

		// Token: 0x040029F9 RID: 10745
		private static readonly List<string> ValidOperators = new List<string>
		{
			"==",
			"!=",
			">",
			"<",
			">=",
			"<=",
			"[less_than]",
			"[greater_than]"
		};

		// Token: 0x040029FA RID: 10746
		private static readonly Regex Pattern = new Regex("\r\n        # hold on to your butts, this is gonna get weird\r\n\r\n        ^\r\n        (?<keyword>[a-zA-Z0-9_/]+)\t\t\t\t\t# keyword; roughly limited to standard C# identifier rules\r\n        (\t\t\t\t\t\t\t\t\t\t\t# parameter list is optional, open the capture group so we can keep it or ignore it\r\n            \\(\t\t\t\t\t\t\t\t\t\t# this is the actual parameter list opening\r\n                (\t\t\t\t\t\t\t\t\t# unlimited number of parameter groups\r\n                    [ ]*                            # leading whitespace for readability\r\n                    (?<paramname>[a-zA-Z0-9_/]+)\t# parameter name is similar\r\n                    (?<paramoperator>==|=|!=|>=|<=|>|<|\\[less_than\\]|\\[greater_than\\]) # operators; empty operator is allowed\r\n                    (?<paramvalue>[^\\,\\)]*)\t\t\t# parameter value, however, allows everything except comma and closeparen!\r\n                    ,?\t\t\t\t\t\t\t\t# comma can be used to separate blocks; it is also silently ignored if it's a trailing comma\r\n                )*\r\n            \\)\r\n        )?\r\n        [ ]*                                        # leading whitespace before -> for readability\r\n        ->(?<output>.*)\t\t\t\t\t\t\t\t# output is anything-goes\r\n        $\r\n\r\n        ", RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

		// Token: 0x040029FB RID: 10747
		private static string tmpPrefix;
	}
}
