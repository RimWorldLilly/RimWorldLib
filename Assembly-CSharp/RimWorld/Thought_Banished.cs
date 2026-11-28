using System;

namespace RimWorld
{
	// Token: 0x02000DFC RID: 3580
	public class Thought_Banished : Thought_Memory
	{
		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x060060E3 RID: 24803 RVA: 0x00230E09 File Offset: 0x0022F009
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.otherPawn.Faction == this.pawn.Faction;
			}
		}
	}
}
