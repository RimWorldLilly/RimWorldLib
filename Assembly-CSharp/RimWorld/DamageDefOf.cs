using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E79 RID: 7801
	[DefOf]
	public static class DamageDefOf
	{
		// Token: 0x0600CA34 RID: 51764 RVA: 0x004A5C36 File Offset: 0x004A3E36
		static DamageDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DamageDefOf));
		}

		// Token: 0x040078D8 RID: 30936
		public static DamageDef Cut;

		// Token: 0x040078D9 RID: 30937
		public static DamageDef Crush;

		// Token: 0x040078DA RID: 30938
		public static DamageDef Blunt;

		// Token: 0x040078DB RID: 30939
		public static DamageDef Stab;

		// Token: 0x040078DC RID: 30940
		public static DamageDef Bullet;

		// Token: 0x040078DD RID: 30941
		public static DamageDef Bomb;

		// Token: 0x040078DE RID: 30942
		public static DamageDef Scratch;

		// Token: 0x040078DF RID: 30943
		public static DamageDef ScratchToxic;

		// Token: 0x040078E0 RID: 30944
		public static DamageDef TornadoScratch;

		// Token: 0x040078E1 RID: 30945
		public static DamageDef Bite;

		// Token: 0x040078E2 RID: 30946
		public static DamageDef Flame;

		// Token: 0x040078E3 RID: 30947
		public static DamageDef Burn;

		// Token: 0x040078E4 RID: 30948
		public static DamageDef AcidBurn;

		// Token: 0x040078E5 RID: 30949
		public static DamageDef Vaporize;

		// Token: 0x040078E6 RID: 30950
		[MayRequireAnomaly]
		public static DamageDef ElectricalBurn;

		// Token: 0x040078E7 RID: 30951
		[MayRequireAnomaly]
		public static DamageDef Psychic;

		// Token: 0x040078E8 RID: 30952
		public static DamageDef SurgicalCut;

		// Token: 0x040078E9 RID: 30953
		public static DamageDef ExecutionCut;

		// Token: 0x040078EA RID: 30954
		public static DamageDef Frostbite;

		// Token: 0x040078EB RID: 30955
		[MayRequireOdyssey]
		public static DamageDef VacuumBurn;

		// Token: 0x040078EC RID: 30956
		public static DamageDef Stun;

		// Token: 0x040078ED RID: 30957
		public static DamageDef EMP;

		// Token: 0x040078EE RID: 30958
		public static DamageDef NerveStun;

		// Token: 0x040078EF RID: 30959
		[MayRequireBiotech]
		public static DamageDef MechBandShockwave;

		// Token: 0x040078F0 RID: 30960
		public static DamageDef Extinguish;

		// Token: 0x040078F1 RID: 30961
		public static DamageDef Smoke;

		// Token: 0x040078F2 RID: 30962
		public static DamageDef Deterioration;

		// Token: 0x040078F3 RID: 30963
		public static DamageDef Mining;

		// Token: 0x040078F4 RID: 30964
		public static DamageDef Rotting;

		// Token: 0x040078F5 RID: 30965
		[MayRequireBiotech]
		public static DamageDef ToxGas;

		// Token: 0x040078F6 RID: 30966
		[MayRequireOdyssey]
		public static DamageDef MiningBomb;
	}
}
