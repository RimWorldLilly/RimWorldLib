using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001733 RID: 5939
	public abstract class CompRitualEffect_Constant : RitualVisualEffectComp
	{
		// Token: 0x0600941A RID: 37914 RVA: 0x003600D9 File Offset: 0x0035E2D9
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			base.OnSetup(parent, ritual, loading);
			this.Spawn(ritual);
		}

		// Token: 0x0600941B RID: 37915 RVA: 0x003600EC File Offset: 0x0035E2EC
		protected virtual void Spawn(LordJob_Ritual ritual)
		{
			Mote mote = this.SpawnMote(ritual, null);
			if (mote != null)
			{
				this.parent.AddMoteToMaintain(mote);
				if (this.props.colorOverride != null)
				{
					mote.instanceColor = this.props.colorOverride.Value;
				}
				else
				{
					mote.instanceColor = this.parent.def.tintColor;
				}
				this.spawned = true;
			}
		}

		// Token: 0x0600941C RID: 37916 RVA: 0x00360160 File Offset: 0x0035E360
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (!this.spawned)
			{
				this.Spawn(this.parent.ritual);
			}
		}

		// Token: 0x04005977 RID: 22903
		protected bool spawned;
	}
}
