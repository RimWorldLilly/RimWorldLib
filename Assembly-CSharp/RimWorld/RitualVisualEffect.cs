using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001730 RID: 5936
	public class RitualVisualEffect : IExposable
	{
		// Token: 0x06009403 RID: 37891 RVA: 0x0035F860 File Offset: 0x0035DA60
		public void Setup(LordJob_Ritual r, bool loading)
		{
			this.ritual = r;
			if (!loading)
			{
				using (List<CompProperties_RitualVisualEffect>.Enumerator enumerator = this.def.comps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CompProperties_RitualVisualEffect compProperties_RitualVisualEffect = enumerator.Current;
						RitualVisualEffectComp instance = compProperties_RitualVisualEffect.GetInstance();
						instance.props = compProperties_RitualVisualEffect;
						instance.OnSetup(this, this.ritual, false);
						this.comps.Add(instance);
					}
					return;
				}
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				RitualVisualEffectComp ritualVisualEffectComp = this.comps[i];
				ritualVisualEffectComp.props = this.def.comps[i];
				ritualVisualEffectComp.OnSetup(this, this.ritual, true);
			}
		}

		// Token: 0x06009404 RID: 37892 RVA: 0x0035F92C File Offset: 0x0035DB2C
		public void Tick()
		{
			foreach (RitualVisualEffectComp ritualVisualEffectComp in this.comps)
			{
				ritualVisualEffectComp.TickInterval(1);
			}
			foreach (Mote mote in this.maintainedMotes)
			{
				mote.Maintain();
			}
			foreach (Pair<Effecter, TargetInfo> pair in this.maintainedEffecters)
			{
				pair.First.EffectTick(pair.Second, pair.Second);
			}
		}

		// Token: 0x06009405 RID: 37893 RVA: 0x0035FA14 File Offset: 0x0035DC14
		public void AddMoteToMaintain(Mote mote)
		{
			this.maintainedMotes.Add(mote);
		}

		// Token: 0x06009406 RID: 37894 RVA: 0x0035FA22 File Offset: 0x0035DC22
		public void AddEffecterToMaintain(TargetInfo target, Effecter eff)
		{
			this.maintainedEffecters.Add(new Pair<Effecter, TargetInfo>(eff, target));
		}

		// Token: 0x06009407 RID: 37895 RVA: 0x0035FA38 File Offset: 0x0035DC38
		public void Cleanup()
		{
			foreach (Pair<Effecter, TargetInfo> pair in this.maintainedEffecters)
			{
				pair.First.Cleanup();
			}
		}

		// Token: 0x06009408 RID: 37896 RVA: 0x0035FA90 File Offset: 0x0035DC90
		public void ExposeData()
		{
			Scribe_Defs.Look<RitualVisualEffectDef>(ref this.def, "def");
			Scribe_Collections.Look<RitualVisualEffectComp>(ref this.comps, "comps", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0400595E RID: 22878
		public LordJob_Ritual ritual;

		// Token: 0x0400595F RID: 22879
		public RitualVisualEffectDef def;

		// Token: 0x04005960 RID: 22880
		public List<RitualVisualEffectComp> comps = new List<RitualVisualEffectComp>();

		// Token: 0x04005961 RID: 22881
		private List<Mote> maintainedMotes = new List<Mote>();

		// Token: 0x04005962 RID: 22882
		private List<Pair<Effecter, TargetInfo>> maintainedEffecters = new List<Pair<Effecter, TargetInfo>>();
	}
}
