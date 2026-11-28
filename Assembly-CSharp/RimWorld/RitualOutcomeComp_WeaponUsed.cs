using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016BF RID: 5823
	public class RitualOutcomeComp_WeaponUsed : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x17001913 RID: 6419
		// (get) Token: 0x060091C7 RID: 37319 RVA: 0x0035485A File Offset: 0x00352A5A
		protected override string LabelForDesc
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17001914 RID: 6420
		// (get) Token: 0x060091C8 RID: 37320 RVA: 0x00354862 File Offset: 0x00352A62
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060091C9 RID: 37321 RVA: 0x00354868 File Offset: 0x00352A68
		public override bool Applies(LordJob_Ritual ritual)
		{
			LordJob_Ritual_Duel lordJob_Ritual_Duel = ritual as LordJob_Ritual_Duel;
			return lordJob_Ritual_Duel != null && lordJob_Ritual_Duel.usedWeapon;
		}

		// Token: 0x060091CA RID: 37322 RVA: 0x00354888 File Offset: 0x00352A88
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			return new QualityFactor
			{
				label = this.LabelForDesc.CapitalizeFirst(),
				present = false,
				uncertainOutcome = true,
				qualityChange = this.ExpectedOffsetDesc(true, -1f),
				quality = this.qualityOffset,
				positive = true
			};
		}
	}
}
