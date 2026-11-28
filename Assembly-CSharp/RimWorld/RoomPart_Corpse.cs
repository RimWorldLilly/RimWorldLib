using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126E RID: 4718
	public class RoomPart_Corpse : RoomPartWorker
	{
		// Token: 0x060074E8 RID: 29928 RVA: 0x00290DE6 File Offset: 0x0028EFE6
		public RoomPart_Corpse(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074E9 RID: 29929 RVA: 0x00290DF0 File Offset: 0x0028EFF0
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			PawnKindDef villager = PawnKindDefOf.Villager;
			if (faction != null)
			{
				faction.RandomPawnKind();
			}
			RoomGenUtility.SpawnCorpses(room, map, RoomPart_Corpse.Count, villager, DamageDefOf.ExecutionCut, null, ThingDefOf.MeleeWeapon_Knife.tools[1], null, null, null, false);
		}

		// Token: 0x04004959 RID: 18777
		private static readonly IntRange Count = IntRange.One;
	}
}
