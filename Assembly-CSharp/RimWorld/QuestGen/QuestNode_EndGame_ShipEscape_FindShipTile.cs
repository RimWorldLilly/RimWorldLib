using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002247 RID: 8775
	public class QuestNode_EndGame_ShipEscape_FindShipTile : QuestNode
	{
		// Token: 0x0600D90D RID: 55565 RVA: 0x005065DD File Offset: 0x005047DD
		private bool TryFindRootTile(out PlanetTile tile)
		{
			return TileFinder.TryFindRandomPlayerTile(out tile, false, delegate(PlanetTile x)
			{
				PlanetTile planetTile;
				return this.TryFindDestinationTileActual(x, 180, out planetTile);
			}, false, null);
		}

		// Token: 0x0600D90E RID: 55566 RVA: 0x005065F4 File Offset: 0x005047F4
		private bool TryFindDestinationTile(PlanetTile rootTile, out PlanetTile tile)
		{
			int num = 800;
			for (int i = 0; i < 1000; i++)
			{
				num = (int)((float)num * Rand.Range(0.5f, 0.75f));
				if (num <= 180)
				{
					num = 180;
				}
				if (this.TryFindDestinationTileActual(rootTile, num, out tile))
				{
					return true;
				}
				if (num <= 180)
				{
					return false;
				}
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x0600D90F RID: 55567 RVA: 0x0050665C File Offset: 0x0050485C
		private bool TryFindDestinationTileActual(PlanetTile rootTile, int minDist, out PlanetTile tile)
		{
			for (int i = 0; i < 2; i++)
			{
				bool canTraverseImpassable = i == 1;
				if (TileFinder.TryFindPassableTileWithTraversalDistance(rootTile, minDist, 800, out tile, (PlanetTile x) => !Find.WorldObjects.AnyWorldObjectAt(x) && Find.WorldGrid[x].PrimaryBiome.canBuildBase && Find.WorldGrid[x].PrimaryBiome.canAutoChoose, true, TileFinderMode.Near, canTraverseImpassable, false))
				{
					return true;
				}
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x0600D910 RID: 55568 RVA: 0x005066BC File Offset: 0x005048BC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			PlanetTile rootTile;
			this.TryFindRootTile(out rootTile);
			PlanetTile var;
			this.TryFindDestinationTile(rootTile, out var);
			slate.Set<PlanetTile>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x0600D911 RID: 55569 RVA: 0x005066F8 File Offset: 0x005048F8
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile rootTile;
			PlanetTile planetTile;
			return this.TryFindRootTile(out rootTile) && this.TryFindDestinationTile(rootTile, out planetTile);
		}

		// Token: 0x04008AB7 RID: 35511
		private const int MinTraversalDistance = 180;

		// Token: 0x04008AB8 RID: 35512
		private const int MaxTraversalDistance = 800;

		// Token: 0x04008AB9 RID: 35513
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
