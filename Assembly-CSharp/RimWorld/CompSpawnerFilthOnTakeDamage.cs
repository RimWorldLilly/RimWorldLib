using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001982 RID: 6530
	public class CompSpawnerFilthOnTakeDamage : ThingComp
	{
		// Token: 0x17001C79 RID: 7289
		// (get) Token: 0x0600A3F9 RID: 41977 RVA: 0x003A952A File Offset: 0x003A772A
		public CompProperties_SpawnFilthOnTakeDamage Props
		{
			get
			{
				return (CompProperties_SpawnFilthOnTakeDamage)this.props;
			}
		}

		// Token: 0x0600A3FA RID: 41978 RVA: 0x003A9538 File Offset: 0x003A7738
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.parent.Spawned && Rand.Chance(this.Props.chance))
			{
				int randomInRange = this.Props.filthCountRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					Filth filth;
					FilthMaker.TryMakeFilth(this.parent.OccupiedRect().ExpandedBy(1).EdgeCells.RandomElement<IntVec3>(), this.parent.Map, this.Props.filthDef, out filth, this.parent.LabelNoParenthesis, FilthSourceFlags.None, false);
				}
			}
		}
	}
}
