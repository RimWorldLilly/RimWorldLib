using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld.Planet
{
	// Token: 0x020023CD RID: 9165
	public class SitePartWorker_PreciousLump : SitePartWorker
	{
		// Token: 0x0600E478 RID: 58488 RVA: 0x0054E703 File Offset: 0x0054C903
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == this.def)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}

		// Token: 0x0600E479 RID: 58489 RVA: 0x0054E720 File Offset: 0x0054C920
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			if (part.site.ActualThreatPoints > 0f)
			{
				outExtraDescriptionRules.Add(new Rule_String("lumpThreatDescription", "\n\n" + "PreciousLumpHostileThreat".Translate()));
				return;
			}
			outExtraDescriptionRules.Add(new Rule_String("lumpThreatDescription", ""));
		}
	}
}
