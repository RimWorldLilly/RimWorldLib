using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B45 RID: 6981
	public class LiquidFuel : Filth
	{
		// Token: 0x0600B347 RID: 45895 RVA: 0x003F2F4B File Offset: 0x003F114B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x0600B348 RID: 45896 RVA: 0x003F2F65 File Offset: 0x003F1165
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600B349 RID: 45897 RVA: 0x003F2F7F File Offset: 0x003F117F
		public void Refill()
		{
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600B34A RID: 45898 RVA: 0x003F2F91 File Offset: 0x003F1191
		protected override void TickInterval(int delta)
		{
			if (this.spawnTick + 1500 < Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600B34B RID: 45899 RVA: 0x003F2FB2 File Offset: 0x003F11B2
		public override void ThickenFilth()
		{
			base.ThickenFilth();
			this.Refill();
		}

		// Token: 0x04006771 RID: 26481
		private int spawnTick;

		// Token: 0x04006772 RID: 26482
		private const int DryOutTime = 1500;
	}
}
