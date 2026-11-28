using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D95 RID: 3477
	public class ThoughtWorker_Precept_Skullspike : ThoughtWorker_Precept
	{
		// Token: 0x06005FCE RID: 24526 RVA: 0x0022D5A8 File Offset: 0x0022B7A8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.surroundings == null || p.IsSlave)
			{
				return false;
			}
			int val = p.surroundings.NumSkullspikeSightings();
			if (ThoughtWorker_Precept_Skullspike.LowRange.Includes(val))
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (ThoughtWorker_Precept_Skullspike.MediumRange.Includes(val))
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (ThoughtWorker_Precept_Skullspike.MaxRange.Includes(val))
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return false;
		}

		// Token: 0x040035DB RID: 13787
		public static readonly IntRange LowRange = new IntRange(1, 3);

		// Token: 0x040035DC RID: 13788
		public static readonly IntRange MediumRange = new IntRange(4, 8);

		// Token: 0x040035DD RID: 13789
		public static readonly IntRange MaxRange = new IntRange(9, int.MaxValue);
	}
}
