using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AD2 RID: 6866
	public class Building_WastepackAtomizer : Building
	{
		// Token: 0x17001F7E RID: 8062
		// (get) Token: 0x0600AEFC RID: 44796 RVA: 0x003D9DFB File Offset: 0x003D7FFB
		public CompAtomizer Atomizer
		{
			get
			{
				if (this.atomizer == null)
				{
					this.atomizer = base.GetComp<CompAtomizer>();
				}
				return this.atomizer;
			}
		}

		// Token: 0x17001F7F RID: 8063
		// (get) Token: 0x0600AEFD RID: 44797 RVA: 0x003D9E17 File Offset: 0x003D8017
		private CompPowerTrader PowerTrader
		{
			get
			{
				if (this.powerTrader == null)
				{
					this.powerTrader = base.GetComp<CompPowerTrader>();
				}
				return this.powerTrader;
			}
		}

		// Token: 0x0600AEFE RID: 44798 RVA: 0x003D9E34 File Offset: 0x003D8034
		protected override void Tick()
		{
			base.Tick();
			if (this.Atomizer.Empty || !this.PowerTrader.PowerOn)
			{
				Effecter effecter = this.operatingEffecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.operatingEffecter = null;
				return;
			}
			if (this.operatingEffecter == null)
			{
				this.operatingEffecter = this.def.building.wastepackAtomizerOperationEffecter.Spawn();
				this.operatingEffecter.Trigger(this, new TargetInfo(this.InteractionCell, base.Map, false), -1);
			}
			this.operatingEffecter.EffectTick(this, new TargetInfo(this.InteractionCell, base.Map, false));
		}

		// Token: 0x0600AEFF RID: 44799 RVA: 0x003D9EE8 File Offset: 0x003D80E8
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			Vector3 loc = drawLoc;
			loc.y -= 0.054878052f;
			this.def.building.wastepackAtomizerBottomGraphic.Graphic.Draw(loc, base.Rotation, this, 0f);
			Vector3 loc2 = drawLoc;
			loc2.y -= 0.018292684f;
			this.def.building.wastepackAtomizerWindowGraphic.Graphic.Draw(loc2, base.Rotation, this, 0f);
		}

		// Token: 0x04006520 RID: 25888
		private CompAtomizer atomizer;

		// Token: 0x04006521 RID: 25889
		private CompPowerTrader powerTrader;

		// Token: 0x04006522 RID: 25890
		private Effecter operatingEffecter;

		// Token: 0x04006523 RID: 25891
		private Graphic contentsGraphic;
	}
}
