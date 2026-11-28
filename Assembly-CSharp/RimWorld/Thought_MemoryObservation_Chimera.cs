using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015DE RID: 5598
	public class Thought_MemoryObservation_Chimera : Thought_MemoryObservation
	{
		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x06008B18 RID: 35608 RVA: 0x00326B58 File Offset: 0x00324D58
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.animalLabel.Named("ANIMAL")).CapitalizeFirst();
			}
		}

		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x06008B19 RID: 35609 RVA: 0x00326B92 File Offset: 0x00324D92
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.animalLabel.Named("ANIMAL"));
			}
		}

		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x06008B1A RID: 35610 RVA: 0x00326BB9 File Offset: 0x00324DB9
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.animalLabel.NullOrEmpty();
			}
		}

		// Token: 0x17001817 RID: 6167
		// (set) Token: 0x06008B1B RID: 35611 RVA: 0x00326BD0 File Offset: 0x00324DD0
		public override Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
				CompChimera compChimera = value.TryGetComp<CompChimera>();
				string text;
				if (compChimera == null)
				{
					text = null;
				}
				else
				{
					CompProperties_Chimera props = compChimera.Props;
					text = ((props != null) ? props.simpleAnimalLabel : null);
				}
				this.animalLabel = text;
			}
		}

		// Token: 0x06008B1C RID: 35612 RVA: 0x00326C02 File Offset: 0x00324E02
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.animalLabel, "animalLabel", null, false);
		}

		// Token: 0x0400558D RID: 21901
		private string animalLabel;
	}
}
