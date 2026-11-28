using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE3 RID: 6883
	public class Dialog_MechBillConfig : Dialog_BillConfig
	{
		// Token: 0x0600AFCB RID: 45003 RVA: 0x003DF2B7 File Offset: 0x003DD4B7
		public Dialog_MechBillConfig(Bill_Mech bill, IntVec3 billGiverPos) : base(bill, billGiverPos)
		{
			ModLister.CheckBiotech("BillWithFormingCycleDialog");
		}

		// Token: 0x0600AFCC RID: 45004 RVA: 0x003DF2CC File Offset: 0x003DD4CC
		protected override void DoIngredientConfigPane(float x, ref float y, float width, float height)
		{
			float y2 = y;
			base.DoIngredientConfigPane(x, ref y2, width, height - Dialog_MechBillConfig.formingInfoHeight);
			Building_MechGestator building_MechGestator = this.bill.billStack.billGiver as Building_MechGestator;
			if (building_MechGestator != null && building_MechGestator.ActiveBill == this.bill)
			{
				Rect rect = new Rect(x, y2, width, 9999f);
				Listing_Standard listing_Standard = new Listing_Standard();
				listing_Standard.Begin(rect);
				StringBuilder stringBuilder = new StringBuilder();
				listing_Standard.Label("FormerIngredients".Translate() + ":", -1f, null);
				building_MechGestator.ActiveBill.AppendCurrentIngredientCount(stringBuilder);
				listing_Standard.Label(stringBuilder.ToString(), -1f, null);
				Bill_Mech bill_Mech = (Bill_Mech)this.bill;
				listing_Standard.Label("GestationCyclesCompleted".Translate() + ": " + bill_Mech.GestationCyclesCompleted.ToString() + " / " + bill_Mech.recipe.gestationCycles.ToString(), -1f, null);
				listing_Standard.Gap(12f);
				listing_Standard.End();
				Dialog_MechBillConfig.formingInfoHeight = listing_Standard.CurHeight;
			}
		}

		// Token: 0x04006570 RID: 25968
		private static float formingInfoHeight;
	}
}
