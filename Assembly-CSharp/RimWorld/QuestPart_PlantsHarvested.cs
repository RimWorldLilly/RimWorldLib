using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D8 RID: 4312
	public class QuestPart_PlantsHarvested : QuestPartActivable
	{
		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06006BD6 RID: 27606 RVA: 0x0025DB28 File Offset: 0x0025BD28
		public override string DescriptionPart
		{
			get
			{
				return "PlantsHarvested".Translate().CapitalizeFirst() + ": " + this.harvested.ToString() + " / " + this.count.ToString();
			}
		}

		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06006BD7 RID: 27607 RVA: 0x0025DB76 File Offset: 0x0025BD76
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				QuestPart_PlantsHarvested.<get_Hyperlinks>d__6 <get_Hyperlinks>d__ = new QuestPart_PlantsHarvested.<get_Hyperlinks>d__6(-2);
				<get_Hyperlinks>d__.<>4__this = this;
				return <get_Hyperlinks>d__;
			}
		}

		// Token: 0x06006BD8 RID: 27608 RVA: 0x0025DB86 File Offset: 0x0025BD86
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.harvested = 0;
		}

		// Token: 0x06006BD9 RID: 27609 RVA: 0x0025DB98 File Offset: 0x0025BD98
		public override void Notify_PlantHarvested(Pawn actor, Thing harvested)
		{
			base.Notify_PlantHarvested(actor, harvested);
			if (base.State == QuestPartState.Enabled && harvested.def == this.plant)
			{
				this.harvested += harvested.stackCount;
				if (this.harvested >= this.count)
				{
					this.harvested = this.count;
					base.Complete();
				}
			}
		}

		// Token: 0x06006BDA RID: 27610 RVA: 0x0025DBF7 File Offset: 0x0025BDF7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plant, "plant");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.harvested, "harvested", 0, false);
		}

		// Token: 0x06006BDB RID: 27611 RVA: 0x0025DC33 File Offset: 0x0025BE33
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.plant = ThingDefOf.RawPotatoes;
			this.count = 10;
		}

		// Token: 0x04004495 RID: 17557
		public ThingDef plant;

		// Token: 0x04004496 RID: 17558
		public int count;

		// Token: 0x04004497 RID: 17559
		private int harvested;
	}
}
