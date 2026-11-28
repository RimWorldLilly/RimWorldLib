using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001481 RID: 5249
	public class CreepJoinerWorker_AggressionBasic : BaseCreepJoinerWorker
	{
		// Token: 0x060080DE RID: 32990 RVA: 0x002DFB30 File Offset: 0x002DDD30
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			base.Pawn.SetFaction(Find.FactionManager.OfEntities, null);
			base.Pawn.guest.Recruitable = false;
			Lord lord = base.Pawn.GetLord();
			if (lord != null)
			{
				lord.RemovePawn(base.Pawn);
			}
			lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_AssaultColony(Faction.OfEntities, false, false, false, false, false, false, false), base.Pawn.MapHeld, null);
			lord.AddPawn(base.Pawn);
		}
	}
}
