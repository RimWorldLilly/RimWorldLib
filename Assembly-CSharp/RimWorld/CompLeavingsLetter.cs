using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DC RID: 6364
	public class CompLeavingsLetter : ThingComp
	{
		// Token: 0x17001B7B RID: 7035
		// (get) Token: 0x06009F9B RID: 40859 RVA: 0x003939E9 File Offset: 0x00391BE9
		public CompProperties_LeavingsLetter Props
		{
			get
			{
				return (CompProperties_LeavingsLetter)this.props;
			}
		}

		// Token: 0x06009F9C RID: 40860 RVA: 0x003939F8 File Offset: 0x00391BF8
		public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
			if (!leavings.NullOrEmpty<Thing>())
			{
				CompLeavingsLetter.tmpLeavings.AddRange((this.Props.leavingsFilter != null) ? (from t in leavings
				where this.Props.leavingsFilter.Allows(t.def)
				select t) : leavings);
				if (CompLeavingsLetter.tmpLeavings.Count > 0)
				{
					Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(CompLeavingsLetter.tmpLeavings[0].Named("LEAVINGS1")), this.Props.letterText.Formatted(CompLeavingsLetter.tmpLeavings[0].Named("LEAVINGS1"), this.parent.Named("PARENT")), this.Props.letterDef, CompLeavingsLetter.tmpLeavings, null, null, null, null, 0, true);
					CompLeavingsLetter.tmpLeavings.Clear();
				}
			}
		}

		// Token: 0x04005E5D RID: 24157
		private static List<Thing> tmpLeavings = new List<Thing>();
	}
}
