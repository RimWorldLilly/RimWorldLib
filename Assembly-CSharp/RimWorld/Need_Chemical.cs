using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200156B RID: 5483
	public class Need_Chemical : Need
	{
		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x06008678 RID: 34424 RVA: 0x0030A140 File Offset: 0x00308340
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x06008679 RID: 34425 RVA: 0x0030A143 File Offset: 0x00308343
		public DrugDesireCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.1f)
				{
					return DrugDesireCategory.Satisfied;
				}
				if (this.CurLevel > 0.01f)
				{
					return DrugDesireCategory.Desire;
				}
				return DrugDesireCategory.Withdrawal;
			}
		}

		// Token: 0x170016D1 RID: 5841
		// (get) Token: 0x0600867A RID: 34426 RVA: 0x0030A164 File Offset: 0x00308364
		// (set) Token: 0x0600867B RID: 34427 RVA: 0x0030A16C File Offset: 0x0030836C
		public override float CurLevel
		{
			get
			{
				return base.CurLevel;
			}
			set
			{
				DrugDesireCategory curCategory = this.CurCategory;
				base.CurLevel = value;
				if (this.CurCategory != curCategory)
				{
					this.CategoryChanged();
				}
			}
		}

		// Token: 0x170016D2 RID: 5842
		// (get) Token: 0x0600867C RID: 34428 RVA: 0x0030A198 File Offset: 0x00308398
		public Hediff_Addiction AddictionHediff
		{
			get
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
					if (hediff_Addiction != null && hediff_Addiction.def.chemicalNeed == this.def)
					{
						return hediff_Addiction;
					}
				}
				return null;
			}
		}

		// Token: 0x170016D3 RID: 5843
		// (get) Token: 0x0600867D RID: 34429 RVA: 0x0030A1F2 File Offset: 0x003083F2
		private float ChemicalFallPerTick
		{
			get
			{
				return this.def.fallPerDay / 60000f;
			}
		}

		// Token: 0x0600867E RID: 34430 RVA: 0x0030A205 File Offset: 0x00308405
		public Need_Chemical(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
		}

		// Token: 0x0600867F RID: 34431 RVA: 0x0030A229 File Offset: 0x00308429
		public override void SetInitialLevel()
		{
			base.CurLevelPercentage = Rand.Range(0.8f, 1f);
		}

		// Token: 0x06008680 RID: 34432 RVA: 0x0030A240 File Offset: 0x00308440
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= this.ChemicalFallPerTick * 150f;
			}
		}

		// Token: 0x06008681 RID: 34433 RVA: 0x0030A263 File Offset: 0x00308463
		private void CategoryChanged()
		{
			Hediff_Addiction addictionHediff = this.AddictionHediff;
			if (addictionHediff == null)
			{
				return;
			}
			addictionHediff.Notify_NeedCategoryChanged();
		}

		// Token: 0x040052D7 RID: 21207
		private const float ThreshDesire = 0.01f;

		// Token: 0x040052D8 RID: 21208
		private const float ThreshSatisfied = 0.1f;
	}
}
