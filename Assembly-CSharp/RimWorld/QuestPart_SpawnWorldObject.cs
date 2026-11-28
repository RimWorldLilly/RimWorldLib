using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FC RID: 4348
	public class QuestPart_SpawnWorldObject : QuestPart
	{
		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06006C7E RID: 27774 RVA: 0x00260C7E File Offset: 0x0025EE7E
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_SpawnWorldObject.<get_QuestLookTargets>d__5 <get_QuestLookTargets>d__ = new QuestPart_SpawnWorldObject.<get_QuestLookTargets>d__5(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x06006C7F RID: 27775 RVA: 0x00260C90 File Offset: 0x0025EE90
		public override bool IncreasesPopulation
		{
			get
			{
				Site site = this.worldObject as Site;
				return site != null && site.IncreasesPopulation;
			}
		}

		// Token: 0x06006C80 RID: 27776 RVA: 0x00260CB8 File Offset: 0x0025EEB8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && !this.spawned)
			{
				PlanetTile planetTile = this.worldObject.Tile;
				if (!planetTile.Valid)
				{
					if (!TileFinder.TryFindNewSiteTile(out planetTile, 7, 27, false, null, 0.5f, true, TileFinderMode.Near, false, false, null, null))
					{
						planetTile = PlanetTile.Invalid;
					}
				}
				else if (Find.WorldObjects.AnyWorldObjectAt(planetTile))
				{
					if (!TileFinder.TryFindPassableTileWithTraversalDistance(planetTile, 1, 50, out planetTile, (PlanetTile x) => !Find.WorldObjects.AnyWorldObjectAt(x), false, TileFinderMode.Near, false, false))
					{
						planetTile = PlanetTile.Invalid;
					}
				}
				if (planetTile.Valid)
				{
					this.worldObject.Tile = planetTile;
					Find.WorldObjects.Add(this.worldObject);
					this.spawned = true;
				}
			}
		}

		// Token: 0x06006C81 RID: 27777 RVA: 0x00260D94 File Offset: 0x0025EF94
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			Site site = this.worldObject as Site;
			if (site != null)
			{
				for (int i = 0; i < site.parts.Count; i++)
				{
					if (site.parts[i].things != null)
					{
						for (int j = 0; j < site.parts[i].things.Count; j++)
						{
							if (site.parts[i].things[j].def == ThingDefOf.PsychicAmplifier)
							{
								Find.History.Notify_PsylinkAvailable();
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06006C82 RID: 27778 RVA: 0x00260E2E File Offset: 0x0025F02E
		public override void Cleanup()
		{
			if (!this.spawned)
			{
				this.worldObject.Destroy();
			}
		}

		// Token: 0x06006C83 RID: 27779 RVA: 0x00260E44 File Offset: 0x0025F044
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && this.spawned && this.worldObject != null && (this.worldObject.Destroyed || !this.worldObject.Spawned))
			{
				this.worldObject = null;
			}
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.spawned, "spawned", false, false);
			Scribe_Collections.Look<ThingDef>(ref this.defsToExcludeFromHyperlinks, "defsToExcludeFromHyperlinks", LookMode.Def, Array.Empty<object>());
			if (this.spawned)
			{
				Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
				return;
			}
			Scribe_Deep.Look<WorldObject>(ref this.worldObject, "worldObject", Array.Empty<object>());
		}

		// Token: 0x06006C84 RID: 27780 RVA: 0x00260EFC File Offset: 0x0025F0FC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			PlanetTile tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, null, 0.5f, true, TileFinderMode.Near, false, false, null, null))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null, null);
			}
		}

		// Token: 0x04004512 RID: 17682
		public string inSignal;

		// Token: 0x04004513 RID: 17683
		public WorldObject worldObject;

		// Token: 0x04004514 RID: 17684
		public List<ThingDef> defsToExcludeFromHyperlinks;

		// Token: 0x04004515 RID: 17685
		private bool spawned;
	}
}
