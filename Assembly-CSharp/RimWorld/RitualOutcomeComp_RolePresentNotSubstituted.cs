using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C0 RID: 5824
	public class RitualOutcomeComp_RolePresentNotSubstituted : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x17001915 RID: 6421
		// (get) Token: 0x060091CC RID: 37324 RVA: 0x003548E6 File Offset: 0x00352AE6
		protected override string LabelForDesc
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x060091CD RID: 37325 RVA: 0x003548EE File Offset: 0x00352AEE
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			if (this.disableInClassicMode && Find.IdeoManager.classicMode)
			{
				return string.Empty;
			}
			return base.GetDesc(ritual, data);
		}

		// Token: 0x060091CE RID: 37326 RVA: 0x00354912 File Offset: 0x00352B12
		public override bool Applies(LordJob_Ritual ritual)
		{
			return (!this.disableInClassicMode || !Find.IdeoManager.classicMode) && ritual.RoleFilled(this.roleId) && !ritual.assignments.RoleSubstituted(this.roleId);
		}

		// Token: 0x060091CF RID: 37327 RVA: 0x00354950 File Offset: 0x00352B50
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			if (this.disableInClassicMode && Find.IdeoManager.classicMode)
			{
				return null;
			}
			bool flag = assignments.AnyPawnAssigned(this.roleId) && !assignments.RoleSubstituted(this.roleId);
			return new QualityFactor
			{
				label = this.LabelForDesc.CapitalizeFirst(),
				present = flag,
				qualityChange = this.ExpectedOffsetDesc(flag, -1f),
				quality = (flag ? this.qualityOffset : 0f),
				positive = flag,
				priority = 3f
			};
		}

		// Token: 0x0400587C RID: 22652
		public string roleId;

		// Token: 0x0400587D RID: 22653
		public bool disableInClassicMode;
	}
}
