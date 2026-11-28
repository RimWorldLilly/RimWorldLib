using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A7 RID: 6311
	public class CompGleamingMonolith : CompVoidStructure
	{
		// Token: 0x17001B11 RID: 6929
		// (get) Token: 0x06009E02 RID: 40450 RVA: 0x0038BAA0 File Offset: 0x00389CA0
		protected override bool Activatable
		{
			get
			{
				return Find.Anomaly.LevelDef == MonolithLevelDefOf.Gleaming;
			}
		}

		// Token: 0x17001B12 RID: 6930
		// (get) Token: 0x06009E03 RID: 40451 RVA: 0x0038BAB3 File Offset: 0x00389CB3
		public override bool Active
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B13 RID: 6931
		// (get) Token: 0x06009E04 RID: 40452 RVA: 0x0038BAB6 File Offset: 0x00389CB6
		protected override SoundDef AmbientSound
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B14 RID: 6932
		// (get) Token: 0x06009E05 RID: 40453 RVA: 0x0038BAB9 File Offset: 0x00389CB9
		public override int TicksToActivate
		{
			get
			{
				if (!this.activated)
				{
					return base.Props.ticksToActivate;
				}
				return 600;
			}
		}

		// Token: 0x06009E06 RID: 40454 RVA: 0x0038BAD4 File Offset: 0x00389CD4
		protected override void OnInteracted(Pawn caster)
		{
			base.OnInteracted(caster);
			this.progress = 0f;
			caster.stances.stunner.StunFor(960, this.parent, true, false, true);
		}

		// Token: 0x04005D80 RID: 23936
		private const int TicksToReactivate = 600;

		// Token: 0x04005D81 RID: 23937
		private const int ActivateStunTicks = 960;
	}
}
