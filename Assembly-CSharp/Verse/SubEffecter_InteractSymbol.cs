using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006CD RID: 1741
	public class SubEffecter_InteractSymbol : SubEffecter
	{
		// Token: 0x06003710 RID: 14096 RVA: 0x00160E7F File Offset: 0x0015F07F
		public SubEffecter_InteractSymbol(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x00160E89 File Offset: 0x0015F089
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.interactMote == null)
			{
				this.interactMote = MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
			}
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x00160EAB File Offset: 0x0015F0AB
		public override void SubCleanup()
		{
			if (this.interactMote != null && !this.interactMote.Destroyed)
			{
				this.interactMote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04002436 RID: 9270
		private Mote interactMote;
	}
}
