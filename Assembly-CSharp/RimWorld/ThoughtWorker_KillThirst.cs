using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E65 RID: 3685
	public class ThoughtWorker_KillThirst : ThoughtWorker
	{
		// Token: 0x060061E7 RID: 25063 RVA: 0x0023434C File Offset: 0x0023254C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			Need_KillThirst need_KillThirst;
			if (p.needs == null || !p.needs.TryGetNeed<Need_KillThirst>(out need_KillThirst))
			{
				return ThoughtState.Inactive;
			}
			return need_KillThirst.CurLevel <= 0.3f;
		}

		// Token: 0x0400360A RID: 13834
		public const float MinLevelForThought = 0.3f;
	}
}
