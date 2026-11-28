using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007F5 RID: 2037
	public class AudioSourcePool
	{
		// Token: 0x06004065 RID: 16485 RVA: 0x0019A3C6 File Offset: 0x001985C6
		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0019A3E4 File Offset: 0x001985E4
		public AudioSource GetSource(bool onCamera)
		{
			if (onCamera)
			{
				return this.sourcePoolCamera.GetSourceCamera();
			}
			return this.sourcePoolWorld.GetSourceWorld();
		}

		// Token: 0x04002ABF RID: 10943
		public AudioSourcePoolCamera sourcePoolCamera;

		// Token: 0x04002AC0 RID: 10944
		public AudioSourcePoolWorld sourcePoolWorld;
	}
}
