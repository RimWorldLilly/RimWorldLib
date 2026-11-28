using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A8 RID: 4776
	public abstract class GenStep_LargeRuins : GenStep_BaseRuins
	{
		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x06007661 RID: 30305
		protected abstract IntRange RuinsMinMaxRange { get; }

		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x06007662 RID: 30306 RVA: 0x0029AC8F File Offset: 0x00298E8F
		protected virtual IntVec2 MinSize { get; } = new IntVec2(40, 40);

		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x06007663 RID: 30307 RVA: 0x0029AC97 File Offset: 0x00298E97
		protected virtual IntVec2 MaxSize { get; } = new IntVec2(60, 60);

		// Token: 0x06007664 RID: 30308 RVA: 0x0029ACA0 File Offset: 0x00298EA0
		protected override IEnumerable<CellRect> GetRectOrder(IEnumerable<CellRect> rects, Map map)
		{
			return (from r in rects
			orderby (float)r.Area - (map.Center - r.CenterCell).Magnitude / 2f descending
			select r).Take(this.RuinsMinMaxRange.TrueMax);
		}

		// Token: 0x06007665 RID: 30309 RVA: 0x0029ACE0 File Offset: 0x00298EE0
		protected override IEnumerable<CellRect> GetRects(CellRect area, Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			List<CellRect> largestClearRects = MapGenUtility.GetLargestClearRects(map, this.MinSize, this.MaxSize, this.UseUsedRects, -1f, 0.7f, this.MinAffordance, true);
			if (largestClearRects.Count < this.RuinsMinMaxRange.TrueMin)
			{
				largestClearRects = MapGenUtility.GetLargestClearRects(map, this.MinSize, this.MaxSize, this.UseUsedRects, -1f, 1f, null, true);
				if (largestClearRects.Count < this.RuinsMinMaxRange.TrueMin)
				{
					largestClearRects = MapGenUtility.GetLargestClearRects(map, this.MinSize, this.MaxSize, this.UseUsedRects, -1f, 1f, null, false);
				}
				while ((float)largestClearRects.Count > this.RuinsMinMaxRange.Average)
				{
					largestClearRects.RemoveAt(largestClearRects.Count - 1);
				}
			}
			return largestClearRects;
		}
	}
}
