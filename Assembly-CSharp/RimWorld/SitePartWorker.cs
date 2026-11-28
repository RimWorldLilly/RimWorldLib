using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FE4 RID: 4068
	public class SitePartWorker
	{
		// Token: 0x060066E1 RID: 26337 RVA: 0x00247C24 File Offset: 0x00245E24
		public virtual void SitePartWorkerTick(SitePart sitePart)
		{
		}

		// Token: 0x060066E2 RID: 26338 RVA: 0x00247C26 File Offset: 0x00245E26
		public virtual void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("", part.def));
			outExtraDescriptionConstants.Add("sitePart", part.def.defName);
		}

		// Token: 0x060066E3 RID: 26339 RVA: 0x00247C55 File Offset: 0x00245E55
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x060066E4 RID: 26340 RVA: 0x00247C57 File Offset: 0x00245E57
		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}

		// Token: 0x060066E5 RID: 26341 RVA: 0x00247C5A File Offset: 0x00245E5A
		public virtual string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			preferredLetterDef = this.def.arrivedLetterDef;
			lookTargets = new LookTargets(map.Parent);
			return this.def.arrivedLetter;
		}

		// Token: 0x060066E6 RID: 26342 RVA: 0x00247C81 File Offset: 0x00245E81
		public virtual string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return this.def.label;
		}

		// Token: 0x060066E7 RID: 26343 RVA: 0x00247C8E File Offset: 0x00245E8E
		public virtual SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			return new SitePartParams
			{
				randomValue = Rand.Int,
				threatPoints = (this.def.wantsThreatPoints ? myThreatPoints : 0f)
			};
		}

		// Token: 0x060066E8 RID: 26344 RVA: 0x00247CBB File Offset: 0x00245EBB
		public virtual bool IncreasesPopulation(SitePartParams parms)
		{
			return this.def.increasesPopulation;
		}

		// Token: 0x060066E9 RID: 26345 RVA: 0x00247CC8 File Offset: 0x00245EC8
		public virtual void Init(Site site, SitePart sitePart)
		{
		}

		// Token: 0x060066EA RID: 26346 RVA: 0x00247CCA File Offset: 0x00245ECA
		public virtual bool IsAvailable()
		{
			return true;
		}

		// Token: 0x060066EB RID: 26347 RVA: 0x00247CCD File Offset: 0x00245ECD
		public virtual void PostDrawExtraSelectionOverlays(SitePart sitePart)
		{
		}

		// Token: 0x060066EC RID: 26348 RVA: 0x00247CD0 File Offset: 0x00245ED0
		public virtual void PostDestroy(SitePart sitePart)
		{
			if (this.def.leaveAbandonedSettlement)
			{
				if (sitePart.site.Tile.LayerDef != PlanetLayerDefOf.Surface)
				{
					return;
				}
				WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedSettlement);
				worldObject.Tile = sitePart.site.Tile;
				worldObject.SetFaction(Faction.OfPlayer);
				Find.WorldObjects.Add(worldObject);
			}
		}

		// Token: 0x060066ED RID: 26349 RVA: 0x00247D37 File Offset: 0x00245F37
		public virtual void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
		}

		// Token: 0x0400404C RID: 16460
		public SitePartDef def;
	}
}
