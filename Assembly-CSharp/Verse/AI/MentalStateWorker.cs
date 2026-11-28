using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x0200088B RID: 2187
	public class MentalStateWorker
	{
		// Token: 0x0600441B RID: 17435 RVA: 0x001AB684 File Offset: 0x001A9884
		public virtual bool StateCanOccur(Pawn pawn)
		{
			if (!this.def.inCaravanCanDo && !pawn.Spawned && pawn.IsCaravanMember())
			{
				return false;
			}
			if (!this.def.unspawnedNotInCaravanCanDo && !pawn.Spawned && !pawn.IsCaravanMember())
			{
				return false;
			}
			if (!this.def.prisonersCanDo && pawn.IsPrisoner)
			{
				return false;
			}
			if (!this.def.slavesCanDo && pawn.IsSlave)
			{
				return false;
			}
			if (this.def.colonistsOnly && pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			if (this.def.slavesOnly && !pawn.IsSlave)
			{
				return false;
			}
			if (!this.def.downedCanDo && pawn.Downed)
			{
				return false;
			}
			if (pawn.IsWorldPawn())
			{
				return false;
			}
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04002C3B RID: 11323
		public MentalStateDef def;
	}
}
