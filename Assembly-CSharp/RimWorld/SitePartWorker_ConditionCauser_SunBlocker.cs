using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001775 RID: 6005
	public class SitePartWorker_ConditionCauser_SunBlocker : SitePartWorker_ConditionCauser
	{
		// Token: 0x0600957A RID: 38266 RVA: 0x00365CB0 File Offset: 0x00363EB0
		public override bool IsAvailable()
		{
			if (ModsConfig.IdeologyActive)
			{
				foreach (Map map in Find.Maps)
				{
					foreach (Pawn pawn in map.mapPawns.FreeColonistsAndPrisoners)
					{
						if ((pawn.IsFreeNonSlaveColonist || pawn.IsPrisonerOfColony) && !pawn.IsQuestLodger())
						{
							Ideo ideo = pawn.Ideo;
							if (ideo != null && (ideo.HasMeme(MemeDefOf.Darkness) || ideo.HasMeme(MemeDefOf.Tunneler)))
							{
								return false;
							}
						}
					}
				}
			}
			return base.IsAvailable();
		}
	}
}
