using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200041E RID: 1054
	public class HediffComp_DisappearsPausable_LethalInjuries : HediffComp_DisappearsPausable
	{
		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001EF2 RID: 7922 RVA: 0x000BD301 File Offset: 0x000BB501
		protected override bool Paused
		{
			get
			{
				return SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(base.Pawn);
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001EF3 RID: 7923 RVA: 0x000BD30E File Offset: 0x000BB50E
		public override string CompTipStringExtra
		{
			get
			{
				if (this.Paused)
				{
					return "PawnWillKeepRegeneratingLethalInjuries".Translate(base.Pawn.Named("PAWN")).Colorize(ColorLibrary.RedReadable);
				}
				return base.CompTipStringExtra;
			}
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x000BD343 File Offset: 0x000BB543
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (base.Pawn.IsHashIntervalTick(120) && !this.Paused)
			{
				this.ticksToDisappear -= 120;
			}
		}

		// Token: 0x04001841 RID: 6209
		private const int PauseCheckInterval = 120;
	}
}
