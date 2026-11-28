using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001937 RID: 6455
	public class CompPlaySoundOnDestroy : ThingComp
	{
		// Token: 0x17001BFB RID: 7163
		// (get) Token: 0x0600A1EB RID: 41451 RVA: 0x0039DEBE File Offset: 0x0039C0BE
		private CompProperties_PlaySoundOnDestroy Props
		{
			get
			{
				return (CompProperties_PlaySoundOnDestroy)this.props;
			}
		}

		// Token: 0x0600A1EC RID: 41452 RVA: 0x0039DECB File Offset: 0x0039C0CB
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode == DestroyMode.Vanish && this.Props.ignoreOnVanish)
			{
				return;
			}
			if (this.Props.onlyWhenKilled && mode != DestroyMode.KillFinalize && mode != DestroyMode.KillFinalizeLeavingsOnly)
			{
				return;
			}
			if (previousMap != null)
			{
				this.Props.sound.PlayOneShotOnCamera(previousMap);
			}
		}
	}
}
