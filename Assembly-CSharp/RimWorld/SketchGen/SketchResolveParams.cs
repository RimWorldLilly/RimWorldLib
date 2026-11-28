using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020B5 RID: 8373
	public struct SketchResolveParams
	{
		// Token: 0x0600D301 RID: 54017 RVA: 0x004D9F44 File Offset: 0x004D8144
		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			ResolveParamsUtility.SetCustom<T>(ref this.custom, name, obj, inherit);
		}

		// Token: 0x0600D302 RID: 54018 RVA: 0x004D9F54 File Offset: 0x004D8154
		public void RemoveCustom(string name)
		{
			ResolveParamsUtility.RemoveCustom(ref this.custom, name);
		}

		// Token: 0x0600D303 RID: 54019 RVA: 0x004D9F62 File Offset: 0x004D8162
		public bool TryGetCustom<T>(string name, out T obj)
		{
			return ResolveParamsUtility.TryGetCustom<T>(this.custom, name, out obj);
		}

		// Token: 0x0600D304 RID: 54020 RVA: 0x004D9F71 File Offset: 0x004D8171
		public T GetCustom<T>(string name)
		{
			return ResolveParamsUtility.GetCustom<T>(this.custom, name);
		}

		// Token: 0x0600D305 RID: 54021 RVA: 0x004D9F80 File Offset: 0x004D8180
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"sketch=",
				(this.sketch != null) ? this.sketch.ToString() : "null",
				", rect=",
				(this.rect != null) ? this.rect.ToString() : "null",
				", allowWood=",
				(this.allowWood != null) ? this.allowWood.ToString() : "null",
				", custom=",
				(this.custom != null) ? this.custom.Count.ToString() : "null",
				", symmetryOrigin=",
				(this.symmetryOrigin != null) ? this.symmetryOrigin.ToString() : "null",
				", symmetryVertical=",
				(this.symmetryVertical != null) ? this.symmetryVertical.ToString() : "null",
				", symmetryOriginIncluded=",
				(this.symmetryOriginIncluded != null) ? this.symmetryOriginIncluded.ToString() : "null",
				", symmetryClear=",
				(this.symmetryClear != null) ? this.symmetryClear.ToString() : "null",
				", connectedGroupsSameStuff=",
				(this.connectedGroupsSameStuff != null) ? this.connectedGroupsSameStuff.ToString() : "null",
				", assignRandomStuffTo=",
				(this.assignRandomStuffTo != null) ? this.assignRandomStuffTo.ToString() : "null",
				", cornerThing=",
				(this.cornerThing != null) ? this.cornerThing.ToString() : "null",
				", floorFillRoomsOnly=",
				(this.floorFillRoomsOnly != null) ? this.floorFillRoomsOnly.ToString() : "null",
				", singleFloorType=",
				(this.singleFloorType != null) ? this.singleFloorType.ToString() : "null",
				", onlyStoneFloors=",
				(this.onlyStoneFloors != null) ? this.onlyStoneFloors.ToString() : "null",
				", thingCentral=",
				(this.thingCentral != null) ? this.thingCentral.ToString() : "null",
				", wallEdgeThing=",
				(this.wallEdgeThing != null) ? this.wallEdgeThing.ToString() : "null",
				", monumentSize=",
				(this.monumentSize != null) ? this.monumentSize.ToString() : "null",
				", monumentOpen=",
				(this.monumentOpen != null) ? this.monumentOpen.ToString() : "null",
				", allowMonumentDoors=",
				(this.allowMonumentDoors != null) ? this.allowMonumentDoors.ToString() : "null",
				", allowedMonumentThings=",
				(this.allowedMonumentThings != null) ? this.allowedMonumentThings.ToString() : "null",
				", useOnlyStonesAvailableOnMap=",
				(this.useOnlyStonesAvailableOnMap != null) ? this.useOnlyStonesAvailableOnMap.ToString() : "null",
				", allowConcrete=",
				(this.allowConcrete != null) ? this.allowConcrete.ToString() : "null",
				", allowFlammableWalls=",
				(this.allowFlammableWalls != null) ? this.allowFlammableWalls.ToString() : "null",
				", onlyBuildableByPlayer=",
				(this.onlyBuildableByPlayer != null) ? this.onlyBuildableByPlayer.ToString() : "null",
				", addFloor=",
				(this.addFloors != null) ? this.addFloors.ToString() : "null",
				", requireFloor=",
				(this.requireFloor != null) ? this.requireFloor.ToString() : "null",
				", mechClusterSize=",
				(this.mechClusterSize != null) ? this.mechClusterSize.ToString() : "null",
				", mechClusterDormant=",
				(this.mechClusterDormant != null) ? this.mechClusterDormant.ToString() : "null",
				", mechClusterForMap=",
				(this.mechClusterForMap != null) ? this.mechClusterForMap.ToString() : "null"
			});
		}

		// Token: 0x040085B0 RID: 34224
		public Sketch sketch;

		// Token: 0x040085B1 RID: 34225
		public CellRect? rect;

		// Token: 0x040085B2 RID: 34226
		public bool? allowWood;

		// Token: 0x040085B3 RID: 34227
		public float? points;

		// Token: 0x040085B4 RID: 34228
		public float? totalPoints;

		// Token: 0x040085B5 RID: 34229
		public float? chance;

		// Token: 0x040085B6 RID: 34230
		public int? symmetryOrigin;

		// Token: 0x040085B7 RID: 34231
		public bool? symmetryVertical;

		// Token: 0x040085B8 RID: 34232
		public bool? symmetryOriginIncluded;

		// Token: 0x040085B9 RID: 34233
		public bool? symmetryClear;

		// Token: 0x040085BA RID: 34234
		public bool? connectedGroupsSameStuff;

		// Token: 0x040085BB RID: 34235
		public ThingDef assignRandomStuffTo;

		// Token: 0x040085BC RID: 34236
		public ThingDef cornerThing;

		// Token: 0x040085BD RID: 34237
		public bool? floorFillRoomsOnly;

		// Token: 0x040085BE RID: 34238
		public bool? singleFloorType;

		// Token: 0x040085BF RID: 34239
		public bool? onlyStoneFloors;

		// Token: 0x040085C0 RID: 34240
		public ThingDef thingCentral;

		// Token: 0x040085C1 RID: 34241
		public ThingDef wallEdgeThing;

		// Token: 0x040085C2 RID: 34242
		public IntVec2? monumentSize;

		// Token: 0x040085C3 RID: 34243
		public bool? monumentOpen;

		// Token: 0x040085C4 RID: 34244
		public bool? allowMonumentDoors;

		// Token: 0x040085C5 RID: 34245
		public ThingFilter allowedMonumentThings;

		// Token: 0x040085C6 RID: 34246
		public Map useOnlyStonesAvailableOnMap;

		// Token: 0x040085C7 RID: 34247
		public bool? allowConcrete;

		// Token: 0x040085C8 RID: 34248
		public bool? allowFlammableWalls;

		// Token: 0x040085C9 RID: 34249
		public bool? onlyBuildableByPlayer;

		// Token: 0x040085CA RID: 34250
		public bool? addFloors;

		// Token: 0x040085CB RID: 34251
		public bool? requireFloor;

		// Token: 0x040085CC RID: 34252
		public IntVec2? mechClusterSize;

		// Token: 0x040085CD RID: 34253
		public bool? mechClusterDormant;

		// Token: 0x040085CE RID: 34254
		public Map mechClusterForMap;

		// Token: 0x040085CF RID: 34255
		public bool? forceNoConditionCauser;

		// Token: 0x040085D0 RID: 34256
		public IntVec2? utilityBuildingSize;

		// Token: 0x040085D1 RID: 34257
		public float? destroyChanceExp;

		// Token: 0x040085D2 RID: 34258
		public IntVec2? landingPadSize;

		// Token: 0x040085D3 RID: 34259
		private Dictionary<string, object> custom;
	}
}
