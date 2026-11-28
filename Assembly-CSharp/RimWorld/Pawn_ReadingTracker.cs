using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015BB RID: 5563
	public class Pawn_ReadingTracker : IExposable
	{
		// Token: 0x170017C8 RID: 6088
		// (get) Token: 0x060089E4 RID: 35300 RVA: 0x0031EA90 File Offset: 0x0031CC90
		// (set) Token: 0x060089E5 RID: 35301 RVA: 0x0031EAE6 File Offset: 0x0031CCE6
		public ReadingPolicy CurrentPolicy
		{
			get
			{
				if (this.pawn.IsMutant && this.pawn.mutant.Def.disablePolicies)
				{
					return null;
				}
				if (this.curPolicy == null)
				{
					this.curPolicy = Current.Game.readingPolicyDatabase.DefaultReadingPolicy();
				}
				return this.curPolicy;
			}
			set
			{
				this.curPolicy = value;
			}
		}

		// Token: 0x060089E6 RID: 35302 RVA: 0x0031EAEF File Offset: 0x0031CCEF
		public Pawn_ReadingTracker()
		{
		}

		// Token: 0x060089E7 RID: 35303 RVA: 0x0031EAF7 File Offset: 0x0031CCF7
		public Pawn_ReadingTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060089E8 RID: 35304 RVA: 0x0031EB06 File Offset: 0x0031CD06
		public void ExposeData()
		{
			Scribe_References.Look<ReadingPolicy>(ref this.curPolicy, "curAssignment", false);
		}

		// Token: 0x040054E1 RID: 21729
		public Pawn pawn;

		// Token: 0x040054E2 RID: 21730
		private ReadingPolicy curPolicy;
	}
}
