using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A1 RID: 6049
	public class MutantCount : StartingPawnCount
	{
		// Token: 0x170019AA RID: 6570
		// (get) Token: 0x060096CE RID: 38606 RVA: 0x0036B474 File Offset: 0x00369674
		public override string Summary
		{
			get
			{
				string key = "PawnCount";
				NamedArgument arg = this.count.Named("COUNT");
				string arg2;
				if ((arg2 = this.description) == null)
				{
					MutantDef mutantDef = this.mutant;
					arg2 = (((mutantDef != null) ? mutantDef.label : null) ?? Find.ActiveLanguageWorker.Pluralize("Colonist".Translate(), this.count));
				}
				return key.Translate(arg, arg2.Named("KINDLABEL"));
			}
		}

		// Token: 0x060096CF RID: 38607 RVA: 0x0036B4EE File Offset: 0x003696EE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<MutantDef>(ref this.mutant, "mutant");
			Scribe_Values.Look<DevelopmentalStage>(ref this.allowedDevelopmentalStages, "allowedDevelopmentalStages", DevelopmentalStage.None, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
		}

		// Token: 0x060096D0 RID: 38608 RVA: 0x0036B52C File Offset: 0x0036972C
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			num ^= this.allowedDevelopmentalStages.GetHashCode();
			num ^= this.description.GetHashCodeSafe();
			if (this.mutant != null)
			{
				num ^= this.mutant.GetHashCode();
			}
			return num;
		}

		// Token: 0x04005A66 RID: 23142
		public MutantDef mutant;

		// Token: 0x04005A67 RID: 23143
		public DevelopmentalStage allowedDevelopmentalStages = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04005A68 RID: 23144
		[MustTranslate]
		public string description;
	}
}
