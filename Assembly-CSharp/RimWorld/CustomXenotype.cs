using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CA4 RID: 7332
	public class CustomXenotype : IExposable
	{
		// Token: 0x17002222 RID: 8738
		// (get) Token: 0x0600BBC1 RID: 48065 RVA: 0x0042BBFD File Offset: 0x00429DFD
		public XenotypeIconDef IconDef
		{
			get
			{
				return this.iconDef ?? XenotypeIconDefOf.Basic;
			}
		}

		// Token: 0x0600BBC2 RID: 48066 RVA: 0x0042BC10 File Offset: 0x00429E10
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<bool>(ref this.inheritable, "inheritable", false, false);
			Scribe_Collections.Look<GeneDef>(ref this.genes, "genes", LookMode.Def, Array.Empty<object>());
			Scribe_Defs.Look<XenotypeIconDef>(ref this.iconDef, "iconDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.genes.RemoveAll((GeneDef x) => x == null) > 0)
				{
					Log.Error("Removed null genes");
				}
			}
		}

		// Token: 0x04006AFC RID: 27388
		public string fileName;

		// Token: 0x04006AFD RID: 27389
		public string name;

		// Token: 0x04006AFE RID: 27390
		public List<GeneDef> genes = new List<GeneDef>();

		// Token: 0x04006AFF RID: 27391
		public XenotypeIconDef iconDef;

		// Token: 0x04006B00 RID: 27392
		public bool inheritable;
	}
}
