using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D61 RID: 7521
	public class ITab_Bills : ITab
	{
		// Token: 0x1700240D RID: 9229
		// (get) Token: 0x0600C2D9 RID: 49881 RVA: 0x0047055E File Offset: 0x0046E75E
		protected Building_WorkTable SelTable
		{
			get
			{
				return (Building_WorkTable)base.SelThing;
			}
		}

		// Token: 0x0600C2DA RID: 49882 RVA: 0x0047056B File Offset: 0x0046E76B
		public ITab_Bills()
		{
			this.size = ITab_Bills.WinSize;
			this.labelKey = "TabBills";
			this.tutorTag = "Bills";
		}

		// Token: 0x0600C2DB RID: 49883 RVA: 0x004705A0 File Offset: 0x0046E7A0
		protected override void FillTab()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.BillsTab, KnowledgeAmount.FrameDisplayed);
			Rect rect = new Rect(ITab_Bills.WinSize.x - ITab_Bills.PasteX, ITab_Bills.PasteY, ITab_Bills.PasteSize, ITab_Bills.PasteSize);
			if (BillUtility.Clipboard != null)
			{
				if (!this.SelTable.def.AllRecipes.Contains(BillUtility.Clipboard.recipe) || !BillUtility.Clipboard.recipe.AvailableNow || !BillUtility.Clipboard.recipe.AvailableOnNow(this.SelTable, null))
				{
					GUI.color = Color.gray;
					Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f, 1f);
					GUI.color = Color.white;
					if (Mouse.IsOver(rect))
					{
						TooltipHandler.TipRegion(rect, "ClipboardBillNotAvailableHere".Translate() + ": " + BillUtility.Clipboard.LabelCap);
					}
				}
				else if (this.SelTable.billStack.Count >= 15)
				{
					GUI.color = Color.gray;
					Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f, 1f);
					GUI.color = Color.white;
					if (Mouse.IsOver(rect))
					{
						TooltipHandler.TipRegion(rect, "PasteBillTip".Translate() + " (" + "PasteBillTip_LimitReached".Translate() + "): " + BillUtility.Clipboard.LabelCap);
					}
				}
				else
				{
					if (Widgets.ButtonImageFitted(rect, TexButton.Paste, Color.white))
					{
						Bill bill = BillUtility.Clipboard.Clone();
						bill.InitializeAfterClone();
						this.SelTable.billStack.AddBill(bill);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
					if (Mouse.IsOver(rect))
					{
						TooltipHandler.TipRegion(rect, "PasteBillTip".Translate() + ": " + BillUtility.Clipboard.LabelCap);
					}
				}
			}
			Rect rect2 = new Rect(0f, 0f, ITab_Bills.WinSize.x, ITab_Bills.WinSize.y).ContractedBy(10f);
			this.mouseoverBill = this.SelTable.billStack.DoListing(rect2, new Func<List<FloatMenuOption>>(this.<FillTab>g__OptionsMaker|10_0), ref this.scrollPosition, ref this.viewHeight);
		}

		// Token: 0x0600C2DC RID: 49884 RVA: 0x004707FC File Offset: 0x0046E9FC
		public override void TabUpdate()
		{
			if (this.mouseoverBill != null)
			{
				this.mouseoverBill.TryDrawIngredientSearchRadiusOnMap(this.SelTable.Position);
				this.mouseoverBill = null;
			}
		}

		// Token: 0x0600C2DE RID: 49886 RVA: 0x00470858 File Offset: 0x0046EA58
		[CompilerGenerated]
		private List<FloatMenuOption> <FillTab>g__OptionsMaker|10_0()
		{
			ITab_Bills.<>c__DisplayClass10_0 CS$<>8__locals1;
			CS$<>8__locals1.opts = new List<FloatMenuOption>();
			ITab_Bills.<>c__DisplayClass10_1 CS$<>8__locals2 = new ITab_Bills.<>c__DisplayClass10_1();
			CS$<>8__locals2.<>4__this = this;
			CS$<>8__locals2.i = 0;
			while (CS$<>8__locals2.i < this.SelTable.def.AllRecipes.Count)
			{
				ITab_Bills.<>c__DisplayClass10_2 CS$<>8__locals3 = new ITab_Bills.<>c__DisplayClass10_2();
				CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals2;
				if (this.SelTable.def.AllRecipes[CS$<>8__locals3.CS$<>8__locals1.i].AvailableNow && this.SelTable.def.AllRecipes[CS$<>8__locals3.CS$<>8__locals1.i].AvailableOnNow(this.SelTable, null))
				{
					CS$<>8__locals3.recipe = this.SelTable.def.AllRecipes[CS$<>8__locals3.CS$<>8__locals1.i];
					CS$<>8__locals3.<FillTab>g__Add|1(null, ref CS$<>8__locals1);
					foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
					{
						foreach (Precept_Building precept_Building in ideo.cachedPossibleBuildings)
						{
							if (precept_Building.ThingDef == CS$<>8__locals3.recipe.ProducedThingDef)
							{
								CS$<>8__locals3.<FillTab>g__Add|1(precept_Building, ref CS$<>8__locals1);
							}
						}
					}
				}
				int i = CS$<>8__locals2.i;
				CS$<>8__locals2.i = i + 1;
			}
			if (!CS$<>8__locals1.opts.Any<FloatMenuOption>())
			{
				CS$<>8__locals1.opts.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			return CS$<>8__locals1.opts;
		}

		// Token: 0x04006F60 RID: 28512
		private float viewHeight = 1000f;

		// Token: 0x04006F61 RID: 28513
		private Vector2 scrollPosition;

		// Token: 0x04006F62 RID: 28514
		private Bill mouseoverBill;

		// Token: 0x04006F63 RID: 28515
		private static readonly Vector2 WinSize = new Vector2(420f, 480f);

		// Token: 0x04006F64 RID: 28516
		[TweakValue("Interface", 0f, 128f)]
		private static float PasteX = 48f;

		// Token: 0x04006F65 RID: 28517
		[TweakValue("Interface", 0f, 128f)]
		private static float PasteY = 3f;

		// Token: 0x04006F66 RID: 28518
		[TweakValue("Interface", 0f, 32f)]
		private static float PasteSize = 24f;
	}
}
