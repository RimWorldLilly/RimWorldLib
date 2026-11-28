using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001524 RID: 5412
	public class InteractionWorker_SanguophageTalk : InteractionWorker
	{
		// Token: 0x06008473 RID: 33907 RVA: 0x002FBD88 File Offset: 0x002F9F88
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			if (initiator.genes == null || recipient.genes == null || initiator.genes.Xenotype != XenotypeDefOf.Sanguophage || recipient.genes.Xenotype != XenotypeDefOf.Sanguophage)
			{
				return 0f;
			}
			Lord lord = initiator.GetLord();
			Lord lord2 = recipient.GetLord();
			if (lord == null || lord2 == null || lord != lord2)
			{
				return 0f;
			}
			if (lord.CurLordToil == null || !(lord.CurLordToil is LordToil_SanguophageMeeting))
			{
				return 0f;
			}
			return 999f;
		}

		// Token: 0x040051CF RID: 20943
		private const float SelectionWeight_SanguophageMeeting = 999f;
	}
}
