using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105C RID: 4188
	public class QuestPart_Filter_AllPawnsDespawned : QuestPart_Filter
	{
		// Token: 0x06006955 RID: 26965 RVA: 0x00252B38 File Offset: 0x00250D38
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = this.pawns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Spawned)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06006956 RID: 26966 RVA: 0x00252BA0 File Offset: 0x00250DA0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06006957 RID: 26967 RVA: 0x00252BF8 File Offset: 0x00250DF8
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

		// Token: 0x04004306 RID: 17158
		public List<Pawn> pawns;

		// Token: 0x04004307 RID: 17159
		public string inSignalRemovePawn;
	}
}
