using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B5D RID: 7005
	public class ThingSetMaker_Conditional_FactionRelation : ThingSetMaker_Conditional
	{
		// Token: 0x0600B3E3 RID: 46051 RVA: 0x003F6860 File Offset: 0x003F4A60
		protected override bool Condition(ThingSetMakerParams parms)
		{
			Faction faction = Find.FactionManager.FirstFactionOfDef(this.factionDef);
			if (faction == null)
			{
				return false;
			}
			switch (faction.RelationKindWith(Faction.OfPlayer))
			{
			case FactionRelationKind.Hostile:
				return this.allowHostile;
			case FactionRelationKind.Neutral:
				return this.allowNeutral;
			case FactionRelationKind.Ally:
				return this.allowAlly;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x040067B6 RID: 26550
		public FactionDef factionDef;

		// Token: 0x040067B7 RID: 26551
		public bool allowHostile;

		// Token: 0x040067B8 RID: 26552
		public bool allowNeutral;

		// Token: 0x040067B9 RID: 26553
		public bool allowAlly;
	}
}
