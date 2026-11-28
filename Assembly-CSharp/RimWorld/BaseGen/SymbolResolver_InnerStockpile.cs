using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200212C RID: 8492
	public class SymbolResolver_InnerStockpile : SymbolResolver
	{
		// Token: 0x0600D4C8 RID: 54472 RVA: 0x004EC71C File Offset: 0x004EA91C
		public override void Resolve(ResolveParams rp)
		{
			CellRect rect;
			if (rp.innerStockpileSize != null)
			{
				if (!this.TryFindPerfectPlaceThenBest(rp.rect, rp.innerStockpileSize.Value, out rect))
				{
					return;
				}
			}
			else if (rp.stockpileConcreteContents != null)
			{
				int num = Mathf.CeilToInt(Mathf.Sqrt((float)rp.stockpileConcreteContents.Count));
				int num2;
				if (!this.TryFindRandomInnerRect(rp.rect, num, out rect, num * num, out num2))
				{
					rect = rp.rect;
				}
			}
			else if (!this.TryFindPerfectPlaceThenBest(rp.rect, 3, out rect))
			{
				return;
			}
			ResolveParams resolveParams = rp;
			resolveParams.rect = rect;
			BaseGen.symbolStack.Push("stockpile", resolveParams, null);
		}

		// Token: 0x0600D4C9 RID: 54473 RVA: 0x004EC7C0 File Offset: 0x004EA9C0
		private bool TryFindPerfectPlaceThenBest(CellRect outerRect, int size, out CellRect rect)
		{
			int num;
			if (!this.TryFindRandomInnerRect(outerRect, size, out rect, size * size, out num))
			{
				if (num == 0)
				{
					return false;
				}
				int num2;
				if (!this.TryFindRandomInnerRect(outerRect, size, out rect, num, out num2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D4CA RID: 54474 RVA: 0x004EC7F4 File Offset: 0x004EA9F4
		private bool TryFindRandomInnerRect(CellRect outerRect, int size, out CellRect rect, int minValidCells, out int maxValidCellsFound)
		{
			Map map = BaseGen.globalSettings.map;
			size = Mathf.Min(size, Mathf.Min(outerRect.Width, outerRect.Height));
			int maxValidCellsFoundLocal = 0;
			bool result = outerRect.TryFindRandomInnerRect(new IntVec2(size, size), out rect, delegate(CellRect x)
			{
				int num = 0;
				foreach (IntVec3 c in x)
				{
					if (c.Standable(map) && c.GetFirstItem(map) == null && c.GetFirstBuilding(map) == null)
					{
						num++;
					}
				}
				maxValidCellsFoundLocal = Mathf.Max(maxValidCellsFoundLocal, num);
				return num >= minValidCells;
			});
			maxValidCellsFound = maxValidCellsFoundLocal;
			return result;
		}

		// Token: 0x040086F9 RID: 34553
		private const int DefaultSize = 3;
	}
}
