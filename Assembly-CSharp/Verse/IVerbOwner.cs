using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000732 RID: 1842
	public interface IVerbOwner
	{
		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06003A35 RID: 14901
		VerbTracker VerbTracker { get; }

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06003A36 RID: 14902
		List<VerbProperties> VerbProperties { get; }

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06003A37 RID: 14903
		List<Tool> Tools { get; }

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06003A38 RID: 14904
		ImplementOwnerTypeDef ImplementOwnerTypeDef { get; }

		// Token: 0x06003A39 RID: 14905
		string UniqueVerbOwnerID();

		// Token: 0x06003A3A RID: 14906
		bool VerbsStillUsableBy(Pawn p);

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06003A3B RID: 14907
		Thing ConstantCaster { get; }
	}
}
