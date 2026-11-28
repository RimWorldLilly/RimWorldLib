using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020F5 RID: 8437
	public class SymbolResolver_AncientCryptosleepCasket : SymbolResolver
	{
		// Token: 0x0600D413 RID: 54291 RVA: 0x004E546C File Offset: 0x004E366C
		public override void Resolve(ResolveParams rp)
		{
			int groupID = rp.ancientCryptosleepCasketGroupID ?? Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID();
			PodContentsType type = rp.podContentsType ?? Gen.RandomEnumValue<PodContentsType>(true);
			Rot4 rot = rp.thingRot ?? Rot4.North;
			Building_AncientCryptosleepCasket pod = RoomGenUtility.SpawnCryptoCasket(rp.rect.RandomCell, BaseGen.globalSettings.map, rot, groupID, type, ThingSetMakerDefOf.MapGen_AncientPodContents);
			if (rp.ancientCryptosleepCasketOpenSignalTag != null)
			{
				RoomGenUtility.SpawnOpenCryptoCasketSignal(pod, BaseGen.globalSettings.map, rp.ancientCryptosleepCasketOpenSignalTag);
			}
		}
	}
}
