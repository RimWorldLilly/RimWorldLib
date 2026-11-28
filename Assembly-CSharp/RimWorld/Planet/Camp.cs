using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002370 RID: 9072
	public class Camp : MapParent
	{
		// Token: 0x1700279C RID: 10140
		// (get) Token: 0x0600E12E RID: 57646 RVA: 0x00539CEB File Offset: 0x00537EEB
		public override bool CanReformFoggedEnemies
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700279D RID: 10141
		// (get) Token: 0x0600E12F RID: 57647 RVA: 0x00539CF0 File Offset: 0x00537EF0
		public override Material Material
		{
			get
			{
				if (this.cachedMat != null)
				{
					return this.cachedMat;
				}
				Faction faction = base.Faction;
				Color color = (faction != null) ? faction.Color : Color.white;
				this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, 3550);
				return this.cachedMat;
			}
		}

		// Token: 0x0600E130 RID: 57648 RVA: 0x00539D50 File Offset: 0x00537F50
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Camp.<GetGizmos>d__6 <GetGizmos>d__ = new Camp.<GetGizmos>d__6(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600E131 RID: 57649 RVA: 0x00539D60 File Offset: 0x00537F60
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
			if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
			{
				return false;
			}
			alsoRemoveWorldObject = true;
			return true;
		}

		// Token: 0x0600E132 RID: 57650 RVA: 0x00539D8C File Offset: 0x00537F8C
		public override void Notify_MyMapRemoved(Map map)
		{
			base.Notify_MyMapRemoved(map);
			if (ModsConfig.OdysseyActive && map.TileInfo.Landmark != null)
			{
				WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedLandmark);
				worldObject.Tile = base.Tile;
				worldObject.SetFaction(base.Faction);
				Find.WorldObjects.Add(worldObject);
				return;
			}
			WorldObject worldObject2 = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedCamp);
			worldObject2.Tile = base.Tile;
			worldObject2.SetFaction(base.Faction);
			worldObject2.GetComponent<TimeoutComp>().StartTimeout(1800000);
			Find.WorldObjects.Add(worldObject2);
		}

		// Token: 0x04009036 RID: 36918
		private Material cachedMat;

		// Token: 0x04009037 RID: 36919
		private static readonly StringBuilder sb = new StringBuilder();
	}
}
