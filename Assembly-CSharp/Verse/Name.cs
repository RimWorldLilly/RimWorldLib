using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004D2 RID: 1234
	public abstract class Name : IExposable
	{
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002266 RID: 8806
		public abstract string ToStringFull { get; }

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002267 RID: 8807
		public abstract string ToStringShort { get; }

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002268 RID: 8808
		public abstract bool IsValid { get; }

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002269 RID: 8809 RVA: 0x000CC594 File Offset: 0x000CA794
		public bool UsedThisGame
		{
			get
			{
				using (IEnumerator<Name> enumerator = NameUseChecker.AllPawnsNamesEverUsed.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ConfusinglySimilarTo(this))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600226A RID: 8810
		public abstract bool ConfusinglySimilarTo(Name other);

		// Token: 0x0600226B RID: 8811
		public abstract void ExposeData();

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x0600226C RID: 8812
		public abstract bool Numerical { get; }
	}
}
