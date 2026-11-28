using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A45 RID: 6725
	public class CompUseEffect_PlaySound : CompUseEffect
	{
		// Token: 0x17001D8F RID: 7567
		// (get) Token: 0x0600A88A RID: 43146 RVA: 0x003BDE66 File Offset: 0x003BC066
		private CompProperties_UseEffectPlaySound Props
		{
			get
			{
				return (CompProperties_UseEffectPlaySound)this.props;
			}
		}

		// Token: 0x0600A88B RID: 43147 RVA: 0x003BDE73 File Offset: 0x003BC073
		public override void DoEffect(Pawn usedBy)
		{
			if (usedBy.Map == Find.CurrentMap && this.Props.soundOnUsed != null)
			{
				this.Props.soundOnUsed.PlayOneShot(SoundInfo.InMap(usedBy, MaintenanceType.None));
			}
		}
	}
}
