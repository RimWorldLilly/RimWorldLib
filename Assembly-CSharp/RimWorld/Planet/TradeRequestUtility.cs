using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200236B RID: 9067
	public static class TradeRequestUtility
	{
		// Token: 0x0600E0DD RID: 57565 RVA: 0x00538B44 File Offset: 0x00536D44
		public static string RequestedThingLabel(ThingDef def, int count)
		{
			string text = GenLabel.ThingLabel(def, null, count);
			if (def.HasComp(typeof(CompQuality)))
			{
				text += " (" + "NormalQualityOrBetter".Translate() + ")";
			}
			if (def.IsApparel)
			{
				text += " (" + "NotTainted".Translate() + ")";
			}
			return text;
		}
	}
}
