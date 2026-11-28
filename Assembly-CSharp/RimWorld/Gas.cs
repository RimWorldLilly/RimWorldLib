using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B1A RID: 6938
	public class Gas : Thing
	{
		// Token: 0x0600B183 RID: 45443 RVA: 0x003E8F64 File Offset: 0x003E7164
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			for (;;)
			{
				Thing gas = base.Position.GetGas(map);
				if (gas == null)
				{
					break;
				}
				gas.Destroy(DestroyMode.Vanish);
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.destroyTick = Find.TickManager.TicksGame + this.def.gas.expireSeconds.RandomInRange.SecondsToTicks();
			}
			this.graphicRotationSpeed = Rand.Range(-this.def.gas.rotationSpeed, this.def.gas.rotationSpeed) / 60f;
		}

		// Token: 0x0600B184 RID: 45444 RVA: 0x003E8FFA File Offset: 0x003E71FA
		protected override void TickInterval(int delta)
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.graphicRotation += this.graphicRotationSpeed * (float)delta;
		}

		// Token: 0x0600B185 RID: 45445 RVA: 0x003E902B File Offset: 0x003E722B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}

		// Token: 0x04006691 RID: 26257
		public int destroyTick;

		// Token: 0x04006692 RID: 26258
		public float graphicRotation;

		// Token: 0x04006693 RID: 26259
		public float graphicRotationSpeed;
	}
}
