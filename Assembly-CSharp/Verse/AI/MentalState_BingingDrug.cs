using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000866 RID: 2150
	public class MentalState_BingingDrug : MentalState_Binging
	{
		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06004377 RID: 17271 RVA: 0x001A9589 File Offset: 0x001A7789
		public override string InspectLine
		{
			get
			{
				return string.Format(base.InspectLine, this.chemical.label);
			}
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x001A95A1 File Offset: 0x001A77A1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ChemicalDef>(ref this.chemical, "chemical");
			Scribe_Values.Look<DrugCategory>(ref this.drugCategory, "drugCategory", DrugCategory.None, false);
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x001A95CC File Offset: 0x001A77CC
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseRandomChemical();
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				string str = "LetterLabelDrugBinge".Translate(this.chemical.label).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
				string text = "LetterDrugBinge".Translate(this.pawn.Label, this.chemical.label, this.pawn).CapitalizeFirst();
				if (!reason.NullOrEmpty())
				{
					text = text + "\n\n" + reason;
				}
				Find.LetterStack.ReceiveLetter(str, text, LetterDefOf.ThreatSmall, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x001A96BC File Offset: 0x001A78BC
		public override void PostEnd()
		{
			base.PostEnd();
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerBingingOnDrug".Translate(this.pawn.LabelShort, this.chemical.label, this.pawn), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x001A972C File Offset: 0x001A792C
		private void ChooseRandomChemical()
		{
			MentalState_BingingDrug.addictions.Clear();
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && AddictionUtility.CanBingeOnNow(this.pawn, hediff_Addiction.Chemical, DrugCategory.Any))
				{
					MentalState_BingingDrug.addictions.Add(hediff_Addiction.Chemical);
				}
			}
			if (MentalState_BingingDrug.addictions.Count > 0)
			{
				this.chemical = MentalState_BingingDrug.addictions.RandomElement<ChemicalDef>();
				this.drugCategory = DrugCategory.Any;
				MentalState_BingingDrug.addictions.Clear();
				return;
			}
			this.chemical = (from x in DefDatabase<ChemicalDef>.AllDefsListForReading
			where AddictionUtility.CanBingeOnNow(this.pawn, x, this.def.drugCategory)
			select x).RandomElementWithFallback(null);
			if (this.chemical != null)
			{
				this.drugCategory = this.def.drugCategory;
				return;
			}
			this.chemical = (from x in DefDatabase<ChemicalDef>.AllDefsListForReading
			where AddictionUtility.CanBingeOnNow(this.pawn, x, DrugCategory.Any)
			select x).RandomElementWithFallback(null);
			if (this.chemical != null)
			{
				this.drugCategory = DrugCategory.Any;
				return;
			}
			this.chemical = DefDatabase<ChemicalDef>.AllDefsListForReading.RandomElement<ChemicalDef>();
			this.drugCategory = DrugCategory.Any;
		}

		// Token: 0x04002C05 RID: 11269
		public ChemicalDef chemical;

		// Token: 0x04002C06 RID: 11270
		public DrugCategory drugCategory;

		// Token: 0x04002C07 RID: 11271
		private static List<ChemicalDef> addictions = new List<ChemicalDef>();
	}
}
