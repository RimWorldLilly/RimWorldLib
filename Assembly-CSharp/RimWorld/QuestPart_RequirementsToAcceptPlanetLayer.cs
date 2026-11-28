using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E9 RID: 4329
	public class QuestPart_RequirementsToAcceptPlanetLayer : QuestPart_RequirementsToAccept
	{
		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x06006C28 RID: 27688 RVA: 0x0025F808 File Offset: 0x0025DA08
		public override bool ShowInRequirementBox
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06006C29 RID: 27689 RVA: 0x0025F80C File Offset: 0x0025DA0C
		public Map TargetMap
		{
			get
			{
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					if (this.quest.IsParentSuitableForQuest(this.mapParent))
					{
						return this.mapParent.Map;
					}
					this.mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
					if (this.mapParent != null)
					{
						return this.mapParent.Map;
					}
				}
				Pawn pawn = this.mapPawn;
				if (pawn == null)
				{
					return null;
				}
				return pawn.MapHeld;
			}
		}

		// Token: 0x06006C2A RID: 27690 RVA: 0x0025F884 File Offset: 0x0025DA84
		public override AcceptanceReport CanAccept()
		{
			if (this.TargetMap == null || !this.TargetMap.Tile.Valid)
			{
				if (this.canBeSpace)
				{
					return true;
				}
				foreach (Map map in Current.Game.PlayerHomeMaps)
				{
					if (map.Tile.Valid && !map.Tile.LayerDef.isSpace)
					{
						return true;
					}
				}
				return "QuestNotSpace".Translate().CapitalizeFirst().Resolve();
			}
			else
			{
				PlanetLayerDef layerDef = this.TargetMap.Tile.LayerDef;
				if (!this.canBeSpace && layerDef.isSpace)
				{
					return QuestPart_RequirementsToAcceptPlanetLayer.GetReport(layerDef);
				}
				if (!this.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !this.layerWhitelist.Contains(layerDef))
				{
					return QuestPart_RequirementsToAcceptPlanetLayer.GetReport(layerDef);
				}
				if (!this.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && this.layerBlacklist.Contains(layerDef))
				{
					return QuestPart_RequirementsToAcceptPlanetLayer.GetReport(layerDef);
				}
				return true;
			}
		}

		// Token: 0x06006C2B RID: 27691 RVA: 0x0025F9CC File Offset: 0x0025DBCC
		private static AcceptanceReport GetReport(PlanetLayerDef def)
		{
			return "QuestRequiredLayer".Translate(def.gerundLabel.Named("GERUND"), def.label.Named("LAYER")).CapitalizeFirst().Resolve();
		}

		// Token: 0x06006C2C RID: 27692 RVA: 0x0025FA18 File Offset: 0x0025DC18
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Pawn>(ref this.mapPawn, "mapPawn", false);
			Scribe_Values.Look<bool>(ref this.canBeSpace, "canBeSpace", false, false);
			Scribe_Collections.Look<PlanetLayerDef>(ref this.layerWhitelist, "layerWhitelist", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<PlanetLayerDef>(ref this.layerBlacklist, "layerBlacklist", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x040044DE RID: 17630
		public MapParent mapParent;

		// Token: 0x040044DF RID: 17631
		public Pawn mapPawn;

		// Token: 0x040044E0 RID: 17632
		public bool canBeSpace;

		// Token: 0x040044E1 RID: 17633
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x040044E2 RID: 17634
		public List<PlanetLayerDef> layerBlacklist;
	}
}
