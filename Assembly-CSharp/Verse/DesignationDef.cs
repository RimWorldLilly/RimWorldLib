using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000163 RID: 355
	public class DesignationDef : Def
	{
		// Token: 0x06000ABD RID: 2749 RVA: 0x0003B80E File Offset: 0x00039A0E
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
			});
		}

		// Token: 0x040007F0 RID: 2032
		[NoTranslate]
		public string texturePath;

		// Token: 0x040007F1 RID: 2033
		public TargetType targetType;

		// Token: 0x040007F2 RID: 2034
		public bool removeIfBuildingDespawned;

		// Token: 0x040007F3 RID: 2035
		public bool designateCancelable = true;

		// Token: 0x040007F4 RID: 2036
		public bool shouldBatchDraw = true;

		// Token: 0x040007F5 RID: 2037
		[Unsaved(false)]
		public Material iconMat;
	}
}
