using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E7 RID: 4327
	public class QuestPart_RequirementsToAcceptNoOngoingBestowingCeremony : QuestPart_RequirementsToAccept
	{
		// Token: 0x06006C22 RID: 27682 RVA: 0x0025F718 File Offset: 0x0025D918
		public override AcceptanceReport CanAccept()
		{
			if (Find.QuestManager.QuestsListForReading.Any((Quest q) => q.State == QuestState.Ongoing && q.root == QuestScriptDefOf.BestowingCeremony))
			{
				return new AcceptanceReport("QuestCanNotStartUntilBestowingCeremonyFinished".Translate());
			}
			return true;
		}
	}
}
