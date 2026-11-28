using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001686 RID: 5766
	public class RitualBehaviorWorker_DancePartyTech : RitualBehaviorWorker
	{
		// Token: 0x170018FA RID: 6394
		// (get) Token: 0x06009095 RID: 37013 RVA: 0x00350F34 File Offset: 0x0034F134
		public override Sustainer SoundPlaying
		{
			get
			{
				return this.soundPlaying;
			}
		}

		// Token: 0x06009096 RID: 37014 RVA: 0x00350F3C File Offset: 0x0034F13C
		public RitualBehaviorWorker_DancePartyTech()
		{
		}

		// Token: 0x06009097 RID: 37015 RVA: 0x00350F44 File Offset: 0x0034F144
		public RitualBehaviorWorker_DancePartyTech(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06009098 RID: 37016 RVA: 0x00350F50 File Offset: 0x0034F150
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			Thing thing = ritual.selectedTarget.Thing;
			if (Find.TickManager.TicksGame % 20 == 0)
			{
				SoundDef soundDef = null;
				using (List<Pawn>.Enumerator enumerator = ritual.lord.ownedPawns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (GatheringsUtility.InGatheringArea(enumerator.Current.Position, ritual.selectedTarget.Cell, ritual.Map))
						{
							soundDef = SoundDefOf.DanceParty_NoMusic;
							break;
						}
					}
				}
				if (thing != null)
				{
					CompLightball compLightball = thing.TryGetComp<CompLightball>();
					if (compLightball != null)
					{
						soundDef = (compLightball.Playing ? compLightball.SoundToPlay : null);
					}
				}
				if (soundDef != null && (this.soundPlaying == null || this.soundPlaying.def != soundDef))
				{
					this.soundPlaying = soundDef.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(ritual.selectedTarget.Cell, ritual.selectedTarget.Map, false), MaintenanceType.PerTick));
				}
			}
			if (this.soundPlaying != null && !this.soundPlaying.Ended)
			{
				this.soundPlaying.Maintain();
			}
		}

		// Token: 0x06009099 RID: 37017 RVA: 0x00351074 File Offset: 0x0034F274
		public override void Cleanup(LordJob_Ritual ritual)
		{
			base.Cleanup(ritual);
			this.soundPlaying = null;
		}

		// Token: 0x04005837 RID: 22583
		private Sustainer soundPlaying;
	}
}
