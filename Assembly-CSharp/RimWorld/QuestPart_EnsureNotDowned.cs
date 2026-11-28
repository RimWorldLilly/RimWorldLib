using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010BF RID: 4287
	public class QuestPart_EnsureNotDowned : QuestPart
	{
		// Token: 0x06006B35 RID: 27445 RVA: 0x0025A008 File Offset: 0x00258208
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				foreach (Pawn p in this.pawns)
				{
					this.EnsureNotDowned(p);
				}
			}
		}

		// Token: 0x06006B36 RID: 27446 RVA: 0x0025A078 File Offset: 0x00258278
		protected void EnsureNotDowned(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			int num = 0;
			while (p.Downed && num++ < 15)
			{
				Hediff hediff = hediffs.FirstOrDefault((Hediff h) => h.def.isBad && h.def.everCurableByItem && !(h is Hediff_MissingPart));
				if (hediff == null)
				{
					break;
				}
				p.health.RemoveHediff(hediff);
			}
			if (p.Downed)
			{
				num = 0;
				while (p.Downed && num++ < 15)
				{
					Hediff hediff2 = hediffs.FirstOrDefault((Hediff h) => h.def.isBad && !(h is Hediff_MissingPart));
					if (hediff2 == null)
					{
						break;
					}
					p.health.RemoveHediff(hediff2);
				}
			}
		}

		// Token: 0x06006B37 RID: 27447 RVA: 0x0025A134 File Offset: 0x00258334
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn p) => p == null);
			}
		}

		// Token: 0x04004425 RID: 17445
		public string inSignal;

		// Token: 0x04004426 RID: 17446
		public List<Pawn> pawns;
	}
}
