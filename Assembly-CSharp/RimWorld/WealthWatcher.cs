using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A2 RID: 5026
	public class WealthWatcher
	{
		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x06007CA2 RID: 31906 RVA: 0x002C8818 File Offset: 0x002C6A18
		public int HealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.totalHealth;
			}
		}

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x06007CA3 RID: 31907 RVA: 0x002C8826 File Offset: 0x002C6A26
		public float WealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems + this.wealthBuildings + this.wealthPawns;
			}
		}

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x06007CA4 RID: 31908 RVA: 0x002C8842 File Offset: 0x002C6A42
		public float WealthItems
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems;
			}
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x06007CA5 RID: 31909 RVA: 0x002C8850 File Offset: 0x002C6A50
		public float WealthBuildings
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthBuildings;
			}
		}

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x06007CA6 RID: 31910 RVA: 0x002C885E File Offset: 0x002C6A5E
		public float WealthFloorsOnly
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthFloorsOnly;
			}
		}

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x06007CA7 RID: 31911 RVA: 0x002C886C File Offset: 0x002C6A6C
		public float WealthPawns
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthPawns;
			}
		}

		// Token: 0x06007CA8 RID: 31912 RVA: 0x002C887C File Offset: 0x002C6A7C
		public static void ResetStaticData()
		{
			int num = -1;
			List<TerrainDef> allDefsListForReading = DefDatabase<TerrainDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				num = Mathf.Max(num, (int)allDefsListForReading[i].index);
			}
			WealthWatcher.cachedTerrainMarketValue = new float[num + 1];
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				WealthWatcher.cachedTerrainMarketValue[(int)allDefsListForReading[j].index] = allDefsListForReading[j].GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
		}

		// Token: 0x06007CA9 RID: 31913 RVA: 0x002C88F7 File Offset: 0x002C6AF7
		public WealthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007CAA RID: 31914 RVA: 0x002C891C File Offset: 0x002C6B1C
		private void RecountIfNeeded()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if ((float)Find.TickManager.TicksGame - this.lastCountTick > 5000f)
			{
				this.ForceRecount(false);
			}
		}

		// Token: 0x06007CAB RID: 31915 RVA: 0x002C8948 File Offset: 0x002C6B48
		public void ForceRecount(bool allowDuringInit = false)
		{
			if (!allowDuringInit && Current.ProgramState != ProgramState.Playing)
			{
				Log.Error("WealthWatcher recount in game mode " + Current.ProgramState.ToString());
				return;
			}
			this.wealthItems = this.CalculateWealthItems();
			this.wealthBuildings = 0f;
			this.wealthPawns = 0f;
			this.wealthFloorsOnly = 0f;
			this.totalHealth = 0;
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.Faction == Faction.OfPlayer)
				{
					this.wealthBuildings += thing.GetStatValue(StatDefOf.MarketValueIgnoreHp, true, -1);
					this.totalHealth += thing.HitPoints;
				}
			}
			this.wealthFloorsOnly = this.CalculateWealthFloors();
			this.wealthBuildings += this.wealthFloorsOnly;
			foreach (Pawn pawn in this.map.mapPawns.PawnsInFaction(Faction.OfPlayer))
			{
				if (!pawn.IsQuestLodger())
				{
					float num = pawn.MarketValue;
					if (pawn.IsSlave)
					{
						num *= 0.75f;
					}
					this.wealthPawns += num;
					if (pawn.IsFreeColonist)
					{
						this.totalHealth += Mathf.RoundToInt(pawn.health.summaryHealth.SummaryHealthPercent * 100f);
					}
				}
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps)
			{
				if (pocketMapParent.HasMap && pocketMapParent.sourceMap == this.map)
				{
					if (Current.ProgramState == ProgramState.Playing)
					{
						pocketMapParent.Map.wealthWatcher.ForceRecount(false);
					}
					this.wealthItems += pocketMapParent.Map.wealthWatcher.wealthItems;
					this.wealthBuildings += pocketMapParent.Map.wealthWatcher.wealthBuildings;
					this.wealthPawns += pocketMapParent.Map.wealthWatcher.wealthPawns;
					this.wealthFloorsOnly += pocketMapParent.Map.wealthWatcher.wealthFloorsOnly;
					this.totalHealth += pocketMapParent.Map.wealthWatcher.totalHealth;
				}
			}
			this.lastCountTick = (float)Find.TickManager.TicksGame;
		}

		// Token: 0x06007CAC RID: 31916 RVA: 0x002C8C18 File Offset: 0x002C6E18
		public static float GetEquipmentApparelAndInventoryWealth(Pawn p)
		{
			float num = 0f;
			if (p.equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = p.equipment.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					num += allEquipmentListForReading[i].MarketValue * (float)allEquipmentListForReading[i].stackCount;
				}
			}
			if (p.apparel != null)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					num += wornApparel[j].MarketValue * (float)wornApparel[j].stackCount;
				}
			}
			if (p.inventory != null)
			{
				ThingOwner<Thing> innerContainer = p.inventory.innerContainer;
				for (int k = 0; k < innerContainer.Count; k++)
				{
					num += innerContainer[k].MarketValue * (float)innerContainer[k].stackCount;
				}
			}
			return num;
		}

		// Token: 0x06007CAD RID: 31917 RVA: 0x002C8D04 File Offset: 0x002C6F04
		private float CalculateWealthItems()
		{
			this.tmpThings.Clear();
			ThingOwnerUtility.GetAllThingsRecursively<Thing>(this.map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), this.tmpThings, false, new Predicate<IThingHolder>(WealthWatcher.WealthItemsFilter), true);
			float num = 0f;
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				if (this.tmpThings[i].SpawnedOrAnyParentSpawned && !this.tmpThings[i].PositionHeld.Fogged(this.map))
				{
					num += this.tmpThings[i].MarketValue * (float)this.tmpThings[i].stackCount;
				}
			}
			this.tmpThings.Clear();
			return num;
		}

		// Token: 0x06007CAE RID: 31918 RVA: 0x002C8DC4 File Offset: 0x002C6FC4
		public static bool WealthItemsFilter(IThingHolder x)
		{
			if (x is PassingShip || x is MapComponent)
			{
				return false;
			}
			Pawn pawn = x as Pawn;
			return (pawn == null || pawn.Faction == Faction.OfPlayer) && (pawn == null || !pawn.IsQuestLodger());
		}

		// Token: 0x06007CAF RID: 31919 RVA: 0x002C8E0C File Offset: 0x002C700C
		private float CalculateWealthFloors()
		{
			TerrainDef[] topGrid = this.map.terrainGrid.topGrid;
			TerrainDef[] foundationGrid = this.map.terrainGrid.foundationGrid;
			NativeBitArray fogGrid_Unsafe = this.map.fogGrid.FogGrid_Unsafe;
			IntVec3 size = this.map.Size;
			float num = 0f;
			if (!fogGrid_Unsafe.IsCreated)
			{
				return 0f;
			}
			int i = 0;
			int num2 = size.x * size.z;
			while (i < num2)
			{
				if (!fogGrid_Unsafe.IsSet(i) && topGrid[i] != null)
				{
					num += WealthWatcher.cachedTerrainMarketValue[(int)topGrid[i].index];
				}
				if (foundationGrid[i] != null)
				{
					num += WealthWatcher.cachedTerrainMarketValue[(int)foundationGrid[i].index];
				}
				i++;
			}
			return num;
		}

		// Token: 0x04004E72 RID: 20082
		private Map map;

		// Token: 0x04004E73 RID: 20083
		private float wealthItems;

		// Token: 0x04004E74 RID: 20084
		private float wealthBuildings;

		// Token: 0x04004E75 RID: 20085
		private float wealthPawns;

		// Token: 0x04004E76 RID: 20086
		private float wealthFloorsOnly;

		// Token: 0x04004E77 RID: 20087
		private int totalHealth;

		// Token: 0x04004E78 RID: 20088
		private float lastCountTick = -99999f;

		// Token: 0x04004E79 RID: 20089
		private static float[] cachedTerrainMarketValue;

		// Token: 0x04004E7A RID: 20090
		private const int MinCountInterval = 5000;

		// Token: 0x04004E7B RID: 20091
		public const float WealthFactor_Slave = 0.75f;

		// Token: 0x04004E7C RID: 20092
		private List<Thing> tmpThings = new List<Thing>();
	}
}
