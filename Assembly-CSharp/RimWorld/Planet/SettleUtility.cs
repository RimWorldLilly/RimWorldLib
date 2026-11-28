using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023A3 RID: 9123
	[StaticConstructorOnStartup]
	public static class SettleUtility
	{
		// Token: 0x170027E4 RID: 10212
		// (get) Token: 0x0600E2FE RID: 58110 RVA: 0x00546E34 File Offset: 0x00545034
		public static bool PlayerSettlementsCountLimitReached
		{
			get
			{
				int num = 0;
				foreach (Map map in Find.Maps)
				{
					if (map.IsPlayerHome && map.Parent is Settlement)
					{
						num++;
					}
					else if (map.wasSpawnedViaGravShipLanding)
					{
						num++;
					}
				}
				return num >= Prefs.MaxNumberOfPlayerSettlements;
			}
		}

		// Token: 0x0600E2FF RID: 58111 RVA: 0x00546EB4 File Offset: 0x005450B4
		public static Settlement AddNewHome(PlanetTile tile, Faction faction)
		{
			AbandonedArchotechStructures abandonedArchotechStructures = Find.WorldObjects.WorldObjectAt<AbandonedArchotechStructures>(tile);
			Settlement settlement;
			if (abandonedArchotechStructures != null)
			{
				settlement = abandonedArchotechStructures.GenerateSettlementAndDestroy();
			}
			else
			{
				settlement = (Settlement)WorldObjectMaker.MakeWorldObject(tile.LayerDef.SettlementWorldObjectDef);
			}
			settlement.Tile = tile;
			settlement.SetFaction(faction);
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
			Find.WorldObjects.Add(settlement);
			if (faction == Faction.OfPlayer)
			{
				TaleRecorder.RecordTale(TaleDefOf.TileSettled, Array.Empty<object>()).customLabel = "NewSettlement".Translate();
				if (Find.IdeoManager != null)
				{
					Find.IdeoManager.lastResettledTick = GenTicks.TicksGame;
				}
			}
			return settlement;
		}

		// Token: 0x040090DD RID: 37085
		public static readonly Texture2D SettleCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Settle", true);

		// Token: 0x040090DE RID: 37086
		public static readonly Texture2D CreateCampCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CreateCamp", true);
	}
}
