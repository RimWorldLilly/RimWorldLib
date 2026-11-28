using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001622 RID: 5666
	internal static class IdeoDescriptionUtility
	{
		// Token: 0x06008DA4 RID: 36260 RVA: 0x0033AAAC File Offset: 0x00338CAC
		private static string MemeGroupOf(Rule rule)
		{
			if (rule.tag == null || !rule.tag.StartsWith("meme_"))
			{
				return null;
			}
			return rule.tag;
		}

		// Token: 0x06008DA5 RID: 36261 RVA: 0x0033AAD0 File Offset: 0x00338CD0
		private static IdeoDescriptionUtility.SegmentEvaluator BuildGrammarRequest(Ideo ideo, IdeoStoryPatternDef pattern, Dictionary<string, string> tokens)
		{
			GrammarRequest request = default(GrammarRequest);
			IdeoDescriptionUtility.DescriptionGrammarCustomizer customizer = new IdeoDescriptionUtility.DescriptionGrammarCustomizer();
			request.customizer = customizer;
			IdeoDescriptionUtility.AddRandomPawn("founder", ideo, ref request);
			IdeoDescriptionUtility.AddRandomPawn("believer", ideo, ref request);
			if (!ideo.memberName.NullOrEmpty())
			{
				tokens.AddDistinct("memberName", ideo.memberName);
			}
			if (!ideo.MemberNamePlural.NullOrEmpty())
			{
				tokens.AddDistinct("memberNamePlural", ideo.MemberNamePlural);
			}
			if (!ideo.adjective.NullOrEmpty())
			{
				tokens.AddDistinct("adjective", ideo.adjective);
			}
			foreach (MemeDef memeDef in ideo.memes)
			{
				if (memeDef.generalRules != null)
				{
					request.IncludesBare.Add(memeDef.generalRules);
				}
				IdeoDescriptionMaker descriptionMaker = memeDef.descriptionMaker;
				if (((descriptionMaker != null) ? descriptionMaker.rules : null) != null)
				{
					request.IncludesBare.Add(memeDef.descriptionMaker.rules);
				}
				IdeoDescriptionMaker descriptionMaker2 = memeDef.descriptionMaker;
				if (((descriptionMaker2 != null) ? descriptionMaker2.constants : null) != null)
				{
					IDictionary<string, string> constants = request.Constants;
					IdeoDescriptionMaker descriptionMaker3 = memeDef.descriptionMaker;
					constants.AddRange((descriptionMaker3 != null) ? descriptionMaker3.constants : null);
				}
			}
			IdeoDescriptionUtility.AddPreceptRole(ideo, "leader", PreceptDefOf.IdeoRole_Leader, tokens);
			IdeoDescriptionUtility.AddPreceptRole(ideo, "moralist", PreceptDefOf.IdeoRole_Moralist, tokens);
			IdeoDescriptionUtility.AddPreceptRules<Precept_Relic>(ideo, "relic", tokens, null);
			IdeoDescriptionUtility.AddPreceptRules<Precept_Animal>(ideo, "animal", tokens, null);
			IdeoDescriptionUtility.AddPreceptRules<Precept_Ritual>(ideo, "ritual", tokens, null);
			IdeoDescriptionUtility.AddPreceptRules<Precept_Building>(ideo, "altar", tokens, (Precept_Building p) => p.ThingDef.isAltar);
			string worshipRoomLabel = ideo.WorshipRoomLabel;
			if (worshipRoomLabel != null)
			{
				tokens.AddDistinct("altarRoomLabel", worshipRoomLabel);
			}
			ideo.foundation.AddPlaceRules(ref request);
			IdeoFoundation_Deity ideoFoundation_Deity = ideo.foundation as IdeoFoundation_Deity;
			if (ideoFoundation_Deity != null)
			{
				ideoFoundation_Deity.AddDeityRules(tokens, ref request);
			}
			if (pattern.rules != null)
			{
				request.IncludesBare.Add(pattern.rules);
			}
			request.Rules.Add(new Rule_String("foeSoldiers", GrammarResolver.Resolve("place_foeSoldiers", request, null, false, null, null, null, false)));
			request.Rules.Add(new Rule_String("foeLeader", GrammarResolver.Resolve("place_foeLeader", request, null, false, null, null, null, false)));
			request.Constants.SetOrAdd("foeLeader_gender", "Male");
			IdeoDescriptionUtility.DecorateRule("memeConcept", ref request);
			IdeoDescriptionUtility.DecorateRule("memeHyphenPrefix", ref request);
			IdeoDescriptionUtility.DecorateRule("attributionJob", ref request);
			IdeoDescriptionUtility.DecorateRule("attributionSource", ref request);
			IdeoDescriptionUtility.DecorateRule("memeConference", ref request);
			return new IdeoDescriptionUtility.SegmentEvaluator(customizer, request);
		}

		// Token: 0x06008DA6 RID: 36262 RVA: 0x0033AD9C File Offset: 0x00338F9C
		private static void DecorateRule(string keyword, ref GrammarRequest request)
		{
			if (request.HasRule(keyword))
			{
				string str = GrammarResolver.Resolve(keyword, request, null, false, null, null, null, false);
				request.Rules.Add(new Rule_String(keyword + "_titleCase", Find.ActiveLanguageWorker.ToTitleCase(str)));
			}
		}

		// Token: 0x06008DA7 RID: 36263 RVA: 0x0033ADEC File Offset: 0x00338FEC
		private static void AddPreceptRules<T>(Ideo ideo, string rulePrefix, Dictionary<string, string> tokens, Func<T, bool> filter = null) where T : Precept
		{
			IEnumerable<T> source = ideo.PreceptsListForReading.OfType<T>();
			if (filter != null)
			{
				source = source.Where(filter);
			}
			List<T> list = source.ToList<T>();
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted(rulePrefix);
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("_");
				string str = defaultInterpolatedStringHandler.ToStringAndClear();
				tokens.AddDistinct(str + "name", t.Label.ApplyTag(TagType.Name, null).Resolve());
				tokens.AddDistinct(str + "label", t.Label.ApplyTag(TagType.Name, null).Resolve());
				Precept_ThingDef precept_ThingDef = t as Precept_ThingDef;
				if (precept_ThingDef != null)
				{
					tokens.AddDistinct(str + "thingDef_label", precept_ThingDef.ThingDef.label.ApplyTag(TagType.Name, null).Resolve());
				}
			}
		}

		// Token: 0x06008DA8 RID: 36264 RVA: 0x0033AF00 File Offset: 0x00339100
		private static void AddPreceptRole(Ideo ideo, string prefix, PreceptDef def, Dictionary<string, string> tokens)
		{
			foreach (Precept_Role precept_Role in ideo.PreceptsListForReading.OfType<Precept_Role>())
			{
				if (precept_Role.def == def)
				{
					tokens.AddDistinct(prefix + "Title", precept_Role.Label);
					break;
				}
			}
		}

		// Token: 0x06008DA9 RID: 36265 RVA: 0x0033AF70 File Offset: 0x00339170
		private static void AddRandomPawn(string key, Ideo ideo, ref GrammarRequest request)
		{
			Gender gender = ideo.SupremeGender;
			if (gender == Gender.None)
			{
				gender = ((Rand.Value < 0.5f) ? Gender.Male : Gender.Female);
			}
			Name name = PawnBioAndNameGenerator.GenerateFullPawnName(ThingDefOf.Human, null, null, null, null, ideo.culture, false, gender, PawnNameCategory.HumanStandard, null, false);
			int num = 21;
			int chronologicalAge = num;
			string relationInfo = "";
			foreach (Rule item in GrammarUtility.RulesForPawn(key, name, null, PawnKindDefOf.Colonist, gender, null, num, chronologicalAge, relationInfo, false, false, false, null, false, string.Empty, request.Constants, false))
			{
				request.Rules.Add(item);
			}
		}

		// Token: 0x06008DAA RID: 36266 RVA: 0x0033B028 File Offset: 0x00339228
		public static List<string> ShuffleSegmentPreferences(IdeoStoryPatternDef pattern, List<MemeDef> memes, Dictionary<string, string> constants)
		{
			List<string> list = new List<string>();
			List<Rule> list2 = (from rule in memes.Where(delegate(MemeDef meme)
			{
				IdeoDescriptionMaker descriptionMaker = meme.descriptionMaker;
				return ((descriptionMaker != null) ? descriptionMaker.rules : null) != null;
			}).SelectMany(delegate(MemeDef meme)
			{
				IdeoDescriptionMaker descriptionMaker = meme.descriptionMaker;
				if (descriptionMaker == null)
				{
					return null;
				}
				return descriptionMaker.rules.Rules;
			})
			where IdeoDescriptionUtility.MemeGroupOf(rule) != null
			where pattern.segments.Contains(rule.keyword)
			where rule.ValidateConstraints(constants)
			select rule).ToList<Rule>();
			List<string> source = (from rule in list2
			select IdeoDescriptionUtility.MemeGroupOf(rule)).Distinct<string>().ToList<string>();
			for (int i = 0; i < 100; i++)
			{
				list.Clear();
				list.AddRange(source.InRandomOrder(null).Take(pattern.segments.Count));
				while (list.Count < pattern.segments.Count)
				{
					list.Add(null);
				}
				list.Shuffle<string>();
				bool flag = true;
				for (int j = 0; j < pattern.segments.Count; j++)
				{
					string segment = pattern.segments[j];
					string tag = list[j];
					if (tag != null && !list2.Any((Rule rule) => rule.tag == tag && rule.keyword == segment))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			return list;
		}

		// Token: 0x06008DAB RID: 36267 RVA: 0x0033B1F4 File Offset: 0x003393F4
		public static IdeoDescriptionResult ResolveDescription(Ideo ideo, IdeoStoryPatternDef pattern, bool force)
		{
			Dictionary<string, string> tokens = new Dictionary<string, string>();
			IdeoDescriptionUtility.SegmentEvaluator segmentEvaluator = IdeoDescriptionUtility.BuildGrammarRequest(ideo, pattern, tokens);
			if (!force && ideo.descriptionTemplate != null && IdeoDescriptionUtility.TokensInTemplate(ideo.descriptionTemplate).TrueForAll((string t) => tokens.ContainsKey(t)))
			{
				return IdeoDescriptionUtility.EvaluateTemplate(ideo.descriptionTemplate, tokens);
			}
			foreach (string text in tokens.Keys)
			{
				segmentEvaluator.request.Rules.Add(new Rule_String(text, "%%" + text + "%%"));
			}
			List<string> list = IdeoDescriptionUtility.ShuffleSegmentPreferences(pattern, ideo.memes, segmentEvaluator.request.Constants);
			List<string> list2 = new List<string>();
			for (int i = 0; i < pattern.segments.Count; i++)
			{
				string text2 = pattern.segments[i];
				string targetTag = list[i];
				bool capitalizeFirstSentence = !pattern.noCapitalizeFirstSentence.Contains(text2);
				string item = segmentEvaluator.NextSegment(text2, targetTag, capitalizeFirstSentence);
				list2.Add(item);
			}
			string tokenTemplate;
			if (segmentEvaluator.request.HasRule("r_pattern"))
			{
				for (int j = 0; j < pattern.segments.Count; j++)
				{
					string str = pattern.segments[j];
					string output = list2[j];
					List<Rule> rules = segmentEvaluator.request.Rules;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
					defaultInterpolatedStringHandler.AppendLiteral("r_segment");
					defaultInterpolatedStringHandler.AppendFormatted<int>(j);
					rules.Add(new Rule_String(defaultInterpolatedStringHandler.ToStringAndClear(), output));
					segmentEvaluator.request.Rules.Add(new Rule_String("r_" + str, output));
				}
				tokenTemplate = GrammarResolver.Resolve("r_pattern", segmentEvaluator.request, null, false, null, null, null, true);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text3 in list2)
				{
					if (!text3.NullOrEmpty())
					{
						stringBuilder.Append(text3).Append(" ");
					}
				}
				tokenTemplate = stringBuilder.ToString().TrimEnd();
			}
			IdeoDescriptionResult ideoDescriptionResult = IdeoDescriptionUtility.EvaluateTemplate(tokenTemplate, tokens);
			ideoDescriptionResult.text = GrammarResolver.ResolveAllFunctions(ideoDescriptionResult.text, segmentEvaluator.request.ConstantsAllowNull);
			return ideoDescriptionResult;
		}

		// Token: 0x06008DAC RID: 36268 RVA: 0x0033B4B8 File Offset: 0x003396B8
		private static string ConvertTokensToRules(string template, List<string> tokensSeen = null)
		{
			IdeoDescriptionUtility.<>c__DisplayClass12_0 CS$<>8__locals1 = new IdeoDescriptionUtility.<>c__DisplayClass12_0();
			CS$<>8__locals1.tokensSeen = tokensSeen;
			return IdeoDescriptionUtility.TokenPattern.Replace(template, new MatchEvaluator(CS$<>8__locals1.<ConvertTokensToRules>g__Replacement|0));
		}

		// Token: 0x06008DAD RID: 36269 RVA: 0x0033B4EC File Offset: 0x003396EC
		private static List<string> TokensInTemplate(string template)
		{
			List<string> list = new List<string>();
			IdeoDescriptionUtility.ConvertTokensToRules(template, list);
			return list;
		}

		// Token: 0x06008DAE RID: 36270 RVA: 0x0033B508 File Offset: 0x00339708
		private static IdeoDescriptionResult EvaluateTemplate(string tokenTemplate, Dictionary<string, string> tokens)
		{
			GrammarRequest request = default(GrammarRequest);
			request.Rules.AddRange(from kv in tokens
			select new Rule_String(kv.Key, kv.Value));
			request.Rules.Add(new Rule_String("r_result", IdeoDescriptionUtility.ConvertTokensToRules(tokenTemplate, null)));
			return new IdeoDescriptionResult
			{
				template = tokenTemplate,
				text = GrammarResolver.Resolve("r_result", request, null, false, null, null, null, true)
			};
		}

		// Token: 0x040056D8 RID: 22232
		private const string CustomPatternName = "r_pattern";

		// Token: 0x040056D9 RID: 22233
		private static readonly Regex TokenPattern = new Regex("%%(?<keyword>[^%]+)%%");

		// Token: 0x0200343C RID: 13372
		internal class DescriptionGrammarCustomizer : GrammarRequest.ICustomizer
		{
			// Token: 0x06013375 RID: 78709 RVA: 0x006565C7 File Offset: 0x006547C7
			public int PriorityBucket(Rule rule)
			{
				if (this.targetTag != null && this.targetTag.Equals(IdeoDescriptionUtility.MemeGroupOf(rule)))
				{
					return 2;
				}
				return 1;
			}

			// Token: 0x06013376 RID: 78710 RVA: 0x006565E7 File Offset: 0x006547E7
			public void Notify_RuleUsed(Rule rule)
			{
				this.usedMemeRules.Increment(rule);
			}

			// Token: 0x06013377 RID: 78711 RVA: 0x006565F5 File Offset: 0x006547F5
			public bool ValidateRule(Rule rule)
			{
				return rule.usesLimit == null || this.usedMemeRules.TryGetValue(rule, 0) < rule.usesLimit.Value;
			}

			// Token: 0x0400D17C RID: 53628
			private readonly Dictionary<Rule, int> usedMemeRules = new Dictionary<Rule, int>();

			// Token: 0x0400D17D RID: 53629
			public string targetTag;
		}

		// Token: 0x0200343D RID: 13373
		public class SegmentEvaluator
		{
			// Token: 0x06013379 RID: 78713 RVA: 0x00656633 File Offset: 0x00654833
			internal SegmentEvaluator(IdeoDescriptionUtility.DescriptionGrammarCustomizer customizer, GrammarRequest request)
			{
				this.customizer = customizer;
				this.request = request;
			}

			// Token: 0x0601337A RID: 78714 RVA: 0x00656649 File Offset: 0x00654849
			public string NextSegment(string segment, string targetTag, bool capitalizeFirstSentence)
			{
				this.customizer.targetTag = targetTag;
				string result = GrammarResolver.Resolve(segment, this.request, null, false, null, null, null, capitalizeFirstSentence);
				this.customizer.targetTag = null;
				return result;
			}

			// Token: 0x0400D17E RID: 53630
			private readonly IdeoDescriptionUtility.DescriptionGrammarCustomizer customizer;

			// Token: 0x0400D17F RID: 53631
			public readonly GrammarRequest request;
		}
	}
}
