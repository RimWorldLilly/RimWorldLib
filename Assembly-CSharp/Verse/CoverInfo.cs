using System;

namespace Verse
{
	// Token: 0x02000725 RID: 1829
	public struct CoverInfo
	{
		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x060039CD RID: 14797 RVA: 0x001702CA File Offset: 0x0016E4CA
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x060039CE RID: 14798 RVA: 0x001702D2 File Offset: 0x0016E4D2
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x060039CF RID: 14799 RVA: 0x001702DA File Offset: 0x0016E4DA
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x001702E7 File Offset: 0x0016E4E7
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		// Token: 0x04002618 RID: 9752
		private Thing thingInt;

		// Token: 0x04002619 RID: 9753
		private float blockChanceInt;
	}
}
