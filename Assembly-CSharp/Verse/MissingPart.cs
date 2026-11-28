using System;

namespace Verse
{
	// Token: 0x020001A7 RID: 423
	public class MissingPart
	{
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x0003E659 File Offset: 0x0003C859
		public BodyPartDef BodyPart
		{
			get
			{
				return this.bodyPart;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x0003E661 File Offset: 0x0003C861
		public HediffDef Injury
		{
			get
			{
				return this.injury;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0003E669 File Offset: 0x0003C869
		public float? Chance
		{
			get
			{
				return this.chance;
			}
		}

		// Token: 0x04000AE3 RID: 2787
		private BodyPartDef bodyPart;

		// Token: 0x04000AE4 RID: 2788
		private HediffDef injury;

		// Token: 0x04000AE5 RID: 2789
		private float? chance;
	}
}
