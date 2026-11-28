using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000488 RID: 1160
	public class Hediff_VatLearning : HediffWithComps
	{
		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x0600204B RID: 8267 RVA: 0x000C2480 File Offset: 0x000C0680
		public override bool ShouldRemove
		{
			get
			{
				return this.pawn.Spawned || !(this.pawn.ParentHolder is Building_GrowthVat);
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x0600204C RID: 8268 RVA: 0x000C24A7 File Offset: 0x000C06A7
		public override string LabelInBrackets
		{
			get
			{
				return this.Severity.ToStringPercent();
			}
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000C24B4 File Offset: 0x000C06B4
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (this.Severity >= this.def.maxSeverity)
			{
				this.Learn();
			}
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000C24D8 File Offset: 0x000C06D8
		public void Learn()
		{
			if (this.pawn.skills != null)
			{
				SkillRecord skillRecord;
				if ((from x in this.pawn.skills.skills
				where !x.TotallyDisabled
				select x).TryRandomElement(out skillRecord))
				{
					skillRecord.Learn(8000f, true, false);
				}
			}
			this.Severity = this.def.initialSeverity;
		}

		// Token: 0x0400190D RID: 6413
		private const float XPToAward = 8000f;
	}
}
