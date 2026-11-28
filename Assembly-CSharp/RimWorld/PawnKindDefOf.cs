using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E7A RID: 7802
	[DefOf]
	public static class PawnKindDefOf
	{
		// Token: 0x0600CA35 RID: 51765 RVA: 0x004A5C47 File Offset: 0x004A3E47
		static PawnKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnKindDefOf));
		}

		// Token: 0x040078F7 RID: 30967
		public static PawnKindDef Colonist;

		// Token: 0x040078F8 RID: 30968
		public static PawnKindDef Slave;

		// Token: 0x040078F9 RID: 30969
		public static PawnKindDef Villager;

		// Token: 0x040078FA RID: 30970
		public static PawnKindDef Drifter;

		// Token: 0x040078FB RID: 30971
		public static PawnKindDef SpaceRefugee;

		// Token: 0x040078FC RID: 30972
		public static PawnKindDef AncientSoldier;

		// Token: 0x040078FD RID: 30973
		public static PawnKindDef WildMan;

		// Token: 0x040078FE RID: 30974
		public static PawnKindDef Pirate;

		// Token: 0x040078FF RID: 30975
		public static PawnKindDef PirateBoss;

		// Token: 0x04007900 RID: 30976
		[MayRequireBiotech]
		public static PawnKindDef Sanguophage;

		// Token: 0x04007901 RID: 30977
		[MayRequireBiotech]
		public static PawnKindDef SanguophageThrall;

		// Token: 0x04007902 RID: 30978
		[MayRequireBiotech]
		public static PawnKindDef Mechanitor_Basic;

		// Token: 0x04007903 RID: 30979
		[MayRequireOdyssey]
		public static PawnKindDef AncientSoldier_Leader;

		// Token: 0x04007904 RID: 30980
		[MayRequireOdyssey]
		public static PawnKindDef Salvager_Elite;

		// Token: 0x04007905 RID: 30981
		public static PawnKindDef Thrumbo;

		// Token: 0x04007906 RID: 30982
		public static PawnKindDef Alphabeaver;

		// Token: 0x04007907 RID: 30983
		public static PawnKindDef Muffalo;

		// Token: 0x04007908 RID: 30984
		[MayRequireOdyssey]
		public static PawnKindDef AlphaThrumbo;

		// Token: 0x04007909 RID: 30985
		public static PawnKindDef Megascarab;

		// Token: 0x0400790A RID: 30986
		public static PawnKindDef Spelopede;

		// Token: 0x0400790B RID: 30987
		public static PawnKindDef Megaspider;

		// Token: 0x0400790C RID: 30988
		[MayRequireOdyssey]
		public static PawnKindDef Locust;

		// Token: 0x0400790D RID: 30989
		[MayRequireOdyssey]
		public static PawnKindDef HiveQueen;

		// Token: 0x0400790E RID: 30990
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Royal_Bestower;

		// Token: 0x0400790F RID: 30991
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Royal_NobleWimp;

		// Token: 0x04007910 RID: 30992
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Fighter_Janissary;

		// Token: 0x04007911 RID: 30993
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Fighter_Trooper;

		// Token: 0x04007912 RID: 30994
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Fighter_Cataphract;

		// Token: 0x04007913 RID: 30995
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Common_Lodger;

		// Token: 0x04007914 RID: 30996
		[MayRequireRoyalty]
		public static PawnKindDef Refugee;

		// Token: 0x04007915 RID: 30997
		[MayRequireIdeology]
		public static PawnKindDef Beggar;

		// Token: 0x04007916 RID: 30998
		[MayRequireIdeology]
		public static PawnKindDef PovertyPilgrim;

		// Token: 0x04007917 RID: 30999
		[MayRequireIdeology]
		public static PawnKindDef WellEquippedTraveler;

		// Token: 0x04007918 RID: 31000
		[MayRequireIdeology]
		public static PawnKindDef Dryad_Basic;

		// Token: 0x04007919 RID: 31001
		[MayRequireIdeology]
		public static PawnKindDef Dryad_Gaumaker;

		// Token: 0x0400791A RID: 31002
		public static PawnKindDef Mech_Pikeman;

		// Token: 0x0400791B RID: 31003
		public static PawnKindDef Mech_Scyther;

		// Token: 0x0400791C RID: 31004
		[MayRequireBiotech]
		public static PawnKindDef Mech_Warqueen;

		// Token: 0x0400791D RID: 31005
		[MayRequireAnomaly]
		public static PawnKindDef ShamblerSwarmer;

		// Token: 0x0400791E RID: 31006
		[MayRequireAnomaly]
		public static PawnKindDef ShamblerGorehulk;

		// Token: 0x0400791F RID: 31007
		[MayRequireAnomaly]
		public static PawnKindDef Revenant;

		// Token: 0x04007920 RID: 31008
		[MayRequireAnomaly]
		public static PawnKindDef Sightstealer;

		// Token: 0x04007921 RID: 31009
		[MayRequireAnomaly]
		public static PawnKindDef Bulbfreak;

		// Token: 0x04007922 RID: 31010
		[MayRequireAnomaly]
		public static PawnKindDef Trispike;

		// Token: 0x04007923 RID: 31011
		[MayRequireAnomaly]
		public static PawnKindDef Toughspike;

		// Token: 0x04007924 RID: 31012
		[MayRequireAnomaly]
		public static PawnKindDef Fingerspike;

		// Token: 0x04007925 RID: 31013
		[MayRequireAnomaly]
		public static PawnKindDef Dreadmeld;

		// Token: 0x04007926 RID: 31014
		[MayRequireAnomaly]
		public static PawnKindDef Nociosphere;

		// Token: 0x04007927 RID: 31015
		[MayRequireAnomaly]
		public static PawnKindDef Metalhorror;

		// Token: 0x04007928 RID: 31016
		[MayRequireAnomaly]
		public static PawnKindDef Ghoul;

		// Token: 0x04007929 RID: 31017
		[MayRequireAnomaly]
		public static PawnKindDef FleshmassNucleus;

		// Token: 0x0400792A RID: 31018
		[MayRequireAnomaly]
		public static PawnKindDef Chimera;

		// Token: 0x0400792B RID: 31019
		[MayRequireOdyssey]
		public static PawnKindDef Drone_Hunter;

		// Token: 0x0400792C RID: 31020
		[MayRequireOdyssey]
		public static PawnKindDef Drone_Wasp;

		// Token: 0x0400792D RID: 31021
		[MayRequireOdyssey]
		public static PawnKindDef Drone_Sentry;
	}
}
