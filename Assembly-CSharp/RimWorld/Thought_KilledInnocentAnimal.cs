using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015DA RID: 5594
	public class Thought_KilledInnocentAnimal : Thought_Memory
	{
		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x06008AF9 RID: 35577 RVA: 0x00326464 File Offset: 0x00324664
		public override string LabelCap
		{
			get
			{
				string text = GenLabel.BestKindLabel(this.animal, this.gender, this.plural, -1);
				if (this.otherAnimals)
				{
					text += ", " + "Etc".Translate();
				}
				return base.CurStage.label.Formatted(text).CapitalizeFirst();
			}
		}

		// Token: 0x06008AFA RID: 35578 RVA: 0x003264D5 File Offset: 0x003246D5
		public void SetAnimal(Pawn animal)
		{
			this.animal = animal.kindDef;
			this.gender = animal.gender;
		}

		// Token: 0x06008AFB RID: 35579 RVA: 0x003264F0 File Offset: 0x003246F0
		public override void Notify_NewThoughtInGroupAdded(Thought_Memory memory)
		{
			base.Notify_NewThoughtInGroupAdded(memory);
			Thought_KilledInnocentAnimal thought_KilledInnocentAnimal = memory as Thought_KilledInnocentAnimal;
			if (thought_KilledInnocentAnimal != null)
			{
				if (thought_KilledInnocentAnimal.animal == this.animal)
				{
					this.plural = true;
					return;
				}
				this.otherAnimals = true;
			}
		}

		// Token: 0x06008AFC RID: 35580 RVA: 0x0032652C File Offset: 0x0032472C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.animal, "animal");
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<bool>(ref this.plural, "plural", false, false);
			Scribe_Values.Look<bool>(ref this.otherAnimals, "otherAnimals", false, false);
		}

		// Token: 0x0400557C RID: 21884
		public PawnKindDef animal;

		// Token: 0x0400557D RID: 21885
		public Gender gender;

		// Token: 0x0400557E RID: 21886
		public bool plural;

		// Token: 0x0400557F RID: 21887
		public bool otherAnimals;
	}
}
