using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D6E RID: 3438
	public class ThinkNode_ConditionalHiveCanReproduce : ThinkNode_Conditional
	{
		// Token: 0x06005F5F RID: 24415 RVA: 0x0022C190 File Offset: 0x0022A390
		protected override bool Satisfied(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			return hive != null && hive.GetComp<CompSpawnerHives>().canSpawnHives;
		}
	}
}
