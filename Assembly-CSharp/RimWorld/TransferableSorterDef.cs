using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100B RID: 4107
	public class TransferableSorterDef : Def
	{
		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x06006793 RID: 26515 RVA: 0x0024A095 File Offset: 0x00248295
		public TransferableComparer Comparer
		{
			get
			{
				if (this.comparerInt == null)
				{
					this.comparerInt = (TransferableComparer)Activator.CreateInstance(this.comparerClass);
				}
				return this.comparerInt;
			}
		}

		// Token: 0x040041B9 RID: 16825
		public Type comparerClass;

		// Token: 0x040041BA RID: 16826
		[Unsaved(false)]
		private TransferableComparer comparerInt;
	}
}
