using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x020007F8 RID: 2040
	public class SoundParams
	{
		// Token: 0x17000C76 RID: 3190
		public float this[string key]
		{
			get
			{
				return this.storedParams[key];
			}
			set
			{
				this.storedParams[key] = value;
			}
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0019A619 File Offset: 0x00198819
		public bool TryGetValue(string key, out float val)
		{
			return this.storedParams.TryGetValue(key, out val);
		}

		// Token: 0x04002AC6 RID: 10950
		private Dictionary<string, float> storedParams = new Dictionary<string, float>();

		// Token: 0x04002AC7 RID: 10951
		public SoundSizeAggregator sizeAggregator;
	}
}
