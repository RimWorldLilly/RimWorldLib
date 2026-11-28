using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001911 RID: 6417
	public class CompMoteEmitterProximityScan : CompMoteEmitter
	{
		// Token: 0x17001BBC RID: 7100
		// (get) Token: 0x0600A0D7 RID: 41175 RVA: 0x003982A7 File Offset: 0x003964A7
		private CompProperties_MoteEmitterProximityScan Props
		{
			get
			{
				return (CompProperties_MoteEmitterProximityScan)this.props;
			}
		}

		// Token: 0x17001BBD RID: 7101
		// (get) Token: 0x0600A0D8 RID: 41176 RVA: 0x003982B4 File Offset: 0x003964B4
		private CompSendSignalOnMotion ProximityComp
		{
			get
			{
				CompSendSignalOnMotion result;
				if ((result = this.proximityCompCached) == null)
				{
					result = (this.proximityCompCached = this.parent.GetComp<CompSendSignalOnMotion>());
				}
				return result;
			}
		}

		// Token: 0x0600A0D9 RID: 41177 RVA: 0x003982DF File Offset: 0x003964DF
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x0600A0DA RID: 41178 RVA: 0x00398304 File Offset: 0x00396504
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.ProximityComp == null || this.ProximityComp.Sent)
			{
				if (this.sustainer != null && !this.sustainer.Ended)
				{
					this.sustainer.End();
				}
				return;
			}
			if (this.mote == null)
			{
				this.Emit();
			}
			if (!this.Props.soundEmitting.NullOrUndefined())
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					this.sustainer = this.Props.soundEmitting.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
				this.sustainer.Maintain();
			}
			if (this.mote == null)
			{
				return;
			}
			this.mote.Maintain();
			float a;
			if (!this.ProximityComp.Enabled)
			{
				if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
				{
					this.ticksSinceLastEmitted = 0;
				}
				else
				{
					this.ticksSinceLastEmitted++;
				}
				float num = (float)this.ticksSinceLastEmitted / 60f;
				if (num <= this.Props.warmupPulseFadeInTime)
				{
					if (this.Props.warmupPulseFadeInTime > 0f)
					{
						a = num / this.Props.warmupPulseFadeInTime;
					}
					else
					{
						a = 1f;
					}
				}
				else if (num <= this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime)
				{
					a = 1f;
				}
				else if (this.Props.warmupPulseFadeOutTime > 0f)
				{
					a = 1f - Mathf.InverseLerp(this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime, this.Props.warmupPulseFadeInTime + this.Props.warmupPulseSolidTime + this.Props.warmupPulseFadeOutTime, num);
				}
				else
				{
					a = 1f;
				}
			}
			else
			{
				a = 1f;
			}
			this.mote.instanceColor.a = a;
		}

		// Token: 0x04005EFA RID: 24314
		private CompSendSignalOnMotion proximityCompCached;

		// Token: 0x04005EFB RID: 24315
		private Sustainer sustainer;
	}
}
