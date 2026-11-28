using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C8 RID: 5832
	public class RitualOutcomeComp_RitualSeatPresent : RitualOutcomeComp_BuildingsPresent
	{
		// Token: 0x1700191B RID: 6427
		// (get) Token: 0x060091F8 RID: 37368 RVA: 0x00355368 File Offset: 0x00353568
		protected override string LabelForDesc
		{
			get
			{
				return "RitualSeat".Translate();
			}
		}

		// Token: 0x060091F9 RID: 37369 RVA: 0x00355379 File Offset: 0x00353579
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			if (assignments.SpectatorsForReading.Count > 0)
			{
				return base.GetQualityFactor(ritual, ritualTarget, obligation, assignments, data);
			}
			return null;
		}

		// Token: 0x060091FA RID: 37370 RVA: 0x0035539C File Offset: 0x0035359C
		public override bool Applies(LordJob_Ritual ritual)
		{
			return !Find.IdeoManager.classicMode && (base.Applies(ritual) && ritual.assignments != null && ritual.assignments.SpectatorsForReading.Count > 0) && ritual.Ritual.ideo.RitualSeatDef != null;
		}

		// Token: 0x060091FB RID: 37371 RVA: 0x003553F0 File Offset: 0x003535F0
		protected override int RequiredAmount(RitualRoleAssignments assignments)
		{
			if (assignments.Ritual.ideo.RitualSeatDef == null)
			{
				return 0;
			}
			int num = assignments.Ritual.ideo.RitualSeatDef.Size.x * assignments.Ritual.ideo.RitualSeatDef.Size.z;
			return Mathf.Max(1, Mathf.CeilToInt((float)assignments.SpectatorsForReading.Count / (float)num));
		}

		// Token: 0x060091FC RID: 37372 RVA: 0x00355464 File Offset: 0x00353664
		protected override string LabelForPredictedOutcomeDesc(Precept_Ritual ritual)
		{
			ThingDef ritualSeatDef = ritual.ideo.RitualSeatDef;
			if (ritualSeatDef == null)
			{
				return this.LabelForDesc;
			}
			return ritualSeatDef.LabelCap;
		}

		// Token: 0x060091FD RID: 37373 RVA: 0x00355494 File Offset: 0x00353694
		protected override Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual)
		{
			ThingDef ritualSeatDef = ritual.ideo.RitualSeatDef;
			if (ritualSeatDef == null)
			{
				return null;
			}
			Thing firstThing = cell.GetFirstThing(map, ritualSeatDef);
			if (firstThing != null)
			{
				return firstThing;
			}
			return null;
		}
	}
}
