using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAB RID: 3499
	public class ThoughtWorker_Precept_HasProsthetic_Count : ThoughtWorker_Precept
	{
		// Token: 0x0600600B RID: 24587 RVA: 0x0022E334 File Offset: 0x0022C534
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			int num = ThoughtWorker_Precept_HasProsthetic_Count.ProstheticsCount(p);
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num - 1);
			}
			return false;
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x0022E35B File Offset: 0x0022C55B
		public static int ProstheticsCount(Pawn p)
		{
			return GeneUtility.AddedAndImplantedPartsWithXenogenesCount(p);
		}
	}
}
