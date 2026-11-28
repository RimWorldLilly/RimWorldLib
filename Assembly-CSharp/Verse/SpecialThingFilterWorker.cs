using System;

namespace Verse
{
	// Token: 0x0200072E RID: 1838
	public abstract class SpecialThingFilterWorker
	{
		// Token: 0x060039FE RID: 14846
		public abstract bool Matches(Thing t);

		// Token: 0x060039FF RID: 14847 RVA: 0x00171CB0 File Offset: 0x0016FEB0
		public virtual bool AlwaysMatches(ThingDef def)
		{
			return false;
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x00171CB3 File Offset: 0x0016FEB3
		public virtual bool CanEverMatch(ThingDef def)
		{
			return true;
		}
	}
}
