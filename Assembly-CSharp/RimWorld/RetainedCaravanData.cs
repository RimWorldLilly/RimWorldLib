using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001373 RID: 4979
	public class RetainedCaravanData : IExposable
	{
		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x06007B79 RID: 31609 RVA: 0x002BF5BF File Offset: 0x002BD7BF
		public bool HasDestinationTile
		{
			get
			{
				return this.destinationTile.Valid;
			}
		}

		// Token: 0x06007B7A RID: 31610 RVA: 0x002BF5CC File Offset: 0x002BD7CC
		public RetainedCaravanData(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007B7B RID: 31611 RVA: 0x002BF5F4 File Offset: 0x002BD7F4
		public void ExposeData()
		{
			Scribe_Values.Look<PlanetTile>(ref this.nextTile, "nextTile", PlanetTile.Invalid, false);
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Values.Look<bool>(ref this.shouldPassStoryState, "shouldPassStoryState", false, false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeftPct, "nextTileCostLeftPct", -1f, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
		}

		// Token: 0x06007B7C RID: 31612 RVA: 0x002BF680 File Offset: 0x002BD880
		public void Notify_GeneratedTempIncidentMapFor(Caravan caravan)
		{
			if (!this.map.Parent.def.isTempIncidentMapOwner)
			{
				return;
			}
			this.Set(caravan);
		}

		// Token: 0x06007B7D RID: 31613 RVA: 0x002BF6A4 File Offset: 0x002BD8A4
		public void Notify_CaravanFormed(Caravan caravan)
		{
			if (this.shouldPassStoryState)
			{
				this.shouldPassStoryState = false;
				this.map.StoryState.CopyTo(caravan.StoryState);
			}
			if (this.nextTile.Valid && this.nextTile != caravan.Tile && caravan.CanReach(this.nextTile))
			{
				caravan.pather.StartPath(this.nextTile, null, true, true);
				caravan.pather.nextTileCostLeft = caravan.pather.nextTileCostTotal * this.nextTileCostLeftPct;
				caravan.pather.Paused = this.paused;
				caravan.tweener.ResetTweenedPosToRoot();
			}
			if (this.HasDestinationTile && this.destinationTile != caravan.Tile)
			{
				caravan.pather.StartPath(this.destinationTile, this.arrivalAction, true, true);
				this.destinationTile = PlanetTile.Invalid;
				this.arrivalAction = null;
			}
		}

		// Token: 0x06007B7E RID: 31614 RVA: 0x002BF79C File Offset: 0x002BD99C
		private void Set(Caravan caravan)
		{
			caravan.StoryState.CopyTo(this.map.StoryState);
			this.shouldPassStoryState = true;
			if (caravan.pather.Moving)
			{
				this.nextTile = caravan.pather.nextTile;
				this.nextTileCostLeftPct = caravan.pather.nextTileCostLeft / caravan.pather.nextTileCostTotal;
				this.paused = caravan.pather.Paused;
				this.destinationTile = caravan.pather.Destination;
				this.arrivalAction = caravan.pather.ArrivalAction;
				return;
			}
			this.nextTile = PlanetTile.Invalid;
			this.nextTileCostLeftPct = 0f;
			this.paused = false;
			this.destinationTile = PlanetTile.Invalid;
			this.arrivalAction = null;
		}

		// Token: 0x04004DEA RID: 19946
		private Map map;

		// Token: 0x04004DEB RID: 19947
		public PlanetTile nextTile = PlanetTile.Invalid;

		// Token: 0x04004DEC RID: 19948
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x04004DED RID: 19949
		private bool shouldPassStoryState;

		// Token: 0x04004DEE RID: 19950
		private float nextTileCostLeftPct;

		// Token: 0x04004DEF RID: 19951
		private bool paused;

		// Token: 0x04004DF0 RID: 19952
		private CaravanArrivalAction arrivalAction;

		// Token: 0x04004DF1 RID: 19953
		private int backCompatTile;
	}
}
