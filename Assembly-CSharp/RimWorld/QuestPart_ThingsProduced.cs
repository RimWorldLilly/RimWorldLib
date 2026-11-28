using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001103 RID: 4355
	public class QuestPart_ThingsProduced : QuestPartActivable
	{
		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x06006CB4 RID: 27828 RVA: 0x00261FBC File Offset: 0x002601BC
		public override string DescriptionPart
		{
			get
			{
				return "ThingsProduced".Translate().CapitalizeFirst() + ": " + this.produced.ToString() + " / " + this.count.ToString();
			}
		}

		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x06006CB5 RID: 27829 RVA: 0x0026200A File Offset: 0x0026020A
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				QuestPart_ThingsProduced.<get_Hyperlinks>d__7 <get_Hyperlinks>d__ = new QuestPart_ThingsProduced.<get_Hyperlinks>d__7(-2);
				<get_Hyperlinks>d__.<>4__this = this;
				return <get_Hyperlinks>d__;
			}
		}

		// Token: 0x06006CB6 RID: 27830 RVA: 0x0026201A File Offset: 0x0026021A
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.produced = 0;
		}

		// Token: 0x06006CB7 RID: 27831 RVA: 0x0026202C File Offset: 0x0026022C
		public override void Notify_ThingsProduced(Pawn actor, List<Thing> things)
		{
			base.Notify_ThingsProduced(actor, things);
			if (base.State == QuestPartState.Enabled)
			{
				for (int i = 0; i < things.Count; i++)
				{
					Thing innerIfMinified = things[i].GetInnerIfMinified();
					if (innerIfMinified.def == this.def && innerIfMinified.Stuff == this.stuff)
					{
						this.produced += things[i].stackCount;
					}
				}
				if (this.produced >= this.count)
				{
					this.produced = this.count;
					base.Complete();
				}
			}
		}

		// Token: 0x06006CB8 RID: 27832 RVA: 0x002620C0 File Offset: 0x002602C0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.produced, "produced", 0, false);
		}

		// Token: 0x06006CB9 RID: 27833 RVA: 0x00262117 File Offset: 0x00260317
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThingDefOf.MealSimple;
			this.count = 10;
		}

		// Token: 0x04004535 RID: 17717
		public ThingDef def;

		// Token: 0x04004536 RID: 17718
		public ThingDef stuff;

		// Token: 0x04004537 RID: 17719
		public int count;

		// Token: 0x04004538 RID: 17720
		private int produced;
	}
}
