using System;
using System.Text;

namespace Verse
{
	// Token: 0x020001EA RID: 490
	public abstract class GameComponent : IExposable
	{
		// Token: 0x06000DE2 RID: 3554 RVA: 0x00048105 File Offset: 0x00046305
		public virtual void GameComponentUpdate()
		{
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00048107 File Offset: 0x00046307
		public virtual void GameComponentTick()
		{
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00048109 File Offset: 0x00046309
		public virtual void GameComponentOnGUI()
		{
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0004810B File Offset: 0x0004630B
		public virtual void ExposeData()
		{
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0004810D File Offset: 0x0004630D
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0004810F File Offset: 0x0004630F
		public virtual void StartedNewGame()
		{
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00048111 File Offset: 0x00046311
		public virtual void LoadedGame()
		{
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00048113 File Offset: 0x00046313
		public virtual void AppendDebugString(StringBuilder sb)
		{
		}
	}
}
