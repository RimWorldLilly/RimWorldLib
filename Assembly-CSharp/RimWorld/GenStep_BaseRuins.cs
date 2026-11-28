using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129A RID: 4762
	public abstract class GenStep_BaseRuins : GenStep
	{
		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x06007619 RID: 30233 RVA: 0x00298CCC File Offset: 0x00296ECC
		protected virtual int RegionSize
		{
			get
			{
				return 45;
			}
		}

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x0600761A RID: 30234 RVA: 0x00298CD0 File Offset: 0x00296ED0
		protected virtual FloatRange DefaultMapFillPercentRange
		{
			get
			{
				return new FloatRange(0.15f, 0.3f);
			}
		}

		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x0600761B RID: 30235 RVA: 0x00298CE1 File Offset: 0x00296EE1
		protected virtual FloatRange MergeRange
		{
			get
			{
				return new FloatRange(0.1f, 0.35f);
			}
		}

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x0600761C RID: 30236 RVA: 0x00298CF2 File Offset: 0x00296EF2
		protected virtual int MoveRangeLimit
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x0600761D RID: 30237 RVA: 0x00298CF5 File Offset: 0x00296EF5
		protected virtual int ContractLimit
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x0600761E RID: 30238 RVA: 0x00298CF8 File Offset: 0x00296EF8
		protected virtual int MinRegionSize
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x17001401 RID: 5121
		// (get) Token: 0x0600761F RID: 30239 RVA: 0x00298CFC File Offset: 0x00296EFC
		protected virtual bool UseUsedRects
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x06007620 RID: 30240 RVA: 0x00298CFF File Offset: 0x00296EFF
		protected virtual bool AvoidWaterRoads
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x06007621 RID: 30241 RVA: 0x00298D02 File Offset: 0x00296F02
		protected virtual TerrainAffordanceDef MinAffordance
		{
			get
			{
				return TerrainAffordanceDefOf.Heavy;
			}
		}

		// Token: 0x17001404 RID: 5124
		// (get) Token: 0x06007622 RID: 30242
		protected abstract LayoutDef LayoutDef { get; }

		// Token: 0x17001405 RID: 5125
		// (get) Token: 0x06007623 RID: 30243
		protected abstract Faction Faction { get; }

		// Token: 0x06007624 RID: 30244 RVA: 0x00298D09 File Offset: 0x00296F09
		protected virtual CellRect GetBounds(Map map)
		{
			return map.BoundsRect(16);
		}

		// Token: 0x06007625 RID: 30245 RVA: 0x00298D13 File Offset: 0x00296F13
		public override void Generate(Map map, GenStepParams parms)
		{
			this.GenerateRuins(map, parms, this.DefaultMapFillPercentRange);
		}

		// Token: 0x06007626 RID: 30246 RVA: 0x00298D24 File Offset: 0x00296F24
		public virtual void GenerateRuins(Map map, GenStepParams parms, FloatRange mapFillPercentRange)
		{
			this.structureSketches.Clear();
			CellRect bounds = this.GetBounds(map);
			List<CellRect> list = this.GetRects(bounds, map).ToList<CellRect>();
			if (!list.Any<CellRect>())
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 2);
				defaultInterpolatedStringHandler.AppendFormatted(base.GetType().Name);
				defaultInterpolatedStringHandler.AppendLiteral(" did not find any valid rects to generate ruins in for bounds ");
				defaultInterpolatedStringHandler.AppendFormatted<CellRect>(bounds);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			float num = (float)bounds.Area * mapFillPercentRange.RandomInRange;
			foreach (CellRect rect in this.GetRectOrder(list, map))
			{
				if (num <= 0f)
				{
					break;
				}
				this.GenerateAndSpawn(rect, map, parms, this.LayoutDef);
				num -= (float)rect.Area;
			}
		}

		// Token: 0x06007627 RID: 30247 RVA: 0x00298E1C File Offset: 0x0029701C
		protected virtual IEnumerable<CellRect> GetRects(CellRect area, Map map)
		{
			List<CellRect> list = MapGenUtility.SubdivideRectIntoChunks(area, this.RegionSize, 1);
			MapGenUtility.TryFixInvalidRects(list, map, this.MinRegionSize, this.UseUsedRects, this.MinAffordance, this.AvoidWaterRoads);
			this.MergeRandomRects(list, map);
			this.OffsetContractRandomRects(list);
			MapGenUtility.RemoveInvalidRects(list, map, this.UseUsedRects, this.MinAffordance, this.AvoidWaterRoads);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (!this.IsValidRect(list[i], map))
				{
					list.RemoveAt(i);
				}
			}
			return list;
		}

		// Token: 0x06007628 RID: 30248 RVA: 0x00298EA9 File Offset: 0x002970A9
		protected virtual IEnumerable<CellRect> GetRectOrder(IEnumerable<CellRect> rects, Map map)
		{
			return rects.InRandomOrder(null);
		}

		// Token: 0x06007629 RID: 30249 RVA: 0x00298EB4 File Offset: 0x002970B4
		protected virtual bool IsValidRect(CellRect rect, Map map)
		{
			List<CellRect> list;
			if (MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) && list.Any(new Predicate<CellRect>(rect.Overlaps)))
			{
				return false;
			}
			using (IEnumerator<IntVec3> enumerator = rect.Cells.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!MapGenUtility.IsCellValid(enumerator.Current, map, this.UseUsedRects, this.MinAffordance, this.AvoidWaterRoads))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600762A RID: 30250 RVA: 0x00298F44 File Offset: 0x00297144
		private void OffsetContractRandomRects(List<CellRect> rects)
		{
			for (int i = 0; i < rects.Count; i++)
			{
				CellRect value = rects[i];
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = value.Width - this.MinRegionSize;
				int num6 = value.Height - this.MinRegionSize;
				if (num5 > this.MinRegionSize)
				{
					num5 -= (num = Rand.Range(0, Mathf.Min(num5, this.MoveRangeLimit)));
					num3 = Rand.Range(0, Mathf.Min(num5, this.ContractLimit));
				}
				if (num6 > this.MinRegionSize)
				{
					num6 -= (num2 = Rand.Range(0, Mathf.Min(num6, this.MoveRangeLimit)));
					num4 = Rand.Range(0, Mathf.Min(num6, this.ContractLimit));
				}
				value = value.MovedBy(num, num2).ContractedBy(num + num3, num2 + num4);
				rects[i] = value;
			}
		}

		// Token: 0x0600762B RID: 30251 RVA: 0x00299030 File Offset: 0x00297230
		private void MergeRandomRects(List<CellRect> rects, Map map)
		{
			int num = Mathf.RoundToInt(this.MergeRange.RandomInRange * (float)rects.Count);
			List<CellRect> merged = new List<CellRect>();
			int num2 = 0;
			Func<CellRect, bool> <>9__0;
			while (num2 < num && rects.Count > 2)
			{
				Func<CellRect, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((CellRect x) => !merged.Contains(x)));
				}
				List<CellRect> list = rects.Where(predicate).ToList<CellRect>();
				if (!list.Any<CellRect>())
				{
					break;
				}
				foreach (CellRect cellRect in list.InRandomOrder(null))
				{
					bool flag = false;
					for (int i = 0; i < rects.Count; i++)
					{
						CellRect cellRect2 = rects[i];
						if (!(cellRect2 == cellRect) && !merged.Contains(cellRect2) && cellRect2.InlineWith(cellRect))
						{
							CellRect cellRect3 = cellRect.Encapsulate(cellRect2);
							bool flag2 = true;
							if (this.IsValidRect(cellRect3, map))
							{
								foreach (CellRect lhs in rects)
								{
									if (!(lhs == cellRect) && !(lhs == cellRect2) && lhs.Overlaps(cellRect3))
									{
										flag2 = false;
										break;
									}
								}
								if (flag2)
								{
									rects.Remove(cellRect2);
									merged.Add(cellRect3);
									rects[rects.IndexOf(cellRect)] = cellRect3;
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
				num2++;
			}
		}

		// Token: 0x0600762C RID: 30252 RVA: 0x00299224 File Offset: 0x00297424
		protected virtual StructureGenParams GetStructureGenParams(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			return new StructureGenParams
			{
				size = rect.Size
			};
		}

		// Token: 0x0600762D RID: 30253 RVA: 0x00299238 File Offset: 0x00297438
		protected virtual LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			StructureGenParams structureGenParams = this.GetStructureGenParams(rect, map, parms, layoutDef);
			LayoutWorker worker = layoutDef.Worker;
			LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(structureGenParams);
			LayoutStructureSketch result;
			using (new RandBlock(layoutStructureSketch.id))
			{
				map.layoutStructureSketches.Add(layoutStructureSketch);
				this.structureSketches.Add(layoutStructureSketch);
				float? threatPoints = null;
				if (parms.sitePart != null)
				{
					threatPoints = new float?(parms.sitePart.parms.points);
				}
				if (threatPoints == null)
				{
					Site site = map.Parent as Site;
					if (site != null)
					{
						threatPoints = new float?(site.ActualThreatPoints);
					}
				}
				worker.Spawn(layoutStructureSketch, map, rect.Min, threatPoints, null, true, false, this.Faction);
				if (this.UseUsedRects)
				{
					MapGenerator.UsedRects.Add(rect.ExpandedBy(1));
				}
				result = layoutStructureSketch;
			}
			return result;
		}

		// Token: 0x04004A0B RID: 18955
		public readonly List<LayoutStructureSketch> structureSketches = new List<LayoutStructureSketch>();
	}
}
