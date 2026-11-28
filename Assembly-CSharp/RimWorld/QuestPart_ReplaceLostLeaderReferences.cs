using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E1 RID: 4321
	public class QuestPart_ReplaceLostLeaderReferences : QuestPart
	{
		// Token: 0x06006C03 RID: 27651 RVA: 0x0025EF74 File Offset: 0x0025D174
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Pawn arg = signal.args.GetArg<Pawn>("SUBJECT");
				Pawn arg2 = signal.args.GetArg<Pawn>("NEWFACTIONLEADER");
				if (arg != null && arg2 != null)
				{
					List<QuestPart> partsListForReading = this.quest.PartsListForReading;
					for (int i = 0; i < partsListForReading.Count; i++)
					{
						partsListForReading[i].ReplacePawnReferences(arg, arg2);
					}
					if (arg.questTags != null)
					{
						if (arg2.questTags == null)
						{
							arg2.questTags = new List<string>();
						}
						arg2.questTags.AddRange(arg.questTags);
					}
				}
			}
		}

		// Token: 0x06006C04 RID: 27652 RVA: 0x0025F01F File Offset: 0x0025D21F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x06006C05 RID: 27653 RVA: 0x0025F03C File Offset: 0x0025D23C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x040044D0 RID: 17616
		public string inSignal;
	}
}
