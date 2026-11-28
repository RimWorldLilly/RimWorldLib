using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D71 RID: 3441
	public class ThinkNode_ConditionalPackAnimalHasColonistToFollowWhilePacking : ThinkNode_Conditional
	{
		// Token: 0x06005F66 RID: 24422 RVA: 0x0022C212 File Offset: 0x0022A412
		protected override bool Satisfied(Pawn pawn)
		{
			return JobGiver_PackAnimalFollowColonists.GetPawnToFollow(pawn) != null;
		}
	}
}
