using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021AA RID: 8618
	public class QuestPart_PawnsNoLongerRequiredForShuttleOnRescue : QuestPart
	{
		// Token: 0x0600D6DA RID: 55002 RVA: 0x004FC118 File Offset: 0x004FA318
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignal && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
				if (compShuttle != null)
				{
					compShuttle.requiredPawns.Remove(item);
				}
				this.pawns.Remove(item);
			}
		}

		// Token: 0x0600D6DB RID: 55003 RVA: 0x004FC184 File Offset: 0x004FA384
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400881B RID: 34843
		public string inSignal;

		// Token: 0x0400881C RID: 34844
		public Thing shuttle;

		// Token: 0x0400881D RID: 34845
		public List<Pawn> pawns = new List<Pawn>();
	}
}
