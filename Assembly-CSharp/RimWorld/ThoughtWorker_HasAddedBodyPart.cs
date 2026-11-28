using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3E RID: 3646
	public class ThoughtWorker_HasAddedBodyPart : ThoughtWorker
	{
		// Token: 0x06006172 RID: 24946 RVA: 0x00232DAC File Offset: 0x00230FAC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			int num = GeneUtility.AddedAndImplantedPartsWithXenogenesCount(p);
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num - 1);
			}
			return false;
		}
	}
}
