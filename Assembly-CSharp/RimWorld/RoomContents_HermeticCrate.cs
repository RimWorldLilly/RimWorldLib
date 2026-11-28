using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001233 RID: 4659
	public class RoomContents_HermeticCrate : RoomContentsWorker
	{
		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x060073A2 RID: 29602 RVA: 0x00287DB5 File Offset: 0x00285FB5
		public virtual ThingSetMakerDef ThingSetMakerDef { get; } = ThingSetMakerDefOf.MapGen_ScarlandsHermeticCrate;

		// Token: 0x060073A3 RID: 29603 RVA: 0x00287DC0 File Offset: 0x00285FC0
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.ThreatSignal = "OpenedSignal" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			RoomGenUtility.SpawnHermeticCrateInRoom(room, map, this.ThingSetMakerDef, true, this.ThreatSignal, new Rot4?(Rot4.Random));
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x040048F4 RID: 18676
		private const string OpenedSignal = "OpenedSignal";
	}
}
