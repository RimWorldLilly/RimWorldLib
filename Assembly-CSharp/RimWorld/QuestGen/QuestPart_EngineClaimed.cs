using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200225D RID: 8797
	public class QuestPart_EngineClaimed : QuestPart
	{
		// Token: 0x0600D966 RID: 55654 RVA: 0x00508E64 File Offset: 0x00507064
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!signal.tag.EndsWith("ChangedFactionToPlayer"))
			{
				return;
			}
			NamedArgument namedArgument;
			if (signal.args.TryGetArg("SUBJECT", out namedArgument))
			{
				Thing thing = namedArgument.arg as Thing;
				if (thing != null && thing.def == ThingDefOf.GravEngine)
				{
					this.quest.End(QuestEndOutcome.Success, true, true);
				}
			}
		}
	}
}
