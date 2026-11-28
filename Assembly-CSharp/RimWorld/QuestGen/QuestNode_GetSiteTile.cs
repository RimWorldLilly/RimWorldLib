using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021E1 RID: 8673
	public class QuestNode_GetSiteTile : QuestNode
	{
		// Token: 0x0600D7B6 RID: 55222 RVA: 0x00500A88 File Offset: 0x004FEC88
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile var;
			if (!this.TryFindTile(slate, out var))
			{
				return false;
			}
			if (this.clampRangeBySiteParts.GetValue(slate).GetValueOrDefault() && this.sitePartDefs.GetValue(slate) == null)
			{
				return false;
			}
			slate.Set<PlanetTile>(this.storeAs.GetValue(slate), var, false);
			return true;
		}

		// Token: 0x0600D7B7 RID: 55223 RVA: 0x00500AE0 File Offset: 0x004FECE0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int num;
			PlanetTile var;
			if (!slate.TryGet<int>(this.storeAs.GetValue(slate), out num, false) && this.TryFindTile(QuestGen.slate, out var))
			{
				QuestGen.slate.Set<PlanetTile>(this.storeAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x0600D7B8 RID: 55224 RVA: 0x00500B34 File Offset: 0x004FED34
		private bool TryFindTile(Slate slate, out PlanetTile tile)
		{
			bool value = this.canSelectSpace.GetValue(slate);
			Map map = slate.Get<Map>("map", null, false) ?? (value ? Find.RandomPlayerHomeMap : Find.RandomSurfacePlayerHomeMap);
			PlanetTile nearTile = (map != null) ? map.Tile : PlanetTile.Invalid;
			if (nearTile.Valid && nearTile.LayerDef.isSpace && !value)
			{
				nearTile = PlanetTile.Invalid;
			}
			int num = int.MaxValue;
			bool? value2 = this.clampRangeBySiteParts.GetValue(slate);
			if (value2 != null && value2.Value)
			{
				foreach (SitePartDef sitePartDef in this.sitePartDefs.GetValue(slate))
				{
					if (sitePartDef.conditionCauserDef != null)
					{
						num = Mathf.Min(num, sitePartDef.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange);
					}
				}
			}
			IntRange intRange;
			if (!slate.TryGet<IntRange>("siteDistRange", out intRange, false))
			{
				intRange = new IntRange(7, Mathf.Min(27, num));
			}
			else if (num != 2147483647)
			{
				intRange = new IntRange(Mathf.Min(intRange.min, num), Mathf.Min(intRange.max, num));
			}
			TileFinderMode tileFinderMode = this.preferCloserTiles.GetValue(slate) ? TileFinderMode.Near : TileFinderMode.Random;
			float num2 = (!ModsConfig.OdysseyActive) ? 0f : this.selectLandmarkChance.GetValue(slate).GetValueOrDefault(0.5f);
			return TileFinder.TryFindNewSiteTile(out tile, nearTile, intRange.min, intRange.max, this.allowCaravans.GetValue(slate), this.allowedLandmarks.GetValue(slate), num2, this.canSelectComboLandmarks.GetValue(slate), tileFinderMode, false, value, null, null);
		}

		// Token: 0x04008911 RID: 35089
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008912 RID: 35090
		public SlateRef<bool> preferCloserTiles;

		// Token: 0x04008913 RID: 35091
		public SlateRef<bool> allowCaravans;

		// Token: 0x04008914 RID: 35092
		public SlateRef<bool> canSelectSpace;

		// Token: 0x04008915 RID: 35093
		public SlateRef<bool?> clampRangeBySiteParts;

		// Token: 0x04008916 RID: 35094
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x04008917 RID: 35095
		public SlateRef<List<LandmarkDef>> allowedLandmarks;

		// Token: 0x04008918 RID: 35096
		public SlateRef<float?> selectLandmarkChance;

		// Token: 0x04008919 RID: 35097
		public SlateRef<bool> canSelectComboLandmarks;
	}
}
