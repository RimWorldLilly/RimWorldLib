using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001482 RID: 5250
	public class CreepJoinerWorker_FleshbeastEmergence : BaseCreepJoinerWorker
	{
		// Token: 0x17001615 RID: 5653
		// (get) Token: 0x060080E0 RID: 32992 RVA: 0x002DFBBB File Offset: 0x002DDDBB
		public override bool CanOccurOnDeath
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060080E1 RID: 32993 RVA: 0x002DFBC0 File Offset: 0x002DDDC0
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			Pawn t = FleshbeastUtility.SpawnFleshbeastFromPawn(base.Pawn, true, false, new PawnKindDef[]
			{
				PawnKindDefOf.Bulbfreak
			});
			looktargets.Add(t);
		}
	}
}
