using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B77 RID: 2935
	public abstract class JoyGiver
	{
		// Token: 0x06005552 RID: 21842 RVA: 0x001FB776 File Offset: 0x001F9976
		public virtual float GetChance(Pawn pawn)
		{
			return this.def.baseChance;
		}

		// Token: 0x06005553 RID: 21843 RVA: 0x001FB784 File Offset: 0x001F9984
		protected virtual void GetSearchSet(Pawn pawn, List<Thing> outCandidates)
		{
			outCandidates.Clear();
			if (this.def.thingDefs == null)
			{
				return;
			}
			if (this.def.thingDefs.Count == 1)
			{
				outCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[0]));
			}
			else
			{
				for (int i = 0; i < this.def.thingDefs.Count; i++)
				{
					outCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[i]));
				}
			}
			if (pawn.ConcernedByVacuum)
			{
				outCandidates.RemoveWhere((Thing thing) => thing.Position.VacuumConcernTo(pawn));
			}
		}

		// Token: 0x06005554 RID: 21844
		public abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06005555 RID: 21845 RVA: 0x001FB859 File Offset: 0x001F9A59
		public virtual Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06005556 RID: 21846 RVA: 0x001FB85C File Offset: 0x001F9A5C
		public virtual Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatherSpot, float maxRadius = -1f)
		{
			return null;
		}

		// Token: 0x06005557 RID: 21847 RVA: 0x001FB860 File Offset: 0x001F9A60
		public virtual bool CanBeGivenTo(Pawn pawn)
		{
			return this.MissingRequiredCapacity(pawn) == null && (!this.def.requiresEnjoyOutdoors || (pawn.needs.PrefersOutdoors && (pawn.genes == null || pawn.genes.EnjoysSunlight))) && this.def.joyKind.PawnCanDo(pawn);
		}

		// Token: 0x06005558 RID: 21848 RVA: 0x001FB8BC File Offset: 0x001F9ABC
		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		// Token: 0x040032CC RID: 13004
		public JoyGiverDef def;
	}
}
