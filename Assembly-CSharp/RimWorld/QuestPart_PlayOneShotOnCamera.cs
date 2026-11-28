using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010DA RID: 4314
	public class QuestPart_PlayOneShotOnCamera : QuestPart
	{
		// Token: 0x06006BE1 RID: 27617 RVA: 0x0025DCB3 File Offset: 0x0025BEB3
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.soundDef.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06006BE2 RID: 27618 RVA: 0x0025DCDB File Offset: 0x0025BEDB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<SoundDef>(ref this.soundDef, "soundDef");
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x0400449A RID: 17562
		public SoundDef soundDef;

		// Token: 0x0400449B RID: 17563
		public string inSignal;
	}
}
