using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001EC RID: 492
	public class GameComponent_DebugTools : GameComponent
	{
		// Token: 0x06000DF1 RID: 3569 RVA: 0x00048318 File Offset: 0x00046518
		public GameComponent_DebugTools(Game game)
		{
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0004832B File Offset: 0x0004652B
		public override void GameComponentUpdate()
		{
			if (this.callbacks.Count > 0 && this.callbacks[0]())
			{
				this.callbacks.RemoveAt(0);
			}
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0004835A File Offset: 0x0004655A
		public void AddPerFrameCallback(Func<bool> callback)
		{
			this.callbacks.Add(callback);
		}

		// Token: 0x04000DE3 RID: 3555
		private List<Func<bool>> callbacks = new List<Func<bool>>();
	}
}
