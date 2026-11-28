using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023F1 RID: 9201
	public class WITab_Caravan_Items : WITab
	{
		// Token: 0x0600E5F4 RID: 58868 RVA: 0x00557C42 File Offset: 0x00555E42
		public WITab_Caravan_Items()
		{
			this.labelKey = "TabCaravanItems";
		}

		// Token: 0x0600E5F5 RID: 58869 RVA: 0x00557C60 File Offset: 0x00555E60
		protected override void FillTab()
		{
			this.CheckCreateSorters();
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y);
			if (Widgets.ButtonText(new Rect(rect.x + 10f, rect.y + 10f, 200f, 27f), "AssignDrugPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_AssignCaravanDrugPolicies(base.SelCaravan));
			}
			rect.yMin += 37f;
			Widgets.BeginGroup(rect.ContractedBy(10f));
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheItems();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheItems();
			});
			Widgets.EndGroup();
			rect.yMin += 25f;
			Widgets.BeginGroup(rect);
			this.CheckCacheItems();
			CaravanItemsTabUtility.DoRows(rect.size, this.cachedItems, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight);
			Widgets.EndGroup();
		}

		// Token: 0x0600E5F6 RID: 58870 RVA: 0x00557D93 File Offset: 0x00555F93
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.CheckCacheItems();
			this.size = CaravanItemsTabUtility.GetSize(this.cachedItems, this.PaneTopY, true);
		}

		// Token: 0x0600E5F7 RID: 58871 RVA: 0x00557DBC File Offset: 0x00555FBC
		private void CheckCacheItems()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			if (list.Count != this.cachedItemsCount)
			{
				this.CacheItems();
				return;
			}
			int num = base.SelCaravan.GetHashCode();
			for (int i = 0; i < list.Count; i++)
			{
				num = Gen.HashCombineInt(num, list[i].GetHashCode());
			}
			if (num != this.cachedItemsHash)
			{
				this.CacheItems();
			}
		}

		// Token: 0x0600E5F8 RID: 58872 RVA: 0x00557E2C File Offset: 0x0055602C
		private void CacheItems()
		{
			this.CheckCreateSorters();
			this.cachedItems.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			int seed = 0;
			for (int i = 0; i < list.Count; i++)
			{
				TransferableImmutable transferableImmutable = TransferableUtility.TransferableMatching<TransferableImmutable>(list[i], this.cachedItems, TransferAsOneMode.Normal);
				if (transferableImmutable == null)
				{
					transferableImmutable = new TransferableImmutable();
					this.cachedItems.Add(transferableImmutable);
				}
				transferableImmutable.things.Add(list[i]);
				seed = Gen.HashCombineInt(seed, list[i].GetHashCode());
			}
			this.cachedItems = this.cachedItems.OrderBy((TransferableImmutable tr) => tr, this.sorter1.Comparer).ThenBy((TransferableImmutable tr) => tr, this.sorter2.Comparer).ThenBy((TransferableImmutable tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ToList<TransferableImmutable>();
			this.cachedItemsCount = list.Count;
			this.cachedItemsHash = seed;
		}

		// Token: 0x0600E5F9 RID: 58873 RVA: 0x00557F5D File Offset: 0x0055615D
		private void CheckCreateSorters()
		{
			if (this.sorter1 == null)
			{
				this.sorter1 = TransferableSorterDefOf.Category;
			}
			if (this.sorter2 == null)
			{
				this.sorter2 = TransferableSorterDefOf.MarketValue;
			}
		}

		// Token: 0x0400922D RID: 37421
		private Vector2 scrollPosition;

		// Token: 0x0400922E RID: 37422
		private float scrollViewHeight;

		// Token: 0x0400922F RID: 37423
		private TransferableSorterDef sorter1;

		// Token: 0x04009230 RID: 37424
		private TransferableSorterDef sorter2;

		// Token: 0x04009231 RID: 37425
		private List<TransferableImmutable> cachedItems = new List<TransferableImmutable>();

		// Token: 0x04009232 RID: 37426
		private int cachedItemsHash;

		// Token: 0x04009233 RID: 37427
		private int cachedItemsCount;

		// Token: 0x04009234 RID: 37428
		private const float SortersSpace = 25f;

		// Token: 0x04009235 RID: 37429
		private const float AssignDrugPoliciesButtonHeight = 27f;
	}
}
