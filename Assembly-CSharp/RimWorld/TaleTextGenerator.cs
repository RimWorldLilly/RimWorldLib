using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017E3 RID: 6115
	public static class TaleTextGenerator
	{
		// Token: 0x060098A1 RID: 39073 RVA: 0x003748B1 File Offset: 0x00372AB1
		public static TaggedString GenerateTextFromTale(TextGenerationPurpose purpose, Tale tale, int seed, RulePackDef extraInclude, List<Rule> extraRules = null, Dictionary<string, string> extraConstants = null)
		{
			return TaleTextGenerator.GenerateTextFromTale(purpose, tale, seed, new List<RulePackDef>
			{
				extraInclude
			}, extraRules, extraConstants);
		}

		// Token: 0x060098A2 RID: 39074 RVA: 0x003748CC File Offset: 0x00372ACC
		public static TaggedString GenerateTextFromTale(TextGenerationPurpose purpose, Tale tale, int seed, List<RulePackDef> extraInclude = null, List<Rule> extraRules = null, Dictionary<string, string> extraConstants = null)
		{
			Rand.PushState();
			Rand.Seed = seed;
			string rootKeyword = null;
			GrammarRequest request = default(GrammarRequest);
			if (extraInclude != null)
			{
				request.Includes.AddRange(extraInclude);
			}
			if (extraRules != null)
			{
				request.Rules.AddRange(extraRules);
			}
			if (extraConstants != null)
			{
				request.Constants.AddRange(extraConstants);
			}
			if (purpose == TextGenerationPurpose.ArtDescription)
			{
				rootKeyword = "r_art_description";
				if (tale != null && !Rand.Chance(0.2f))
				{
					request.Includes.Add(RulePackDefOf.ArtDescriptionRoot_HasTale);
					request.IncludesBare.AddRange(tale.GetTextGenerationIncludes());
					request.Rules.AddRange(tale.GetTextGenerationRules(request.Constants));
				}
				else
				{
					request.Includes.Add(RulePackDefOf.ArtDescriptionRoot_Taleless);
					request.Includes.Add(RulePackDefOf.TalelessImages);
				}
				request.Includes.Add(RulePackDefOf.ArtDescriptionUtility_Global);
			}
			else if (purpose == TextGenerationPurpose.ArtName)
			{
				rootKeyword = "r_art_name";
				if (tale != null)
				{
					request.IncludesBare.AddRange(tale.GetTextGenerationIncludes());
					request.Rules.AddRange(tale.GetTextGenerationRules(request.Constants));
				}
			}
			string str = GrammarResolver.Resolve(rootKeyword, request, (tale != null) ? tale.def.defName : "null_tale", false, null, null, null, true);
			Rand.PopState();
			return str;
		}

		// Token: 0x04005B34 RID: 23348
		private const float TalelessChanceWithTales = 0.2f;
	}
}
