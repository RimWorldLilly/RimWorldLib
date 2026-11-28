using System;

namespace Verse.Sound
{
	// Token: 0x020007FE RID: 2046
	public class SustainerScopeFader
	{
		// Token: 0x06004091 RID: 16529 RVA: 0x0019B648 File Offset: 0x00199848
		public void SustainerScopeUpdate()
		{
			if (this.inScope)
			{
				float num = this.inScopePercent + 0.05f;
				this.inScopePercent = num;
				if (this.inScopePercent > 1f)
				{
					this.inScopePercent = 1f;
					return;
				}
			}
			else
			{
				this.inScopePercent -= 0.03f;
				if (this.inScopePercent <= 0.001f)
				{
					this.inScopePercent = 0f;
				}
			}
		}

		// Token: 0x04002AE2 RID: 10978
		public bool inScope = true;

		// Token: 0x04002AE3 RID: 10979
		public float inScopePercent = 1f;

		// Token: 0x04002AE4 RID: 10980
		private const float ScopeMatchFallRate = 0.03f;

		// Token: 0x04002AE5 RID: 10981
		private const float ScopeMatchRiseRate = 0.05f;
	}
}
