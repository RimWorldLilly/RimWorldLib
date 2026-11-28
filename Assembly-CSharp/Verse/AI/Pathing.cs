using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008A5 RID: 2213
	public class Pathing : IDisposable
	{
		// Token: 0x0600446B RID: 17515 RVA: 0x001ACAD8 File Offset: 0x001AACD8
		public Pathing(Map map)
		{
			this.map = map;
			foreach (PathGridDef pathGridDef in DefDatabase<PathGridDef>.AllDefsListForReading)
			{
				if (!(pathGridDef.workerType == null))
				{
					PathGrid pathGrid = (PathGrid)Activator.CreateInstance(pathGridDef.workerType, new object[]
					{
						map,
						pathGridDef
					});
					PathingContext pathingContext = this.contexts[pathGridDef] = new PathingContext(map, pathGrid);
					if (pathGridDef == PathGridDefOf.Normal)
					{
						this.normal = pathingContext;
					}
					else if (pathGridDef == PathGridDefOf.FenceBlocked)
					{
						this.fenceBlocked = pathingContext;
					}
					else if (pathGridDef == PathGridDefOf.Flying)
					{
						this.flying = pathingContext;
					}
				}
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x0600446C RID: 17516 RVA: 0x001ACBB8 File Offset: 0x001AADB8
		public PathingContext Normal
		{
			get
			{
				return this.normal;
			}
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x0600446D RID: 17517 RVA: 0x001ACBC0 File Offset: 0x001AADC0
		public PathingContext FenceBlocked
		{
			get
			{
				return this.fenceBlocked;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x0600446E RID: 17518 RVA: 0x001ACBC8 File Offset: 0x001AADC8
		public PathingContext Flying
		{
			get
			{
				return this.flying;
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x0600446F RID: 17519 RVA: 0x001ACBD0 File Offset: 0x001AADD0
		public bool IncrementalDirtyingDisabled
		{
			get
			{
				return this.incrementalDirtyingDisabled;
			}
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x001ACBD8 File Offset: 0x001AADD8
		public PathingContext Get(PathGridDef def)
		{
			return this.contexts[def];
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x001ACBE6 File Offset: 0x001AADE6
		public PathingContext For(TraverseParms parms)
		{
			if (parms.fenceBlocked && !parms.canBashFences)
			{
				return this.fenceBlocked;
			}
			return this.normal;
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x001ACC05 File Offset: 0x001AAE05
		public PathingContext For(Pawn pawn)
		{
			if (pawn == null)
			{
				return this.normal;
			}
			return pawn.GetPathContext(this) ?? this.normal;
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x001ACC24 File Offset: 0x001AAE24
		public void RecalculateAllPerceivedPathCosts()
		{
			foreach (KeyValuePair<PathGridDef, PathingContext> keyValuePair in this.contexts)
			{
				PathGridDef pathGridDef;
				PathingContext pathingContext;
				keyValuePair.Deconstruct(out pathGridDef, out pathingContext);
				pathingContext.pathGrid.RecalculateAllPerceivedPathCosts();
			}
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x001ACC88 File Offset: 0x001AAE88
		public void RecalculatePerceivedPathCostUnderThing(Thing thing)
		{
			this.map.pathFinder.MapData.Notify_CellDelta(thing.OccupiedRect());
			ThingDef def = thing.def;
			if (def.pathCost == 0 && def.passability != Traversability.Impassable)
			{
				return;
			}
			if (thing.def.size == IntVec2.One)
			{
				this.RecalculatePerceivedPathCostAt(thing.Position);
				return;
			}
			CellRect cellRect = thing.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x001ACD34 File Offset: 0x001AAF34
		public void RecalculatePerceivedPathCostAt(IntVec3 c)
		{
			bool flag = false;
			foreach (KeyValuePair<PathGridDef, PathingContext> keyValuePair in this.contexts)
			{
				PathGridDef pathGridDef;
				PathingContext pathingContext;
				keyValuePair.Deconstruct(out pathGridDef, out pathingContext);
				pathingContext.pathGrid.RecalculatePerceivedPathCostAt(c, ref flag);
			}
			this.map.events.Notify_PathCostRecalculated(c);
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x001ACDB0 File Offset: 0x001AAFB0
		public Pathing.DisableDirtyingScope DisableIncrementalScope()
		{
			return new Pathing.DisableDirtyingScope(this);
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x001ACDB8 File Offset: 0x001AAFB8
		public void DisableIncrementalDirtying()
		{
			foreach (KeyValuePair<PathGridDef, PathingContext> keyValuePair in this.contexts)
			{
				PathGridDef pathGridDef;
				PathingContext pathingContext;
				keyValuePair.Deconstruct(out pathGridDef, out pathingContext);
				pathingContext.pathGrid.DisableIncrementalDirtying();
			}
			this.incrementalDirtyingDisabled = true;
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x001ACE24 File Offset: 0x001AB024
		public void ReEnableIncrementalDirtying()
		{
			foreach (KeyValuePair<PathGridDef, PathingContext> keyValuePair in this.contexts)
			{
				PathGridDef pathGridDef;
				PathingContext pathingContext;
				keyValuePair.Deconstruct(out pathGridDef, out pathingContext);
				pathingContext.pathGrid.ReEnableIncrementalDirtying();
			}
			this.incrementalDirtyingDisabled = false;
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x001ACE90 File Offset: 0x001AB090
		public void Dispose()
		{
			foreach (KeyValuePair<PathGridDef, PathingContext> keyValuePair in this.contexts)
			{
				PathGridDef pathGridDef;
				PathingContext pathingContext;
				keyValuePair.Deconstruct(out pathGridDef, out pathingContext);
				pathingContext.Dispose();
			}
		}

		// Token: 0x04002C4F RID: 11343
		private readonly Map map;

		// Token: 0x04002C50 RID: 11344
		private readonly Dictionary<PathGridDef, PathingContext> contexts = new Dictionary<PathGridDef, PathingContext>();

		// Token: 0x04002C51 RID: 11345
		private bool incrementalDirtyingDisabled;

		// Token: 0x04002C52 RID: 11346
		private readonly PathingContext normal;

		// Token: 0x04002C53 RID: 11347
		private readonly PathingContext fenceBlocked;

		// Token: 0x04002C54 RID: 11348
		private readonly PathingContext flying;

		// Token: 0x02002B97 RID: 11159
		public class DisableDirtyingScope : IDisposable
		{
			// Token: 0x06010AB2 RID: 68274 RVA: 0x005D6839 File Offset: 0x005D4A39
			public DisableDirtyingScope(Pathing pathing)
			{
				this.pathing = pathing;
				pathing.DisableIncrementalDirtying();
			}

			// Token: 0x06010AB3 RID: 68275 RVA: 0x005D684E File Offset: 0x005D4A4E
			public void Dispose()
			{
				this.pathing.ReEnableIncrementalDirtying();
			}

			// Token: 0x0400B217 RID: 45591
			private readonly Pathing pathing;
		}
	}
}
