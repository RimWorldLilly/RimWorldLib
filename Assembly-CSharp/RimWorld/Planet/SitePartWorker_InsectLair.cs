using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023CA RID: 9162
	public class SitePartWorker_InsectLair : SitePartWorker
	{
		// Token: 0x0600E471 RID: 58481 RVA: 0x0054E468 File Offset: 0x0054C668
		public override void PostMapGenerate(Map map)
		{
			Thing thing = map.listerThings.ThingsOfDef(ThingDefOf.InsectLairEntrance).FirstOrDefault<Thing>();
			if (thing != null)
			{
				Find.LetterStack.ReceiveLetter("CaveEntranceLetter".Translate(), "CaveEntranceLetterText".Translate(), LetterDefOf.NeutralEvent, thing, null, null, null, null, 0, true);
			}
		}
	}
}
