using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020004AD RID: 1197
	public class Hediff_DisruptorFlash : HediffWithComps
	{
		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06002130 RID: 8496 RVA: 0x000C5D90 File Offset: 0x000C3F90
		public override HediffStage CurStage
		{
			get
			{
				return this.stage;
			}
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x000C5D98 File Offset: 0x000C3F98
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.SetupStage();
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x000C5DA7 File Offset: 0x000C3FA7
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.SetupStage();
			}
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x000C5DC0 File Offset: 0x000C3FC0
		public void SetupStage()
		{
			this.stage = new HediffStage
			{
				capMods = new List<PawnCapacityModifier>
				{
					new PawnCapacityModifier
					{
						capacity = PawnCapacityDefOf.Consciousness,
						postFactor = 1f - this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) * 0.1f
					},
					new PawnCapacityModifier
					{
						capacity = PawnCapacityDefOf.Moving,
						postFactor = 1f - this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) * 0.2f
					}
				}
			};
		}

		// Token: 0x0400195D RID: 6493
		private HediffStage stage;
	}
}
