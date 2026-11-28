using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D0D RID: 3341
	public class LordToil_Concert : LordToil_Party
	{
		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x06005DF7 RID: 24055 RVA: 0x00226885 File Offset: 0x00224A85
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		// Token: 0x06005DF8 RID: 24056 RVA: 0x0022688D File Offset: 0x00224A8D
		public LordToil_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef, joyPerTick)
		{
			this.organizer = organizer;
		}

		// Token: 0x04003556 RID: 13654
		private Pawn organizer;
	}
}
