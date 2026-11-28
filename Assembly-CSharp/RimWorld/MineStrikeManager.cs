using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001354 RID: 4948
	public class MineStrikeManager : IExposable
	{
		// Token: 0x06007A6D RID: 31341 RVA: 0x002BA00A File Offset: 0x002B820A
		public void ExposeData()
		{
			Scribe_Collections.Look<StrikeRecord>(ref this.strikeRecords, "strikeRecords", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06007A6E RID: 31342 RVA: 0x002BA024 File Offset: 0x002B8224
		public void CheckStruckOre(IntVec3 justMinedPos, ThingDef justMinedDef, Thing miner)
		{
			if (miner.Faction != Faction.OfPlayer)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = justMinedPos + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(miner.Map))
				{
					Building edifice = intVec.GetEdifice(miner.Map);
					if (edifice != null && edifice.def != justMinedDef && MineStrikeManager.MineableIsValuable(edifice.def) && !this.AlreadyVisibleNearby(intVec, miner.Map, edifice.def) && !this.RecentlyStruck(intVec, edifice.def))
					{
						StrikeRecord item = default(StrikeRecord);
						item.cell = intVec;
						item.def = edifice.def;
						item.ticksGame = Find.TickManager.TicksGame;
						this.strikeRecords.Add(item);
						Messages.Message("StruckMineable".Translate(edifice.def.label), new LookTargets(edifice.Position, miner.Map), MessageTypeDefOf.PositiveEvent, true);
						TaleRecorder.RecordTale(TaleDefOf.StruckMineable, new object[]
						{
							miner,
							edifice
						});
					}
				}
			}
		}

		// Token: 0x06007A6F RID: 31343 RVA: 0x002BA15C File Offset: 0x002B835C
		public bool AlreadyVisibleNearby(IntVec3 center, Map map, ThingDef mineableDef)
		{
			CellRect cellRect = CellRect.CenteredOn(center, 1);
			for (int i = 1; i < MineStrikeManager.RadialVisibleCells; i++)
			{
				IntVec3 c = center + GenRadial.RadialPattern[i];
				if (c.InBounds(map) && !c.Fogged(map) && !cellRect.Contains(c))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def == mineableDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007A70 RID: 31344 RVA: 0x002BA1C8 File Offset: 0x002B83C8
		private bool RecentlyStruck(IntVec3 cell, ThingDef def)
		{
			for (int i = this.strikeRecords.Count - 1; i >= 0; i--)
			{
				StrikeRecord strikeRecord = this.strikeRecords[i];
				if (strikeRecord.Expired)
				{
					this.strikeRecords.RemoveAt(i);
				}
				else if (this.strikeRecords[i].def == def)
				{
					strikeRecord = this.strikeRecords[i];
					if (strikeRecord.cell.InHorDistOf(cell, 12f))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007A71 RID: 31345 RVA: 0x002BA24C File Offset: 0x002B844C
		public static bool MineableIsValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 10f;
		}

		// Token: 0x06007A72 RID: 31346 RVA: 0x002BA29C File Offset: 0x002B849C
		public static bool MineableIsVeryValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 100f;
		}

		// Token: 0x06007A73 RID: 31347 RVA: 0x002BA2EC File Offset: 0x002B84EC
		public string DebugStrikeRecords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (StrikeRecord strikeRecord in this.strikeRecords)
			{
				stringBuilder.AppendLine(strikeRecord.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04004D62 RID: 19810
		private List<StrikeRecord> strikeRecords = new List<StrikeRecord>();

		// Token: 0x04004D63 RID: 19811
		private const int RecentStrikeIgnoreRadius = 12;

		// Token: 0x04004D64 RID: 19812
		private static readonly int RadialVisibleCells = GenRadial.NumCellsInRadius(5.9f);
	}
}
