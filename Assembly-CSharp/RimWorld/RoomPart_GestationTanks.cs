using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001274 RID: 4724
	public class RoomPart_GestationTanks : RoomPartWorker
	{
		// Token: 0x170013D8 RID: 5080
		// (get) Token: 0x060074F7 RID: 29943 RVA: 0x00290FA3 File Offset: 0x0028F1A3
		public new RoomPart_GestationTankDef def
		{
			get
			{
				return (RoomPart_GestationTankDef)this.def;
			}
		}

		// Token: 0x060074F8 RID: 29944 RVA: 0x00290FB0 File Offset: 0x0028F1B0
		public RoomPart_GestationTanks(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074F9 RID: 29945 RVA: 0x00290FBC File Offset: 0x0028F1BC
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomPart_GestationTanks.<>c__DisplayClass4_0 CS$<>8__locals1 = new RoomPart_GestationTanks.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientMechGestatorTank, RoomPart_GestationTanks.CountPer100Range, room, CS$<>8__locals1.map, new Rot4?(Rot4.South), new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals1.<FillRoom>g__Validator|0), null, 1, null, false, true, new Func<IntVec3, Rot4, Map, Thing>(this.SpawnTank), null);
		}

		// Token: 0x060074FA RID: 29946 RVA: 0x00291014 File Offset: 0x0028F214
		private Thing SpawnTank(IntVec3 cell, Rot4 rot, Map map)
		{
			RoomPart_GestationTankDef.State state = this.def.options.RandomElementByWeight((RoomPart_GestationTankDef.TankOption value) => value.weight).state;
			ThingWithComps thingWithComps = (ThingWithComps)GenSpawn.Spawn(ThingDefOf.AncientMechGestatorTank, cell, map, rot, WipeMode.Vanish);
			CompMechGestatorTank comp = thingWithComps.GetComp<CompMechGestatorTank>();
			CompMechGestatorTank.TankState state2;
			if (state == RoomPart_GestationTankDef.State.Proximity)
			{
				state2 = CompMechGestatorTank.TankState.Proximity;
			}
			else if (state == RoomPart_GestationTankDef.State.Dormant)
			{
				state2 = CompMechGestatorTank.TankState.Dormant;
			}
			else
			{
				state2 = CompMechGestatorTank.TankState.Empty;
			}
			comp.State = state2;
			return thingWithComps;
		}

		// Token: 0x0400495A RID: 18778
		private static readonly FloatRange CountPer100Range = new FloatRange(3f, 7f);
	}
}
