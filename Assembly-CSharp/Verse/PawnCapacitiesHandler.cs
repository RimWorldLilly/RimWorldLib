using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004CF RID: 1231
	public class PawnCapacitiesHandler
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002256 RID: 8790 RVA: 0x000CBAD4 File Offset: 0x000C9CD4
		public bool CanBeAwake
		{
			get
			{
				return (this.pawn.RaceProps.alwaysAwake || this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.3f) && !this.pawn.IsDeactivated();
			}
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x000CBB0A File Offset: 0x000C9D0A
		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000CBB19 File Offset: 0x000C9D19
		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x000CBB24 File Offset: 0x000C9D24
		public float GetLevel(PawnCapacityDef capacity)
		{
			if (this.pawn.health.Dead)
			{
				return 0f;
			}
			if (this.cachedCapacityLevels == null)
			{
				this.Notify_CapacityLevelsDirty();
			}
			PawnCapacitiesHandler.CacheElement cacheElement = this.cachedCapacityLevels[capacity];
			if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Caching)
			{
				Log.Error(string.Format("Detected infinite stat recursion when evaluating {0}", capacity));
				return 0f;
			}
			if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Uncached)
			{
				cacheElement.status = PawnCapacitiesHandler.CacheStatus.Caching;
				try
				{
					cacheElement.value = PawnCapacityUtility.CalculateCapacityLevel(this.pawn.health.hediffSet, capacity, null, false);
				}
				finally
				{
					cacheElement.status = PawnCapacitiesHandler.CacheStatus.Cached;
				}
			}
			return cacheElement.value;
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x000CBBD0 File Offset: 0x000C9DD0
		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x000CBBE4 File Offset: 0x000C9DE4
		public void Notify_CapacityLevelsDirty()
		{
			if (this.cachedCapacityLevels == null)
			{
				this.cachedCapacityLevels = new DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement>();
			}
			for (int i = 0; i < this.cachedCapacityLevels.Count; i++)
			{
				this.cachedCapacityLevels[i].status = PawnCapacitiesHandler.CacheStatus.Uncached;
			}
		}

		// Token: 0x040019C8 RID: 6600
		private Pawn pawn;

		// Token: 0x040019C9 RID: 6601
		private DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement> cachedCapacityLevels;

		// Token: 0x02002701 RID: 9985
		private enum CacheStatus
		{
			// Token: 0x04009FD8 RID: 40920
			Uncached,
			// Token: 0x04009FD9 RID: 40921
			Caching,
			// Token: 0x04009FDA RID: 40922
			Cached
		}

		// Token: 0x02002702 RID: 9986
		private class CacheElement
		{
			// Token: 0x04009FDB RID: 40923
			public PawnCapacitiesHandler.CacheStatus status;

			// Token: 0x04009FDC RID: 40924
			public float value;
		}
	}
}
