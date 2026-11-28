using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AFF RID: 6911
	public class PowerBeam : OrbitalStrike
	{
		// Token: 0x0600B0BF RID: 45247 RVA: 0x003E478F File Offset: 0x003E298F
		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakePowerBeamMote(base.Position, base.Map);
		}

		// Token: 0x0600B0C0 RID: 45248 RVA: 0x003E47A8 File Offset: 0x003E29A8
		protected override void Tick()
		{
			base.Tick();
			if (base.Destroyed)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				this.StartRandomFireAndDoFlameDamage();
			}
		}

		// Token: 0x0600B0C1 RID: 45249 RVA: 0x003E47D8 File Offset: 0x003E29D8
		private void StartRandomFireAndDoFlameDamage()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 15f, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => 1f - Mathf.Min(x.DistanceTo(base.Position) / 15f, 1f) + 0.05f);
			FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f), this.instigator, null);
			PowerBeam.tmpThings.Clear();
			PowerBeam.tmpThings.AddRange(c.GetThingList(base.Map));
			for (int i = 0; i < PowerBeam.tmpThings.Count; i++)
			{
				int num = (PowerBeam.tmpThings[i] is Corpse) ? PowerBeam.CorpseFlameDamageAmountRange.RandomInRange : PowerBeam.FlameDamageAmountRange.RandomInRange;
				Pawn pawn = PowerBeam.tmpThings[i] as Pawn;
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
				if (pawn != null)
				{
					battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_PowerBeam, this.instigator as Pawn);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
				}
				PowerBeam.tmpThings[i].TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.instigator, null, this.weaponDef, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false)).AssociateWithLog(battleLogEntry_DamageTaken);
			}
			PowerBeam.tmpThings.Clear();
		}

		// Token: 0x040065FD RID: 26109
		public const float Radius = 15f;

		// Token: 0x040065FE RID: 26110
		private const int FiresStartedPerTick = 4;

		// Token: 0x040065FF RID: 26111
		private static readonly IntRange FlameDamageAmountRange = new IntRange(65, 100);

		// Token: 0x04006600 RID: 26112
		private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(5, 10);

		// Token: 0x04006601 RID: 26113
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
