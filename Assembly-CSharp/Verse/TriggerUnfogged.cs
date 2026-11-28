using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000554 RID: 1364
	public class TriggerUnfogged : Thing
	{
		// Token: 0x06002886 RID: 10374 RVA: 0x000FA337 File Offset: 0x000F8537
		protected override void Tick()
		{
			if (!base.Spawned)
			{
				return;
			}
			if (base.Position.Fogged(base.Map))
			{
				this.everFogged = true;
				return;
			}
			if (this.everFogged)
			{
				this.Activated();
				return;
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x000FA373 File Offset: 0x000F8573
		public void Activated()
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag, false));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x000FA39A File Offset: 0x000F859A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.everFogged, "everFogged", false, false);
		}

		// Token: 0x04001C9C RID: 7324
		public string signalTag;

		// Token: 0x04001C9D RID: 7325
		private bool everFogged;
	}
}
