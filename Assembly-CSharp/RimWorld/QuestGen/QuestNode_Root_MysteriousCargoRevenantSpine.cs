using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002286 RID: 8838
	public class QuestNode_Root_MysteriousCargoRevenantSpine : QuestNode_Root_MysteriousCargo
	{
		// Token: 0x17002621 RID: 9761
		// (get) Token: 0x0600DA4A RID: 55882 RVA: 0x00511E6C File Offset: 0x0051006C
		protected override bool RequiresPawn { get; }

		// Token: 0x0600DA4B RID: 55883 RVA: 0x00511E74 File Offset: 0x00510074
		protected override Thing GenerateThing(Pawn _)
		{
			return ThingMaker.MakeThing(ThingDefOf.RevenantSpine, null);
		}
	}
}
