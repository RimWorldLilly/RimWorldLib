using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001777 RID: 6007
	public class SitePartWorker_MechCluster : SitePartWorker
	{
		// Token: 0x06009582 RID: 38274 RVA: 0x00365EEB File Offset: 0x003640EB
		public override bool IsAvailable()
		{
			return base.IsAvailable() && Faction.OfMechanoids != null;
		}

		// Token: 0x06009583 RID: 38275 RVA: 0x00365EFF File Offset: 0x003640FF
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x06009584 RID: 38276 RVA: 0x00365F17 File Offset: 0x00364117
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, 750f);
			return sitePartParams;
		}

		// Token: 0x04005A17 RID: 23063
		public const float MinPoints = 750f;
	}
}
