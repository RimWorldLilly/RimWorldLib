using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002205 RID: 8709
	public class QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle : QuestPart
	{
		// Token: 0x0600D835 RID: 55349 RVA: 0x00502BCC File Offset: 0x00500DCC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			IEnumerable<Thing> sentThings;
			if (signal.tag == this.inSignal && signal.args.TryGetArg<IEnumerable<Thing>>("SENT", out sentThings))
			{
				this.DoWork(sentThings);
			}
		}

		// Token: 0x0600D836 RID: 55350 RVA: 0x00502C08 File Offset: 0x00500E08
		private void DoWork(IEnumerable<Thing> sentThings)
		{
			int num = 0;
			using (IEnumerator<Thing> enumerator = sentThings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is Pawn)
					{
						num++;
					}
				}
			}
			int num2 = 0;
			using (List<Pawn>.Enumerator enumerator2 = this.pawns.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (!enumerator2.Current.Dead)
					{
						num2++;
					}
				}
			}
			if (num < num2)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.faction, this.goodwillChange * (num2 - num), true, true, this.historyEvent, null);
			}
		}

		// Token: 0x0600D837 RID: 55351 RVA: 0x00502CD0 File Offset: 0x00500ED0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<int>(ref this.goodwillChange, "goodwillChange", 0, false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04008996 RID: 35222
		public string inSignal;

		// Token: 0x04008997 RID: 35223
		public List<Pawn> pawns;

		// Token: 0x04008998 RID: 35224
		public Faction faction;

		// Token: 0x04008999 RID: 35225
		public int goodwillChange;

		// Token: 0x0400899A RID: 35226
		public HistoryEventDef historyEvent;
	}
}
