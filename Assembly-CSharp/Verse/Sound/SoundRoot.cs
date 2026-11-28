using System;

namespace Verse.Sound
{
	// Token: 0x020007EF RID: 2031
	public class SoundRoot
	{
		// Token: 0x06004057 RID: 16471 RVA: 0x00199F48 File Offset: 0x00198148
		public SoundRoot()
		{
			this.sourcePool = new AudioSourcePool();
			this.sustainerManager = new SustainerManager();
			this.oneShotManager = new SampleOneShotManager();
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x00199F71 File Offset: 0x00198171
		public void Update()
		{
			this.sustainerManager.SustainerManagerUpdate();
			this.oneShotManager.SampleOneShotManagerUpdate();
		}

		// Token: 0x04002AB8 RID: 10936
		public AudioSourcePool sourcePool;

		// Token: 0x04002AB9 RID: 10937
		public SampleOneShotManager oneShotManager;

		// Token: 0x04002ABA RID: 10938
		public SustainerManager sustainerManager;
	}
}
