using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000932 RID: 2354
	public class PsychicRitualToil_SummonFleshbeastsCultist : PsychicRitualToil
	{
		// Token: 0x06004816 RID: 18454 RVA: 0x001C4C78 File Offset: 0x001C2E78
		protected PsychicRitualToil_SummonFleshbeastsCultist()
		{
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x001C4C80 File Offset: 0x001C2E80
		public PsychicRitualToil_SummonFleshbeastsCultist(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x001C4C8F File Offset: 0x001C2E8F
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			this.ApplyOutcome(psychicRitual);
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x001C4CA0 File Offset: 0x001C2EA0
		private void ApplyOutcome(PsychicRitual psychicRitual)
		{
			float x2;
			psychicRitual.def.CalculateMaxPower(psychicRitual.assignments, null, out x2);
			float num = psychicRitual.power = ((PsychicRitualDef_SummonFleshbeasts)psychicRitual.def).fleshbeastPointsFromThreatPointsCurve.Evaluate(x2);
			int num2 = 0;
			List<Thing> list = new List<Thing>();
			Predicate<IntVec3> <>9__0;
			while (num > 0f)
			{
				IntVec3 cell;
				if (!this.TryFindBurrowCell(psychicRitual.Map, out cell))
				{
					int minEdgeDistance = 10;
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = ((IntVec3 x) => x.Walkable(psychicRitual.Map)));
					}
					if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(minEdgeDistance, validator, psychicRitual.Map, out cell))
					{
						num2++;
						if (num2 > 200)
						{
							break;
						}
						continue;
					}
				}
				float num3 = Mathf.Min(num, 500f);
				num -= num3;
				list.Add(FleshbeastUtility.SpawnFleshbeastsFromPitBurrowEmergence(cell, psychicRitual.Map, num3, PsychicRitualToil_SummonFleshbeastsCultist.PitBurrowEmergenceDelayRangeTicks, PsychicRitualToil_SummonFleshbeastsCultist.FleshbeastSpawnDelayTicks, true));
				num2++;
				if (num2 > 200)
				{
					break;
				}
			}
			if (list.Count > 0 && !psychicRitual.def.letterAICompleteLabel.NullOrEmpty() && !psychicRitual.def.letterAICompleteText.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(psychicRitual.def.letterAICompleteLabel, psychicRitual.def.letterAICompleteText, LetterDefOf.ThreatBig, list, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x001C4E3C File Offset: 0x001C303C
		private bool TryFindBurrowCell(Map map, out IntVec3 cell)
		{
			PsychicRitualToil_SummonFleshbeastsCultist.tmpPawns.Clear();
			PsychicRitualToil_SummonFleshbeastsCultist.tmpPawns.AddRange(map.mapPawns.FreeColonistsSpawned);
			PsychicRitualToil_SummonFleshbeastsCultist.tmpPawns.Shuffle<Pawn>();
			LargeBuildingSpawnParms parms = PsychicRitualToil_SummonFleshbeastsCultist.BurrowSpawnParms.ForThing(ThingDefOf.PitBurrow);
			using (List<Pawn>.Enumerator enumerator = PsychicRitualToil_SummonFleshbeastsCultist.tmpPawns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (LargeBuildingCellFinder.TryFindCellNear(enumerator.Current.Position, map, 6, parms, out cell, false))
					{
						return true;
					}
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x001C4EE8 File Offset: 0x001C30E8
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			foreach (Pawn pawn in psychicRitual.assignments.AllAssignedPawns)
			{
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Idle, null, null, null, null, 0f, null);
			}
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x001C4F6C File Offset: 0x001C316C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x04002E92 RID: 11922
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x04002E93 RID: 11923
		private static readonly IntRange FleshbeastSpawnDelayTicks = new IntRange(180, 180);

		// Token: 0x04002E94 RID: 11924
		private static readonly IntRange PitBurrowEmergenceDelayRangeTicks = new IntRange(420, 420);

		// Token: 0x04002E95 RID: 11925
		private const int MaxIterationsGuard = 200;

		// Token: 0x04002E96 RID: 11926
		private static readonly LargeBuildingSpawnParms BurrowSpawnParms = new LargeBuildingSpawnParms
		{
			maxDistanceToColonyBuilding = -1f,
			minDistToEdge = 10,
			canSpawnOnImpassable = false
		};

		// Token: 0x04002E97 RID: 11927
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
