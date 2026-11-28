using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC1 RID: 3521
	public class Thought_TameVeneratedAnimalDied : Thought_Memory
	{
		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x0600604B RID: 24651 RVA: 0x0022EFFC File Offset: 0x0022D1FC
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.animalKindLabel.Named("ANIMALKIND")).CapitalizeFirst();
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x0600604C RID: 24652 RVA: 0x0022F038 File Offset: 0x0022D238
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.animalKindLabel.Named("ANIMALKIND")).CapitalizeFirst() + base.CausedByBeliefInPrecept;
			}
		}

		// Token: 0x0600604D RID: 24653 RVA: 0x0022F07D File Offset: 0x0022D27D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.animalKindLabel, "animalKindLabel", null, false);
		}

		// Token: 0x040035F3 RID: 13811
		public string animalKindLabel;
	}
}
