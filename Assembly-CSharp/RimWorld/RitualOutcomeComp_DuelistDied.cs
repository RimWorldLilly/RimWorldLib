using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016BE RID: 5822
	public class RitualOutcomeComp_DuelistDied : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x17001911 RID: 6417
		// (get) Token: 0x060091C2 RID: 37314 RVA: 0x003547AC File Offset: 0x003529AC
		protected override string LabelForDesc
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17001912 RID: 6418
		// (get) Token: 0x060091C3 RID: 37315 RVA: 0x003547B4 File Offset: 0x003529B4
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060091C4 RID: 37316 RVA: 0x003547B8 File Offset: 0x003529B8
		public override bool Applies(LordJob_Ritual ritual)
		{
			LordJob_Ritual_Duel lordJob_Ritual_Duel = ritual as LordJob_Ritual_Duel;
			if (lordJob_Ritual_Duel != null)
			{
				return lordJob_Ritual_Duel.duelists.Any((Pawn d) => d.Dead);
			}
			return false;
		}

		// Token: 0x060091C5 RID: 37317 RVA: 0x003547FC File Offset: 0x003529FC
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
