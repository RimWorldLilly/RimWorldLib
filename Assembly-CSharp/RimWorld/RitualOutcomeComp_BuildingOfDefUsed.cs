using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016BD RID: 5821
	public class RitualOutcomeComp_BuildingOfDefUsed : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x17001910 RID: 6416
		// (get) Token: 0x060091BD RID: 37309 RVA: 0x0035465A File Offset: 0x0035285A
		protected override string LabelForDesc
		{
			get
			{
				return "Used".Translate(this.def.LabelCap);
			}
		}

		// Token: 0x060091BE RID: 37310 RVA: 0x0035467B File Offset: 0x0035287B
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.usedThings.Any((Thing t) => t.def == this.def);
		}

		// Token: 0x060091BF RID: 37311 RVA: 0x00354694 File Offset: 0x00352894
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			bool flag = false;
			foreach (Thing thing in ritualTarget.Map.listerThings.ThingsOfDef(this.def))
			{
				if (GatheringsUtility.InGatheringArea(thing.Position, ritualTarget.Cell, ritualTarget.Map) && (this.maxHorDistFromTarget == 0 || thing.Position.InHorDistOf(ritualTarget.Cell, (float)this.maxHorDistFromTarget)))
				{
					flag = true;
					break;
				}
			}
			return new QualityFactor
			{
				label = this.def.LabelCap,
				present = flag,
				qualityChange = this.ExpectedOffsetDesc(flag, -1f),
				quality = (flag ? this.qualityOffset : 0f),
				positive = flag,
				priority = 1f
			};
		}

		// Token: 0x0400587A RID: 22650
		public ThingDef def;

		// Token: 0x0400587B RID: 22651
		public int maxHorDistFromTarget;
	}
}
