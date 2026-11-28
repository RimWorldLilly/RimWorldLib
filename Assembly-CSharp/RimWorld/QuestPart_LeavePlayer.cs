using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CC RID: 4300
	public class QuestPart_LeavePlayer : QuestPart
	{
		// Token: 0x06006B79 RID: 27513 RVA: 0x0025B11C File Offset: 0x0025931C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].Faction == Faction.OfPlayer)
					{
						this.pawns[i].SetFaction(this.replacementFaction, null);
					}
				}
			}
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
		}

		// Token: 0x06006B7A RID: 27514 RVA: 0x0025B1F7 File Offset: 0x002593F7
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.replacementFaction == faction)
			{
				this.replacementFaction = null;
			}
		}

		// Token: 0x06006B7B RID: 27515 RVA: 0x0025B209 File Offset: 0x00259409
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06006B7C RID: 27516 RVA: 0x0025B217 File Offset: 0x00259417
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x06006B7D RID: 27517 RVA: 0x0025B228 File Offset: 0x00259428
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.replacementFaction, "replacementFaction", false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400444D RID: 17485
		public string inSignal;

		// Token: 0x0400444E RID: 17486
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400444F RID: 17487
		public Faction replacementFaction;

		// Token: 0x04004450 RID: 17488
		public string inSignalRemovePawn;
	}
}
