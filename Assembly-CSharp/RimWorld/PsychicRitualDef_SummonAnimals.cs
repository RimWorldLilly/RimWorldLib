using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CA0 RID: 3232
	public class PsychicRitualDef_SummonAnimals : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B89 RID: 23433 RVA: 0x0021A479 File Offset: 0x00218679
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_SummonAnimals(this.InvokerRole));
			return list;
		}

		// Token: 0x06005B8A RID: 23434 RVA: 0x0021A494 File Offset: 0x00218694
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(this.manhunterSpawnChanceFromQualityCurve.Evaluate(qualityRange.min).ToStringPercent());
		}

		// Token: 0x06005B8B RID: 23435 RVA: 0x0021A4BC File Offset: 0x002186BC
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			PsychicRitualDef_SummonAnimals.<BlockingIssues>d__4 <BlockingIssues>d__ = new PsychicRitualDef_SummonAnimals.<BlockingIssues>d__4(-2);
			<BlockingIssues>d__.<>4__this = this;
			<BlockingIssues>d__.<>3__assignments = assignments;
			<BlockingIssues>d__.<>3__map = map;
			return <BlockingIssues>d__;
		}

		// Token: 0x06005B8C RID: 23436 RVA: 0x0021A4DC File Offset: 0x002186DC
		public override void InitializeCast(Map map)
		{
			base.InitializeCast(map);
			IntVec3 intVec;
			this.animalsAllowed = (map.Biome.AllWildAnimals.EnumerableCount() > 0 && map.Biome.wildAnimalsCanWanderInto && RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null));
		}

		// Token: 0x04003468 RID: 13416
		public SimpleCurve manhunterSpawnChanceFromQualityCurve;

		// Token: 0x04003469 RID: 13417
		private bool animalsAllowed;
	}
}
