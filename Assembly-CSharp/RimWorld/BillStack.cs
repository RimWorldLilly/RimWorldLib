using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000998 RID: 2456
	public class BillStack : IExposable
	{
		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06004A20 RID: 18976 RVA: 0x001CCCDD File Offset: 0x001CAEDD
		public List<Bill> Bills
		{
			get
			{
				return this.bills;
			}
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x001CCCE5 File Offset: 0x001CAEE5
		public IEnumerator<Bill> GetEnumerator()
		{
			return this.bills.GetEnumerator();
		}

		// Token: 0x17000D92 RID: 3474
		public Bill this[int index]
		{
			get
			{
				return this.bills[index];
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06004A23 RID: 18979 RVA: 0x001CCD05 File Offset: 0x001CAF05
		public int Count
		{
			get
			{
				return this.bills.Count;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06004A24 RID: 18980 RVA: 0x001CCD14 File Offset: 0x001CAF14
		public Bill FirstShouldDoNow
		{
			get
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.bills[i].ShouldDoNow())
					{
						return this.bills[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06004A25 RID: 18981 RVA: 0x001CCD54 File Offset: 0x001CAF54
		public bool AnyShouldDoNow
		{
			get
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.bills[i].ShouldDoNow())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x001CCD88 File Offset: 0x001CAF88
		public BillStack(IBillGiver giver)
		{
			this.billGiver = giver;
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x001CCDA2 File Offset: 0x001CAFA2
		public void AddBill(Bill bill)
		{
			bill.billStack = this;
			this.bills.Add(bill);
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x001CCDB7 File Offset: 0x001CAFB7
		public void Delete(Bill bill)
		{
			bill.deleted = true;
			this.bills.Remove(bill);
			this.billGiver.Notify_BillDeleted(bill);
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x001CCDD9 File Offset: 0x001CAFD9
		public void Clear()
		{
			this.bills.Clear();
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x001CCDE8 File Offset: 0x001CAFE8
		public void Reorder(Bill bill, int offset)
		{
			int num = this.bills.IndexOf(bill);
			num += offset;
			if (num >= 0)
			{
				this.bills.Remove(bill);
				this.bills.Insert(num, bill);
			}
		}

		// Token: 0x06004A2B RID: 18987 RVA: 0x001CCE24 File Offset: 0x001CB024
		public void RemoveIncompletableBills()
		{
			for (int i = this.bills.Count - 1; i >= 0; i--)
			{
				if (!this.bills[i].CompletableEver)
				{
					Bill bill = this.bills[i];
					this.bills.Remove(bill);
					this.billGiver.Notify_BillDeleted(bill);
				}
			}
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x001CCE82 File Offset: 0x001CB082
		public int IndexOf(Bill bill)
		{
			return this.bills.IndexOf(bill);
		}

		// Token: 0x06004A2D RID: 18989 RVA: 0x001CCE90 File Offset: 0x001CB090
		public void ExposeData()
		{
			Scribe_Collections.Look<Bill>(ref this.bills, "bills", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.bills.RemoveAll((Bill x) => x == null) != 0)
				{
					Log.Error("Some bills were null after loading.");
				}
				if (this.bills.RemoveAll((Bill x) => x.recipe == null) != 0)
				{
					Log.Error("Some bills had null recipe after loading.");
				}
				for (int i = 0; i < this.bills.Count; i++)
				{
					this.bills[i].billStack = this;
				}
			}
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x001CCF54 File Offset: 0x001CB154
		public Bill DoListing(Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
		{
			Bill result = null;
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Small;
			if (this.Count < 15)
			{
				Rect rect2 = new Rect(0f, 0f, 150f, 29f);
				if (Widgets.ButtonText(rect2, "AddBill".Translate(), true, true, true, null))
				{
					Find.WindowStack.Add(new FloatMenu(recipeOptionsMaker()));
				}
				UIHighlighter.HighlightOpportunity(rect2, "AddBill");
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 35f, rect.width, rect.height - 35f);
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, viewHeight);
			Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
			float num = 0f;
			for (int i = 0; i < this.Count; i++)
			{
				Bill bill = this.bills[i];
				Rect rect3 = bill.DoInterface(0f, num, viewRect.width, i);
				if (!bill.DeletedOrDereferenced && Mouse.IsOver(rect3))
				{
					result = bill;
				}
				num += rect3.height + 6f;
			}
			if (Event.current.type == EventType.layout)
			{
				viewHeight = num + 60f;
			}
			Widgets.EndScrollView();
			Widgets.EndGroup();
			return result;
		}

		// Token: 0x04002F75 RID: 12149
		[Unsaved(false)]
		public IBillGiver billGiver;

		// Token: 0x04002F76 RID: 12150
		private List<Bill> bills = new List<Bill>();

		// Token: 0x04002F77 RID: 12151
		public const int MaxCount = 15;

		// Token: 0x04002F78 RID: 12152
		private const float TopAreaHeight = 35f;

		// Token: 0x04002F79 RID: 12153
		private const float BillInterfaceSpacing = 6f;

		// Token: 0x04002F7A RID: 12154
		private const float ExtraViewHeight = 60f;
	}
}
