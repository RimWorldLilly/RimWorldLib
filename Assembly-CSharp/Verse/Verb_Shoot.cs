using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200073E RID: 1854
	public class Verb_Shoot : Verb_LaunchProjectile
	{
		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06003AC9 RID: 15049 RVA: 0x00176C80 File Offset: 0x00174E80
		protected override int ShotsPerBurst
		{
			get
			{
				return base.BurstShotCount;
			}
		}

		// Token: 0x06003ACA RID: 15050 RVA: 0x00176C88 File Offset: 0x00174E88
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Pawn pawn = this.currentTarget.Thing as Pawn;
			if (pawn != null && !pawn.Downed && !pawn.IsColonyMech && this.CasterIsPawn && this.CasterPawn.skills != null)
			{
				float num = pawn.HostileTo(this.caster) ? 170f : 20f;
				float num2 = this.verbProps.AdjustedFullCycleTime(this, this.CasterPawn);
				this.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2, false, false);
			}
		}

		// Token: 0x06003ACB RID: 15051 RVA: 0x00176D1C File Offset: 0x00174F1C
		protected override bool TryCastShot()
		{
			bool flag = base.TryCastShot();
			if (flag && this.CasterIsPawn)
			{
				this.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
			}
			return flag;
		}
	}
}
