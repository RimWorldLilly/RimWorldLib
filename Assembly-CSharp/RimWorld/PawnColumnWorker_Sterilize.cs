using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DD5 RID: 7637
	public class PawnColumnWorker_Sterilize : PawnColumnWorker_Checkbox
	{
		// Token: 0x0600C674 RID: 50804 RVA: 0x0048B552 File Offset: 0x00489752
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (this.AnimalSterile(pawn))
			{
				this.DrawSterile(rect, pawn);
				return;
			}
			base.DoCell(rect, pawn, table);
		}

		// Token: 0x0600C675 RID: 50805 RVA: 0x0048B570 File Offset: 0x00489770
		private void DrawSterile(Rect rect, Pawn animal)
		{
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			Vector2 vector = new Vector2(rect.x + (float)num, rect.y + (float)num2);
			Rect rect2 = new Rect(vector.x, vector.y, 24f, 24f);
			GUI.DrawTexture(rect2, PawnColumnWorker_Sterilize.AnimalSterilized.Texture);
			if (Mouse.IsOver(rect2))
			{
				string tip = this.GetTip(animal);
				if (!tip.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect2, tip);
				}
			}
		}

		// Token: 0x0600C676 RID: 50806 RVA: 0x0048B60C File Offset: 0x0048980C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C677 RID: 50807 RVA: 0x0048B62F File Offset: 0x0048982F
		private int GetValueToCompare(Pawn pawn)
		{
			if (this.AnimalSterile(pawn))
			{
				return -1;
			}
			if (this.SterilizeOperations(pawn).Any<Bill>())
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600C678 RID: 50808 RVA: 0x0048B64D File Offset: 0x0048984D
		protected override string GetTip(Pawn pawn)
		{
			if (this.AnimalSterile(pawn))
			{
				return "AnimalAlreadySterile".Translate();
			}
			return "SterilizeAnimal".Translate();
		}

		// Token: 0x0600C679 RID: 50809 RVA: 0x0048B677 File Offset: 0x00489877
		protected override bool GetValue(Pawn pawn)
		{
			return this.SterilizeOperations(pawn).Any<Bill>();
		}

		// Token: 0x0600C67A RID: 50810 RVA: 0x0048B688 File Offset: 0x00489888
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			if (this.AnimalSterile(pawn))
			{
				return;
			}
			bool flag = this.SterilizeOperations(pawn).Any<Bill>();
			if (value == flag)
			{
				return;
			}
			if (table.SortingBy == this.def)
			{
				table.SetDirty();
			}
			if (!value)
			{
				this.CancelSterilizeOperations(pawn);
				return;
			}
			if (pawn.HomeFaction != Faction.OfPlayer && pawn.HomeFaction != null)
			{
				TaggedString taggedString = "AnimalSterilizeConfirm".Translate(pawn.Named("PAWN"), pawn.HomeFaction.Named("FACTION"));
				Find.WindowStack.Add(new Dialog_Confirm(taggedString, delegate()
				{
					this.AddSterilizeOperation(pawn);
				}));
				return;
			}
			this.AddSterilizeOperation(pawn);
		}

		// Token: 0x0600C67B RID: 50811 RVA: 0x0048B770 File Offset: 0x00489970
		private bool AnimalSterile(Pawn animal)
		{
			return animal.health.hediffSet.HasHediff(HediffDefOf.Sterilized, false);
		}

		// Token: 0x0600C67C RID: 50812 RVA: 0x0048B788 File Offset: 0x00489988
		private List<Bill> SterilizeOperations(Pawn animal)
		{
			return (from b in animal.BillStack.Bills
			where b.recipe == RecipeDefOf.Sterilize
			select b).ToList<Bill>();
		}

		// Token: 0x0600C67D RID: 50813 RVA: 0x0048B7BE File Offset: 0x004899BE
		private void AddSterilizeOperation(Pawn animal)
		{
			HealthCardUtility.CreateSurgeryBill(animal, RecipeDefOf.Sterilize, null, null, true);
		}

		// Token: 0x0600C67E RID: 50814 RVA: 0x0048B7D0 File Offset: 0x004899D0
		private void CancelSterilizeOperations(Pawn animal)
		{
			foreach (Bill bill in this.SterilizeOperations(animal))
			{
				animal.BillStack.Delete(bill);
			}
		}

		// Token: 0x04007101 RID: 28929
		public static readonly CachedTexture AnimalSterilized = new CachedTexture("UI/Icons/Animal/Sterile");
	}
}
