using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001782 RID: 6018
	public abstract class SitePartWorker_WorkSite : SitePartWorker
	{
		// Token: 0x1700198F RID: 6543
		// (get) Token: 0x060095B4 RID: 38324
		public abstract IEnumerable<PreceptDef> DisallowedPrecepts { get; }

		// Token: 0x17001990 RID: 6544
		// (get) Token: 0x060095B5 RID: 38325
		public abstract PawnGroupKindDef WorkerGroupKind { get; }

		// Token: 0x060095B6 RID: 38326 RVA: 0x003668E1 File Offset: 0x00364AE1
		public virtual bool CanSpawnOn(PlanetTile tile)
		{
			return this.LootThings(tile).Any<SitePartWorker_WorkSite.CampLootThingStruct>();
		}

		// Token: 0x060095B7 RID: 38327 RVA: 0x003668F0 File Offset: 0x00364AF0
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			SitePartWorker_WorkSite.CampLootThingStruct campLootThingStruct = this.LootThings(site.Tile).RandomElementByWeight((SitePartWorker_WorkSite.CampLootThingStruct t) => t.weight);
			this.OnLootChosen(site, sitePart, campLootThingStruct);
			float x = SitePartWorker_WorkSite.PointsMarketValue.Evaluate(sitePart.parms.threatPoints);
			List<ThingDefCount> list = new List<ThingDefCount>();
			sitePart.things = new ThingOwner<Thing>(sitePart);
			sitePart.things.dontTickContents = true;
			List<ThingDefCount> list2 = new List<ThingDefCount>();
			float num = SitePartWorker_WorkSite.PointsMarketValue.Evaluate(x);
			if (campLootThingStruct.thing2 == null)
			{
				list2.Add(new ThingDefCount(campLootThingStruct.thing, Mathf.CeilToInt(num / campLootThingStruct.thing.BaseMarketValue)));
			}
			else
			{
				list2.Add(new ThingDefCount(campLootThingStruct.thing, Mathf.CeilToInt(num / 2f / campLootThingStruct.thing.BaseMarketValue)));
				list2.Add(new ThingDefCount(campLootThingStruct.thing2, Mathf.CeilToInt(num / 2f / campLootThingStruct.thing2.BaseMarketValue)));
			}
			foreach (ThingDefCount thingDefCount in list2)
			{
				int i = thingDefCount.Count;
				ThingDef thingDef = thingDefCount.ThingDef;
				while (i > 0)
				{
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = Mathf.Min(i, thing.def.stackLimit);
					list.Add(new ThingDefCount(thingDef, thing.stackCount));
					i -= thing.stackCount;
					sitePart.things.TryAdd(thing, true);
				}
			}
			sitePart.lootThings = list;
			sitePart.expectedEnemyCount = this.GetEnemiesCount(site, sitePart.parms);
		}

		// Token: 0x060095B8 RID: 38328 RVA: 0x00366ACC File Offset: 0x00364CCC
		protected virtual void OnLootChosen(Site site, SitePart sitePart, SitePartWorker_WorkSite.CampLootThingStruct loot)
		{
		}

		// Token: 0x060095B9 RID: 38329 RVA: 0x00366ACE File Offset: 0x00364CCE
		public virtual IEnumerable<SitePartWorker_WorkSite.CampLootThingStruct> LootThings(PlanetTile tile)
		{
			SitePartWorker_WorkSite.<LootThings>d__9 <LootThings>d__ = new SitePartWorker_WorkSite.<LootThings>d__9(-2);
			<LootThings>d__.<>4__this = this;
			return <LootThings>d__;
		}

		// Token: 0x060095BA RID: 38330 RVA: 0x00366ADE File Offset: 0x00364CDE
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x060095BB RID: 38331 RVA: 0x00366AF8 File Offset: 0x00364CF8
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int enemiesCount = this.GetEnemiesCount(part.site, part.parms);
			outExtraDescriptionRules.Add(new Rule_String("enemiesCount", enemiesCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("enemiesLabel", this.GetEnemiesLabel(part.site, enemiesCount)));
		}

		// Token: 0x060095BC RID: 38332 RVA: 0x00366B58 File Offset: 0x00364D58
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return (site.Label + ": " + "KnownSiteThreatEnemyCountAppend".Translate(this.GetEnemiesCount(site, sitePart.parms), "People".Translate())).TrimEndNewlines() + ("\n" + "Contains".Translate() + ": " + string.Join(", ", (from t in sitePart.lootThings
			select t.ThingDef).Distinct<ThingDef>().Select(delegate(ThingDef t)
			{
				int num = 0;
				foreach (ThingDefCount thingDefCount in sitePart.lootThings)
				{
					if (thingDefCount.ThingDef == t)
					{
						num += thingDefCount.Count;
					}
				}
				return t.LabelCap + " x" + num.ToString();
			})));
		}

		// Token: 0x060095BD RID: 38333 RVA: 0x00366C3D File Offset: 0x00364E3D
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Settlement, null));
			return sitePartParams;
		}

		// Token: 0x060095BE RID: 38334 RVA: 0x00366C6C File Offset: 0x00364E6C
		public override bool FactionCanOwn(Faction faction)
		{
			if (faction.Hidden || faction.temporary || !faction.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == this.WorkerGroupKind))
			{
				return false;
			}
			if (faction.ideos == null || faction.ideos.PrimaryIdeo == null)
			{
				return true;
			}
			Ideo ideology = faction.ideos.PrimaryIdeo;
			return !this.DisallowedPrecepts.Any((PreceptDef pDef) => ideology.PreceptsListForReading.Any((Precept p) => p.def == pDef));
		}

		// Token: 0x060095BF RID: 38335 RVA: 0x00366CF8 File Offset: 0x00364EF8
		protected int GetEnemiesCount(Site site, SitePartParams parms)
		{
			return GenStep_WorkSitePawns.GetEnemiesCount(site, parms, this.WorkerGroupKind);
		}

		// Token: 0x060095C0 RID: 38336 RVA: 0x00366D08 File Offset: 0x00364F08
		protected string GetEnemiesLabel(Site site, int enemiesCount)
		{
			if (site.Faction == null)
			{
				return (enemiesCount == 1) ? "Enemy".Translate() : "Enemies".Translate();
			}
			if (enemiesCount != 1)
			{
				return site.Faction.def.pawnsPlural;
			}
			return site.Faction.def.pawnSingular;
		}

		// Token: 0x04005A1C RID: 23068
		public static readonly SimpleCurve PointsMarketValue = new SimpleCurve
		{
			{
				new CurvePoint(100f, 200f),
				true
			},
			{
				new CurvePoint(250f, 450f),
				true
			},
			{
				new CurvePoint(800f, 2000f),
				true
			},
			{
				new CurvePoint(10000f, 5000f),
				true
			}
		};

		// Token: 0x0200355A RID: 13658
		public struct CampLootThingStruct
		{
			// Token: 0x0400D56E RID: 54638
			public ThingDef thing;

			// Token: 0x0400D56F RID: 54639
			public ThingDef thing2;

			// Token: 0x0400D570 RID: 54640
			public float weight;
		}
	}
}
