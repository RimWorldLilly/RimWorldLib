using System;

namespace Verse
{
	// Token: 0x02000422 RID: 1058
	public class HediffComp_DisappearsAndKills : HediffComp_DisappearsDisableable
	{
		// Token: 0x06001EFE RID: 7934 RVA: 0x000BD418 File Offset: 0x000BB618
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (this.disabled)
			{
				return;
			}
			if (this.ticksToDisappear > 0)
			{
				return;
			}
			if (base.Pawn.Dead)
			{
				return;
			}
			base.Pawn.Kill(null, null);
			this.parent.Notify_PawnDied(null, null);
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x000BD476 File Offset: 0x000BB676
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<bool>(ref this.disabled, "disabled", false, false);
		}
	}
}
