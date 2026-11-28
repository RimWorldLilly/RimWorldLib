using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000423 RID: 1059
	public class HediffComp_DisappearsAndKills_Shambler : HediffComp_DisappearsAndKills
	{
		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001F01 RID: 7937 RVA: 0x000BD498 File Offset: 0x000BB698
		private CompHoldingPlatformTarget compHoldingPlatformTarget
		{
			get
			{
				return base.Pawn.TryGetComp<CompHoldingPlatformTarget>();
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x000BD4A5 File Offset: 0x000BB6A5
		protected override bool Paused
		{
			get
			{
				if (!this.disabled)
				{
					CompHoldingPlatformTarget compHoldingPlatformTarget = this.compHoldingPlatformTarget;
					return compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform;
				}
				return true;
			}
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x000BD4C4 File Offset: 0x000BB6C4
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (this.disabled)
			{
				return;
			}
			if (!base.Pawn.Dead)
			{
				base.Pawn.Kill(null, null);
			}
		}
	}
}
