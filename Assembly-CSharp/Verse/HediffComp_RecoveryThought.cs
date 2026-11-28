using System;

namespace Verse
{
	// Token: 0x0200046E RID: 1134
	public class HediffComp_RecoveryThought : HediffComp
	{
		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001FEB RID: 8171 RVA: 0x000C0ECA File Offset: 0x000BF0CA
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000C0ED8 File Offset: 0x000BF0D8
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (!base.Pawn.Dead && base.Pawn.needs.mood != null)
			{
				base.Pawn.needs.mood.thoughts.memories.TryGainMemory(this.Props.thought, null, null);
			}
		}
	}
}
