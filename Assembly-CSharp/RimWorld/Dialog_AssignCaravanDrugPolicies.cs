using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C91 RID: 7313
	public class Dialog_AssignCaravanDrugPolicies : Window
	{
		// Token: 0x170021E2 RID: 8674
		// (get) Token: 0x0600BAAB RID: 47787 RVA: 0x0041DCB6 File Offset: 0x0041BEB6
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(550f, 500f);
			}
		}

		// Token: 0x0600BAAC RID: 47788 RVA: 0x0041DCC7 File Offset: 0x0041BEC7
		public Dialog_AssignCaravanDrugPolicies(Caravan caravan) : base(null)
		{
			this.caravan = caravan;
			this.doCloseButton = true;
		}

		// Token: 0x0600BAAD RID: 47789 RVA: 0x0041DCE0 File Offset: 0x0041BEE0
		public override void DoWindowContents(Rect rect)
		{
			rect.height -= Window.CloseButSize.y;
			float num = 0f;
			if (Widgets.ButtonText(new Rect(rect.width - 354f - 16f, num, 354f, 32f), "ManageDrugPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
			}
			num += 42f;
			Rect outRect = new Rect(0f, num, rect.width, rect.height - num);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, this.lastHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPos, viewRect, true);
			float num2 = 0f;
			foreach (Pawn pawn in this.caravan.pawns)
			{
				if (pawn.drugs != null && !pawn.DevelopmentalStage.Baby())
				{
					if (num2 + 30f >= this.scrollPos.y && num2 <= this.scrollPos.y + outRect.height)
					{
						this.DoRow(new Rect(0f, num2, viewRect.width, 30f), pawn);
					}
					num2 += 30f;
				}
			}
			this.lastHeight = num2;
			Widgets.EndScrollView();
		}

		// Token: 0x0600BAAE RID: 47790 RVA: 0x0041DE80 File Offset: 0x0041C080
		private void DoRow(Rect rect, Pawn pawn)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width - 354f, 30f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect2, pawn.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			DrugPolicyUIUtility.DoAssignDrugPolicyButtons(new Rect(rect.x + rect.width - 354f, rect.y, 354f, 30f), pawn);
		}

		// Token: 0x04006A18 RID: 27160
		private Caravan caravan;

		// Token: 0x04006A19 RID: 27161
		private Vector2 scrollPos;

		// Token: 0x04006A1A RID: 27162
		private float lastHeight;

		// Token: 0x04006A1B RID: 27163
		private const float RowHeight = 30f;

		// Token: 0x04006A1C RID: 27164
		private const float AssignDrugPolicyButtonsTotalWidth = 354f;

		// Token: 0x04006A1D RID: 27165
		private const int ManageDrugPoliciesButtonHeight = 32;
	}
}
