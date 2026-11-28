using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020014DB RID: 5339
	public class PawnGroupMakerParms
	{
		// Token: 0x0600828C RID: 33420 RVA: 0x002ED808 File Offset: 0x002EBA08
		public override string ToString()
		{
			string[] array = new string[26];
			array[0] = "groupKind=";
			int num = 1;
			PawnGroupKindDef pawnGroupKindDef = this.groupKind;
			array[num] = ((pawnGroupKindDef != null) ? pawnGroupKindDef.ToString() : null);
			array[2] = ", tile=";
			array[3] = this.tile.ToString();
			array[4] = ", inhabitants=";
			array[5] = this.inhabitants.ToString();
			array[6] = ", points=";
			array[7] = this.points.ToString();
			array[8] = ", faction=";
			int num2 = 9;
			Faction faction = this.faction;
			array[num2] = ((faction != null) ? faction.ToString() : null);
			array[10] = ", ideo=";
			int num3 = 11;
			Ideo ideo = this.ideo;
			array[num3] = ((ideo != null) ? ideo.name : null);
			array[12] = ", traderKind=";
			int num4 = 13;
			TraderKindDef traderKindDef = this.traderKind;
			array[num4] = ((traderKindDef != null) ? traderKindDef.ToString() : null);
			array[14] = ", generateFightersOnly=";
			array[15] = this.generateFightersOnly.ToString();
			array[16] = ", dontUseSingleUseRocketLaunchers=";
			array[17] = this.dontUseSingleUseRocketLaunchers.ToString();
			array[18] = ", raidStrategy=";
			int num5 = 19;
			RaidStrategyDef raidStrategyDef = this.raidStrategy;
			array[num5] = ((raidStrategyDef != null) ? raidStrategyDef.ToString() : null);
			array[20] = ", forceOneDowned=";
			array[21] = this.forceOneDowned.ToString();
			array[22] = ", seed=";
			int num6 = 23;
			int? num7 = this.seed;
			array[num6] = num7.ToString();
			array[24] = ", raidAgeRestriction=";
			int num8 = 25;
			RaidAgeRestrictionDef raidAgeRestrictionDef = this.raidAgeRestriction;
			array[num8] = ((raidAgeRestrictionDef != null) ? raidAgeRestrictionDef.ToString() : null);
			return string.Concat(array);
		}

		// Token: 0x040050DB RID: 20699
		public PawnGroupKindDef groupKind;

		// Token: 0x040050DC RID: 20700
		public PlanetTile tile = PlanetTile.Invalid;

		// Token: 0x040050DD RID: 20701
		public bool inhabitants;

		// Token: 0x040050DE RID: 20702
		public float points;

		// Token: 0x040050DF RID: 20703
		public Faction faction;

		// Token: 0x040050E0 RID: 20704
		public Ideo ideo;

		// Token: 0x040050E1 RID: 20705
		public TraderKindDef traderKind;

		// Token: 0x040050E2 RID: 20706
		public bool generateFightersOnly;

		// Token: 0x040050E3 RID: 20707
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x040050E4 RID: 20708
		public RaidStrategyDef raidStrategy;

		// Token: 0x040050E5 RID: 20709
		public bool forceOneDowned;

		// Token: 0x040050E6 RID: 20710
		public int? seed;

		// Token: 0x040050E7 RID: 20711
		public RaidAgeRestrictionDef raidAgeRestriction;

		// Token: 0x040050E8 RID: 20712
		public bool ignoreGroupCommonality;
	}
}
