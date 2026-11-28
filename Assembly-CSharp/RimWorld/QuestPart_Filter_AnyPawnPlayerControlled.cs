using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106C RID: 4204
	public class QuestPart_Filter_AnyPawnPlayerControlled : QuestPart_Filter
	{
		// Token: 0x06006987 RID: 27015 RVA: 0x002536B8 File Offset: 0x002518B8
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (!pawn.Destroyed && pawn.IsColonistPlayerControlled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006988 RID: 27016 RVA: 0x0025372C File Offset: 0x0025192C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06006989 RID: 27017 RVA: 0x00253784 File Offset: 0x00251984
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400431A RID: 17178
		public List<Pawn> pawns;

		// Token: 0x0400431B RID: 17179
		public string inSignalRemovePawn;
	}
}
