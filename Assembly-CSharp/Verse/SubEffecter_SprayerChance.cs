using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020006D2 RID: 1746
	public class SubEffecter_SprayerChance : SubEffecter_Sprayer
	{
		// Token: 0x06003720 RID: 14112 RVA: 0x00161BC4 File Offset: 0x0015FDC4
		public SubEffecter_SprayerChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilMote = def.initialDelayTicks;
			this.lifespanMaxTicks = Find.TickManager.TicksGame + def.lifespanMaxTicks + this.ticksUntilMote;
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x00161BF8 File Offset: 0x0015FDF8
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float num = base.EffectiveChancePerTick;
			if (base.EffectiveSpawnLocType == MoteSpawnLocType.RandomCellOnTarget && B.HasThing)
			{
				num *= (float)(B.Thing.def.size.x * B.Thing.def.size.z);
			}
			this.ticksUntilMote--;
			if (Find.TickManager.TicksGame >= this.lastSpawnedTicks + this.def.chancePeriodTicks && Find.TickManager.TicksGame < this.lifespanMaxTicks && this.ticksUntilMote <= 0 && Rand.Chance(num))
			{
				this.lastSpawnedTicks = Find.TickManager.TicksGame;
				base.MakeMote(A, B, -1);
				if (this.def.soundDef != null)
				{
					this.def.soundDef.PlayOneShot(A);
				}
			}
		}

		// Token: 0x0400243C RID: 9276
		private int lastSpawnedTicks;

		// Token: 0x0400243D RID: 9277
		private int ticksUntilMote;

		// Token: 0x0400243E RID: 9278
		private int lifespanMaxTicks;
	}
}
