using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001499 RID: 5273
	public class Hediff_Addiction : HediffWithComps
	{
		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x0600813F RID: 33087 RVA: 0x002E22C4 File Offset: 0x002E04C4
		public Need_Chemical Need
		{
			get
			{
				if (this.pawn.Dead)
				{
					return null;
				}
				Need need;
				if (this.pawn.needs.TryGetNeed(this.def.chemicalNeed, out need))
				{
					return (Need_Chemical)need;
				}
				return null;
			}
		}

		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x06008140 RID: 33088 RVA: 0x002E2308 File Offset: 0x002E0508
		public ChemicalDef Chemical
		{
			get
			{
				List<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].addictionHediff == this.def)
					{
						return allDefsListForReading[i];
					}
				}
				return null;
			}
		}

		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x06008141 RID: 33089 RVA: 0x002E234C File Offset: 0x002E054C
		public override string LabelInBrackets
		{
			get
			{
				string labelInBrackets = base.LabelInBrackets;
				string text = (1f - this.Severity).ToStringPercent("F0");
				if (this.def.CompProps<HediffCompProperties_SeverityPerDay>() == null)
				{
					return labelInBrackets;
				}
				if (!labelInBrackets.NullOrEmpty())
				{
					return labelInBrackets + " " + text;
				}
				return text;
			}
		}

		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x06008142 RID: 33090 RVA: 0x002E239C File Offset: 0x002E059C
		public override string TipStringExtra
		{
			get
			{
				string text = base.TipStringExtra;
				Need_Chemical need = this.Need;
				if (need != null)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "CreatesNeed".Translate() + ": " + need.LabelCap + " (" + need.CurLevelPercentage.ToStringPercent("F0") + ")";
				}
				return text;
			}
		}

		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x06008143 RID: 33091 RVA: 0x002E2424 File Offset: 0x002E0624
		public override int CurStageIndex
		{
			get
			{
				Need_Chemical need = this.Need;
				if (need == null || need.CurCategory != DrugDesireCategory.Withdrawal)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06008144 RID: 33092 RVA: 0x002E2446 File Offset: 0x002E0646
		public void Notify_NeedCategoryChanged()
		{
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x04005064 RID: 20580
		private const int DefaultStageIndex = 0;

		// Token: 0x04005065 RID: 20581
		private const int WithdrawalStageIndex = 1;
	}
}
