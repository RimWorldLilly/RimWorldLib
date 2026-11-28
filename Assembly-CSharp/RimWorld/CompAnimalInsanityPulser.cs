using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200180B RID: 6155
	public class CompAnimalInsanityPulser : ThingComp
	{
		// Token: 0x17001A1C RID: 6684
		// (get) Token: 0x060099B1 RID: 39345 RVA: 0x003793FA File Offset: 0x003775FA
		public CompProperties_AnimalInsanityPulser Props
		{
			get
			{
				return (CompProperties_AnimalInsanityPulser)this.props;
			}
		}

		// Token: 0x060099B2 RID: 39346 RVA: 0x00379407 File Offset: 0x00377607
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		// Token: 0x060099B3 RID: 39347 RVA: 0x00379436 File Offset: 0x00377636
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToInsanityPulse, "ticksToInsanityPulse", 0, false);
		}

		// Token: 0x060099B4 RID: 39348 RVA: 0x00379450 File Offset: 0x00377650
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.ticksToInsanityPulse--;
			if (this.ticksToInsanityPulse <= 0)
			{
				this.DoAnimalInsanityPulse();
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		// Token: 0x060099B5 RID: 39349 RVA: 0x003794A0 File Offset: 0x003776A0
		private void DoAnimalInsanityPulse()
		{
			IEnumerable<Pawn> enumerable = from p in this.parent.Map.mapPawns.AllPawnsSpawned
			where p.IsAnimal && p.Position.InHorDistOf(this.parent.Position, (float)this.Props.radius)
			select p;
			bool flag = false;
			using (IEnumerator<Pawn> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, false, null, false, false, false))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				Messages.Message("MessageAnimalInsanityPulse".Translate(this.parent.Named("SOURCE")), this.parent, MessageTypeDefOf.ThreatSmall, true);
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
				if (this.parent.Map == Find.CurrentMap)
				{
					Find.CameraDriver.shaker.DoShake(4f);
				}
			}
		}

		// Token: 0x04005BA8 RID: 23464
		private int ticksToInsanityPulse;
	}
}
