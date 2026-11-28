using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020006E3 RID: 1763
	public class RegionProcessorClosestThingReachable : RegionProcessorDelegateCache
	{
		// Token: 0x060037D8 RID: 14296 RVA: 0x00165828 File Offset: 0x00163A28
		public void SetParameters(TraverseParms traverseParams, float maxDistance, IntVec3 root, bool ignoreEntirelyForbiddenRegions, ThingRequest req, PathEndMode peMode, Func<Thing, float> priorityGetter, Predicate<Thing> validator, int minRegions, float closestDistSquared = 9999999f, int regionsSeenScan = 0, float bestPrio = -3.4028235E+38f, Thing closestThing = null, bool lookInHaulSources = false)
		{
			this.traverseParams = traverseParams;
			this.maxDistance = maxDistance;
			this.root = root;
			this.regionsSeenScan = regionsSeenScan;
			this.ignoreEntirelyForbiddenRegions = ignoreEntirelyForbiddenRegions;
			this.req = req;
			this.peMode = peMode;
			this.priorityGetter = priorityGetter;
			this.validator = validator;
			this.bestPrio = bestPrio;
			this.closestDistSquared = closestDistSquared;
			this.closestThing = closestThing;
			this.minRegions = minRegions;
			this.lookInHaulSources = lookInHaulSources;
			this.maxDistSquared = maxDistance * maxDistance;
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x001658AC File Offset: 0x00163AAC
		public void Clear()
		{
			this.SetParameters(default(TraverseParms), 0f, default(IntVec3), false, default(ThingRequest), PathEndMode.None, null, null, 0, 0f, 0, 0f, null, false);
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x001658F1 File Offset: 0x00163AF1
		protected override bool RegionEntryPredicate(Region from, Region to)
		{
			return to.Allows(this.traverseParams, false) && (this.maxDistance > 5000f || to.extentsClose.ClosestDistSquaredTo(this.root) < this.maxDistSquared);
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x0016592C File Offset: 0x00163B2C
		protected override bool RegionProcessor(Region reg)
		{
			if (RegionTraverser.ShouldCountRegion(reg))
			{
				this.regionsSeenScan++;
			}
			if (!reg.IsDoorway && !reg.Allows(this.traverseParams, true))
			{
				return false;
			}
			if (!this.ignoreEntirelyForbiddenRegions || !reg.IsForbiddenEntirely(this.traverseParams.pawn))
			{
				List<Thing> list = reg.ListerThings.ThingsMatching(this.req);
				for (int i = 0; i < list.Count; i++)
				{
					this.ProcessThing(reg, list[i]);
				}
				Pawn pawn = this.traverseParams.pawn;
				bool flag = pawn != null && pawn.IsColonist;
				if (this.lookInHaulSources)
				{
					foreach (Building building in reg.ListerThings.GetThingsOfType<Building>())
					{
						IHaulSource haulSource = building as IHaulSource;
						if (haulSource != null && (!flag || haulSource.HaulSourceEnabled))
						{
							foreach (Thing t in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
							{
								if (this.req.Accepts(t))
								{
									this.ProcessThing(reg, t);
								}
							}
						}
					}
				}
			}
			return this.regionsSeenScan >= this.minRegions && this.closestThing != null;
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x00165A9C File Offset: 0x00163C9C
		private void ProcessThing(Region reg, Thing t)
		{
			if (!ReachabilityWithinRegion.ThingFromRegionListerReachable(t.SpawnedParentOrMe, reg, this.peMode, this.traverseParams.pawn))
			{
				return;
			}
			Func<Thing, float> func = this.priorityGetter;
			float num = (func != null) ? func(t) : 0f;
			if (num < this.bestPrio)
			{
				return;
			}
			float num2 = (float)(t.PositionHeld - this.root).LengthHorizontalSquared;
			if ((num > this.bestPrio || num2 < this.closestDistSquared) && num2 < this.maxDistSquared && (this.validator == null || this.validator(t)))
			{
				this.closestThing = t;
				this.closestDistSquared = num2;
				this.bestPrio = num;
			}
		}

		// Token: 0x0400246B RID: 9323
		private TraverseParms traverseParams;

		// Token: 0x0400246C RID: 9324
		private float maxDistance;

		// Token: 0x0400246D RID: 9325
		private IntVec3 root;

		// Token: 0x0400246E RID: 9326
		public Thing closestThing;

		// Token: 0x0400246F RID: 9327
		public int regionsSeenScan;

		// Token: 0x04002470 RID: 9328
		private bool ignoreEntirelyForbiddenRegions;

		// Token: 0x04002471 RID: 9329
		private ThingRequest req;

		// Token: 0x04002472 RID: 9330
		private PathEndMode peMode;

		// Token: 0x04002473 RID: 9331
		private Func<Thing, float> priorityGetter;

		// Token: 0x04002474 RID: 9332
		private Predicate<Thing> validator;

		// Token: 0x04002475 RID: 9333
		private float bestPrio;

		// Token: 0x04002476 RID: 9334
		private float closestDistSquared;

		// Token: 0x04002477 RID: 9335
		private int minRegions;

		// Token: 0x04002478 RID: 9336
		private bool lookInHaulSources;

		// Token: 0x04002479 RID: 9337
		private float maxDistSquared;
	}
}
