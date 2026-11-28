using System;
using RimWorld;
using Verse;

// Token: 0x0200000D RID: 13
public class LayoutWorkerComplex_Ancient : LayoutWorkerComplex
{
	// Token: 0x0600003E RID: 62 RVA: 0x000033EF File Offset: 0x000015EF
	public LayoutWorkerComplex_Ancient(LayoutDef def) : base(def)
	{
	}

	// Token: 0x0600003F RID: 63 RVA: 0x000033F8 File Offset: 0x000015F8
	public override Faction GetFixedHostileFactionForThreats()
	{
		if (!Rand.Chance(base.Def.fixedHostileFactionChance))
		{
			return null;
		}
		if (Faction.OfInsects != null && Faction.OfMechanoids != null)
		{
			if (!Rand.Bool)
			{
				return Faction.OfMechanoids;
			}
			return Faction.OfInsects;
		}
		else
		{
			if (Faction.OfInsects != null)
			{
				return Faction.OfInsects;
			}
			return Faction.OfMechanoids;
		}
	}
}
