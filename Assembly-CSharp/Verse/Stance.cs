using System;

namespace Verse
{
	// Token: 0x020004E8 RID: 1256
	public abstract class Stance : IExposable
	{
		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06002555 RID: 9557 RVA: 0x000E3155 File Offset: 0x000E1355
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06002556 RID: 9558 RVA: 0x000E3158 File Offset: 0x000E1358
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000E3165 File Offset: 0x000E1365
		public virtual void StanceTick()
		{
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000E3167 File Offset: 0x000E1367
		public virtual void StanceDraw()
		{
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000E3169 File Offset: 0x000E1369
		public virtual void ExposeData()
		{
		}

		// Token: 0x04001AF7 RID: 6903
		public Pawn_StanceTracker stanceTracker;
	}
}
