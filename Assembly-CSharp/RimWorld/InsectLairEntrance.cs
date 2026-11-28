using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B25 RID: 6949
	public class InsectLairEntrance : MapPortal
	{
		// Token: 0x0600B218 RID: 45592 RVA: 0x003EB8C0 File Offset: 0x003E9AC0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.spawnGravcore, "spawnGravcore", false, false);
		}

		// Token: 0x0600B219 RID: 45593 RVA: 0x003EB8DC File Offset: 0x003E9ADC
		public override void OnEntered(Pawn pawn)
		{
			if (!this.beenEntered)
			{
				TaggedString taggedString = "EnteredMegahiveText".Translate(pawn.Named("PAWN"));
				if (this.spawnGravcore)
				{
					taggedString += "\n\n" + "EnteredMegahiveGravcoreExtra".Translate();
				}
				Find.LetterStack.ReceiveLetter("EnteredMegahiveLabel".Translate(), taggedString, LetterDefOf.ThreatBig, this.exit, null, null, null, null, 0, true);
			}
			base.OnEntered(pawn);
		}

		// Token: 0x0600B21A RID: 45594 RVA: 0x003EB95C File Offset: 0x003E9B5C
		protected override void ReceiveCompSignal(string signal)
		{
			base.ReceiveCompSignal(signal);
			if (signal != "Sealed")
			{
				return;
			}
			EffecterDefOf.ImpactSmallDustCloud.Spawn(base.Position, base.Map, 1f).Cleanup();
			SoundDefOf.Crater_FilledIn.PlayOneShot(this);
		}

		// Token: 0x040066BB RID: 26299
		public bool spawnGravcore;
	}
}
