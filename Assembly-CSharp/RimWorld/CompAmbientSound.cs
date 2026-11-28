using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001802 RID: 6146
	public class CompAmbientSound : ThingComp
	{
		// Token: 0x17001A13 RID: 6675
		// (get) Token: 0x06009982 RID: 39298 RVA: 0x003787DA File Offset: 0x003769DA
		private CompProperties_AmbientSound Props
		{
			get
			{
				return (CompProperties_AmbientSound)this.props;
			}
		}

		// Token: 0x06009983 RID: 39299 RVA: 0x003787E7 File Offset: 0x003769E7
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			LongEventHandler.ExecuteWhenFinished(new Action(this.StartSustanier));
		}

		// Token: 0x06009984 RID: 39300 RVA: 0x003787FA File Offset: 0x003769FA
		public override void CompTick()
		{
			Sustainer sustainer = this.sustainer;
			if (sustainer == null)
			{
				return;
			}
			sustainer.Maintain();
		}

		// Token: 0x06009985 RID: 39301 RVA: 0x0037880C File Offset: 0x00376A0C
		public override void ReceiveCompSignal(string signal)
		{
			if (this.Props.disableOnHacked && signal == "Hacked")
			{
				this.EndSustainer();
			}
			if (this.Props.disabledOnUnpowered && signal == "PowerTurnedOff")
			{
				this.EndSustainer();
			}
			if (this.Props.disabledOnUnpowered && signal == "PowerTurnedOn")
			{
				this.StartSustanier();
			}
			if (this.Props.disableOnInteracted && signal == "Interacted")
			{
				this.EndSustainer();
			}
		}

		// Token: 0x06009986 RID: 39302 RVA: 0x00378899 File Offset: 0x00376A99
		public void StartSustanier()
		{
			if (this.sustainer == null)
			{
				this.sustainer = this.Props.sound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
		}

		// Token: 0x06009987 RID: 39303 RVA: 0x003788CA File Offset: 0x00376ACA
		public void EndSustainer()
		{
			Sustainer sustainer = this.sustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.sustainer = null;
		}

		// Token: 0x06009988 RID: 39304 RVA: 0x003788E4 File Offset: 0x00376AE4
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			Sustainer sustainer = this.sustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.sustainer = null;
		}

		// Token: 0x04005B90 RID: 23440
		private Sustainer sustainer;
	}
}
