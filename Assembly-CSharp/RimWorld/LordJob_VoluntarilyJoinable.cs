using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CB6 RID: 3254
	public abstract class LordJob_VoluntarilyJoinable : LordJob
	{
		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x06005C71 RID: 23665 RVA: 0x0021FA3D File Offset: 0x0021DC3D
		public override bool ShouldExistWithoutPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005C72 RID: 23666 RVA: 0x0021FA40 File Offset: 0x0021DC40
		public virtual float VoluntaryJoinPriorityFor(Pawn p)
		{
			return 0f;
		}

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06005C73 RID: 23667 RVA: 0x0021FA47 File Offset: 0x0021DC47
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}
	}
}
