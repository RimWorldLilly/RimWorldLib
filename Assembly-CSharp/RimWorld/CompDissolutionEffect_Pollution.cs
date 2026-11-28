using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019E2 RID: 6626
	public class CompDissolutionEffect_Pollution : CompDissolutionEffect
	{
		// Token: 0x17001D32 RID: 7474
		// (get) Token: 0x0600A6DE RID: 42718 RVA: 0x003B78F4 File Offset: 0x003B5AF4
		public CompProperties_DissolutionEffectPollution Props
		{
			get
			{
				return (CompProperties_DissolutionEffectPollution)this.props;
			}
		}

		// Token: 0x0600A6DF RID: 42719 RVA: 0x003B7904 File Offset: 0x003B5B04
		public override void DoDissolutionEffectMap(int amount)
		{
			PollutionUtility.GrowPollutionAt(this.parent.PositionHeld, this.parent.MapHeld, amount * this.Props.cellsToPollutePerDissolution, null, false, null);
			FleckMaker.Static(this.parent.PositionHeld, this.parent.MapHeld, FleckDefOf.Fleck_WastePackDissolutionSource, 1f);
			SoundDefOf.WastepackDissolution.PlayOneShot(this.parent);
		}

		// Token: 0x0600A6E0 RID: 42720 RVA: 0x003B7978 File Offset: 0x003B5B78
		public override void DoDissolutionEffectWorld(int amount, PlanetTile tileId)
		{
			if (!tileId.Valid)
			{
				return;
			}
			Tile tile = Find.World.grid[tileId];
			if (tile == null)
			{
				return;
			}
			float num = this.Props.tilePollutionPerDissolution * (float)amount;
			if (tile.WaterCovered)
			{
				num *= this.Props.waterTilePollutionFactor;
			}
			CompDissolutionEffect_Pollution.WorldPollutionEvent item = new CompDissolutionEffect_Pollution.WorldPollutionEvent
			{
				tile = tileId,
				amount = num
			};
			CompDissolutionEffect_Pollution.pendingWorldEvents.Add(item);
		}

		// Token: 0x0600A6E1 RID: 42721 RVA: 0x003B79F0 File Offset: 0x003B5BF0
		public static void WorldUpdate()
		{
			if (CompDissolutionEffect_Pollution.pendingWorldEvents.Count <= 0)
			{
				return;
			}
			foreach (IGrouping<PlanetTile, CompDissolutionEffect_Pollution.WorldPollutionEvent> grouping in from e in CompDissolutionEffect_Pollution.pendingWorldEvents
			group e by e.tile)
			{
				WorldPollutionUtility.PolluteWorldAtTile(grouping.Key, grouping.Sum((CompDissolutionEffect_Pollution.WorldPollutionEvent e) => e.amount));
			}
			CompDissolutionEffect_Pollution.pendingWorldEvents.Clear();
		}

		// Token: 0x0400624F RID: 25167
		private static List<CompDissolutionEffect_Pollution.WorldPollutionEvent> pendingWorldEvents = new List<CompDissolutionEffect_Pollution.WorldPollutionEvent>();

		// Token: 0x0200372A RID: 14122
		private struct WorldPollutionEvent
		{
			// Token: 0x0400DC4D RID: 56397
			public PlanetTile tile;

			// Token: 0x0400DC4E RID: 56398
			public float amount;
		}
	}
}
