using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B0B RID: 6923
	public class SignalAction_SoundOneShot : SignalAction
	{
		// Token: 0x0600B0F5 RID: 45301 RVA: 0x003E5757 File Offset: 0x003E3957
		protected override void DoAction(SignalArgs args)
		{
			this.sound.PlayOneShot(this);
		}

		// Token: 0x0600B0F6 RID: 45302 RVA: 0x003E576A File Offset: 0x003E396A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<SoundDef>(ref this.sound, "sound");
		}

		// Token: 0x0400662D RID: 26157
		public SoundDef sound;
	}
}
