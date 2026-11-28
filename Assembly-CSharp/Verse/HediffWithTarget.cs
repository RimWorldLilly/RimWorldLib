using System;

namespace Verse
{
	// Token: 0x0200049F RID: 1183
	public class HediffWithTarget : HediffWithComps
	{
		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x000C4220 File Offset: 0x000C2420
		public override bool ShouldRemove
		{
			get
			{
				if (this.target != null)
				{
					Pawn pawn = this.target as Pawn;
					if (pawn == null || !pawn.Dead)
					{
						return base.ShouldRemove;
					}
				}
				return true;
			}
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x000C4254 File Offset: 0x000C2454
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x04001930 RID: 6448
		public Thing target;
	}
}
