using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200171F RID: 5919
	public class RitualStageAction_SoundOneshotOnTarget : RitualStageAction
	{
		// Token: 0x060093BC RID: 37820 RVA: 0x0035E55F File Offset: 0x0035C75F
		public override void Apply(LordJob_Ritual ritual)
		{
			this.sound.PlayOneShot(SoundInfo.InMap(ritual.selectedTarget, MaintenanceType.None));
		}

		// Token: 0x060093BD RID: 37821 RVA: 0x0035E578 File Offset: 0x0035C778
		public override void ExposeData()
		{
			Scribe_Defs.Look<SoundDef>(ref this.sound, "sound");
		}

		// Token: 0x0400594D RID: 22861
		public SoundDef sound;
	}
}
