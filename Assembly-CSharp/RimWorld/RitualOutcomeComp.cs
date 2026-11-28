using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016BA RID: 5818
	public abstract class RitualOutcomeComp
	{
		// Token: 0x1700190D RID: 6413
		// (get) Token: 0x060091A0 RID: 37280 RVA: 0x0035425C File Offset: 0x0035245C
		public virtual bool DataRequired
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060091A1 RID: 37281 RVA: 0x0035425F File Offset: 0x0035245F
		public virtual float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return 0f;
		}

		// Token: 0x060091A2 RID: 37282 RVA: 0x00354266 File Offset: 0x00352466
		public virtual RitualOutcomeComp_Data MakeData()
		{
			return null;
		}

		// Token: 0x060091A3 RID: 37283
		public abstract bool Applies(LordJob_Ritual ritual);

		// Token: 0x060091A4 RID: 37284 RVA: 0x00354269 File Offset: 0x00352469
		public virtual void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
		}

		// Token: 0x060091A5 RID: 37285 RVA: 0x0035426B File Offset: 0x0035246B
		public virtual string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			return this.label;
		}

		// Token: 0x060091A6 RID: 37286 RVA: 0x00354273 File Offset: 0x00352473
		public virtual string GetDescAbstract(bool positive, float quality = -1f)
		{
			return this.label;
		}

		// Token: 0x060091A7 RID: 37287 RVA: 0x0035427B File Offset: 0x0035247B
		public virtual string GetBonusDescShort()
		{
			return this.label;
		}

		// Token: 0x060091A8 RID: 37288 RVA: 0x00354283 File Offset: 0x00352483
		public virtual QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			return null;
		}

		// Token: 0x060091A9 RID: 37289 RVA: 0x00354286 File Offset: 0x00352486
		protected virtual string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			return "";
		}

		// Token: 0x060091AA RID: 37290 RVA: 0x0035428D File Offset: 0x0035248D
		public virtual void Notify_AssignmentsChanged(RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
		}

		// Token: 0x060091AB RID: 37291 RVA: 0x0035428F File Offset: 0x0035248F
		public virtual IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
		{
			return new RitualOutcomeComp.<BlockingIssues>d__15(-2);
		}

		// Token: 0x04005876 RID: 22646
		[MustTranslate]
		protected string label;

		// Token: 0x04005877 RID: 22647
		[MustTranslate]
		protected string labelAbstract;

		// Token: 0x04005878 RID: 22648
		protected float qualityOffset;
	}
}
