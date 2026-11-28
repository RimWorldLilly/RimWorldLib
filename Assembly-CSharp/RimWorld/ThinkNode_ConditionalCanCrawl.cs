using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D3D RID: 3389
	public class ThinkNode_ConditionalCanCrawl : ThinkNode_Conditional
	{
		// Token: 0x06005EF7 RID: 24311 RVA: 0x0022B6F4 File Offset: 0x002298F4
		protected override bool Satisfied(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return (lord == null || !(lord.LordJob is LordJob_PsychicRitual)) && pawn.health.CanCrawl;
		}
	}
}
